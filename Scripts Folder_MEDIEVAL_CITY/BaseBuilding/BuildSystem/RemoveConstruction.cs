using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class RemoveConstruction : MonoBehaviour
{
    public static RemoveConstruction Instance { get; set; }

    public Transform selectionTransform;
    public GameObject RemoveConstructionModeUI;
    public Image centerDotImage;
    public Image removeIcon;

    public bool inRemoveConstructionMode = false;
    public bool isRemoveState = false; 

    public string removedConstructionName;

    public GameObject interaction_Info_UI;
    Text interaction_text;

    // Dictionary to store lists of GameObjects by their names
    public Dictionary<string, List<GameObject>> objectsByName;
    public List<string> names; // List to keep track of the names
    public int currentIndex = -1; // Index of the currently active group of objects

    public GameObject[] allObjects;

    public string indexToRemove;
    public string indexOfGhostItem;

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

    // Start is called before the first frame update
    void Start()
    {
        interaction_text = interaction_Info_UI.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inRemoveConstructionMode)
        {
            isRemoveState = false;   
            interaction_text.text = "";

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            RemoveConstructionModeUI.SetActive(true);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                selectionTransform = hit.transform;
                // Check the gameObject whether we have placed item
                if (selectionTransform.gameObject.CompareTag("placedFoundation") || selectionTransform.gameObject.CompareTag("placedWall"))
                {
                    interaction_text.text = selectionTransform.gameObject.name;

                    SelectionManager.instance.DisableSelection();
                    SelectionManager.instance.GetComponent<SelectionManager>().enabled = false;
                    interaction_Info_UI.SetActive(true);

                    removeIcon.gameObject.SetActive(true);
                    if (Input.GetMouseButtonDown(0))
                    {
                        try
                        {
                            removedConstructionName = selectionTransform.gameObject.name;

                            // Find the index of the dollar sign
                            int dollarIndex = selectionTransform.gameObject.name.IndexOf('$');
                            if (dollarIndex > 0)
                            {
                                // Extract the ID after the dollar sign
                                indexToRemove = selectionTransform.gameObject.name.Substring(dollarIndex + 1);
                                DestroyImmediate(selectionTransform.gameObject);
                                DeleteAllItems();

                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.LogError("Unable to remove the construction: " + ex.Message);
                        }

                    }
                }
                else
                {
                    SelectionManager.instance.EnableSelection();
                    SelectionManager.instance.GetComponent<SelectionManager>().enabled = true;
                    interaction_Info_UI.SetActive(false);
                    removeIcon.gameObject.SetActive(false);
                }

            }
            else
            {
                interaction_Info_UI.SetActive(false);

                SelectionManager.instance.DisableSelection();
                SelectionManager.instance.GetComponent<SelectionManager>().enabled = false;
                removeIcon.gameObject.SetActive(false);
            }
        }

        // Cancel the removing construction
        if (Input.GetKeyDown(KeyCode.X))
        {     // Left Mouse Button
            if (inRemoveConstructionMode)
            {
                SelectionManager.instance.EnableSelection();
                SelectionManager.instance.GetComponent<SelectionManager>().enabled = true;
                interaction_Info_UI.SetActive(false);

                inRemoveConstructionMode = false;
                removeIcon.gameObject.SetActive(false);
                RemoveConstructionModeUI.SetActive(false);
            }

        }
    }

    // Delete all the ghost item after the removing the construction that inside this construction 
    public void DeleteAllItems()
    {
        // Find all GameObjects in the scene
        allObjects = SceneManager.GetActiveScene().GetRootGameObjects();

        foreach (GameObject obj in allObjects)
        {
            // Check if the GameObject is a parent
            if (obj.transform.parent == null)
            {
                // Check if the GameObject has a GhostItem component
                GhostItem ghostItem = obj.GetComponent<GhostItem>();
                if (ghostItem != null)
                {
                    int dollarIndex = obj.name.IndexOf('$');
                    if (dollarIndex > 0)
                    {
                        // Extract the ID after the dollar sign
                        indexOfGhostItem = obj.name.Substring(dollarIndex + 1);
                        if(indexOfGhostItem == indexToRemove)
                        {
                            DestroyImmediate(obj);
                        }
                    }
                }
            }
        }
        
        ToggleGhostItems.Instance.FindGhostItems();
    }

    public void DisableSelection()
    {
        removeIcon.enabled = false;
        centerDotImage.enabled = false;
    }

    public void EnableSelection()
    {
        removeIcon.enabled = true;
        centerDotImage.enabled = true;
    }
}
