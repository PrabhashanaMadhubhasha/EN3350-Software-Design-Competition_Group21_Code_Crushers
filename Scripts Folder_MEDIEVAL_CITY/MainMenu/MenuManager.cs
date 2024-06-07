using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Enumeration;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; set; }

    public GameObject menuCanvas;
    public GameObject uiCanvas;
    public bool isMenuOpen;

    public GameObject saveMenu;
    public GameObject settingsMenu;
    public GameObject Menu;

    #region || --------------Variable assigning Section -----------------------||

    [Header("Volume Settings")]
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private GameObject ConfirmationPrompt = null;
    [SerializeField] private float defaultVolume = 0.5f;

    [Header("Game Play Settings")]
    [SerializeField] private TMP_Text controllerSensitivityTextvalue = null;
    [SerializeField] private Slider ControllerSliderValue = null;
    [SerializeField] private int defaultSens = 4;
    public int mainControllerSens = 4;

    [Header("Toggle Settings")]
    [SerializeField] private Toggle invertYtoggle = null;

    [Header("Graphics Settings")]
    [SerializeField] private Slider brightnesslider = null;
    [SerializeField] private TMP_Text brightnessTextValue = null;
    [SerializeField] private float defaultBrightness = 1.0f;
    private int _qualitylevel;
    private bool _isFullScreen;
    private float _brightnesslevel;

    [Header("Resolution Settings")]
    public TMP_Dropdown resolutionDropDown;
    private Resolution[] resolutions;

    [Space(10)]
    [SerializeField] private TMP_Dropdown qualityDropDown;
    [SerializeField] private Toggle fullScreenToggle;
    #endregion

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);

        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        
        LoadSettings();
        ApplySettings();


    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isMenuOpen)
            {
                OpenMenu();
            }
            else
            {
                CloseMenu();
            }
        }
    }

    private void OpenMenu()
    {
        uiCanvas.SetActive(false);
        menuCanvas.SetActive(true);
        Menu.SetActive(true);
        isMenuOpen = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (SelectionManager.instance != null)
        {
            SelectionManager.instance.DisableSelection();
        }
    }

    private void CloseMenu()
    {
        saveMenu.SetActive(false);
        settingsMenu.SetActive(false);
        Menu.SetActive(false);

        uiCanvas.SetActive(true);
        menuCanvas.SetActive(false);
        isMenuOpen = false;

        if (CraftingSystem.Instance != null && InventorySystem.Instance != null)
        {
            if (!CraftingSystem.Instance.isOpen && !InventorySystem.Instance.isOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        if (SelectionManager.instance != null)
        {
            SelectionManager.instance.EnableSelection();
        }
    }

    #region ||----------------Option Menu settings--------------||

    public void Setvolume(float volume)
    {
        AudioListener.volume = volume;
        volumeTextValue.text = volume.ToString("0.0");
    }

    public void setResolution(int resoltionindex)
    {
        Resolution resolution = resolutions[resoltionindex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("ranuka_file", AudioListener.volume);
        StartCoroutine(ConfirmationBox());
    }

    public void setControllerSen(float sensitivity)
    {
        mainControllerSens = Mathf.RoundToInt(sensitivity);
        if (controllerSensitivityTextvalue != null)
        {
            controllerSensitivityTextvalue.text = sensitivity.ToString("0");
        }
        else
        {
            Debug.LogWarning("controllerSensitivityTextvalue is not assigned in the Inspector.");
        }
    }

    public void gameplayApply()
    {
        if (invertYtoggle.isOn)
        {
            PlayerPrefs.SetInt("ranuka_invert", 1);
        }
        else
        {
            PlayerPrefs.SetInt("ranuka_invert", 0);
        }
        PlayerPrefs.SetFloat("ranukasens", mainControllerSens);
        StartCoroutine(ConfirmationBox());
    }

    public void ResetButton(string Menutype)
    {
        if (Menutype == "Audio")
        {
            AudioListener.volume = defaultVolume;
            volumeSlider.value = defaultVolume;
            volumeTextValue.text = defaultVolume.ToString("0.0");
            VolumeApply();
        }
        if (Menutype == "GamePlay")
        {
            controllerSensitivityTextvalue.text = defaultSens.ToString("0");
            ControllerSliderValue.value = defaultSens;
            mainControllerSens = defaultSens;
            invertYtoggle.isOn = false;
            gameplayApply();
        }

        if (Menutype == "Graphics")
        {
            brightnesslider.value = defaultBrightness;
            brightnessTextValue.text = defaultBrightness.ToString("0.0");

            qualityDropDown.value = 1;
            QualitySettings.SetQualityLevel(1);

            fullScreenToggle.isOn = false;
            Screen.fullScreen = false;

            Resolution currentResolution = Screen.currentResolution;
            Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);
            resolutionDropDown.value = resolutions.Length;
            GraphicsApply();
        }
    }

    public IEnumerator ConfirmationBox()
    {
        ConfirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        ConfirmationPrompt.SetActive(false);
    }

    public void setBrightness(float brightness)
    {
        _brightnesslevel = brightness;
        brightnessTextValue.text = brightness.ToString("0.0");
    }

    public void setFullScreen(bool isFullScreen)
    {
        _isFullScreen = isFullScreen;
    }

    public void SetQuality(int qulaityindex)
    {
        _qualitylevel = qulaityindex;
    }

    public void GraphicsApply()
    {
        PlayerPrefs.SetFloat("ranukabrightness", _brightnesslevel);
        PlayerPrefs.SetInt("ranukaqality", _qualitylevel);
        QualitySettings.SetQualityLevel(_qualitylevel);

        PlayerPrefs.SetInt("ranukaFullScreen", (_isFullScreen ? 1 : 0));
        Screen.fullScreen = _isFullScreen;

        StartCoroutine(ConfirmationBox());
    }

    private void LoadSettings()
    {
        // Load Volume
        if (PlayerPrefs.HasKey("ranuka_file"))
        {
            float volume = PlayerPrefs.GetFloat("ranuka_file");
            if (volumeSlider != null)
            {
                volumeSlider.value = volume;
            }
            else
            {
                Debug.LogWarning("volumeSlider is not assigned in the Inspector.");
            }
            if (volumeTextValue != null)
            {
                volumeTextValue.text = volume.ToString("0.0");
            }
            else
            {
                Debug.LogWarning("volumeTextValue is not assigned in the Inspector.");
            }
            AudioListener.volume = volume;
        }

        // Load Controller Sensitivity
        if (PlayerPrefs.HasKey("ranukasens"))
        {
            mainControllerSens = Mathf.RoundToInt(PlayerPrefs.GetFloat("ranukasens"));
            if (ControllerSliderValue != null)
            {
                ControllerSliderValue.value = mainControllerSens;
            }
            else
            {
                Debug.LogWarning("ControllerSliderValue is not assigned in the Inspector.");
            }
            if (controllerSensitivityTextvalue != null)
            {
                controllerSensitivityTextvalue.text = mainControllerSens.ToString("0");
            }
            else
            {
                Debug.LogWarning("controllerSensitivityTextvalue is not assigned in the Inspector.");
            }
        }

        // Load Invert Y toggle
        if (PlayerPrefs.HasKey("ranuka_invert"))
        {
            bool invertY = PlayerPrefs.GetInt("ranuka_invert") == 1;
            if (invertYtoggle != null)
            {
                invertYtoggle.isOn = invertY;
            }
            else
            {
                Debug.LogWarning("invertYtoggle is not assigned in the Inspector.");
            }
        }

        // Load Brightness
        if (PlayerPrefs.HasKey("ranukabrightness"))
        {
            _brightnesslevel = PlayerPrefs.GetFloat("ranukabrightness");
            if (brightnesslider != null)
            {
                brightnesslider.value = _brightnesslevel;
            }
            else
            {
                Debug.LogWarning("brightnesslider is not assigned in the Inspector.");
            }
            if (brightnessTextValue != null)
            {
                brightnessTextValue.text = _brightnesslevel.ToString("0.0");
            }
            else
            {
                Debug.LogWarning("brightnessTextValue is not assigned in the Inspector.");
            }
        }

        // Load Quality Level
        if (PlayerPrefs.HasKey("ranukaqality"))
        {
            _qualitylevel = PlayerPrefs.GetInt("ranukaqality");
            if (qualityDropDown != null)
            {
                qualityDropDown.value = _qualitylevel;
            }
            else
            {
                Debug.LogWarning("qualityDropDown is not assigned in the Inspector.");
            }
            QualitySettings.SetQualityLevel(_qualitylevel);
        }

        // Load Full Screen
        if (PlayerPrefs.HasKey("ranukaFullScreen"))
        {
            _isFullScreen = PlayerPrefs.GetInt("ranukaFullScreen") == 1;
            if (fullScreenToggle != null)
            {
                fullScreenToggle.isOn = _isFullScreen;
            }
            else
            {
                Debug.LogWarning("fullScreenToggle is not assigned in the Inspector.");
            }
            Screen.fullScreen = _isFullScreen;
        }
    }



    private void ApplySettings()
    {
        Setvolume(AudioListener.volume);
        setControllerSen(mainControllerSens);
        setBrightness(_brightnesslevel);
        SetQuality(_qualitylevel);
        setFullScreen(_isFullScreen);
    }

    #endregion

}