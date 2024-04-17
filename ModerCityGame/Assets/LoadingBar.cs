using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadingBar : MonoBehaviour
{
    [Header("Player Profile Settings")]
    private string baseUrl = "http://20.15.114.131:8080/api";
    private string apiKey = "NjVkNDRmNWQ0ZjZjN2U1YTFhNDVhNDAwOjY1ZDQ0ZjVkNGY2YzdlNWExYTQ1YTNmNg";
    private string JWTToken;


    [SerializeField] private GameObject loading_Bar_Holder;
    [SerializeField] private Image loading_Bar_progress;

    private float progress_value = 0f;
    public float progress_multiplier_1 = 0.005f;
    public float progress_multiplier_2 = 0.0007f;

    private void Start()
    {
        // Show the loading bar holder
        loading_Bar_Holder.SetActive(true);
        // Initialize the progress value
        progress_value = 0f;
        OutputProfile();
    }

    void Update()
    {
        // Update the loading screen until progress reaches 1.0
        if (progress_value < 1f)
        {
            UpdateLoadingScreen();
        }
        else
        {
            // Load the main menu scene when progress reaches 1.0
            LoadMainMenu();
        }
    }
    public void OutputProfile()

    {   /*
        outputArea = GameObject.Find("UserProfile").GetComponent<InputField>();
        if (outputArea == null)
        {
            Debug.LogError("Output area is not assigned properly!");
        }*/

        StartCoroutine(AuthenticatePlayerCoroutine());
    }

    private IEnumerator AuthenticatePlayerCoroutine()

    {
        Application.runInBackground = true;
        // Create JSON request body
        string requestBody = "{\"apiKey\": \"" + apiKey + "\"}";

        // Create request
        using (UnityWebRequest request = UnityWebRequest.PostWwwForm(baseUrl + "/login", "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(requestBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            // Send request
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Authentication failed: " + request.error);
                yield break;
            }

            // Extract JWT token from response
            string responseText = request.downloadHandler.text;
            JWTToken = JsonUtility.FromJson<TokenResponse>(responseText).token;

            FetchPlayerProfile();
            Application.runInBackground = false;
        }
    }

    // Now that we have the token, we can make other API calls
    private void FetchPlayerProfile()
    {
        StartCoroutine(FetchPlayerProfileCoroutine());
    }

    private IEnumerator FetchPlayerProfileCoroutine()
    {
        // Create request
        using (UnityWebRequest request = UnityWebRequest.Get(baseUrl + "/user/profile/view"))
        {
            request.SetRequestHeader("Authorization", "Bearer " + JWTToken);
            Debug.Log(request);

            // Send request
            yield return request.SendWebRequest();
            Debug.Log(request.result);

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to fetch player profile: " + request.error);
                yield break;
            }

            // Extract player profile from response
            string profileJson = request.downloadHandler.text;
            PlayerProfile profile = JsonUtility.FromJson<PlayerProfile>(profileJson);
            string profileTextString = "First Name: " + profile.user.firstname + "\n" +
                                       "Last Name: " + profile.user.lastname + "\n" +
                                       "Username: " + profile.user.username + "\n" +
                                       "NIC: " + profile.user.nic + "\n" +
                                       "Phone Number: " + profile.user.phoneNumber + "\n" +
                                       "Email: " + profile.user.email;
            string profileJson_1 = profileTextString;
            PlayerPrefs.SetString("PlayerProfileData", profileJson_1);
            PlayerPrefs.SetString("JWTToken", JWTToken);
           
            


        }
    }

    void UpdateLoadingScreen()
    {
        // Increase the progress value
        progress_value += progress_multiplier_1 * progress_multiplier_2;
        // Update the loading bar
        loading_Bar_progress.fillAmount = progress_value;
    }

    void LoadMainMenu()
    {
        // Load the main menu scene
        SceneManager.LoadScene("MainMenu");
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
    }
}
