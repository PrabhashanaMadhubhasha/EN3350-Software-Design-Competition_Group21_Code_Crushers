using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using System;

public class APIManager : MonoBehaviour
{
    private string baseUrl = "http://20.15.114.131:8080/api";
    private string apiKey = "NjVkNDRmNWQ0ZjZjN2U1YTFhNDVhNDAwOjY1ZDQ0ZjVkNGY2YzdlNWExYTQ1YTNmNg";
    private string JWTToken;
    InputField outputArea;

    public void Start()
    {
        outputArea = GameObject.Find("OutputArea").GetComponent<InputField>();
        GameObject.Find("GetButton").GetComponent<Button>().onClick.AddListener(OutputProfile);
    }

    private void OutputProfile()
    {
        StartCoroutine(AuthenticatePlayerCoroutine());
    }

    private IEnumerator AuthenticatePlayerCoroutine()
    {
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

            // Send request
            yield return request.SendWebRequest();

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
                                       "Email: " + profile.user.email + "\n" +
                                       "Profile Picture URL: " + profile.user.profilePictureUrl;
            outputArea.text = profileTextString;
        }
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