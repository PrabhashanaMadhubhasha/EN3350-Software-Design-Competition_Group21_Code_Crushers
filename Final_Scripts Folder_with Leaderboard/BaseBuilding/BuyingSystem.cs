using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyingSystem : MonoBehaviour
{
    [Header("Weapons Fab")]
    public RaycastWeapon rifleWeaponFab;
    public RaycastWeapon pistolWeaponFab;

    public GameObject player;
    public AmmoWidget ammoWidget;

    [Header("Main Screen UIs")]
    public GameObject marketPlaceScreenUI;
    public GameObject fundsScreenUI, materialsScreenUI, weaponScreenUI, sellAssetsScreenUI;

    public List<string> inventoryItemList = new List<string>();

    [Header("Category Buttons")]
    Button fundsBTN, materialsBTN, weaponBTN, sellAssetsBTN;
    Button getFundsBTN, buyPlankBTN, buyBlock_StoneBTN, buySeedBTN, buyRifle_WeaponBTN, buyPistol_WeaponBTN, buyAmmoBTN, sellElectricityBTN, sellWaterBTN, sellFoodBTN;

    [Header("Requirement Text")]
    Text BuyPlankReq1, BuyBlock_StoneReq1, BuySeedReq1, BuyRifle_WeaponReq1, BuyPistol_WeaponReq1, BuyAmmoReq1;

    //Items to Buy
    public List<itemCost> itemsToBuy = new List<itemCost>();

    [Header("Funds Related")]
    public int fundsAmount;
    public int fundForUnitMark = 100;

    [Header("Factors for power selling")]
    public float factorForEnergyPowerSelling;
    public float factorForWaterCapacitySelling;
    public float factorForFoodMassSelling;

    [Header("Some conditions")]
    public bool isOpen;
    public bool openMarketPlace;

    public bool isRifleWasBought;
    public bool isPistolWasBought;

    [Header("Ammo Related")]
    public int totalAmmoCount = 0;
    public int ammoInMagazine;

    public static BuyingSystem Instance { get; set; }


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

        isOpen = false;

        // Get all the button and et the functionlity for each
        fundsBTN = marketPlaceScreenUI.transform.Find("GetFundsButton").GetComponent<Button>();
        fundsBTN.onClick.AddListener(delegate { OpenFundsCategory(); });

        materialsBTN = marketPlaceScreenUI.transform.Find("BuyMaterialsButton").GetComponent<Button>();
        materialsBTN.onClick.AddListener(delegate { OpenMaterialsCategory(); });

        weaponBTN = marketPlaceScreenUI.transform.Find("BuyWeaponsButton").GetComponent<Button>();
        weaponBTN.onClick.AddListener(delegate { OpenWeaponsCategory(); });

        sellAssetsBTN = marketPlaceScreenUI.transform.Find("SellAssetsButton").GetComponent<Button>();
        sellAssetsBTN.onClick.AddListener(delegate { OpenSellAssetsCategory(); });

        // Funds
        getFundsBTN = fundsScreenUI.transform.Find("Coreland_Funds").transform.Find("Button").GetComponent<Button>();
        getFundsBTN.onClick.AddListener(delegate { GetFunds(); });

        // Plank
        BuyPlankReq1 = materialsScreenUI.transform.Find("Plank").transform.Find("req1").GetComponent<Text>();

        buyPlankBTN = materialsScreenUI.transform.Find("Plank").transform.Find("Button").GetComponent<Button>();
        buyPlankBTN.onClick.AddListener(delegate { BuyAnyMaterial(itemsToBuy[0]); });

        // Block Stone
        BuyBlock_StoneReq1 = materialsScreenUI.transform.Find("Block_Stone").transform.Find("req1").GetComponent<Text>();

        buyBlock_StoneBTN = materialsScreenUI.transform.Find("Block_Stone").transform.Find("Button").GetComponent<Button>();
        buyBlock_StoneBTN.onClick.AddListener(delegate { BuyAnyMaterial(itemsToBuy[1]); });

        // Seed
        BuySeedReq1 = materialsScreenUI.transform.Find("Seed").transform.Find("req1").GetComponent<Text>();

        buySeedBTN = materialsScreenUI.transform.Find("Seed").transform.Find("Button").GetComponent<Button>();
        buySeedBTN.onClick.AddListener(delegate { BuyAnyMaterial(itemsToBuy[2]); });

        // Rifle Weapon
        BuyRifle_WeaponReq1 = weaponScreenUI.transform.Find("Rifle_Weapon").transform.Find("req1").GetComponent<Text>();

        buyRifle_WeaponBTN = weaponScreenUI.transform.Find("Rifle_Weapon").transform.Find("Button").GetComponent<Button>();
        buyRifle_WeaponBTN.onClick.AddListener(delegate { BuyAnyWeapon(itemsToBuy[3], rifleWeaponFab); });

        // Pistol Weapon
        BuyPistol_WeaponReq1 = weaponScreenUI.transform.Find("Pistol_Weapon").transform.Find("req1").GetComponent<Text>();

        buyPistol_WeaponBTN = weaponScreenUI.transform.Find("Pistol_Weapon").transform.Find("Button").GetComponent<Button>();
        buyPistol_WeaponBTN.onClick.AddListener(delegate { BuyAnyWeapon(itemsToBuy[4], pistolWeaponFab); });

        // Ammo
        BuyAmmoReq1 = weaponScreenUI.transform.Find("Ammo").transform.Find("req1").GetComponent<Text>();

        buyAmmoBTN = weaponScreenUI.transform.Find("Ammo").transform.Find("Button").GetComponent<Button>();
        buyAmmoBTN.onClick.AddListener(delegate { BuyAmmo(itemsToBuy[5]); });

        // Sell Electricity
        sellElectricityBTN = sellAssetsScreenUI.transform.Find("Electricity").transform.Find("Button").GetComponent<Button>();
        sellElectricityBTN.onClick.AddListener(delegate { AssetsManager.Instance.SellElectricity(); });

        // Sell Water
        sellWaterBTN = sellAssetsScreenUI.transform.Find("Water").transform.Find("Button").GetComponent<Button>();
        sellWaterBTN.onClick.AddListener(delegate { AssetsManager.Instance.SellWaterCapacity(); });

        // Sell Food
        sellFoodBTN = sellAssetsScreenUI.transform.Find("Food").transform.Find("Button").GetComponent<Button>();
        sellFoodBTN.onClick.AddListener(delegate { AssetsManager.Instance.SellFoodMass(); });

        RefreshNeededItems();

    }

    void OpenFundsCategory()
    {
        marketPlaceScreenUI.SetActive(false);

        fundsScreenUI.SetActive(true);

        materialsScreenUI.SetActive(false);
        weaponScreenUI.SetActive(false);
        sellAssetsScreenUI.SetActive(false);    
    }

    void OpenMaterialsCategory()
    {
        marketPlaceScreenUI.SetActive(false);
        fundsScreenUI.SetActive(false);

        materialsScreenUI.SetActive(true);

        weaponScreenUI.SetActive(false);
        sellAssetsScreenUI.SetActive(false);
    }

    void OpenWeaponsCategory()
    {
        marketPlaceScreenUI.SetActive(false);
        fundsScreenUI.SetActive(false);
        materialsScreenUI.SetActive(false);

        weaponScreenUI.SetActive(true);

        sellAssetsScreenUI.SetActive(false);
    }

    void OpenSellAssetsCategory()
    {
        marketPlaceScreenUI.SetActive(false);
        fundsScreenUI.SetActive(false);
        materialsScreenUI.SetActive(false);
        weaponScreenUI.SetActive(false);

        sellAssetsScreenUI.SetActive(true);
    }

    // Get funds from the coreland by the agent
    void GetFunds()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.craftingSound);

        AssetsManager.Instance.currentCoins += fundsAmount;
        fundsAmount = 0;
        AssetsManager.Instance.fundCoinsBar.text = $"{fundsAmount}";
        AssetsManager.Instance.SetCurrentCoins();
    }

    // Initially upon quiz marks Get funds from the coreland by the agent
    public void AddFundsCoinsUponQuizMarks(int marks)
    {
        fundsAmount += marks * fundForUnitMark;
        AssetsManager.Instance.fundCoinsBar.text = $"{fundsAmount}";
    }

    // Buy materials from the coreland by the agent
    void BuyAnyMaterial(itemCost item)
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.craftingSound);

        StartCoroutine(buoughtDelayForSound(item.itemName));

        StartCoroutine(calculate());
        AssetsManager.Instance.currentCoins -= item.cost;
        AssetsManager.Instance.SetCurrentCoins();
    }

    // Buy weapons from the coreland by the agent
    void BuyAnyWeapon(itemCost item, RaycastWeapon weaponFab)
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.craftingSound);

        if(item.itemName == itemsToBuy[3].itemName)
        {
            isRifleWasBought = true;    
        }
        if (item.itemName == itemsToBuy[4].itemName)
        {
            isPistolWasBought = true;
        }

        StartCoroutine(buoughtDelayForSound(item.itemName));

        StartCoroutine(calculate());
        InstantiateBoughtWeapon(weaponFab);
        AssetsManager.Instance.currentCoins -= item.cost;
        AssetsManager.Instance.SetCurrentCoins();
    }

    // Buy Ammo
    void BuyAmmo(itemCost item)
    {
        totalAmmoCount += ammoInMagazine;
        ammoWidget.SetTotalAmmoCount(totalAmmoCount);
    }

    // reclculte the inventory list
    public IEnumerator calculate()
    {
        yield return 0;

        InventorySystem.Instance.ReCalculateList();
        RefreshNeededItems();
    }

    IEnumerator buoughtDelayForSound(string itemName)
    {
        yield return new WaitForSeconds(1f);

        // Prodduce item
        InventorySystem.Instance.AddToInventory(itemName);

    }

    // Appear on the body of the player b, the bought weapon
    private void InstantiateBoughtWeapon(RaycastWeapon weaponFab)
    {
        ActiveWeapon activeWeapon = player.GetComponent<ActiveWeapon>();
        if (activeWeapon)
        {
            RaycastWeapon newWeapon = Instantiate(weaponFab);
            activeWeapon.Equip(newWeapon);
        }
    }

    // Update is called once per frame
    void Update()
    {
        RefreshNeededItems();

        // Set buttons active or deactive upon some consitions
        if (!AssetsManager.Instance.isSellingElectricity)
        {
            sellElectricityBTN.gameObject.SetActive(true);
        }
        else
        {
            sellElectricityBTN.gameObject.SetActive(false);
        }

        if (!AssetsManager.Instance.isSellingWaterCapacity)
        {
            sellWaterBTN.gameObject.SetActive(true);
        }
        else
        {
            sellWaterBTN.gameObject.SetActive(false);
        }

        if (!AssetsManager.Instance.isSellingFoodMass)
        {
            sellFoodBTN.gameObject.SetActive(true);
        }
        else
        {
            sellFoodBTN.gameObject.SetActive(false);
        }

        // Open the marketPlace menu
        if (!WeaponWheelController.Instance.weaponWheelSelected)
        {
            if (openMarketPlace && !isOpen && !ConstructionManager.Instance.inConstructionMode && !RemoveConstruction.Instance.inRemoveConstructionMode && !DialogSystem.Instance.dialogUIActive 
                && !CraftingSystem.Instance.isOpen && !MissionObjectMenuController.Instance.isOpen)
            {
                openMarketPlace = false;

                marketPlaceScreenUI.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                SelectionManager.instance.DisableSelection();
                SelectionManager.instance.GetComponent<SelectionManager>().enabled = false;


                isOpen = true;

            }
            else if (Input.GetKeyDown(KeyCode.X) && isOpen || RemoveConstruction.Instance.isRemoveState)
            {
                marketPlaceScreenUI.SetActive(false);
                fundsScreenUI.SetActive(false);
                materialsScreenUI.SetActive(false);
                weaponScreenUI.SetActive(false);
                sellAssetsScreenUI.SetActive(false);

                if (!InventorySystem.Instance.isOpen)
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

    // Checking the reqirements and for appearing the each buttons 
    public void RefreshNeededItems()
    {
        int currentCoins = AssetsManager.Instance.currentCoins;

        inventoryItemList = InventorySystem.Instance.itemList;

        //--PLANK--//

        BuyPlankReq1.text = "" + itemsToBuy[0].cost + " Coins [" + currentCoins + "]";

        if (currentCoins >= itemsToBuy[0].cost && InventorySystem.Instance.CheckSlotsAvailable(1))
        {
            buyPlankBTN.gameObject.SetActive(true);
        }
        else
        {
            buyPlankBTN.gameObject.SetActive(false);
        }

        //--BLOCK--//

        BuyBlock_StoneReq1.text = "" + itemsToBuy[1].cost + " Coins [" + currentCoins + "]";

        if (currentCoins >= itemsToBuy[1].cost && InventorySystem.Instance.CheckSlotsAvailable(1))
        {
            buyBlock_StoneBTN.gameObject.SetActive(true);
        }
        else
        {
            buyBlock_StoneBTN.gameObject.SetActive(false);
        }

        //--SEED--//

        BuySeedReq1.text = "" + itemsToBuy[2].cost + " Coins [" + currentCoins + "]";

        if (currentCoins >= itemsToBuy[2].cost && InventorySystem.Instance.CheckSlotsAvailable(1))
        {
            buySeedBTN.gameObject.SetActive(true);
        }
        else
        {
            buySeedBTN.gameObject.SetActive(false);
        }

        //--RIFLE--//

        BuyRifle_WeaponReq1.text = "" + itemsToBuy[3].cost + " Coins [" + currentCoins + "]";

        if (currentCoins >= itemsToBuy[3].cost && InventorySystem.Instance.CheckSlotsAvailable(1) && !isRifleWasBought)
        {
            buyRifle_WeaponBTN.gameObject.SetActive(true);
        }
        else
        {
            buyRifle_WeaponBTN.gameObject.SetActive(false);
        }

        //--PISTOL--//

        BuyPistol_WeaponReq1.text = "" + itemsToBuy[4].cost + " Coins [" + currentCoins + "]";

        if (currentCoins >= itemsToBuy[4].cost && InventorySystem.Instance.CheckSlotsAvailable(1) && !isPistolWasBought)
        {
            buyPistol_WeaponBTN.gameObject.SetActive(true);
        }
        else
        {
            buyPistol_WeaponBTN.gameObject.SetActive(false);
        }

        //--AMMO--//

        BuyAmmoReq1.text = "" + itemsToBuy[5].cost + " Coins [" + currentCoins + "]";

        if (currentCoins >= itemsToBuy[5].cost && (isPistolWasBought || isRifleWasBought))
        {
            buyAmmoBTN.gameObject.SetActive(true);
        }
        else
        {
            buyAmmoBTN.gameObject.SetActive(false);
        }
    }

    [System.Serializable]
    public class itemCost
    {
        public string itemName;
        public int cost;
    }
}