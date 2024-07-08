using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;

public class ToggleGhostItems : MonoBehaviour
{
    public static ToggleGhostItems Instance { get; set; }

    // Dictionary to store lists of GameObjects by their names
    public Dictionary<string, List<GameObject>> objectsByName;
    public List<string> names; // List to keep track of the names
    public int currentIndex = -1; // Index of the currently active group of objects

    public GameObject[] allObjects;

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

    void Update()
    {
        // Check if the space key is pressed
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (names != null)
            {
                // Deactivate the currently active group of objects
                if (currentIndex != -1)
                {
                    foreach (GameObject obj in objectsByName[names[currentIndex]])
                    {
                        obj.SetActive(false);
                    }
                }

                if(currentIndex == names.Count - 1)
                {
                    currentIndex = -1;
                }
                else
                {
                    // Move to the next group of objects
                    currentIndex = (currentIndex + 1) % names.Count;

                    // Activate the new group of objects
                    foreach (GameObject obj in objectsByName[names[currentIndex]])
                    {
                        obj.SetActive(true);
                    }

                    Debug.Log($"Activated: {names[currentIndex]}");
                }

                
            }
        }
    }

    // Active ghost item when selecting corresponding construction to placed
    public void ActiveCurrentGhostItem(string itemName)
    {
        if (objectsByName.ContainsKey(itemName))
        {
            foreach (GameObject obj in objectsByName[itemName])
            {
                obj.SetActive(true);
            }
        }
    }

    // Find the all the ghost item and thier foundation construction
    public void FindGhostItems()
    {
        try
        {
            currentIndex = -1;
            // Find all GameObjects in the scene
            allObjects = SceneManager.GetActiveScene().GetRootGameObjects();
            objectsByName = new Dictionary<string, List<GameObject>>();
            names = new List<string>();

            foreach (GameObject obj in allObjects)
            {
                //Debug.Log("allObjects");
                // Check if the GameObject is a parent (i.e., it has no parent)
                if (obj.transform.parent == null)
                {
                    //Debug.Log("obj.transform.parent == null");
                    // Check if the GameObject has a GhostItem component
                    GhostItem ghostItem = obj.GetComponent<GhostItem>();
                    if (ghostItem != null)
                    {
                        //Debug.Log("ghostItem != null)");
                        // Check if the GameObject's name starts with "Road" or "Fence"
                        if (obj.name.StartsWith("Road") || obj.name.StartsWith("Fence") || obj.name.StartsWith("Main_") || obj.name.StartsWith("Basic") || obj.name.StartsWith("Tree_"))
                        {
                            //Debug.Log("obj.name.StartsWith(\"Road\") || obj.name.StartsWith(\"Fence\")");
                            string initialName = ConstructionManager.Instance.GetInitialNameOfObject(obj.name);
                            // Add the GameObject to the dictionary
                            if (!objectsByName.ContainsKey(initialName))
                            {
                                objectsByName[initialName] = new List<GameObject>();
                            }
                            objectsByName[initialName].Add(obj);

                            // Initially deactivate the GameObject
                            obj.SetActive(false);
                        }
                    }
                }
            }

            // Initialize the list of names
            names = new List<string>(objectsByName.Keys);
        }
        catch (Exception ex)
        {
            Debug.LogError("Unable find all the ghost items: " + ex.Message);
        }
    }
}
