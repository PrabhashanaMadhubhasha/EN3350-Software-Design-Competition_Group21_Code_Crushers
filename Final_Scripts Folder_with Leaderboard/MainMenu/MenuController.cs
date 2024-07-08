using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Networking;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SocialPlatforms.Impl;

public class MenuController : MonoBehaviour
{
    #region || --------------Variable assigning Section -----------------------||
    [Header("CheckUserProfile Settings")]
    [SerializeField] private GameObject playbutton_dialog_box;
    [SerializeField] private GameObject error_box;

    [Header("Player Profile Settings")]
    /*
    private string baseUrl = "http://20.15.114.131:8080/api";
    private string apiKey = "NjVkNDRmNWQ0ZjZjN2U1YTFhNDVhNDAwOjY1ZDQ0ZjVkNGY2YzdlNWExYTQ1YTNmNg";*/
    private string baseurl_for_check_user = "http://localhost:8080/getStatus";

    public InputField outputArea;

    [Header("Edit Profile Settings")]
    [SerializeField] private TMP_InputField[] profileFields = new TMP_InputField[5];


    [Header("Volume Settings")]
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private GameObject ConfirmationPrompt = null;
    [SerializeField] private float defaultVolume = 0.5f;

    [Header("Levels to Load")]
    public static string _newGameLevel = "Level1"; // Updated to static
    public static string _leaderBoard = "LeaderBoard";
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
    #endregion

    public static MenuController Instance { get; set; }


    [System.Serializable]


    
    public class ResponseData
    {
        public bool status;
        // You can add other fields from your JSON response here
    }
    //public static MenuController Instance { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //Destroy(gameObject);
        }
    }

    public bool isSavingToJson;
    public Button LoadGameBTN;
    string jsonPathProject;

    public bool isLoading;

    // Json External/Real Save Path
    string jsonPathPersistant;

    //Bibnary save path
    string binaryPath;


    string fileName = "SaveGame";

    bool isStartedQuiz;

    public GameObject mainMenu;

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

        gameplayApply();
        VolumeApply();
        GraphicsApply();

        
        jsonPathProject = Application.dataPath + Path.AltDirectorySeparatorChar;
        jsonPathPersistant = Application.persistentDataPath + Path.AltDirectorySeparatorChar;
        binaryPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar;
    }

    /*
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
    }*/

    public void Quiz()
    {
        NewGameDIalogYes();
/*        Debug.Log(outputArea.text);
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
        UnityWebRequest request = UnityWebRequest.Get(baseurl_for_check_user);
        StartCoroutine(SendRequest(request));*/
    }

    public void StartQuiz()
    {
        string url = "http://localhost:5173/" + 0;
        Application.OpenURL(url);
        playbutton_dialog_box.SetActive(false);
        mainMenu.SetActive(true);  
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


    public void NewGameDIalogYes()
    {
        SceneManager.LoadScene(_newGameLevel);
    }

    public void OpenLeaderBoard()
    {
        SceneManager.LoadScene(_leaderBoard);
    }



    #region ||--------------Option Section------------------||
    public void Setvolume(float volume)
    {
        AudioListener.volume=volume;
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
    public void setResolution(int resoltionindex)
    {
        Resolution resolution = resolutions[resoltionindex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
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
        StartCoroutine (ConfirmationBox());
    }

    public void ResetButton(string Menutype)
    {
        if (Menutype == "Audio")
        {
            AudioListener.volume = defaultVolume;
            volumeSlider.value= defaultVolume;
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

            Resolution currentResolution =Screen.currentResolution;
            Screen.SetResolution(currentResolution.width,currentResolution.height,Screen.fullScreen);
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
    #endregion

    #region ||--------------Update User Profile----------------------||
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
            playbutton_dialog_box.SetActive(true);


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
        request.SetRequestHeader("Authorization", "Bearer "+JWTToken);

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
            PlayerPrefs.SetString("PlayerProfileData", profileTextString);

            PlayerPrefs.SetString("PlayerFirstname", newFirstName);
            //PlayerPrefs.SetString("PlayerMarks", "0");

            Debug.Log(outputArea.text);
        }
        
    }
    #endregion

    #region ||---------------Graphics Section------------------||
    public void setBrightness(float brightness)
    {
        _brightnesslevel = brightness;
        brightnessTextValue.text=brightness.ToString("0.0");
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

        PlayerPrefs.SetInt("ranukaFullScreen",(_isFullScreen ? 1:0));
        Screen.fullScreen = _isFullScreen;

        StartCoroutine(ConfirmationBox());

    }
    #endregion



    #region || -------------- General Section -----------------------------||


    #region || ------------------ Saving ---------------- ||

    
    public void SaveGame(int slotNumber)
    {
        AllGameData data = new AllGameData();
        data.playerData = GetPlayerData(slotNumber);
        //data.environmentData = GetEnvironmentData();
        SavingTypeSwitch(data, slotNumber);
    }

    private EnvironmentData GetEnvironmentData()
    {
        List<string> itemsPickedup = InventorySystem.Instance.itemsPickedup;

        return new EnvironmentData(itemsPickedup);
    }

    private PlayerData GetPlayerData(int slotNumber)
    {
        float[] playerStats = new float[3];
        playerStats[0] = PlayerState.Instance.currentHealth;
        playerStats[1] = PlayerState.Instance.currentCalories;
        playerStats[2] = PlayerState.Instance.currentHydrationPercent;

        float[] playerPosAndRot = new float[6];
        playerPosAndRot[0] = PlayerState.Instance.playerBody.transform.position.x;
        playerPosAndRot[1] = PlayerState.Instance.playerBody.transform.position.y;
        playerPosAndRot[2] = PlayerState.Instance.playerBody.transform.position.z;

        playerPosAndRot[3] = PlayerState.Instance.playerBody.transform.rotation.x;
        playerPosAndRot[4] = PlayerState.Instance.playerBody.transform.rotation.y;
        playerPosAndRot[5] = PlayerState.Instance.playerBody.transform.rotation.z;

        string[] inventory = InventorySystem.Instance.itemList.ToArray();

        string[] quickSlots = GetQuickSlotContent();

        int[] date = new int[4];
        date[0] = DayNightSystem.Instance.currentHour;
        date[1] = TimeManager.Instance.dayInGame;
        date[2] = TimeManager.Instance.monthInGame;
        date[3] = TimeManager.Instance.yearInGame;

        int[] assetsAmount = new int[2];
        assetsAmount[0] = AssetsManager.Instance.score;
        assetsAmount[1] = AssetsManager.Instance.currentCoins;

        SaveScore(slotNumber, assetsAmount[0], assetsAmount[1]);

        return new PlayerData(playerStats, playerPosAndRot, inventory, quickSlots, date, assetsAmount);
    }

    // Save Data Relted to Score Calculation
    private void SaveScore(int slotNumber, int score, int rewardLevel)
    {
        PlayerPrefs.SetString("slot_" + slotNumber.ToString() + "_playerScore", score.ToString());
        PlayerPrefs.SetString("slot_" + slotNumber.ToString() + "_rewardLevel", AssetsManager.Instance.rewardLevel.ToString());
        PlayerPrefs.SetString("slot_" + slotNumber.ToString() + "_rewardCoins", "0");
    }

    private string[] GetQuickSlotContent()
    {
        List<string> temp = new List<string>();

        foreach (GameObject slot in EquipSystem.Instance.quickSlotsList)
        {
            if (slot.transform.childCount != 0)
            {
                string name = slot.transform.GetChild(0).name;
                string str2 = "(Clone)";
                string cleanName = name.Replace(str2, "");
                temp.Add(cleanName);
            }
        }

        return temp.ToArray();
    }

    public void SavingTypeSwitch(AllGameData gameData,int slotNumber)
    {
        if (isSavingToJson)
        {
            //save game data to json file
            SaveGameDataToJsonFile(gameData,slotNumber);

        }
        else
        {
            SaveGameDataToBinaryFile(gameData,slotNumber);
        }
    }
    #endregion

    #region || -------------- Loading ------------------||
    public AllGameData LoadingTypeSwitch(int slotNumber)
    {
        if (isSavingToJson)
        {
            AllGameData gameData = LoadGamedataFromJsonFile(slotNumber);
            return gameData;
        }
        else
        {
            AllGameData gameData = LoadGamedataFromBinaryFile(slotNumber);
            return gameData;
        }

    }
    public void LoadGame(int slotNumber)
    {
        SetPlayerData(slotNumber, LoadingTypeSwitch(slotNumber).playerData);

        //setEnvironmentData(LoadingTypeSwitch(slotNumber).environmentData);
        isLoading = false;

    }

    private void setEnvironmentData(EnvironmentData environmentData)
    {
        throw new NotImplementedException();
    }

    private void SetPlayerData(int slotNumber,PlayerData playerData)
    {
        //Select Player Stats
        Debug.Log("playerData.playerStats[0]    " + playerData.playerStats[0]);
        PlayerState.Instance.currentHealth = playerData.playerStats[0];
        PlayerState.Instance.healthSystem.health = playerData.playerStats[0];
        PlayerState.Instance.currentCalories = playerData.playerStats[1];
        PlayerState.Instance.currentHydrationPercent = playerData.playerStats[2];

        //Setting Player position


        Vector3 loadedPosition;
        loadedPosition.x = playerData.playerPositionAndRotation[0];
        loadedPosition.y = playerData.playerPositionAndRotation[1];
        loadedPosition.z = playerData.playerPositionAndRotation[2];

        PlayerState.Instance.playerBody.transform.position = loadedPosition;

        // setting player Rotation

        Vector3 loadedRotation;
        loadedRotation.x = playerData.playerPositionAndRotation[3];
        loadedRotation.y = playerData.playerPositionAndRotation[4];
        loadedRotation.z = playerData.playerPositionAndRotation[5];

        PlayerState.Instance.playerBody.transform.rotation = Quaternion.Euler(loadedRotation);

        PlayerState.Instance.SetLoadPositionRotation(loadedPosition, loadedRotation);

        foreach(string item in playerData.inventoryContent)
        {
            InventorySystem.Instance.AddToInventory(item);
        }

        foreach (string item in playerData.quickSlotsContent)
        {
            GameObject availableSlot = EquipSystem.Instance.FindNextEmptySlot();
            var itemToAdd = Instantiate(Resources.Load<GameObject>(item));

            itemToAdd.transform.SetParent(availableSlot.transform, false);
        }
        DayNightSystem.Instance.currentHour = playerData.date[0];
        TimeManager.Instance.dayInGame = playerData.date[1];
        TimeManager.Instance.monthInGame = playerData.date[2];
        TimeManager.Instance.yearInGame = playerData.date[3];

        // Load Data related to score
        int rewardCoins = int.Parse(PlayerPrefs.GetString("slot_" + slotNumber.ToString() + "_rewardCoins", "No rewardCoins found"));
        AssetsManager.Instance.rewardLevel = int.Parse(PlayerPrefs.GetString("slot_" + slotNumber.ToString() + "_rewardLevel", "No rewardLevel found"));

        AssetsManager.Instance.AddRewardCoins(slotNumber, rewardCoins);
        AssetsManager.Instance.score = playerData.assetsAmount[0];

        AssetsManager.Instance.currentCoins = playerData.assetsAmount[1];
    }

    public void StartedLoadedGame(int slotNumber)
    {
        isLoading = true;

        Debug.Log("hi I came!!");
        SceneManager.LoadScene("Level1");

        StartCoroutine(DelayedLoading(slotNumber));
    }

    private IEnumerator DelayedLoading(int slotNumber)
    {
        yield return new WaitForSeconds(1f);
        LoadGame(slotNumber);

    }

    #endregion

    #endregion

    #region ||------------------- Binary Section -----------------------------

    public void SaveGameDataToBinaryFile(AllGameData gameData,int slotNumber)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        FileStream stream = new FileStream(binaryPath+fileName+slotNumber+".bin", FileMode.Create);

        formatter.Serialize(stream, gameData);
        stream.Close();

        print("Data save to" + binaryPath + fileName + slotNumber + ".bin");

    }
    public AllGameData LoadGamedataFromBinaryFile(int slotNumber)
    {

        if (File.Exists(binaryPath + fileName + slotNumber + ".bin"))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(binaryPath + fileName + slotNumber + ".bin", FileMode.Open);

            AllGameData data = formatter.Deserialize(stream) as AllGameData;
            stream.Close();

            print("Data loaded from" + binaryPath + fileName + slotNumber + ".bin");


            return data;
        }
        else
        {
            return null;
        }

    }
    #endregion

    

    #region ||-----------Json Section-------------||
    public void SaveGameDataToJsonFile(AllGameData gameData,int slotNumber)
    {
        string json = JsonUtility.ToJson(gameData);

        string encrypted = EncryptionDecryption(json);
        using (StreamWriter writer = new StreamWriter(jsonPathProject+fileName+slotNumber+".json"))
        {
            writer.Write(encrypted);
            print("Save Game to Json file at :" + jsonPathProject+fileName+slotNumber+".json");
        };

    }
    public AllGameData LoadGamedataFromJsonFile(int slotNumber)
    {
        using (StreamReader reader = new StreamReader(jsonPathProject + fileName + slotNumber + ".json"))
        {
            string json = reader.ReadToEnd();

            string decrypted = EncryptionDecryption(json);
            AllGameData data = JsonUtility.FromJson<AllGameData>(decrypted);
            return data;
        }


    }
#endregion

    #region ||-------------Encryption--------------||
    public string EncryptionDecryption(string data)
    {
        string keyword = "1234567";
        string result = "";

        for (int i = 0; i < data.Length; i++)
        {
            result += (char)(data[i] ^ keyword[i % keyword.Length]);
        }

        return result;
    }

    #endregion

    #region ||------------ Utility --------------||

    public bool DoesFileExists(int slotNumber)
    {
        if (isSavingToJson)
        {
            if (System.IO.File.Exists(jsonPathProject+fileName+slotNumber+".json"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (System.IO.File.Exists(binaryPath+ fileName + slotNumber + ".bin"))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }


    public bool IsSlotEmpty(int slotNumber)
    {
        if (DoesFileExists(slotNumber))
        {
            return false;
        }
        else
        {
            return true;
        }
    }


    public void DeselectButton()
    {
        GameObject myEventSystem = GameObject.Find("EventSystem");
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
    }
    #endregion










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
