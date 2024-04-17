using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Networking;
using System;


public class MenuController : MonoBehaviour
{
    [Header("CheckUserProfile Settings")]
    [SerializeField] private GameObject playbutton_dialog_box;
    [SerializeField] private GameObject error_box;

    [Header("Player Profile Settings")]
    /*
    private string baseUrl = "http://20.15.114.131:8080/api";
    private string apiKey = "NjVkNDRmNWQ0ZjZjN2U1YTFhNDVhNDAwOjY1ZDQ0ZjVkNGY2YzdlNWExYTQ1YTNmNg";*/
    private string baseurl_for_check_user = "http://localhost:8080/getStatus/";
    public InputField outputArea;

    [Header("Edit Profile Settings")]
    [SerializeField] private TMP_InputField[] profileFields = new TMP_InputField[7];


    [Header("Volume Settings")]
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private GameObject ConfirmationPrompt = null;
    [SerializeField] private float defaultVolume = 0.5f;

    [Header("Levels to Load")]
    public string _newGameLevel;
    private string leveltoload;
    [SerializeField] private GameObject noSavedGameDialog = null;

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

    [System.Serializable]
    public class ResponseData
    {
        public bool status;
        // You can add other fields from your JSON response here
    }

    private void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropDown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }



        }
        string profileData = PlayerPrefs.GetString("PlayerProfileData", "No profile data found");
        outputArea.text = profileData;
        resolutionDropDown.AddOptions(options);
        resolutionDropDown.value = currentResolutionIndex;
        resolutionDropDown.RefreshShownValue();
    }

    public void setResolution(int resoltionindex)
    {
        Resolution resolution = resolutions[resoltionindex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }



    public void NewGameDIalogYes()
    {
        SceneManager.LoadScene(_newGameLevel);
    }




    public void LoadGameDialogYes()
    {
        if (PlayerPrefs.HasKey("SavedLevel"))
        {
            leveltoload = PlayerPrefs.GetString("Savedlevel");
            SceneManager.LoadScene(leveltoload);
        }
        else
        {
            noSavedGameDialog.SetActive(true);
        }
    }

    public void Quiz()
    {

        Debug.Log(outputArea.text);
        string firstName = null;
        string outputText = outputArea.text;
        string[] lines = outputText.Split('\n');
        foreach (string line in lines)
        {
            if (line.StartsWith("First Name:"))
            {
                // Extract the first name
                firstName = line.Split(':')[1].Trim();
                break; // Exit loop once the first name is found
            }
        }
        UnityWebRequest request = UnityWebRequest.Get(baseurl_for_check_user + firstName);
        StartCoroutine(SendRequest(request));
    }
    IEnumerator SendRequest(UnityWebRequest request)
    {
        string firstName = null;
        string outputText = outputArea.text;
        string[] lines = outputText.Split('\n');
        foreach (string line in lines)
        {
            if (line.StartsWith("First Name:"))
            {
                // Extract the first name
                firstName = line.Split(':')[1].Trim();
                break; // Exit loop once the first name is found
            }
        }
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError ||
            request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {

            string responseText = request.downloadHandler.text;
            Debug.Log("Response: " + responseText);
            ResponseData responseData = JsonUtility.FromJson<ResponseData>(responseText);
            if (responseData != null)
            {
                bool status = responseData.status;
                Debug.Log("Status: " + status);
                if (status)
                {
                    NewGameDIalogYes();
                }
                else
                {
                    playbutton_dialog_box.SetActive(true);
                    
                }
            }
            else
            {
                Debug.LogError("Failed to parse JSON response.");
            }
        }
    }

    public void startQuiz()
    {
        string firstName = null;
        string outputText = outputArea.text;
        string[] lines = outputText.Split('\n');
        foreach (string line in lines)
        {
            if (line.StartsWith("First Name:"))
            {
                // Extract the first name
                firstName = line.Split(':')[1].Trim();
                break; // Exit loop once the first name is found
            }
        }
        string url = "http://localhost:5173/" + firstName;
        Application.OpenURL(url);
    }

    public void Exitbutton()
    {
        Application.Quit();
    }

    public void Setvolume(float volume)
    {
        AudioListener.volume = volume;
        volumeTextValue.text = volume.ToString("0.0");

    }

    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("ranuka_file", AudioListener.volume);
        StartCoroutine(ConfirmationBox());
    }
    public void setControllerSen(float sensitivity)
    {
        mainControllerSens = Mathf.RoundToInt(sensitivity);
        controllerSensitivityTextvalue.text = sensitivity.ToString("0");

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


    public void CheckUserProfileFilled()
    {
        // Split the outputArea text into lines
        string[] lines = outputArea.text.Split('\n');

        // Create a dictionary to store the values of each field
        Dictionary<string, string> profileFields = new Dictionary<string, string>();

        // Populate the dictionary with the field names and values
        foreach (string line in lines)
        {
            // Split each line by ':'
            string[] parts = line.Split(':');

            // Trim leading and trailing whitespace
            string fieldName = parts[0].Trim();
            string fieldValue = parts[1].Trim();

            // Add the field name and value to the dictionary
            profileFields.Add(fieldName, fieldValue);
        }

        // Check if any field value is empty
        bool isProfileFilled = true;
        foreach (string value in profileFields.Values)
        {
            if (string.IsNullOrEmpty(value))
            {
                isProfileFilled = false;
                break;
            }
        }

        if (isProfileFilled)
        {
            Debug.Log("User profile is filled.");
            Quiz();
            //playbutton_dialog_box.SetActive(true);

            // Proceed with further actions if the profile is filled
        }
        else
        {
            Debug.Log("User profile is not filled.");
            // Handle the case where the profile is not filled
            error_box.SetActive(true);

        }
    }
    public void ApplyProfileChanges()
    {
        string newFirstName = profileFields[0].text;
        string newLastName = profileFields[1].text;
        string newNIC = profileFields[2].text;
        string newPhoneNumber = profileFields[3].text;
        string newEmail = profileFields[4].text;




        StartCoroutine(UpdateUserProfile(newFirstName, newLastName, newNIC, newPhoneNumber, newEmail));
    }
    /*public void CheckUserProfile()
    {
        UserProfile profile = JsonUtility.FromJson<UserProfile>(outputArea.text);

        // Check if any field is empty
        List<string> emptyFields = new List<string>();

        if (string.IsNullOrEmpty(profile.firstname))
            emptyFields.Add("firstname");
        if (string.IsNullOrEmpty(profile.lastname))
            emptyFields.Add("lastname");
        if (string.IsNullOrEmpty(profile.nic))
            emptyFields.Add("nic");
        if (string.IsNullOrEmpty(profile.phoneNumber))
            emptyFields.Add("phoneNumber");
        if (string.IsNullOrEmpty(profile.email))
            emptyFields.Add("email");

        if (emptyFields.Count > 0)
        {
            // Generate error message
            string errorMessage = "Error: The following fields are empty - " + string.Join(", ", emptyFields);
            Debug.LogError(errorMessage);
        }
        else
        {
            Debug.Log("All fields in the user profile are filled.");
        }
    }*/

    private IEnumerator UpdateUserProfile(string newFirstName, string newLastName, string newNIC, string newPhoneNumber, string newEmail)

    {
        // URL of the API endpoint
        string url = "http://20.15.114.131:8080/api/user/profile/update";

        // Create a JSON string with the data to be sent

        string json = "{\"firstname\":\"" + newFirstName + "\",\"lastname\":\"" + newLastName + "\",\"nic\":\"" + newNIC + "\",\"phoneNumber\":\"" + newPhoneNumber + "\",\"email\":\"" + newEmail + "\"}";

        Debug.Log(json);

        // Create a UnityWebRequest object
        UnityWebRequest request = UnityWebRequest.Put(url, json);
        string JWTToken = PlayerPrefs.GetString("JWTToken");
        // Set headers
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + JWTToken);

        // Send the request and wait for the response
        yield return request.SendWebRequest();

        // Check for errors
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            Debug.Log("User profile updated successfully!");
            string profileTextString = "First Name: " + newFirstName + "\n" +
                                   "Last Name: " + newLastName + "\n" +
                                   "User Name: " + "oversight_g21" + "\n" +
                                   "NIC: " + newNIC + "\n" +
                                   "Phone Number: " + newPhoneNumber + "\n" +
                                   "Email: " + newEmail;
            outputArea.text = profileTextString;
            Debug.Log(outputArea.text);
        }

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




    [Serializable]
    private class TokenResponse
    {
        public string token;
    }

    [Serializable]
    private class PlayerProfile
    {
        public User user;
    }

    [Serializable]
    private class User
    {
        public string firstname;
        public string lastname;
        public string username;
        public string nic;
        public string phoneNumber;
        public string email;
        public string profilePictureUrl;
    }


}
