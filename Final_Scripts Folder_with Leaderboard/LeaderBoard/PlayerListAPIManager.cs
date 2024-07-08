using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.TextCore.Text;

public class PlayerListAPIManager : MonoBehaviour
{
    public static PlayerListAPIManager Instance { get; set; }

    [Header("API Access")]
    private string baseUrl = "http://20.15.114.131:8080/api";
    private string apiKey = "NjVkNDRmNWQ0ZjZjN2U1YTFhNDVhNDAwOjY1ZDQ0ZjVkNGY2YzdlNWExYTQ1YTNmNg";
    private string jwtToken;

    [Header("API Responses")]
    public string getPlayerListRes;

    public List<string> playerFirstNames;

    public string playerMarks;
    public int maxScoreOutOfAll = 0;

    public LeaderBoard leaderBoard;
    private AudioSource audioSource;

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

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(AuthenticatePlayerCoroutine());
    }

    // Get the JWTToken for Authentication
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
            jwtToken = JsonUtility.FromJson<TokenResponse>(responseText).token;

            StartCoroutine(GetPlayerList());
        }
    }

    // GetPlayerList
    private IEnumerator GetPlayerList()
    {
        yield return new WaitForSeconds(0.5f);
        using (UnityWebRequest request = UnityWebRequest.Get(baseUrl + "/user/profile/list"))
        {
            request.SetRequestHeader("Authorization", "Bearer " + jwtToken);

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to fetch player list: " + request.error);
                yield break;
            }

            getPlayerListRes = request.downloadHandler.text;

            PlayerListResponse playerListResponse = JsonUtility.FromJson<PlayerListResponse>(getPlayerListRes);

            SetCurrentLeaderboard(playerListResponse);
        }
    }

    // Assign random scores to all the players
    private void SetCurrentLeaderboard(PlayerListResponse playerListResponse)
    {
        playerFirstNames = new List<string>();
        System.Random random = new System.Random();

        string playerFirstname = PlayerPrefs.GetString("PlayerFirstname", "No playerFirstname found");
        playerMarks = PlayerPrefs.GetString("PlayerMarks", "No playerMarks found");
        leaderBoard.CreateEmptyHighscoreTable();

        foreach (Player player in playerListResponse.userViews)
        {
            int marks;
            if (player.firstname == playerFirstname)
            {
                marks = Mathf.RoundToInt(float.Parse(playerMarks));
            }
            else
            {
                marks = random.Next(0, 1001);
            }

            if(marks > maxScoreOutOfAll)
            {
                maxScoreOutOfAll = marks;   
            }

            playerFirstNames.Add(player.firstname);

            leaderBoard.AddHighscoreEntry(player.firstname, marks);
        }
    }

    // Appear congratulations if the player is #1 in rank
    public IEnumerator AppearCongratulations(GameObject congratulations)
    {
        yield return new WaitForSeconds(3f);

        try
        {
            int playerMarksInt = Mathf.RoundToInt(float.Parse(playerMarks));
            Debug.Log(playerMarksInt);
            if (playerMarksInt == maxScoreOutOfAll)
            {
                StartCoroutine(ActivateGameObjectForADuaration(6f, congratulations));
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to convert player marks to an integer. Invalid format: " + ex.Message);
        }
    }

    // Coroutine to activate the GameObject for the specified duration
    private IEnumerator ActivateGameObjectForADuaration(float seconds, GameObject targetGameObject)
    {
        audioSource.Play();
        targetGameObject.SetActive(true);
        yield return new WaitForSeconds(seconds);
        targetGameObject.SetActive(false);
        audioSource.Stop();
    }

    [Serializable]
    private class TokenResponse
    {
        public string token;
    }

    [Serializable]
    private class Player
    {
        public string firstname;
        public string lastname;
        public string username;
        public string nic;
        public string phoneNumber;
        public string email;
    }

    [Serializable]
    private class PlayerListResponse
    {
        public List<Player> userViews;
    }

}
