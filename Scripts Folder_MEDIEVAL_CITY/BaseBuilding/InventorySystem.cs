using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    public GameObject ItemInfoUI;
    public static InventorySystem Instance { get; set; }

    public GameObject inventoryScreenUI;
    public GameObject sellSlot;

    public List<GameObject> slotList = new List<GameObject>();  

    public List<string> itemList = new List<string>();

    private GameObject itemToAdd;

    private GameObject whatSlotToEquip;

    public bool isOpen;

    public List<string> itemsPickedup;

    //Pickup PopUp
    public GameObject pickupAlert;
    public Text pickupName;
    public Image pickupImage;

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


    void Start()
    {
        isOpen = false;

        PopulateSlotList();
    }

    // t initilly get all the inventory slots
    private void PopulateSlotList()
    {
        foreach(Transform child in inventoryScreenUI.transform)
        {
            if (child.CompareTag("Slot"))
            {
                slotList.Add(child.gameObject);
            }
        }
    }

    void Update()
    {
        if (BuyingSystem.Instance.isOpen)
        {
            sellSlot.gameObject.SetActive(true);
        }
        else
        {
            sellSlot.gameObject.SetActive(false);
        }

        // Open the Inventory Screen
        if (!WeaponWheelController.Instance.weaponWheelSelected)
        {
            if (Input.GetKeyDown(KeyCode.I) && !isOpen && !ConstructionManager.Instance.inConstructionMode && !RemoveConstruction.Instance.inRemoveConstructionMode && !DialogSystem.Instance.dialogUIActive 
                && !AssetsManager.Instance.isOpen || ConstructionManager.Instance.inAfterConstruction && !MissionObjectMenuController.Instance.isOpen)
            {

                Debug.Log("i is pressed");
                inventoryScreenUI.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                SelectionManager.instance.DisableSelection();
                SelectionManager.instance.GetComponent<SelectionManager>().enabled = false;

                isOpen = true;

                ConstructionManager.Instance.inAfterConstruction = false;

            }
            else if (Input.GetKeyDown(KeyCode.I) && isOpen || RemoveConstruction.Instance.isRemoveState)
            {
                inventoryScreenUI.SetActive(false);
                if (!CraftingSystem.Instance.isOpen && !BuyingSystem.Instance.isOpen)
                {
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;

                    SelectionManager.instance.EnableSelection();
                    SelectionManager.instance.GetComponent<SelectionManager>().enabled = true;
                }

                isOpen = false;
            }
        }  
    }

    // Add items to inventory
    public void AddToInventory(string itemName)
    {
        try
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.pickItemSound);

            whatSlotToEquip = FindNextEmptySlot();
            Debug.Log("item Nme: " + itemName);
            itemToAdd = Instantiate(Resources.Load<GameObject>(itemName), whatSlotToEquip.transform.position, whatSlotToEquip.transform.rotation);
            itemToAdd.transform.SetParent(whatSlotToEquip.transform);
            itemList.Add(itemName);

            Sprite sprite = itemToAdd.GetComponent<Image>().sprite;

            TriggerPickupPopUp(itemName, sprite);

            ReCalculateList();
            CraftingSystem.Instance.RefreshNeededItems();
        }
        catch (Exception ex)
        {
            Debug.LogError("Unable to add to the inventory: " + ex.Message);
        }
    }

    public void TriggerPickupPopUp(string itemName, Sprite itemSprite)
    {
        StartCoroutine(ShowPickupPopup(itemName, itemSprite));
    }

    // When we re on the item icon then a Ui describing details about item will be appeared
    IEnumerator ShowPickupPopup(string itemName, Sprite itemSprite)
    {
        pickupAlert.SetActive(true);

        pickupName.text = itemName;
        pickupImage.sprite = itemSprite;

        // Wait for 1 seconds
        yield return new WaitForSeconds(1f);

        // After 1 seconds, hide the pickup popup
        pickupAlert.SetActive(false);
    }
    private GameObject FindNextEmptySlot()
    {
        foreach(GameObject slot in slotList)
        {
            if(slot.transform.childCount == 0)
            {
                return slot;
            }
        }

        return new GameObject();
    }

    public bool CheckSlotsAvailable(int emptyNeeded)
    {
        int emptySlot = 0;

        foreach(GameObject slot in slotList)
        {
            if(slot.transform.childCount <= 0)
            {
                emptySlot += 1;
            }
        }

        if(emptySlot >= emptyNeeded)
        {
            return true;
        }
        else
        {
            return false;   
        }
    }

    // Removing item 
    public void RemoveItem(String nameToRemove, int amountToRemove)
    {
        int counter = amountToRemove;

        for(var i = slotList.Count - 1; i >= 0; i--)
        {
            if (slotList[i].transform.childCount > 0)
            {
                if (slotList[i].transform.GetChild(0).name == nameToRemove+"(Clone)" && counter != 0)
                {
                    DestroyImmediate(slotList[i].transform.GetChild(0).gameObject);

                    counter -= 1;
                }
            }
        }

        ReCalculateList();
        CraftingSystem.Instance.RefreshNeededItems();
    }

    // Recalculating all the inventory items
    public void ReCalculateList()
    {
        itemList.Clear();

        foreach(GameObject slot in slotList)
        {
            if(slot.transform.childCount > 0)
            {
                string name = slot.transform.GetChild(0).name;

                string str2 = "(Clone)";

                string result = name.Replace(str2, "");

                itemList.Add(result);
            }
        }
    }

}