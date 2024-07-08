using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    // --- Is this item trashable --- //
    public bool isTrashable;

    // --- Is this item sellable --- //
    public bool isSellable;

    // --- Item Info UI --- //
    private GameObject itemInfoUI;

    private Text itemInfoUI_itemName;
    private Text itemInfoUI_itemDescription;
    private Text itemInfoUI_itemFunctionality;

    public string thisName, thisDescription, thisFunctionality;

    // --- Consumption --- //
    public GameObject itemPendingConsumption;
    public bool isConsumable;

    public int sellRevenue;

    public float healthEffect;
    public float caloriesEffect;
    public float hydrationEffect;

    public bool isEquippable;
    public bool isInsideQuickSlot;

    public bool isSelected;
    public bool isSelectingQuickSlot;

    public bool isUseable;

    //public GameObject itemPendingToBeUsed;


    private void Start()
    {
        itemInfoUI = InventorySystem.Instance.ItemInfoUI;
        itemInfoUI_itemName = itemInfoUI.transform.Find("itemName").GetComponent<Text>();
        itemInfoUI_itemDescription = itemInfoUI.transform.Find("itemDescription").GetComponent<Text>();
        itemInfoUI_itemFunctionality = itemInfoUI.transform.Find("itemFunctionality").GetComponent<Text>();
    }

    void Update()
    {
        if(isSelected)
        { 
            gameObject.GetComponent<DragDrop>().enabled = false;
            SelectingItem();
            AfterSelectingItem();
        }
        else
        {
            gameObject.GetComponent<DragDrop>().enabled = true;
        }
    }

    // Triggered when the mouse enters into the area of the item that has this script.
    public void OnPointerEnter(PointerEventData eventData)
    {
        itemInfoUI.SetActive(true);
        itemInfoUI_itemName.text = thisName;
        itemInfoUI_itemDescription.text = thisDescription;
        itemInfoUI_itemFunctionality.text = thisFunctionality;
    }

    // Triggered when the mouse exits the area of the item that has this script.
    public void OnPointerExit(PointerEventData eventData)
    {
        itemInfoUI.SetActive(false);
    }

    // Triggered when the mouse is clicked over the item that has this script.
    public void OnPointerDown(PointerEventData eventData)
    {
        //Right Mouse Button Click on
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (isConsumable)
            {
                // Setting this specific gameobject to be the item we want to destroy later
                itemPendingConsumption = gameObject;
                consumingFunction(healthEffect, caloriesEffect, hydrationEffect);
            }

            if (isEquippable && isInsideQuickSlot == false && EquipSystem.Instance.CheckIfFull() == false)
            {
                EquipSystem.Instance.AddToQuickSlots(gameObject);
                isInsideQuickSlot = true;
            }

            if (isUseable)
            {
                ConstructionManager.Instance.itemToBeDestroyed = gameObject;
                gameObject.SetActive(false);
                UseItem();
            }
        }
    }

    // Triggered when the mouse button is released over the item that has this script.
    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (isConsumable && itemPendingConsumption == gameObject)
            {
                DestroyImmediate(gameObject);
                InventorySystem.Instance.ReCalculateList();
                CraftingSystem.Instance.RefreshNeededItems();
            }
        }
    }

    private void SelectingItem()
    {
        if (isConsumable)
        {
            // Setting this specific gameobject to be the item we want to destroy later
            itemPendingConsumption = gameObject;
            consumingFunction(healthEffect, caloriesEffect, hydrationEffect);
        }

        if (isUseable)
        {
            ConstructionManager.Instance.constructionInQuickSlot = true;
            ConstructionManager.Instance.itemToBeDestroyed = gameObject;
            gameObject.SetActive(false);
            UseItem();
        }
    }

    private void AfterSelectingItem()
    {
        if (isConsumable && itemPendingConsumption == gameObject)
        {
            DestroyImmediate(gameObject);
            InventorySystem.Instance.ReCalculateList();
            CraftingSystem.Instance.RefreshNeededItems();
            EquipSystem.Instance.SelectCurrentQuickSlot();
        }
    }
    public void UseItem() // Use constructable items
    {
        itemInfoUI.SetActive(false);

        InventorySystem.Instance.isOpen = false;             
        InventorySystem.Instance.inventoryScreenUI.SetActive(false);      

        CraftingSystem.Instance.isOpen = false;
        CraftingSystem.Instance.craftingScreenUI.SetActive(false);
        CraftingSystem.Instance.toolsScreenUI.SetActive(false);
        CraftingSystem.Instance.survivalScreenUI.SetActive(false);
        CraftingSystem.Instance.refineScreenUI.SetActive(false);
        CraftingSystem.Instance.constructionScreenUI.SetActive(false);
        CraftingSystem.Instance.roadConstructionScreenUI.SetActive(false);
        CraftingSystem.Instance.baseConstructionScreenUI.SetActive(false);
        CraftingSystem.Instance.mainConstructionScreenUI.SetActive(false);
        CraftingSystem.Instance.basicConstructionScreenUI.SetActive(false);
        CraftingSystem.Instance.fenceConstructionScreenUI.SetActive(false);
        CraftingSystem.Instance.treeConstructionScreenUI.SetActive(false);
        CraftingSystem.Instance.specialConstructionScreenUI.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SelectionManager.instance.EnableSelection();
        SelectionManager.instance.enabled = true;
        switch (gameObject.name)
        {
            case "Foundation(Clone)":
                ConstructionManager.Instance.ActivateConstructionPlacement("FoundationModel");
                break;
            case "Foundation":
                ConstructionManager.Instance.ActivateConstructionPlacement("FoundationModel");
                break;
            case "Wall(Clone)":
                ConstructionManager.Instance.ActivateConstructionPlacement("WallModel");
                break;
            case "Wall":
                ConstructionManager.Instance.ActivateConstructionPlacement("WallModel");
                break;
            case "Road_Mid(Clone)":
                ConstructionManager.Instance.ActivateConstructionPlacement("Road_Mid");
                break;
            case "Road_Mid":
                ConstructionManager.Instance.ActivateConstructionPlacement("Road_Mid");
                break;
            case "Road_End(Clone)":
                ConstructionManager.Instance.ActivateConstructionPlacement("Road_End");
                break;
            case "Road_End":
                ConstructionManager.Instance.ActivateConstructionPlacement("Road_End");
                break;
            case "Road_Junction(Clone)":
                ConstructionManager.Instance.ActivateConstructionPlacement("Road_Junction");
                break;
            case "Road_Junction":
                ConstructionManager.Instance.ActivateConstructionPlacement("Road_Junction");
                break;
            case "Road_Corner(Clone)":
                ConstructionManager.Instance.ActivateConstructionPlacement("Road_Corner");
                break;
            case "Road_Corner":
                ConstructionManager.Instance.ActivateConstructionPlacement("Road_Corner");
                break;
            case "Road_Stair(Clone)":
                ConstructionManager.Instance.ActivateConstructionPlacement("Road_Stair");
                break;
            case "Road_Stair":
                ConstructionManager.Instance.ActivateConstructionPlacement("Road_Stair");
                break;

            case "Main_ElectricityFan(Clone)":
                ConstructionManager.Instance.ActivateConstructionPlacement("Main_ElectricityFan");
                break;
            case "Main_ElectricityFan":
                ConstructionManager.Instance.ActivateConstructionPlacement("Main_ElectricityFan");
                break;
            case "Main_ElectricityWater(Clone)":
                ConstructionManager.Instance.ActivateConstructionPlacement("Main_ElectricityWater");
                break;
            case "Main_ElectricityWater":
                ConstructionManager.Instance.ActivateConstructionPlacement("Main_ElectricityWater");
                break;
            case "Main_Hospital(Clone)":
                ConstructionManager.Instance.ActivateConstructionPlacement("Main_Hospital");
                break;
            case "Main_Hospital":
                ConstructionManager.Instance.ActivateConstructionPlacement("Main_Hospital");
                break;
            case "Main_LightHouse(Clone)":
                ConstructionManager.Instance.ActivateConstructionPlacement("Main_LightHouse");
                break;
            case "Main_LightHouse":
                ConstructionManager.Instance.ActivateConstructionPlacement("Main_LightHouse");
                break;
            case "Main_School(Clone)":
                ConstructionManager.Instance.ActivateConstructionPlacement("Main_School");
                break;
            case "Main_School":
                ConstructionManager.Instance.ActivateConstructionPlacement("Main_School");
                break;
            case "Main_TownHall(Clone)":
                ConstructionManager.Instance.ActivateConstructionPlacement("Main_TownHall");
                break;
            case "Main_TownHall":
                ConstructionManager.Instance.ActivateConstructionPlacement("Main_TownHall");
                break;
            case "Main_WaterTower(Clone)":
                ConstructionManager.Instance.ActivateConstructionPlacement("Main_WaterTower");
                break;
            case "Main_WaterTower":
                ConstructionManager.Instance.ActivateConstructionPlacement("Main_WaterTower");
                break;
            case "Main_WaterWell(Clone)":
                ConstructionManager.Instance.ActivateConstructionPlacement("Main_WaterWell");
                break;
            case "Main_WaterWell":
                ConstructionManager.Instance.ActivateConstructionPlacement("Main_WaterWell");
                break;

            case "Base_Huge(Clone)":
                ConstructionManager.Instance.ActivateConstructionPlacement("Base_Huge");
                break;
            case "Base_Huge":
                ConstructionManager.Instance.ActivateConstructionPlacement("Base_Huge");
                break;
            case "Base_Large(Clone)":
                ConstructionManager.Instance.ActivateConstructionPlacement("Base_Large");
                break;
            case "Base_Large":
                ConstructionManager.Instance.ActivateConstructionPlacement("Base_Large");
                break;
            case "Base_Medium(Clone)":
                ConstructionManager.Instance.ActivateConstructionPlacement("Base_Medium");
                break;
            case "Base_Medium":
                ConstructionManager.Instance.ActivateConstructionPlacement("Base_Medium");
                break;
            case "Base_Small(Clone)":
                ConstructionManager.Instance.ActivateConstructionPlacement("Base_Small");
                break;
            case "Base_Small":
                ConstructionManager.Instance.ActivateConstructionPlacement("Base_Small");
                break;
            case "Base_Tree(Clone)":
                ConstructionManager.Instance.ActivateConstructionPlacement("Base_Tree");
                break;
            case "Base_Tree":
                ConstructionManager.Instance.ActivateConstructionPlacement("Base_Tree");
                break;

            case "Basic_Factory1(Clone)":
                ConstructionManager.Instance.ActivateConstructionPlacement("Basic_Factory1");
                break;
            case "Basic_Factory1":
                ConstructionManager.Instance.ActivateConstructionPlacement("Basic_Factory1");
                break;
            case "Basic_Factory2(Clone)":
                ConstructionManager.Instance.ActivateConstructionPlacement("Basic_Factory2");
                break;
            case "Basic_Factory2":
                ConstructionManager.Instance.ActivateConstructionPlacement("Basic_Factory2");
                break;
            case "Basic_Hotel1(Clone)":
                ConstructionManager.Instance.ActivateConstructionPlacement("Basic_Hotel1");
                break;
            case "Basic_Hotel1":
                ConstructionManager.Instance.ActivateConstructionPlacement("Basic_Hotel1");
                break;
            case "Basic_House1(Clone)":
                ConstructionManager.Instance.ActivateConstructionPlacement("Basic_House1");
                break;
            case "Basic_House1":
                ConstructionManager.Instance.ActivateConstructionPlacement("Basic_House1");
                break;
            case "Basic_House2(Clone)":
                ConstructionManager.Instance.ActivateConstructionPlacement("Basic_House2");
                break;
            case "Basic_House2":
                ConstructionManager.Instance.ActivateConstructionPlacement("Basic_House2");
                break;
            case "Basic_House3(Clone)":
                ConstructionManager.Instance.ActivateConstructionPlacement("Basic_House3");
                break;
            case "Basic_House3":
                ConstructionManager.Instance.ActivateConstructionPlacement("Basic_House3");
                break;
            
            case "Fence_Column(Clone)":
                ConstructionManager.Instance.ActivateConstructionPlacement("Fence_Column");
                break;
            case "Fence_Column":
                ConstructionManager.Instance.ActivateConstructionPlacement("Fence_Column");
                break;
            case "Fence_Mid(Clone)":
                ConstructionManager.Instance.ActivateConstructionPlacement("Fence_Mid");
                break;
            case "Fence_Mid":
                ConstructionManager.Instance.ActivateConstructionPlacement("Fence_Mid");
                break;
            case "Fence_Gate(Clone)":
                ConstructionManager.Instance.ActivateConstructionPlacement("Fence_Gate");
                break;
            case "Fence_Gate":
                ConstructionManager.Instance.ActivateConstructionPlacement("Fence_Gate");
                break;
            case "Fence_Pole(Clone)":
                ConstructionManager.Instance.ActivateConstructionPlacement("Fence_Pole");
                break;
            case "Fence_Pole":
                ConstructionManager.Instance.ActivateConstructionPlacement("Fence_Pole");
                break;


            case "Fence_House1(Clone)":
                ConstructionManager.Instance.ActivateConstructionPlacement("Fence_House1");
                break;
            case "Fence_House1":
                ConstructionManager.Instance.ActivateConstructionPlacement("Fence_House1");
                break;
            case "Fence_House2(Clone)":
                ConstructionManager.Instance.ActivateConstructionPlacement("Fence_House2");
                break;
            case "Fence_House2":
                ConstructionManager.Instance.ActivateConstructionPlacement("Fence_House2");
                break;
            case "Fence_House3(Clone)":
                ConstructionManager.Instance.ActivateConstructionPlacement("Fence_House3");
                break;
            case "Fence_House3":
                ConstructionManager.Instance.ActivateConstructionPlacement("Fence_House3");
                break;
            case "Fence_Guard(Clone)":
                ConstructionManager.Instance.ActivateConstructionPlacement("Fence_Guard");
                break;
            case "Fence_Guard":
                ConstructionManager.Instance.ActivateConstructionPlacement("Fence_Guard");
                break;
            case "Fence_GuardRoom(Clone)":
                ConstructionManager.Instance.ActivateConstructionPlacement("Fence_GuardRoom");
                break;
            case "Fence_GuardRoom":
                ConstructionManager.Instance.ActivateConstructionPlacement("Fence_GuardRoom");
                break;

            case "Tree_Conifer(Clone)":
                ConstructionManager.Instance.ActivateConstructionPlacement("Tree_Conifer");
                break;
            case "Tree_Conifer":
                ConstructionManager.Instance.ActivateConstructionPlacement("Tree_Conifer");
                break;
            case "Tree_Cypress(Clone)":
                ConstructionManager.Instance.ActivateConstructionPlacement("Tree_Cypress");
                break;
            case "Tree_Cypress":
                ConstructionManager.Instance.ActivateConstructionPlacement("Tree_Cypress");
                break;
            case "Tree_Pine(Clone)":
                ConstructionManager.Instance.ActivateConstructionPlacement("Tree_Pine");
                break;
            case "Tree_Pine":
                ConstructionManager.Instance.ActivateConstructionPlacement("Tree_Pine");
                break;

            case "SpecialArea(Clone)":
                string itemToPlaced = MissionSystem.Instance.itemToPlaced;
                ConstructionManager.Instance.ActivateConstructionPlacement(itemToPlaced);
                break;
            case "SpecialArea":

                break;
            default:
                break;
        }
    }

    // For consumable items after consuming it affect to player satatus
    public void consumingFunction(float healthEffect, float caloriesEffect, float hydrationEffect)
    {
        itemInfoUI.SetActive(false);

        healthEffectCalculation(healthEffect);

        caloriesEffectCalculation(caloriesEffect);

        hydrationEffectCalculation(hydrationEffect);

    }


    private static void healthEffectCalculation(float healthEffect)
    {
        // --- Health --- //

        float healthBeforeConsumption = PlayerState.Instance.currentHealth;
        float maxHealth = PlayerState.Instance.maxHealth;

        if (healthEffect != 0)
        {
            if ((healthBeforeConsumption + healthEffect) > maxHealth)
            {
                PlayerState.Instance.setHealth(maxHealth);
            }
            else
            {
                PlayerState.Instance.setHealth(healthBeforeConsumption + healthEffect);
            }
        }
    }


    private static void caloriesEffectCalculation(float caloriesEffect)
    {
        // --- Calories --- //

        float caloriesBeforeConsumption = PlayerState.Instance.currentCalories;
        float maxCalories = PlayerState.Instance.maxCalories;

        if (caloriesEffect != 0)
        {
            if ((caloriesBeforeConsumption + caloriesEffect) > maxCalories)
            {
                PlayerState.Instance.setCalories(maxCalories);
            }
            else
            {
                PlayerState.Instance.setCalories(caloriesBeforeConsumption + caloriesEffect);
            }
        }
    }


    private static void hydrationEffectCalculation(float hydrationEffect)
    {
        // --- Hydration --- //

        float hydrationBeforeConsumption = PlayerState.Instance.currentHydrationPercent;
        float maxHydration = PlayerState.Instance.maxHydrationPercent;

        if (hydrationEffect != 0)
        {
            if ((hydrationBeforeConsumption + hydrationEffect) > maxHydration)
            {
                PlayerState.Instance.setHydration(maxHydration);
            }
            else
            {
                PlayerState.Instance.setHydration(hydrationBeforeConsumption + hydrationEffect);
            }
        }
    }


}