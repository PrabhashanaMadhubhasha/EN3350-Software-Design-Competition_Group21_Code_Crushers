using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EquipSystem : MonoBehaviour
{
    public static EquipSystem Instance { get; set; }

    // -- UI -- //
    public GameObject quickSlotsPanel;

    public List<GameObject> quickSlotsList = new List<GameObject>();
    public List<string> itemList = new List<string>();

    public GameObject numberHolder;

    public int selectedNumber = -1;
    public GameObject selectedItem;

    public GameObject toolHolder;
    public GameObject selectedItemModel;

    public bool isAxeIsEquipped;
    public Character character;

    public bool haveDamagedAxe = false;
    public bool activeQuickSlot;
    public bool triggeringQuickSlot;

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


    private void Start()
    {
        PopulateSlotList();
    }

    void Update()
    {
        // Use / Equip / Consume the item on corresponding Quick slot
        if (Input.GetKeyDown(KeyCode.Alpha4)){
            StartCoroutine(SelectQuickSlot(1));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5)){
            StartCoroutine(SelectQuickSlot(2));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6)){
            StartCoroutine(SelectQuickSlot(3));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7)){
            StartCoroutine(SelectQuickSlot(1));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8)){
            StartCoroutine(SelectQuickSlot(5));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9)){
            StartCoroutine(SelectQuickSlot(6));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0)){
            StartCoroutine(SelectQuickSlot(7));
        }
    }

    // We re selecting a quick slot
    IEnumerator SelectQuickSlot(int number)
    {
        yield return new WaitForSeconds(0.01f);
        if (checkSlotIsFull(number) == true)
        {
            if (selectedNumber != number)
            {
                selectedNumber = number;
                // Unselect previously selected item
                if (selectedItem != null)
                {
                    selectedItem.gameObject.GetComponent<InventoryItem>().isSelected = false;
                    //Debug.Log("constructionInQuickSlot moide   " + ConstructionManager.Instance.constructionInQuickSlot);
                    triggeringQuickSlot = true;
                    isAxeIsEquipped = false;
                }
                StartCoroutine(ActiveQuickSlot(number));

            }
            else // We are trying to select same slot
            {
                SelectCurrentQuickSlot();
            }
        }
    }

    public IEnumerator ActiveQuickSlot(int number)
    {
        yield return new WaitForSeconds(0.01f);
        selectedItem = getSelectedItem(number);
        selectedItem.GetComponent<InventoryItem>().isSelected = true;

        SetEquippedModel(selectedItem);

        // Changing color
        foreach (Transform child in numberHolder.transform)
        {
            child.transform.Find("Text").GetComponent<Text>().color = Color.gray;
        }

        Text toBeChanged = numberHolder.transform.Find("number" + number).transform.Find("Text").GetComponent<Text>();
        toBeChanged.color = Color.white;
        activeQuickSlot = true;
    }

    public void SelectCurrentQuickSlot()
    {
        selectedNumber = -1;

        // Unselect previously selected item
        if (selectedItem != null)
        {
            activeQuickSlot = false;
            selectedItem.gameObject.GetComponent<InventoryItem>().isSelected = false;
            isAxeIsEquipped = false;
            selectedItem = null;
        }

        if (selectedItemModel != null)
        {
            DestroyImmediate(selectedItemModel.gameObject);
            selectedItemModel = null;
        }

        // Changing color
        foreach (Transform child in numberHolder.transform)
        {
            child.transform.Find("Text").GetComponent<Text>().color = Color.gray;
        }
    }

    private void SetEquippedModel(GameObject selectedItem)
    {
        try
        {
            if (selectedItemModel != null)
            {
                DestroyImmediate(selectedItemModel.gameObject);
                selectedItemModel = null;
            }

            string selectedItemName = selectedItem.name.Replace("(Clone)", "");
            if (selectedItemName == "Axe")
            {
                isAxeIsEquipped = true;
                character.isEquipedAxe = true;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Unable equipping the item: " + ex.Message);
        }

    }

    GameObject getSelectedItem(int slotnumber)
    {
        return quickSlotsList[slotnumber-1].transform.GetChild(0).gameObject;
    }

    bool checkSlotIsFull(int slotNumber)
    {
        if (quickSlotsList[slotNumber-1].transform.childCount > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // Get all the quick slot at the start
    private void PopulateSlotList()
    {
        foreach (Transform child in quickSlotsPanel.transform)
        {
            if (child.CompareTag("QuickSlot"))
            {
                quickSlotsList.Add(child.gameObject);
            }
        }
    }

    public void AddToQuickSlots(GameObject itemToEquip)
    {
        // Find next free slot
        GameObject availableSlot = FindNextEmptySlot();
        // Set transform of our object
        itemToEquip.transform.SetParent(availableSlot.transform, false);
        // Getting clean name
        string cleanName = itemToEquip.name.Replace("(Clone)", "");
        // Adding item to list
        itemList.Add(cleanName);

        InventorySystem.Instance.ReCalculateList();

    }

    // We have to find next 
    public GameObject FindNextEmptySlot()
    {
        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }
        }
        return new GameObject();
    }

    public bool CheckIfFull()
    {

        int counter = 0;

        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount > 0)
            {
                counter += 1;
            }
        }

        if (counter == 7)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}