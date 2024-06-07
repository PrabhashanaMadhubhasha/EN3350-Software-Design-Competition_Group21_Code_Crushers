using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionSystem : MonoBehaviour
{
    public static MissionSystem Instance { get; set; }

    public GameObject EnemiesObjects;
    public List<GameObject> enemyList = new List<GameObject>();

    public GameObject constructionHolderArea1;
    public int missionObjectIndex;
    public List<string> specialAreaNames = new List<string> { "FoundationArea1", "FoundationArea2", "FoundationArea3", "FoundationArea4" };
    public List<string> specialObjectNames = new List<string> { "Main_LightHouse" };

    [Header("MissionObject: 1")]
    public List<string> enemyNamesDiedInObject1;
    private int enemyNumber;
    public int noOfEnemiesInObject1 = 4;

    [Header("MissionObject: 3")]
    public Dictionary<string, int> gameObjectNameCount;
    public List<string> noParentNames;
    public GameObject[] allGameObjects;
    public List<GameObject> parentGameObjects;
    public List<string> uniqueGameObjectNames;

    public string itemToPlaced = null;
    public bool canPlaceSpecialItem = false;
    public string itemPlaced;

    int currentIndexOfItem = 0;
    int currentCountOfItem = 0;

    public bool active = true;

    [Header("MissionObject: 5")]
    public int noOfGuardRooms;

    [Header("MissionObject: 7")]
    public List<string> enemyNamesDiedInObject7;
    public int noOfEnemiesInObject7 = 2;

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
        missionObjectIndex = 1;
        StartCoroutine(MissionObjectMenuController.Instance.ActivateMissionsFromMissionIndex(missionObjectIndex - 1));
        ConstructionManager.Instance.SpecialAreaActivation(specialAreaNames[0]);
        StartCoroutine(GetGhostGameObject());
    }

    private void Update()
    {
        switch (missionObjectIndex)
        {
            case 1:
                MissionObject1_Execution();
                break;
            case 2:
                MissionObject2_Execution();
                break;
            case 3:
                MissionObject3_Execution();
                break;
            case 4:
                MissionObject4_Execution();
                break;
            case 5:
                MissionObject5_Execution();
                break;
            case 6:
                MissionObject6_Execution();
                break;
            case 7:
                MissionObject7_Execution();
                break;
            case 8:
                break;
            default:
                break;
        }

    }

    // Total enemies is 60 and according to the quiz marks enemies will be reduce (ex: quiz marks = 2, in mission 1 enemies 20 - 2 = 18,   and in mission 7 enemies 40 - 2 = 38)
    public void ActiveEnemiesUponQuestionnaireMarks(int questionnaireMarks)
    {
        foreach (Transform child in EnemiesObjects.transform)
        {
            if (child.CompareTag("Enemy"))
            {
                enemyList.Add(child.gameObject);
            }
        }

        for (int i = noOfEnemiesInObject1 - questionnaireMarks; i < noOfEnemiesInObject1; i++)
            {
            if (enemyList[i] != null)
            {
                enemyList[i].SetActive(false);
            }
        }
        
        for (int i = noOfEnemiesInObject7 + noOfEnemiesInObject1 - questionnaireMarks; i < noOfEnemiesInObject7 + noOfEnemiesInObject1; i++)
        {
            if (enemyList[i] != null)
            {
                enemyList[i].SetActive(false);
            }
        }

        noOfEnemiesInObject1 -= questionnaireMarks;
        noOfEnemiesInObject7 -= questionnaireMarks;
    }

    //---MissionObject1---//
    public void MissionObject1_Execution() // Kill enemis
    {
        if (enemyNamesDiedInObject1.Count == noOfEnemiesInObject1)
        {
            missionObjectIndex = 2;
            StartCoroutine(MissionObjectMenuController.Instance.ActivateMissionsFromMissionIndex(missionObjectIndex - 1));
        }
    }

    //Get the enemy index at end of the name
    public void ExtractEnemyIndex(string enemyName)
    {
        // Use a regular expression to extract the number from the string
        Match match = Regex.Match(enemyName, @"\d+");
        if (match.Success && int.TryParse(match.Value, out enemyNumber))
        {
            if (enemyNumber <= noOfEnemiesInObject1)
            {
                enemyNamesDiedInObject1.Add(enemyName);
            }
            else if (enemyNumber > noOfEnemiesInObject1 && enemyNumber <= noOfEnemiesInObject7 + noOfEnemiesInObject1)
            {
                enemyNamesDiedInObject7.Add(enemyName);
            }
        }
        else
        {
            Debug.LogWarning("Failed to extract enemy number from string: " + enemyName);
        }

    }


    //---MissionObject2---//
    public void MissionObject2_Execution() // Craft an Axe
    {
        if (CraftingSystem.Instance.isAxeInInventory)
        {
            if (missionObjectIndex == 2)
            {
                missionObjectIndex = 3;
                StartCoroutine(MissionObjectMenuController.Instance.ActivateMissionsFromMissionIndex(missionObjectIndex - 1));
            }
        }
    }


    //---MissionObject3---//
    public void MissionObject3_Execution() // Building the Harbour
    {
        if (uniqueGameObjectNames.Count > 0)
        {
            if (active)
            {
                StartCoroutine(ActivteCurrentGhostItems(uniqueGameObjectNames[currentIndexOfItem]));
                canPlaceSpecialItem = true;
                active = false;
            }

            if (itemPlaced == uniqueGameObjectNames[currentIndexOfItem])
            {
                if(itemPlaced == specialObjectNames[1])
                {
                    ConstructionManager.Instance.SpecialAreaActivation(specialAreaNames[1]);
                    missionObjectIndex = 4;
                    StartCoroutine(MissionObjectMenuController.Instance.ActivateMissionsFromMissionIndex(missionObjectIndex - 1));
                }
                if (currentCountOfItem < gameObjectNameCount[itemPlaced] - 1)
                {
                    itemPlaced = null;
                    currentCountOfItem++;
                }
                else
                {

                    currentIndexOfItem++;
                    if (currentIndexOfItem < uniqueGameObjectNames.Count)
                    {
                        Debug.Log(uniqueGameObjectNames[currentIndexOfItem]);
                        StartCoroutine(ActivteCurrentGhostItems(uniqueGameObjectNames[currentIndexOfItem]));
                    }
                    else
                    {
                        canPlaceSpecialItem = false;

                    }
                    currentCountOfItem = 0;
                }
                itemPlaced = null;
            }
            else
            {
                itemPlaced = null;
            }
        }
    }

    // Activate Ghost items of the Harbour
    public IEnumerator ActivteCurrentGhostItems(string itemName)
    {
        yield return new WaitForSeconds(0.2f);

        GetParentGameObjectInScene();
        itemToPlaced = itemName;

        if (parentGameObjects != null)
        {
            foreach (GameObject obj in parentGameObjects)
            {
                //Debug.Log("obj.name ===" + obj.name);
                if (obj.name != null)
                {
                    string initialItemName = ConstructionManager.Instance.GetInitialNameOfObject(obj.name);
                    if (initialItemName == itemName)
                    {
                        obj.SetActive(true);
                    }
                }

            }
        }

    }

    public IEnumerator GetGhostGameObject()
    {
        yield return new WaitForSeconds(6f);
        // Flag to indicate whether to start counting GameObject names
        bool startCounting = false;

        // Dictionary to store unique GameObject names and their count
        gameObjectNameCount = new Dictionary<string, int>();

        // Get all GameObjects in the scene

        GetParentGameObjectInScene();

        // Loop through each GameObject
        foreach (GameObject obj in parentGameObjects)
        {
            // If we've started counting, proceed
            if (startCounting)
            {
                // Get the name of the GameObject
                string gameObjectName = ConstructionManager.Instance.GetInitialNameOfObject(obj.name);

                // Check if the GameObject name is already in the dictionary
                if (gameObjectNameCount.ContainsKey(gameObjectName))
                {
                    // Increment the count of the existing GameObject name
                    gameObjectNameCount[gameObjectName]++;
                }
                else
                {
                    // Add the GameObject name to the dictionary with a count of 1
                    gameObjectNameCount[gameObjectName] = 1;
                    uniqueGameObjectNames.Add(gameObjectName);
                }
            }
            string initialItemName = ConstructionManager.Instance.GetInitialNameOfObject(obj.name);
            // Check if we should start counting GameObject names
            if (!startCounting && initialItemName == specialAreaNames[0])
            {
                startCounting = true;
            }
        }

    }

    public void GetParentGameObjectInScene()
    {
        try
        {
            noParentNames = new List<string>();
            parentGameObjects = new List<GameObject>();
            //allGameObjects = FindObjectsOfType<GameObject>();
            allGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();

            // Loop through each GameObject
            foreach (GameObject obj in allGameObjects)
            {
                // Check if the GameObject's parent is null and it is not itself a child of another GameObject
                if (obj.transform.parent == null)
                {

                    // Add the name of the GameObject to the list of GameObjects with no parent
                    string initialItemName = ConstructionManager.Instance.GetInitialNameOfObject(obj.name);
                    noParentNames.Add(initialItemName);
                    parentGameObjects.Add(obj);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Unable to get parent game objects: " + ex.Message);
        }
    }

    //---MissionObject4---//
    public void MissionObject4_Execution() // Build the Light house
    {
        if (itemPlaced == specialObjectNames[0])
        {
            ConstructionManager.Instance.SpecialAreaActivation(specialAreaNames[2]);
            missionObjectIndex = 5;
            StartCoroutine(MissionObjectMenuController.Instance.ActivateMissionsFromMissionIndex(missionObjectIndex - 1));
        }
    }

    //---MissionObject5---//
    public void MissionObject5_Execution() // Build the Guard Fence
    {
        if (itemPlaced == specialObjectNames[2])
        {
            noOfGuardRooms++;
        }

        if(noOfGuardRooms >= 3)
        {
            missionObjectIndex = 6;
            StartCoroutine(MissionObjectMenuController.Instance.ActivateMissionsFromMissionIndex(missionObjectIndex - 1));
        }
    }

    //---MissionObject6---//
    public void MissionObject6_Execution() // Build the TownHall
    {
        if (itemPlaced == specialObjectNames[3])
        {
            missionObjectIndex = 7;
            StartCoroutine(MissionObjectMenuController.Instance.ActivateMissionsFromMissionIndex(missionObjectIndex - 1));
        }
    }

    //---MissionObject7---//
    public void MissionObject7_Execution() // Kill enemies
    {
        if (enemyNamesDiedInObject7.Count == noOfEnemiesInObject7)
        {
            ConstructionManager.Instance.SpecialAreaActivation(specialAreaNames[3]);
            missionObjectIndex = 8;
            StartCoroutine(MissionObjectMenuController.Instance.ActivateMissionsFromMissionIndex(missionObjectIndex - 1));
        }
    }

}
