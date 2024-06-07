using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionManager : MonoBehaviour
{
    public static ConstructionManager Instance { get; set; }

    public GameObject itemToBeConstructed;
    public bool inConstructionMode = false;
    public bool inAfterConstruction = false;   
    public bool constructionInQuickSlot = false;
    public GameObject constructionHoldingSpot;
    public float itemToBeConstructed_YRoation = 0f;

    public GameObject constructionHoldingSpotArea;

    public bool isValidPlacement;
    public bool isArea1;


    public bool selectingAGhost;
    public GameObject selectedGhost;

    // Materials we store as refereces for the ghosts
    public Material ghostSelectedMat;
    public Material ghostSemiTransparentMat;
    public Material ghostFullTransparentMat;

    // We keep a reference to all ghosts currently in our world,
    // so the manager can monitor them for various operations
    public List<GameObject> allGhostsInExistence = new List<GameObject>();

    public GameObject itemToBeDestroyed;

    public GameObject constructionUI;

    public GameObject player;

    public Transform selectionTransform;

    public GameObject item = null;
    public string itemClass;
    public string itemSubClass;
    public string hitItemName;

    public int placedItemCount = 1;

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

    // Appear the selected construction in front of the player
    public void ActivateConstructionPlacement(string itemToConstruct)
    {
        // Get the class nme
        itemClass = itemToConstruct.Substring(0, 5);

        string path = itemClass + "/" + itemToConstruct; // Assuming itemToConstruct is the name of your prefab
        //Debug.Log("pth: " + path);
        if (itemClass == "Road_" || itemClass == "Base_"  || itemClass == "Main_" || itemClass == "Basic" || itemClass == "Fence" || itemClass == "Tree_")
        {
            item = Instantiate(Resources.Load<GameObject>(path));
        }
        else
        {
            item = Instantiate(Resources.Load<GameObject>("Others/" + itemToConstruct));
        } 

        //change the name of the gameobject so it will not be (clone)
        item.name = itemToConstruct;
        ToggleGhostItems.Instance.ActiveCurrentGhostItem(item.name);

        item.transform.SetParent(constructionHoldingSpot.transform, false);
        itemToBeConstructed_YRoation = 0;

        itemToBeConstructed = item;
        itemToBeConstructed.gameObject.tag = "activeConstructable";

        // Disabling the non-trigger collider so our mouse can cast a ray
        if (itemToBeConstructed.GetComponent<Constructable>().solidCollider != null)
        {
            itemToBeConstructed.GetComponent<Constructable>().solidCollider.enabled = false;
        }
        

        // Actiavting Construction mode
        inConstructionMode = true;
    }

    // Get ghost items of the construction
    private void GetAllGhosts(GameObject itemToBeConstructed)
    {
        try
        {
            List<GameObject> ghostlist = itemToBeConstructed.gameObject.GetComponent<Constructable>().ghostList;

            foreach (GameObject ghost in ghostlist)
            {
                allGhostsInExistence.Add(ghost);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Unable to Get all ghost items: " + ex.Message);
        }
    }

    private void PerformGhostDeletionScan()
    {

        foreach (GameObject ghost in allGhostsInExistence)
        {
            if (ghost != null)
            {
                if (ghost.GetComponent<GhostItem>().hasSamePosition == false) // if we did not already add a flag
                {
                    foreach (GameObject ghostX in allGhostsInExistence)
                    {
                        // First we check that it is not the same object
                        if (ghost.gameObject != ghostX.gameObject)
                        {
                            // If its not the same object but they have the same position
                            if (XPositionToAccurateFloat(ghost) == XPositionToAccurateFloat(ghostX) && ZPositionToAccurateFloat(ghost) == ZPositionToAccurateFloat(ghostX))
                            {
                                if (ghost != null && ghostX != null)
                                {
                                    // setting the flag
                                    ghostX.GetComponent<GhostItem>().hasSamePosition = true;
                                    break;
                                }

                            }

                        }

                    }

                }
            }
        }

        foreach (GameObject ghost in allGhostsInExistence)
        {
            if (ghost != null)
            {
                if (ghost.GetComponent<GhostItem>().hasSamePosition)
                {
                    DestroyImmediate(ghost);
                }
            }

        }
    }

    private float XPositionToAccurateFloat(GameObject ghost)
    {
        if (ghost != null)
        {
            // Turning the position to a 2 decimal rounded float
            Vector3 targetPosition = ghost.gameObject.transform.position;
            float pos = targetPosition.x;
            float xFloat = Mathf.Round(pos * 100f) / 100f;
            return xFloat;
        }
        return 0;
    }

    private float ZPositionToAccurateFloat(GameObject ghost)
    {

        if (ghost != null)
        {
            // Turning the position to a 2 decimal rounded float
            Vector3 targetPosition = ghost.gameObject.transform.position;
            float pos = targetPosition.z;
            float zFloat = Mathf.Round(pos * 100f) / 100f;
            return zFloat;

        }
        return 0;
    }

    private void Update()
    {
        if (itemToBeConstructed != null && inConstructionMode)
        {
            // Change the rotation of the construction by clicking Mouse button 2
            if (itemClass == "Road_" || itemClass == "Base_")
            {
                if (Input.GetKeyDown(KeyCode.Mouse2))
                {
                    itemToBeConstructed_YRoation += 90;
                }
                itemToBeConstructed.transform.eulerAngles = new Vector3(0, itemToBeConstructed_YRoation, 0);
            }
            else
            {
                itemToBeConstructed.transform.eulerAngles = new Vector3(0, itemToBeConstructed.transform.eulerAngles.y, 0);
            }

            // Check validation to place
            if (itemClass == "Road_" || itemClass == "Base_")
            {
                if (CheckValidConstructionPosition())
                {
                    isValidPlacement = true;
                    itemToBeConstructed.GetComponent<Constructable>().SetValidColor();
                }
                else
                {
                    isValidPlacement = false;
                    itemToBeConstructed.GetComponent<Constructable>().SetInvalidColor();
                }
            }
            
            // By moving mouse can be locate in to envirnment objects and get responses
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                selectionTransform = hit.transform;

                // Get the initial name because at the end of the construction real name we have add new ID by starting $ mark
                hitItemName =  GetInitialNameOfObject(selectionTransform.gameObject.name);

                if (selectionTransform.gameObject.CompareTag("ghost") && itemToBeConstructed.name == "FoundationModel")
                {
                    itemToBeConstructed.SetActive(false);
                    selectingAGhost = true;
                    selectedGhost = selectionTransform.gameObject;
                }
                else if (selectionTransform.gameObject.CompareTag("wallGhost") && itemToBeConstructed.name == "WallModel")
                {
                    itemToBeConstructed.SetActive(false);
                    selectingAGhost = true;
                    selectedGhost = selectionTransform.gameObject;
                }
                else if (hitItemName == itemToBeConstructed.name)
                {
                    itemToBeConstructed.SetActive(false);
                    selectingAGhost = true;
                    selectedGhost = selectionTransform.gameObject;
                }
                else
                {
                    itemToBeConstructed.SetActive(true);
                    selectedGhost = null;
                    selectingAGhost = false;
                }

            }
        }

        if(itemToBeConstructed != null)
        {
            // Left Mouse Click to Place item
            if (Input.GetMouseButtonDown(0) && inConstructionMode)
            {
                try
                {
                    if (isValidPlacement && selectedGhost == false && (itemClass == "Road_" || itemClass == "Base_")) // We don't want the freestyle to be triggered when we select a ghost.
                    {
                        PlaceItemFreeStyle();
                        DestroyItem(itemToBeDestroyed);
                    }

                    if (selectingAGhost)
                    {
                        MissionSystem.Instance.itemPlaced = itemToBeConstructed.name;
                        PlaceItemInGhostPosition(selectedGhost);
                        DestroyItem(itemToBeDestroyed);
                    }
                    if (constructionInQuickSlot)
                    {
                        EquipSystem.Instance.SelectCurrentQuickSlot();
                        constructionInQuickSlot = false;
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError("Construction was not placed due to an Error: " + ex.Message);
                }
            }
        }

        
        // Right Mouse Click to Cancel the placing construction            
        if (Input.GetKeyDown(KeyCode.X) || ((!EquipSystem.Instance.activeQuickSlot || EquipSystem.Instance.triggeringQuickSlot) && constructionInQuickSlot))
        {
            if (constructionInQuickSlot)
            {
                EquipSystem.Instance.triggeringQuickSlot = false;
                EquipSystem.Instance.SelectCurrentQuickSlot();
                constructionInQuickSlot = false;
            }
            // Left Mouse Button
            if (inConstructionMode)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                itemToBeDestroyed.SetActive(true);
                itemToBeDestroyed = null;
                DestroyItem(itemToBeConstructed);
                itemToBeConstructed = null;
                inConstructionMode = false;
            }
            
        }
        
    }

    // Placing the construction inside already placed constructions ghost places
    private void PlaceItemInGhostPosition(GameObject copyOfGhost)
    {

        Vector3 ghostPosition = copyOfGhost.transform.position;
        Quaternion ghostRotation = copyOfGhost.transform.rotation;

        selectedGhost.gameObject.SetActive(false);

        // Setting the item to be active again (after we disabled it in the ray cast)
        itemToBeConstructed.gameObject.SetActive(true);

        itemToBeConstructed.gameObject.name += $"${placedItemCount}";
        
        // Setting the parent to be the root of our scene
        itemToBeConstructed.transform.SetParent(transform.parent, true);

        var randomOffset = UnityEngine.Random.Range(0.01f, 0.03f);

        itemToBeConstructed.transform.position = ghostPosition;
        itemToBeConstructed.transform.rotation = ghostRotation;

        itemToBeConstructed.GetComponent<Constructable>().solidCollider.enabled = true;
        // Setting the default color/material
        itemToBeConstructed.GetComponent<Constructable>().SetDefaultColor();
        // Enabling back the solider collider waht we earlier
        if(itemClass == "Road_" || itemClass == "Base_")
        {
            // Making the Ghost Children to no longer be children of this item
            itemToBeConstructed.GetComponent<Constructable>().ExtractGhostMembers();
            // Enabling back the solider collider that we disabled earlier
            //itemToBeConstructed.GetComponent<Constructable>().solidCollider.enabled = true;
            itemToBeConstructed.tag = "placedFoundation";

            //Adding all the ghosts of this item into the manager's ghost bank
            GetAllGhosts(itemToBeConstructed);
        }
        else
        {
            itemToBeConstructed.tag = "placedWall";
            DestroyItem(selectedGhost); // We delete this wallGhost, because the manager will not do it
        }
        // For placed treed w have to set the correct tag
        if(itemClass == "Tree_")
        {
            itemToBeConstructed.tag = "Tree";
        }

        ToggleGhostItems.Instance.FindGhostItems();

        itemToBeConstructed = null;

        inConstructionMode = false;

        inAfterConstruction = true;

        placedItemCount++;
    }

    // Removing the item from the inventory after construction
    void DestroyItem(GameObject item)
    {
        DestroyImmediate(item);
        InventorySystem.Instance.ReCalculateList();
        CraftingSystem.Instance.RefreshNeededItems();
    }

    // Place the construction as new
    private void PlaceItemFreeStyle()
    {
        // Set the Height of the Terrain for the all area for a sme value (flat the area) covered by Base Type construction
        if (itemClass == "Base_")
        {
            Vector2Int area = itemToBeConstructed.GetComponent<Constructable>().area;
            //NavMeshUpdater.Instance.UpdateNavMesh();
            if (itemToBeConstructed_YRoation % 180 != 0)
            {
                Vector2Int _area = new Vector2Int(area.y, area.x);
                TerrainController.Instance.SetTerrainHeight(itemToBeConstructed.transform.position, _area, itemToBeConstructed.transform.rotation.y);
            }
            else
            {
                TerrainController.Instance.SetTerrainHeight(itemToBeConstructed.transform.position, area, itemToBeConstructed.transform.rotation.y);
            }
            
        }
        itemToBeConstructed.gameObject.name += $"${placedItemCount}";
        

        // Setting the parent to be the root of our scene
        itemToBeConstructed.transform.SetParent(transform.parent, true);

        // Making the Ghost Children to no longer be children of this item
        itemToBeConstructed.GetComponent<Constructable>().ExtractGhostMembers();
        // Setting the default color/material
        itemToBeConstructed.GetComponent<Constructable>().SetDefaultColor();          
        itemToBeConstructed.tag = "placedFoundation";
        itemToBeConstructed.GetComponent<Constructable>().enabled = false;
        // Enabling back the solider collider that we disabled earlier
        itemToBeConstructed.GetComponent<Constructable>().solidCollider.enabled = true;     

        //Adding all the ghosts of this item into the manager's ghost bank
        GetAllGhosts(itemToBeConstructed);

        ToggleGhostItems.Instance.FindGhostItems();

        itemToBeConstructed = null;

        inConstructionMode = false;

        inAfterConstruction = true;

        placedItemCount++;
    }

    public void SpecialAreaActivation(string itemToConstruct)
    {
        try
        {
            // Get the correct special area
            string className = "Foundation";

            string areaName = itemToConstruct.Replace(className, string.Empty);
            string path = areaName + "/" + itemToConstruct; // Assuming itemToConstruct is the name of your prefab
            GameObject item = Instantiate(Resources.Load<GameObject>(path));

            DisableCollidersInChildren(item);
            item.name = itemToConstruct;

            item.transform.SetParent(constructionHoldingSpotArea.transform, false);
            itemToBeConstructed = item;
            itemToBeConstructed.gameObject.tag = "activeConstructable";

            // Disabling the non-trigger collider so our mouse can cast a ray
            itemToBeConstructed.GetComponent<Constructable>().solidCollider.enabled = false;

            // Actiavting Construction mode
            inConstructionMode = true;

            StartCoroutine(AreaStartingPlaceItemFreeStyle());
        }
        catch (Exception ex)
        {
            Debug.LogError("Unable place special area: " + ex.Message);
        }
    }
    
    public IEnumerator AreaStartingPlaceItemFreeStyle()
    {
        yield return new WaitForSeconds(4f);
        PlaceItemFreeStyle();
    }

    // When placing the constrcution we have to disable all colliders of the ghost item, if not we cannot go through the ghost items
    public void DisableCollidersInChildren(GameObject parentObject)
    {
        Transform parentTransform = parentObject.transform;

        if (parentTransform != null)
        {
            foreach (Transform child in parentTransform)
            {
                Collider[] colliders = child.GetComponentsInChildren<Collider>();
                foreach (Collider collider in colliders)
                {
                    collider.enabled = false;
                }
            }
        }
        else
        {
            Debug.LogError("Parent GameObject is null!");
        }
    }

    private bool CheckValidConstructionPosition()
    {
        if (itemToBeConstructed != null)
        {
            return itemToBeConstructed.GetComponent<Constructable>().isValidToBeBuilt;
        }

        return false;
    }

    // Since we use some ID at the end of all items starting "$" mark so we have to remove it
    public string GetInitialNameOfObject(string currentName)
    {
        string initialName;
        // Find the index of the dollar sign
        int dollarIndex = currentName.IndexOf('$');

        // If a dollar sign is found and it is not the first character
        if (dollarIndex > 0)
        {
            // Extract the name without the dollar sign and trailing numbers
            initialName = currentName.Substring(0, dollarIndex);
        }
        else
        {
            initialName = currentName;
        }

        return initialName; 
    }
}