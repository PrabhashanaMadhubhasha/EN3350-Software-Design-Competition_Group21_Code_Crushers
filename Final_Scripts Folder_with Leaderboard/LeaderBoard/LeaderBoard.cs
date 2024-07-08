using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoard : MonoBehaviour
{
    public int lengthOfName;
    private Transform entryContainer;
    private Transform entryTemplate_1;
    private Transform entryTemplate_2;
    private List<Transform> entryTemplates = new List<Transform>();
    private List<Transform> highscoreEntryTransformList;

    private Transform searchArea;

    public GameObject congratulations;

    private void Awake()
    {
        entryContainer = transform.GetChild(0).GetChild(0).Find("HighscoreEntryContainer");
        entryTemplate_1 = entryContainer.Find("HighscoreEntryTemplate1");
        entryTemplate_2 = entryContainer.Find("HighscoreEntryTemplate2");

        entryTemplates.Add(entryTemplate_1);
        entryTemplates.Add(entryTemplate_2);

        entryTemplate_1.gameObject.SetActive(false);
        entryTemplate_2.gameObject.SetActive(false);

        searchArea = transform.Find("Search Box").Find("Search Area");

        string jsonString = PlayerPrefs.GetString("HighscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        highscoreEntryTransformList = new List<Transform>();
        for (int highscoreEntryIndex = 0; highscoreEntryIndex < highscores.highscoreEntryList.Count; highscoreEntryIndex++)
        {
            int entryTemplateIndex = highscoreEntryIndex % 2;
            CreateHighscoreEntryTransform(highscores.highscoreEntryList[highscoreEntryIndex], entryContainer, entryTemplates[entryTemplateIndex], highscoreEntryTransformList);
        }
    }

    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, Transform entryTemplate, List<Transform> transformList)
    {
        Transform entryTransform = Instantiate(entryTemplate, container);

        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string rankString;
        switch (rank)
        {
            default:
                rankString = rank + "TH"; break;
            case 1:
                rankString = "1ST"; break;
            case 2:
                rankString = "2ND"; break;
            case 3:
                rankString = "3RD"; break;
        }

        entryTransform.Find("posText").GetComponent<Text>().text = rankString;

        int score = highscoreEntry.score;
        entryTransform.Find("scoreText").GetComponent<Text>().text = score.ToString();

        string name = highscoreEntry.name;
        entryTransform.Find("nameText").GetComponent<Text>().text = name;

        switch (rank)
        {
            default:
                entryTransform.Find("trophyGold").gameObject.SetActive(false);
                entryTransform.Find("trophySilver").gameObject.SetActive(false);
                entryTransform.Find("trophyBronze").gameObject.SetActive(false);

                string playerFirstname = PlayerPrefs.GetString("PlayerFirstname", "No playerFirstname found");
                if(name == playerFirstname)
                {
                    entryTransform.Find("posText").GetComponent<Text>().color = Color.yellow;
                    entryTransform.Find("scoreText").GetComponent<Text>().color = Color.yellow;
                    entryTransform.Find("nameText").GetComponent<Text>().color = Color.yellow;
                }
                break;
            case 1:
                entryTransform.Find("trophyGold").gameObject.SetActive(true);
                entryTransform.Find("trophySilver").gameObject.SetActive(false);
                entryTransform.Find("trophyBronze").gameObject.SetActive(false);

                entryTransform.Find("posText").GetComponent<Text>().color = Color.green;
                entryTransform.Find("scoreText").GetComponent<Text>().color = Color.green;
                entryTransform.Find("nameText").GetComponent<Text>().color = Color.green;
                break;
            case 2:
                entryTransform.Find("trophyGold").gameObject.SetActive(false);
                entryTransform.Find("trophySilver").gameObject.SetActive(true);
                entryTransform.Find("trophyBronze").gameObject.SetActive(false);
                break;
            case 3:
                entryTransform.Find("trophyGold").gameObject.SetActive(false);
                entryTransform.Find("trophySilver").gameObject.SetActive(false);
                entryTransform.Find("trophyBronze").gameObject.SetActive(true);
                break;
        }

        transformList.Add(entryTransform);
    }

    public void AddHighscoreEntry(string name, int score)
    {
        // Create HighscoreEntry
        HighscoreEntry highscoreEntry = new HighscoreEntry { score = score, name = name};

        // Load saved Highscores
        string jsonString = PlayerPrefs.GetString("HighscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        int index = BinarySearch(highscores.highscoreEntryList, highscoreEntry.score);
        // The insertion operation has a time complexity of O(n) in the worst case due to the need to shift elements
        highscores.highscoreEntryList.Insert(index, highscoreEntry);

        // Save updated Highscores
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("HighscoreTable", json);
        PlayerPrefs.Save();
    }

    private void SortHighscoreEntryList(Highscores highscores)
    {
        // Sort entry list by score
        for (int i = 0; i < highscores.highscoreEntryList.Count; i++)
        {
            for (int j = i + 1; j < highscores.highscoreEntryList.Count; j++)
            {
                if (highscores.highscoreEntryList[j].score >= highscores.highscoreEntryList[i].score)
                {
                    // Swap  
                    HighscoreEntry tmp = highscores.highscoreEntryList[i];
                    highscores.highscoreEntryList[i] = highscores.highscoreEntryList[j];
                    highscores.highscoreEntryList[j] = tmp;
                }
            }
        }
    }

    // The binary search reduces the time complexity of finding the insertion point to O(log n)
    private static int BinarySearch(List<HighscoreEntry> highscoreEntryList, int score)
    {
        int low = 0;
        int high = highscoreEntryList.Count - 1;

        while (low <= high)
        {
            int mid = (low + high) / 2;

            if (highscoreEntryList[mid].score == score)
            {
                return mid + 1; // Insert after the equal value
            }
            else if (highscoreEntryList[mid].score > score)
            {
                low = mid + 1;
            }
            else
            {
                high = mid - 1;
            }
        }

        return low; // This will be the correct insertion index
    }

    public void CreateEmptyHighscoreTable()
    {
        // Add new entry to Highscores
        Highscores highscores = new Highscores();
        highscores.highscoreEntryList = new List<HighscoreEntry>();

        // Save updated Highscores
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("HighscoreTable", json);
        PlayerPrefs.Save();
    }

    public void SearchByName()
    {
        string searchedName = searchArea.GetComponent<TMP_InputField>().text;
        lengthOfName = searchedName.Length;
        RefreshLeaderBoard();
        print( searchedName + "searched name");

        if (lengthOfName > 0 )
        {
            foreach (Transform entryTransform in highscoreEntryTransformList)
            {
                if (entryTransform.Find("nameText").GetComponent<Text>().text.Length >= lengthOfName)
                {
                    print("Not null  " + (entryTransform.Find("nameText").GetComponent<Text>().text.Substring(0, lengthOfName)) + searchedName+"Start");
                    if (entryTransform.Find("nameText").GetComponent<Text>().text.Substring(0, lengthOfName) == searchedName)
                    {
                        print("True");
                        entryTransform.gameObject.SetActive(true);
                    }
                    else
                    {
                        print("False");
                        entryTransform.gameObject.SetActive(false);
                    }
                }
                else
                {
                    print("Nulllll");
                    entryTransform.gameObject.SetActive(false);
                }
            }
        }
    }

    public void RefreshLeaderBoard()
    {
        foreach (Transform entryTransform in highscoreEntryTransformList)
        {
            entryTransform.gameObject.SetActive(true);
        }
    }

    private class Highscores
    {
        public List<HighscoreEntry> highscoreEntryList;
    }

    // Represent a single High Score entry;
    [Serializable]
    private class HighscoreEntry
    {
        public int score;
        public string name; 
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayerListAPIManager.Instance.AppearCongratulations(congratulations));
    }

}
