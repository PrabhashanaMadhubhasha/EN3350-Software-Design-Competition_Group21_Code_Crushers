using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{
    [Header("Main Screen UIs")]
    public GameObject craftingScreenUI;
    public GameObject toolsScreenUI, survivalScreenUI, refineScreenUI, constructionScreenUI;
    public GameObject roadConstructionScreenUI, baseConstructionScreenUI, mainConstructionScreenUI, basicConstructionScreenUI, fenceConstructionScreenUI, treeConstructionScreenUI, specialConstructionScreenUI;

    public List<string> inventoryItemList = new List<string>();

    [Header("Category Buttons")]
    Button toolsBTN, survivalBTN, refineBTN, constructionBTN;
    Button roadConstructionBTN, baseConstructionBTN, mainConstructionBTN, basicConstructionBTN, fenceConstructionBTN, treeConstructionBTN, specialConstructionBTN, removeConstructionBTN;

    [Header("Craft Buttons")]
    Button craftAxeBTN, craftPlankBTN, craftBlockBTN, craftFoundationBTN, craftWallBTN, craftSpecialAreaBTN;
    Button craftRoad_MidBTN, craftRoad_EndBTN, craftRoad_JunctionBTN, craftRoad_CornerBTN, craftRoad_StairBTN;
    Button craftBase_HugeBTN, craftBase_LargeBTN, craftBase_MediumBTN, craftBase_SmallBTN, craftBase_TreeBTN;
    Button craftMain_TownHallBTN, craftMain_SchoolBTN, craftMain_HospitalBTN, craftMain_ElectricityWaterBTN, craftMain_ElectricityFanBTN, craftMain_WaterTowerBTN, craftMain_WaterWellBTN, craftMain_LightHouseBTN;
    Button craftBasic_House1BTN, craftBasic_House2BTN, craftBasic_House3BTN, craftBasic_Factory1BTN, craftBasic_Factory2BTN, craftBasic_Hotel1BTN;
    Button craftFence_ColumnBTN, craftFence_MidBTN, craftFence_GateBTN, craftFence_PoleBTN, craftFence_House1BTN, craftFence_House2BTN, craftFence_House3BTN, craftFence_GuardBTN, craftFence_GuardRoomBTN;
    Button craftConifer_TreeBTN, craftCypress_TreeBTN, craftPine_TreeBTN;

    [Header("Requirement Text")]
    Text AxeReq1, AxeReq2, PlankReq1, BlockReq1, FoundationReq1, WallReq1, specialAreaReq1, specialAreaReq2;
    Text Road_MidReq1, Road_EndReq1, Road_JunctionReq1, Road_CornerReq1, Road_StairReq1;

    Text Base_HugeReq1, Base_LargeReq1, Base_MediumReq1, Base_SmallReq1;
    Text Base_HugeReq2, Base_LargeReq2, Base_MediumReq2, Base_SmallReq2, Base_TreeReq1;
    Text Main_TownHallReq1, Main_SchoolReq1, Main_HospitalReq1, Main_ElectricityWaterReq1, Main_ElectricityFanReq1, Main_WaterTowerReq1, Main_WaterWellReq1, Main_LightHouseReq1;
    Text Main_TownHallReq2, Main_SchoolReq2, Main_HospitalReq2, Main_ElectricityWaterReq2, Main_ElectricityFanReq2, Main_WaterTowerReq2, Main_WaterWellReq2, Main_LightHouseReq2;
    Text Basic_House1Req1, Basic_House2Req1, Basic_House3Req1, Basic_Factory1Req1, Basic_Factory2Req1, Basic_Hotel1Req1;
    Text Basic_House1Req2, Basic_House2Req2, Basic_House3Req2, Basic_Factory1Req2, Basic_Factory2Req2, Basic_Hotel1Req2;

    Text Fence_ColumnReq1, Fence_MidReq1, Fence_MidReq2, Fence_GateReq1, Fence_PoleReq1, Fence_PoleReq2, Fence_House1Req1, Fence_House2Req1, Fence_House3Req1, Fence_GuardReq1, Fence_GuardRoomReq1;
    Text Conifer_TreeReq1, Cypress_TreeReq1, Pine_TreeReq1;

    public bool isOpen;

    [Header("All Blueprints")]
    public Blueprint AxeBLP = new Blueprint("Axe", 1, 2, "Stone (Small)", 1, "Stick", 1);

    public Blueprint PlankBLP = new Blueprint("Plank", 2, 1, "Tree Log", 1, "Stick", 0);

    public Blueprint BlockBLP = new Blueprint("Block Stone", 2, 1, "Stone (Small)", 1, "Stick", 0);

    public Blueprint FoundationBLP = new Blueprint("Foundation", 1, 1, "Plank", 1, "Stick", 0);

    public Blueprint WallBLP = new Blueprint("Wall", 1, 1, "Plank", 1, "Stick", 0);

    public Blueprint SpecialAreaBLP = new Blueprint("SpecialArea", 1, 2, "Plank", 0, "Tree Log", 0);

    [Header("Road Blueprints")]
    public Blueprint Road_MidBLP = new Blueprint("Road_Mid", 1, 1, "Block Stone", 4, "Tree Log", 0);
    public Blueprint Road_EndBLP = new Blueprint("Road_End", 1, 1, "Block Stone", 4, "Tree Log", 0);
    public Blueprint Road_JunctionBLP = new Blueprint("Road_Junction", 1, 1, "Block Stone", 4, "Tree Log", 0);
    public Blueprint Road_CornerBLP = new Blueprint("Road_Corner", 1, 1, "Block Stone", 4, "Tree Log", 0);
    public Blueprint Road_StairBLP = new Blueprint("Road_Stair", 1, 1, "Block Stone", 4, "Tree Log", 0);

    [Header("Base BluePrints")]
    public Blueprint Base_HugeBLP = new Blueprint("Base_Huge", 1, 2, "Block Stone", 6, "Plank", 6);
    public Blueprint Base_LargeBLP = new Blueprint("Base_Large", 1, 2, "Block Stone", 6, "Plank", 6);
    public Blueprint Base_MediumBLP = new Blueprint("Base_Medium", 1, 2, "Block Stone", 6, "Plank", 6);
    public Blueprint Base_SmallBLP = new Blueprint("Base_Small", 1, 2, "Block Stone", 6, "Plank", 6);
    public Blueprint Base_TreeBLP = new Blueprint("Base_Tree", 1, 1, "Block Stone", 2, "Plank", 0);

    [Header("Main BluePrints")]
    public Blueprint Main_TownHallBLP = new Blueprint("Main_TownHall", 1, 2, "Block Stone", 12, "Plank", 12);
    public Blueprint Main_SchoolBLP = new Blueprint("Main_School", 1, 2, "Block Stone", 8, "Plank", 8);
    public Blueprint Main_HospitalBLP = new Blueprint("Main_Hospital", 1, 2, "Block Stone", 8, "Plank", 8);
    public Blueprint Main_ElectricityWaterBLP = new Blueprint("Main_ElectricityWater", 1, 2, "Block Stone", 6, "Plank", 6);
    public Blueprint Main_ElectricityFanBLP = new Blueprint("Main_ElectricityFan", 1, 2, "Block Stone", 6, "Plank", 6);
    public Blueprint Main_WaterTowerBLP = new Blueprint("Main_WaterTower", 1, 2, "Block Stone", 6, "Plank", 6);
    public Blueprint Main_WaterWellBLP = new Blueprint("Main_WaterWell", 1, 2, "Block Stone", 6, "Plank", 6);
    public Blueprint Main_LightHouseBLP = new Blueprint("Main_LightHouse", 1, 2, "Block Stone", 8, "Plank", 8);

    [Header("Basic BluePrints")]
    public Blueprint Basic_House1BLP = new Blueprint("Basic_House1", 1, 2, "Block Stone", 6, "Plank", 6);
    public Blueprint Basic_House2BLP = new Blueprint("Basic_House2", 1, 2, "Block Stone", 6, "Plank", 6);
    public Blueprint Basic_House3BLP = new Blueprint("Basic_House3", 1, 2, "Block Stone", 6, "Plank", 6);
    public Blueprint Basic_Factory1BLP = new Blueprint("Basic_Factory1", 1, 2, "Block Stone", 6, "Plank", 6);
    public Blueprint Basic_Factory2BLP = new Blueprint("Basic_Factory2", 1, 2, "Block Stone", 6, "Plank", 6);
    public Blueprint Basic_Hotel1BLP = new Blueprint("Basic_Hotel1", 1, 2, "Block Stone", 6, "Plank", 6);

    [Header("Basic BluePrints")]
    public Blueprint Fence_ColumnBLP = new Blueprint("Fence_Column", 1, 1, "Block Stone", 4, "Tree Log", 0);
    public Blueprint Fence_MidBLP = new Blueprint("Fence_Mid", 1, 2, "Stone (Small)", 4, "Tree Log", 1);
    public Blueprint Fence_GateBLP = new Blueprint("Fence_Gate", 1, 1, "Block Stone", 4, "Tree Log", 0);
    public Blueprint Fence_PoleBLP = new Blueprint("Fence_Pole", 1, 2, "Stone (Small)", 2, "Tree Log", 1);
    public Blueprint Fence_House1BLP = new Blueprint("Fence_House1", 1, 1, "Tree Log", 1, "Stone (Small)", 0);
    public Blueprint Fence_House2BLP = new Blueprint("Fence_House2", 1, 1, "Tree Log", 1, "Stone (Small)", 0);
    public Blueprint Fence_House3BLP = new Blueprint("Fence_House3", 1, 1, "Block Stone", 4, "Tree Log", 0);
    public Blueprint Fence_GuardBLP = new Blueprint("Fence_Guard", 1, 0, "Block Stone", 0, "Tree Log", 0);
    public Blueprint Fence_GuardRoomBLP = new Blueprint("Fence_GuardRoom", 1, 0, "Block Stone", 0, "Tree Log", 0);

    //Tree BluePrints
    [Header("All Blueprints")]
    public Blueprint Conifer_TreeBLP = new Blueprint("Tree_Conifer", 1, 1, "Seed", 1, "Tree Log", 0);
    public Blueprint Cypress_TreeBLP = new Blueprint("Tree_Cypress", 1, 1, "Seed", 1, "Tree Log", 0);
    public Blueprint Pine_TreeBLP = new Blueprint("Tree_Pine", 1, 1, "Seed", 1, "Tree Log", 0);

    public bool isAxeInInventory;

    public static CraftingSystem Instance { get; set; }


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
        toolsBTN = craftingScreenUI.transform.Find("ToolsButton").GetComponent<Button>();
        toolsBTN.onClick.AddListener(delegate { OpenToolsCategory(); });

        survivalBTN = craftingScreenUI.transform.Find("SurvivalButton").GetComponent<Button>();
        survivalBTN.onClick.AddListener(delegate { OpenSurvivalCategory(); });

        refineBTN = craftingScreenUI.transform.Find("RefineButton").GetComponent<Button>();
        refineBTN.onClick.AddListener(delegate { OpenRefineCategory(); });

        constructionBTN = craftingScreenUI.transform.Find("ConstructionButton").GetComponent<Button>();           ////////////////////
        constructionBTN.onClick.AddListener(delegate { OpenConstructionCategory(); });


        // Sub Buttons of ConstructionUI
        roadConstructionBTN = constructionScreenUI.transform.Find("RoadConstructionButton").GetComponent<Button>();
        roadConstructionBTN.onClick.AddListener(delegate { OpenRoadConstructionCategory(); });

        baseConstructionBTN = constructionScreenUI.transform.Find("BaseConstructionButton").GetComponent<Button>();
        baseConstructionBTN.onClick.AddListener(delegate { OpenBaseConstructionCategory(); });

        mainConstructionBTN = constructionScreenUI.transform.Find("MainConstructionButton").GetComponent<Button>();
        mainConstructionBTN.onClick.AddListener(delegate { OpenMainConstructionCategory(); });

        basicConstructionBTN = constructionScreenUI.transform.Find("BasicConstructionButton").GetComponent<Button>();
        basicConstructionBTN.onClick.AddListener(delegate { OpenBasicRoadConstructionCategory(); });

        fenceConstructionBTN = constructionScreenUI.transform.Find("FenceConstructionButton").GetComponent<Button>();
        fenceConstructionBTN.onClick.AddListener(delegate { OpenFenceConstructionCategory(); });

        treeConstructionBTN = constructionScreenUI.transform.Find("TreeConstructionButton").GetComponent<Button>();
        treeConstructionBTN.onClick.AddListener(delegate { OpenTreeConstructionCategory(); });

        specialConstructionBTN = constructionScreenUI.transform.Find("SpecialConstructionButton").GetComponent<Button>();
        specialConstructionBTN.onClick.AddListener(delegate { OpenSpecialConstructionCategory(); });

        removeConstructionBTN = constructionScreenUI.transform.Find("RemoveConstructionButton").GetComponent<Button>();
        removeConstructionBTN.onClick.AddListener(delegate { OpenRemoveConstructionCategory(); });


        // AXE
        AxeReq1 = toolsScreenUI.transform.Find("Axe").transform.Find("req1").GetComponent<Text>();
        AxeReq2 = toolsScreenUI.transform.Find("Axe").transform.Find("req2").GetComponent<Text>();

        craftAxeBTN = toolsScreenUI.transform.Find("Axe").transform.Find("Button").GetComponent<Button>();
        craftAxeBTN.onClick.AddListener(delegate { CraftAnyItem(AxeBLP); });

        // Plank
        PlankReq1 = refineScreenUI.transform.Find("Plank").transform.Find("req1").GetComponent<Text>();

        craftPlankBTN = refineScreenUI.transform.Find("Plank").transform.Find("Button").GetComponent<Button>();
        craftPlankBTN.onClick.AddListener(delegate { CraftAnyItem(PlankBLP); });

        // Block Stone
        BlockReq1 = refineScreenUI.transform.Find("Block Stone").transform.Find("req1").GetComponent<Text>();

        craftBlockBTN = refineScreenUI.transform.Find("Block Stone").transform.Find("Button").GetComponent<Button>();
        craftBlockBTN.onClick.AddListener(delegate { CraftAnyItem(BlockBLP); });

        // Foundation
        FoundationReq1 = specialConstructionScreenUI.transform.Find("Foundation").transform.Find("req1").GetComponent<Text>();

        craftFoundationBTN = specialConstructionScreenUI.transform.Find("Foundation").transform.Find("Button").GetComponent<Button>();
        craftFoundationBTN.onClick.AddListener(delegate { CraftAnyItem(FoundationBLP); });

        // Wall
        WallReq1 = specialConstructionScreenUI.transform.Find("Wall").transform.Find("req1").GetComponent<Text>();

        craftWallBTN = specialConstructionScreenUI.transform.Find("Wall").transform.Find("Button").GetComponent<Button>();
        craftWallBTN.onClick.AddListener(delegate { CraftAnyItem(WallBLP); });

        // SPECIALAREA
        specialAreaReq1 = specialConstructionScreenUI.transform.Find("SpecialArea").transform.Find("req1").GetComponent<Text>();
        specialAreaReq2 = specialConstructionScreenUI.transform.Find("SpecialArea").transform.Find("req2").GetComponent<Text>();

        craftSpecialAreaBTN = specialConstructionScreenUI.transform.Find("SpecialArea").transform.Find("Button").GetComponent<Button>();
        craftSpecialAreaBTN.onClick.AddListener(delegate { CraftAnyItem(SpecialAreaBLP); });

        //--ROAD--//  

        // Road_Mid
        Road_MidReq1 = roadConstructionScreenUI.transform.Find("Road_Mid").transform.Find("req1").GetComponent<Text>();

        craftRoad_MidBTN = roadConstructionScreenUI.transform.Find("Road_Mid").transform.Find("Button").GetComponent<Button>();
        craftRoad_MidBTN.onClick.AddListener(delegate { CraftAnyItem(Road_MidBLP); });

        // Road_End
        Road_EndReq1 = roadConstructionScreenUI.transform.Find("Road_End").transform.Find("req1").GetComponent<Text>();

        craftRoad_EndBTN = roadConstructionScreenUI.transform.Find("Road_End").transform.Find("Button").GetComponent<Button>();
        craftRoad_EndBTN.onClick.AddListener(delegate { CraftAnyItem(Road_EndBLP); });

        // Road_Junction
        Road_JunctionReq1 = roadConstructionScreenUI.transform.Find("Road_Junction").transform.Find("req1").GetComponent<Text>();

        craftRoad_JunctionBTN = roadConstructionScreenUI.transform.Find("Road_Junction").transform.Find("Button").GetComponent<Button>();
        craftRoad_JunctionBTN.onClick.AddListener(delegate { CraftAnyItem(Road_JunctionBLP); });

        // Road_Corner
        Road_CornerReq1 = roadConstructionScreenUI.transform.Find("Road_Corner").transform.Find("req1").GetComponent<Text>();

        craftRoad_CornerBTN = roadConstructionScreenUI.transform.Find("Road_Corner").transform.Find("Button").GetComponent<Button>();
        craftRoad_CornerBTN.onClick.AddListener(delegate { CraftAnyItem(Road_CornerBLP); });

        // Road_Stair
        Road_StairReq1 = roadConstructionScreenUI.transform.Find("Road_Stair").transform.Find("req1").GetComponent<Text>();

        craftRoad_StairBTN = roadConstructionScreenUI.transform.Find("Road_Stair").transform.Find("Button").GetComponent<Button>();
        craftRoad_StairBTN.onClick.AddListener(delegate { CraftAnyItem(Road_StairBLP); });

        //--BASE--//

        // Base_Huge
        Base_HugeReq1 = baseConstructionScreenUI.transform.Find("Base_Huge").transform.Find("req1").GetComponent<Text>();
        Base_HugeReq2 = baseConstructionScreenUI.transform.Find("Base_Huge").transform.Find("req2").GetComponent<Text>();

        craftBase_HugeBTN = baseConstructionScreenUI.transform.Find("Base_Huge").transform.Find("Button").GetComponent<Button>();
        craftBase_HugeBTN.onClick.AddListener(delegate { CraftAnyItem(Base_HugeBLP); });

        // Base_Large
        Base_LargeReq1 = baseConstructionScreenUI.transform.Find("Base_Large").transform.Find("req1").GetComponent<Text>();
        Base_LargeReq2 = baseConstructionScreenUI.transform.Find("Base_Large").transform.Find("req2").GetComponent<Text>();

        craftBase_LargeBTN = baseConstructionScreenUI.transform.Find("Base_Large").transform.Find("Button").GetComponent<Button>();
        craftBase_LargeBTN.onClick.AddListener(delegate { CraftAnyItem(Base_LargeBLP); });

        // Base_Medium
        Base_MediumReq1 = baseConstructionScreenUI.transform.Find("Base_Medium").transform.Find("req1").GetComponent<Text>();
        Base_MediumReq2 = baseConstructionScreenUI.transform.Find("Base_Medium").transform.Find("req2").GetComponent<Text>();

        craftBase_MediumBTN = baseConstructionScreenUI.transform.Find("Base_Medium").transform.Find("Button").GetComponent<Button>();
        craftBase_MediumBTN.onClick.AddListener(delegate { CraftAnyItem(Base_MediumBLP); });

        // Base_Small
        Base_SmallReq1 = baseConstructionScreenUI.transform.Find("Base_Small").transform.Find("req1").GetComponent<Text>();
        Base_SmallReq2 = baseConstructionScreenUI.transform.Find("Base_Small").transform.Find("req2").GetComponent<Text>();

        craftBase_SmallBTN = baseConstructionScreenUI.transform.Find("Base_Small").transform.Find("Button").GetComponent<Button>();
        craftBase_SmallBTN.onClick.AddListener(delegate { CraftAnyItem(Base_SmallBLP); });

        // Base_Tree
        Base_TreeReq1 = baseConstructionScreenUI.transform.Find("Base_Tree").transform.Find("req1").GetComponent<Text>();

        craftBase_TreeBTN = baseConstructionScreenUI.transform.Find("Base_Tree").transform.Find("Button").GetComponent<Button>();
        craftBase_TreeBTN.onClick.AddListener(delegate { CraftAnyItem(Base_TreeBLP); });

        //--MAIN--//

        // Main_TownHall
        Main_TownHallReq1 = mainConstructionScreenUI.transform.Find("Main_TownHall").transform.Find("req1").GetComponent<Text>();
        Main_TownHallReq2 = mainConstructionScreenUI.transform.Find("Main_TownHall").transform.Find("req2").GetComponent<Text>();

        craftMain_TownHallBTN = mainConstructionScreenUI.transform.Find("Main_TownHall").transform.Find("Button").GetComponent<Button>();
        craftMain_TownHallBTN.onClick.AddListener(delegate { CraftAnyItem(Main_TownHallBLP); });

        // Main_School
        Main_SchoolReq1 = mainConstructionScreenUI.transform.Find("Main_School").transform.Find("req1").GetComponent<Text>();
        Main_SchoolReq2 = mainConstructionScreenUI.transform.Find("Main_School").transform.Find("req2").GetComponent<Text>();

        craftMain_SchoolBTN = mainConstructionScreenUI.transform.Find("Main_School").transform.Find("Button").GetComponent<Button>();
        craftMain_SchoolBTN.onClick.AddListener(delegate { CraftAnyItem(Main_SchoolBLP); });

        // Main_Hospital
        Main_HospitalReq1 = mainConstructionScreenUI.transform.Find("Main_Hospital").transform.Find("req1").GetComponent<Text>();
        Main_HospitalReq2 = mainConstructionScreenUI.transform.Find("Main_Hospital").transform.Find("req2").GetComponent<Text>();

        craftMain_HospitalBTN = mainConstructionScreenUI.transform.Find("Main_Hospital").transform.Find("Button").GetComponent<Button>();
        craftMain_HospitalBTN.onClick.AddListener(delegate { CraftAnyItem(Main_HospitalBLP); });

        // Main_ElectricityWater
        Main_ElectricityWaterReq1 = mainConstructionScreenUI.transform.Find("Main_ElectricityWater").transform.Find("req1").GetComponent<Text>();
        Main_ElectricityWaterReq2 = mainConstructionScreenUI.transform.Find("Main_ElectricityWater").transform.Find("req2").GetComponent<Text>();

        craftMain_ElectricityWaterBTN = mainConstructionScreenUI.transform.Find("Main_ElectricityWater").transform.Find("Button").GetComponent<Button>();
        craftMain_ElectricityWaterBTN.onClick.AddListener(delegate { CraftAnyItem(Main_ElectricityWaterBLP); });

        // Main_ElectricityFan
        Main_ElectricityFanReq1 = mainConstructionScreenUI.transform.Find("Main_ElectricityFan").transform.Find("req1").GetComponent<Text>();
        Main_ElectricityFanReq2 = mainConstructionScreenUI.transform.Find("Main_ElectricityFan").transform.Find("req2").GetComponent<Text>();

        craftMain_ElectricityFanBTN = mainConstructionScreenUI.transform.Find("Main_ElectricityFan").transform.Find("Button").GetComponent<Button>();
        craftMain_ElectricityFanBTN.onClick.AddListener(delegate { CraftAnyItem(Main_ElectricityFanBLP); });

        // Main_WaterTower
        Main_WaterTowerReq1 = mainConstructionScreenUI.transform.Find("Main_WaterTower").transform.Find("req1").GetComponent<Text>();
        Main_WaterTowerReq2 = mainConstructionScreenUI.transform.Find("Main_WaterTower").transform.Find("req2").GetComponent<Text>();

        craftMain_WaterTowerBTN = mainConstructionScreenUI.transform.Find("Main_WaterTower").transform.Find("Button").GetComponent<Button>();
        craftMain_WaterTowerBTN.onClick.AddListener(delegate { CraftAnyItem(Main_WaterTowerBLP); });

        // Main_WaterWell
        Main_WaterWellReq1 = mainConstructionScreenUI.transform.Find("Main_WaterWell").transform.Find("req1").GetComponent<Text>();
        Main_WaterWellReq2 = mainConstructionScreenUI.transform.Find("Main_WaterWell").transform.Find("req2").GetComponent<Text>();

        craftMain_WaterWellBTN = mainConstructionScreenUI.transform.Find("Main_WaterWell").transform.Find("Button").GetComponent<Button>();
        craftMain_WaterWellBTN.onClick.AddListener(delegate { CraftAnyItem(Main_WaterWellBLP); });

        // Main_LightHouse
        Main_LightHouseReq1 = mainConstructionScreenUI.transform.Find("Main_LightHouse").transform.Find("req1").GetComponent<Text>();
        Main_LightHouseReq2 = mainConstructionScreenUI.transform.Find("Main_LightHouse").transform.Find("req2").GetComponent<Text>();

        craftMain_LightHouseBTN = mainConstructionScreenUI.transform.Find("Main_LightHouse").transform.Find("Button").GetComponent<Button>();
        craftMain_LightHouseBTN.onClick.AddListener(delegate { CraftAnyItem(Main_LightHouseBLP); });

        //--BASIC--//

        // Basic_House1
        Basic_House1Req1 = basicConstructionScreenUI.transform.Find("Basic_House1").transform.Find("req1").GetComponent<Text>();
        Basic_House1Req2 = basicConstructionScreenUI.transform.Find("Basic_House1").transform.Find("req2").GetComponent<Text>();

        craftBasic_House1BTN = basicConstructionScreenUI.transform.Find("Basic_House1").transform.Find("Button").GetComponent<Button>();
        craftBasic_House1BTN.onClick.AddListener(delegate { CraftAnyItem(Basic_House1BLP); });

        // Basic_House2
        Basic_House2Req1 = basicConstructionScreenUI.transform.Find("Basic_House2").transform.Find("req1").GetComponent<Text>();
        Basic_House2Req2 = basicConstructionScreenUI.transform.Find("Basic_House2").transform.Find("req2").GetComponent<Text>();

        craftBasic_House2BTN = basicConstructionScreenUI.transform.Find("Basic_House2").transform.Find("Button").GetComponent<Button>();
        craftBasic_House2BTN.onClick.AddListener(delegate { CraftAnyItem(Basic_House2BLP); });

        // Basic_House3
        Basic_House3Req1 = basicConstructionScreenUI.transform.Find("Basic_House3").transform.Find("req1").GetComponent<Text>();
        Basic_House3Req2 = basicConstructionScreenUI.transform.Find("Basic_House3").transform.Find("req2").GetComponent<Text>();

        craftBasic_House3BTN = basicConstructionScreenUI.transform.Find("Basic_House3").transform.Find("Button").GetComponent<Button>();
        craftBasic_House3BTN.onClick.AddListener(delegate { CraftAnyItem(Basic_House3BLP); });

        // Basic_Factory1
        Basic_Factory1Req1 = basicConstructionScreenUI.transform.Find("Basic_Factory1").transform.Find("req1").GetComponent<Text>();
        Basic_Factory1Req2 = basicConstructionScreenUI.transform.Find("Basic_Factory1").transform.Find("req2").GetComponent<Text>();

        craftBasic_Factory1BTN = basicConstructionScreenUI.transform.Find("Basic_Factory1").transform.Find("Button").GetComponent<Button>();
        craftBasic_Factory1BTN.onClick.AddListener(delegate { CraftAnyItem(Basic_Factory1BLP); });

        // Basic_Factory2
        Basic_Factory2Req1 = basicConstructionScreenUI.transform.Find("Basic_Factory2").transform.Find("req1").GetComponent<Text>();
        Basic_Factory2Req2 = basicConstructionScreenUI.transform.Find("Basic_Factory2").transform.Find("req2").GetComponent<Text>();

        craftBasic_Factory2BTN = basicConstructionScreenUI.transform.Find("Basic_Factory2").transform.Find("Button").GetComponent<Button>();
        craftBasic_Factory2BTN.onClick.AddListener(delegate { CraftAnyItem(Basic_Factory2BLP); });

        // Basic_Hotel1
        Basic_Hotel1Req1 = basicConstructionScreenUI.transform.Find("Basic_Hotel1").transform.Find("req1").GetComponent<Text>();
        Basic_Hotel1Req2 = basicConstructionScreenUI.transform.Find("Basic_Hotel1").transform.Find("req2").GetComponent<Text>();

        craftBasic_Hotel1BTN = basicConstructionScreenUI.transform.Find("Basic_Hotel1").transform.Find("Button").GetComponent<Button>();
        craftBasic_Hotel1BTN.onClick.AddListener(delegate { CraftAnyItem(Basic_Hotel1BLP); });

        //--FENCE--//

        // Fence_Column
        Fence_ColumnReq1 = fenceConstructionScreenUI.transform.Find("Fence_Column").transform.Find("req1").GetComponent<Text>();

        craftFence_ColumnBTN = fenceConstructionScreenUI.transform.Find("Fence_Column").transform.Find("Button").GetComponent<Button>();
        craftFence_ColumnBTN.onClick.AddListener(delegate { CraftAnyItem(Fence_ColumnBLP); });

        // Fence_Gate
        Fence_GateReq1 = fenceConstructionScreenUI.transform.Find("Fence_Gate").transform.Find("req1").GetComponent<Text>();

        craftFence_GateBTN = fenceConstructionScreenUI.transform.Find("Fence_Gate").transform.Find("Button").GetComponent<Button>();
        craftFence_GateBTN.onClick.AddListener(delegate { CraftAnyItem(Fence_GateBLP); });

        // Fence_Mid
        Fence_MidReq1 = fenceConstructionScreenUI.transform.Find("Fence_Mid").transform.Find("req1").GetComponent<Text>();
        Fence_MidReq2 = fenceConstructionScreenUI.transform.Find("Fence_Mid").transform.Find("req2").GetComponent<Text>();

        craftFence_MidBTN = fenceConstructionScreenUI.transform.Find("Fence_Mid").transform.Find("Button").GetComponent<Button>();
        craftFence_MidBTN.onClick.AddListener(delegate { CraftAnyItem(Fence_MidBLP); });

        // Fence_Pole
        Fence_PoleReq1 = fenceConstructionScreenUI.transform.Find("Fence_Pole").transform.Find("req1").GetComponent<Text>();
        Fence_PoleReq2 = fenceConstructionScreenUI.transform.Find("Fence_Pole").transform.Find("req2").GetComponent<Text>();

        craftFence_PoleBTN = fenceConstructionScreenUI.transform.Find("Fence_Pole").transform.Find("Button").GetComponent<Button>();
        craftFence_PoleBTN.onClick.AddListener(delegate { CraftAnyItem(Fence_PoleBLP); });

        // Fence_House1
        Fence_House1Req1 = fenceConstructionScreenUI.transform.Find("Fence_House1").transform.Find("req1").GetComponent<Text>();

        craftFence_House1BTN = fenceConstructionScreenUI.transform.Find("Fence_House1").transform.Find("Button").GetComponent<Button>();
        craftFence_House1BTN.onClick.AddListener(delegate { CraftAnyItem(Fence_House1BLP); });

        // Fence_House2
        Fence_House2Req1 = fenceConstructionScreenUI.transform.Find("Fence_House2").transform.Find("req1").GetComponent<Text>();

        craftFence_House2BTN = fenceConstructionScreenUI.transform.Find("Fence_House2").transform.Find("Button").GetComponent<Button>();
        craftFence_House2BTN.onClick.AddListener(delegate { CraftAnyItem(Fence_House2BLP); });

        // Fence_House3
        Fence_House3Req1 = fenceConstructionScreenUI.transform.Find("Fence_House3").transform.Find("req1").GetComponent<Text>();

        craftFence_House3BTN = fenceConstructionScreenUI.transform.Find("Fence_House3").transform.Find("Button").GetComponent<Button>();
        craftFence_House3BTN.onClick.AddListener(delegate { CraftAnyItem(Fence_House3BLP); });

        // Fence_Guard
        Fence_GuardReq1 = fenceConstructionScreenUI.transform.Find("Fence_Guard").transform.Find("req1").GetComponent<Text>();

        craftFence_GuardBTN = fenceConstructionScreenUI.transform.Find("Fence_Guard").transform.Find("Button").GetComponent<Button>();
        craftFence_GuardBTN.onClick.AddListener(delegate { CraftAnyItem(Fence_GuardBLP); });

        // Fence_GuardRoom
        Fence_GuardRoomReq1 = fenceConstructionScreenUI.transform.Find("Fence_GuardRoom").transform.Find("req1").GetComponent<Text>();

        craftFence_GuardRoomBTN = fenceConstructionScreenUI.transform.Find("Fence_GuardRoom").transform.Find("Button").GetComponent<Button>();
        craftFence_GuardRoomBTN.onClick.AddListener(delegate { CraftAnyItem(Fence_GuardRoomBLP); });

        //--TREE--//

        // Conifer_Tree
        Conifer_TreeReq1 = treeConstructionScreenUI.transform.Find("Conifer_Tree").transform.Find("req1").GetComponent<Text>();

        craftConifer_TreeBTN = treeConstructionScreenUI.transform.Find("Conifer_Tree").transform.Find("Button").GetComponent<Button>();
        craftConifer_TreeBTN.onClick.AddListener(delegate { CraftAnyItem(Conifer_TreeBLP); });

        // Cypress_Tree
        Cypress_TreeReq1 = treeConstructionScreenUI.transform.Find("Cypress_Tree").transform.Find("req1").GetComponent<Text>();

        craftCypress_TreeBTN = treeConstructionScreenUI.transform.Find("Cypress_Tree").transform.Find("Button").GetComponent<Button>();
        craftCypress_TreeBTN.onClick.AddListener(delegate { CraftAnyItem(Cypress_TreeBLP); });

        // Pine_Tree
        Pine_TreeReq1 = treeConstructionScreenUI.transform.Find("Pine_Tree").transform.Find("req1").GetComponent<Text>();

        craftPine_TreeBTN = treeConstructionScreenUI.transform.Find("Pine_Tree").transform.Find("Button").GetComponent<Button>();
        craftPine_TreeBTN.onClick.AddListener(delegate { CraftAnyItem(Pine_TreeBLP); });

        RefreshNeededItems();
    }


    void OpenToolsCategory()
    {
        craftingScreenUI.SetActive(false);

        toolsScreenUI.SetActive(true);

        survivalScreenUI.SetActive(false);
        refineScreenUI.SetActive(false);
        constructionScreenUI.SetActive(false);
        roadConstructionScreenUI.SetActive(false);
        baseConstructionScreenUI.SetActive(false);
        mainConstructionScreenUI.SetActive(false);
        basicConstructionScreenUI.SetActive(false);
        fenceConstructionScreenUI.SetActive(false);
        treeConstructionScreenUI.SetActive(false);
        specialConstructionScreenUI.SetActive(false);
    }

    void OpenSurvivalCategory()
    {
        craftingScreenUI.SetActive(false);
        toolsScreenUI.SetActive(false);

        survivalScreenUI.SetActive(true);

        refineScreenUI.SetActive(false);
        constructionScreenUI.SetActive(false);
        roadConstructionScreenUI.SetActive(false);
        baseConstructionScreenUI.SetActive(false);
        mainConstructionScreenUI.SetActive(false);
        basicConstructionScreenUI.SetActive(false);
        fenceConstructionScreenUI.SetActive(false);
        treeConstructionScreenUI.SetActive(false);
        specialConstructionScreenUI.SetActive(false);
    }

    void OpenRefineCategory()
    {
        craftingScreenUI.SetActive(false);
        toolsScreenUI.SetActive(false);
        survivalScreenUI.SetActive(false);

        refineScreenUI.SetActive(true);

        constructionScreenUI.SetActive(false);
        roadConstructionScreenUI.SetActive(false);
        baseConstructionScreenUI.SetActive(false);
        mainConstructionScreenUI.SetActive(false);
        basicConstructionScreenUI.SetActive(false);
        fenceConstructionScreenUI.SetActive(false);
        treeConstructionScreenUI.SetActive(false);
        specialConstructionScreenUI.SetActive(false);
    }

    void OpenConstructionCategory()
    {
        craftingScreenUI.SetActive(false);
        toolsScreenUI.SetActive(false);
        survivalScreenUI.SetActive(false);
        refineScreenUI.SetActive(false);

        constructionScreenUI.SetActive(true);

        roadConstructionScreenUI.SetActive(false);
        baseConstructionScreenUI.SetActive(false);
        mainConstructionScreenUI.SetActive(false);
        basicConstructionScreenUI.SetActive(false);
        fenceConstructionScreenUI.SetActive(false);
        treeConstructionScreenUI.SetActive(false);
        specialConstructionScreenUI.SetActive(false);
    }

    void OpenRoadConstructionCategory()
    {
        craftingScreenUI.SetActive(false);
        toolsScreenUI.SetActive(false);
        survivalScreenUI.SetActive(false);
        refineScreenUI.SetActive(false);
        constructionScreenUI.SetActive(false);

        roadConstructionScreenUI.SetActive(true);

        mainConstructionScreenUI.SetActive(false);
        basicConstructionScreenUI.SetActive(false);
        fenceConstructionScreenUI.SetActive(false);
        treeConstructionScreenUI.SetActive(false);
        specialConstructionScreenUI.SetActive(false);   
    }

    void OpenBaseConstructionCategory()
    {
        craftingScreenUI.SetActive(false);
        toolsScreenUI.SetActive(false);
        survivalScreenUI.SetActive(false);
        refineScreenUI.SetActive(false);
        constructionScreenUI.SetActive(false);
        roadConstructionScreenUI.SetActive(false);
        baseConstructionScreenUI.SetActive(false);

        baseConstructionScreenUI.SetActive(true);

        mainConstructionScreenUI.SetActive(false);
        basicConstructionScreenUI.SetActive(false);
        fenceConstructionScreenUI.SetActive(false);
        treeConstructionScreenUI.SetActive(false);
        specialConstructionScreenUI.SetActive(false);
    }

    void OpenMainConstructionCategory()
    {
        craftingScreenUI.SetActive(false);
        toolsScreenUI.SetActive(false);
        survivalScreenUI.SetActive(false);
        refineScreenUI.SetActive(false);
        constructionScreenUI.SetActive(false);
        roadConstructionScreenUI.SetActive(false);
        baseConstructionScreenUI.SetActive(false);

        mainConstructionScreenUI.SetActive(true);

        basicConstructionScreenUI.SetActive(false);
        fenceConstructionScreenUI.SetActive(false);
        treeConstructionScreenUI.SetActive(false);
        specialConstructionScreenUI.SetActive(false);
    }

    void OpenBasicRoadConstructionCategory()
    {
        craftingScreenUI.SetActive(false);
        toolsScreenUI.SetActive(false);
        survivalScreenUI.SetActive(false);
        refineScreenUI.SetActive(false);
        constructionScreenUI.SetActive(false);
        roadConstructionScreenUI.SetActive(false);
        baseConstructionScreenUI.SetActive(false);
        mainConstructionScreenUI.SetActive(false);

        basicConstructionScreenUI.SetActive(true);

        fenceConstructionScreenUI.SetActive(false);
        treeConstructionScreenUI.SetActive(false);
        specialConstructionScreenUI.SetActive(false);
    }

    void OpenFenceConstructionCategory()
    {
        craftingScreenUI.SetActive(false);
        toolsScreenUI.SetActive(false);
        survivalScreenUI.SetActive(false);
        refineScreenUI.SetActive(false);
        constructionScreenUI.SetActive(false);
        roadConstructionScreenUI.SetActive(false);
        baseConstructionScreenUI.SetActive(false);
        mainConstructionScreenUI.SetActive(false);
        basicConstructionScreenUI.SetActive(false);

        fenceConstructionScreenUI.SetActive(true);

        treeConstructionScreenUI.SetActive(false);
        specialConstructionScreenUI.SetActive(false);
    }

    void OpenTreeConstructionCategory()
    {
        craftingScreenUI.SetActive(false);
        toolsScreenUI.SetActive(false);
        survivalScreenUI.SetActive(false);
        refineScreenUI.SetActive(false);
        constructionScreenUI.SetActive(false);
        roadConstructionScreenUI.SetActive(false);
        baseConstructionScreenUI.SetActive(false);
        mainConstructionScreenUI.SetActive(false);
        basicConstructionScreenUI.SetActive(false);
        fenceConstructionScreenUI.SetActive(false);

        treeConstructionScreenUI.SetActive(true);

        specialConstructionScreenUI.SetActive(false);
    }

    void OpenSpecialConstructionCategory()
    {
        craftingScreenUI.SetActive(false);
        toolsScreenUI.SetActive(false);
        survivalScreenUI.SetActive(false);
        refineScreenUI.SetActive(false);
        constructionScreenUI.SetActive(false);
        roadConstructionScreenUI.SetActive(false);
        baseConstructionScreenUI.SetActive(false);
        mainConstructionScreenUI.SetActive(false);
        basicConstructionScreenUI.SetActive(false);
        fenceConstructionScreenUI.SetActive(false);
        treeConstructionScreenUI.SetActive(false);

        specialConstructionScreenUI.SetActive(true);
    }

    void OpenRemoveConstructionCategory()
    {
        RemoveConstruction.Instance.inRemoveConstructionMode = true;
        RemoveConstruction.Instance.isRemoveState = true;
    }

    // After crafting inventory should be updated 
    void CraftAnyItem(Blueprint blueprintToCraft)
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.craftingSound);

        StartCoroutine(craftedDelayForSound(blueprintToCraft));
        
        if (blueprintToCraft.itemName == "Axe")
        {
            isAxeInInventory = true;
            EquipSystem.Instance.haveDamagedAxe = false;
        }

        if (blueprintToCraft.numOfRequirements == 1 )
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1amount);
        }
        else if (blueprintToCraft.numOfRequirements == 2)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1amount);
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req2, blueprintToCraft.Req2amount);
        }

        StartCoroutine(calculate());

    }

    public IEnumerator calculate()
    {
        yield return 0;

        InventorySystem.Instance.ReCalculateList();
        RefreshNeededItems();
    }

    IEnumerator craftedDelayForSound(Blueprint blueprintToCraft)
    {
        yield return new WaitForSeconds(1f);

        // Prodduce amount of items
        for (var i = blueprintToCraft.numOfItemsToProduce - 1; i >= 0; i--)
        {
            InventorySystem.Instance.AddToInventory(blueprintToCraft.itemName);
        }
    }

    // Update is called once per frame
    void Update()
    {
        RefreshNeededItems();

        // Open the crafting Screen
        if (!WeaponWheelController.Instance.weaponWheelSelected)
        {
            if (Input.GetKeyDown(KeyCode.C) && !isOpen && !ConstructionManager.Instance.inConstructionMode && !RemoveConstruction.Instance.inRemoveConstructionMode && !DialogSystem.Instance.dialogUIActive 
                && !BuyingSystem.Instance.isOpen && !AssetsManager.Instance.isOpen && !MissionObjectMenuController.Instance.isOpen && !MenuManager.Instance.isMenuOpen)
            {

                craftingScreenUI.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
               
                SelectionManager.instance.DisableSelection();
                SelectionManager.instance.GetComponent<SelectionManager>().enabled = false;


                isOpen = true;

            }
            else if (Input.GetKeyDown(KeyCode.C) && isOpen || RemoveConstruction.Instance.isRemoveState)
            {
                craftingScreenUI.SetActive(false);
                toolsScreenUI.SetActive(false);
                survivalScreenUI.SetActive(false);
                refineScreenUI.SetActive(false);
                constructionScreenUI.SetActive(false);
                roadConstructionScreenUI.SetActive(false);
                baseConstructionScreenUI.SetActive(false);
                mainConstructionScreenUI.SetActive(false);
                basicConstructionScreenUI.SetActive(false); 
                fenceConstructionScreenUI.SetActive(false);
                treeConstructionScreenUI.SetActive(false);
                specialConstructionScreenUI.SetActive(false);

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

    // Check requirements in the inventory and open the each buttons
    public void RefreshNeededItems()
    {
        int stone_small_count = 1000;
        int stick_count = 1000;
        int tree_log_count = 1000;
        int plank_count = 1000;
        int block_stone_count = 1000;
        int seed_count = 1000;

        inventoryItemList = InventorySystem.Instance.itemList;

        foreach (string itemName in inventoryItemList)
        {
            switch (itemName)
            {
                case "Stone (Small)":
                    stone_small_count += 1;
                    break;
                case "Stick":
                    stick_count += 1;
                    break;
                case "Tree Log":
                    tree_log_count += 1;
                    break;
                case "Plank":
                    plank_count += 1;
                    break;
                case "Block Stone":
                    block_stone_count += 1;
                    break;
                case "Seed":
                    seed_count += 1;
                    break;

            }
        }

        //--AXE--//

        AxeReq1.text = "" + AxeBLP.Req1amount + " Stone [" + stone_small_count + "]";
        AxeReq2.text = "" + AxeBLP.Req2amount + " Stick [" + stick_count + "]";

        if (stone_small_count >= AxeBLP.Req1amount && stick_count >= AxeBLP.Req2amount && InventorySystem.Instance.CheckSlotsAvailable(1))
        {
            craftAxeBTN.gameObject.SetActive(true);
        }
        else
        {
            craftAxeBTN.gameObject.SetActive(false);
        }

        //--PLANK--//

        PlankReq1.text = "" + PlankBLP.Req1amount + " Tree Log [" + tree_log_count + "]";

        if (tree_log_count >= PlankBLP.Req1amount && InventorySystem.Instance.CheckSlotsAvailable(2))
        {
            craftPlankBTN.gameObject.SetActive(true);
        }
        else
        {
            craftPlankBTN.gameObject.SetActive(false);
        }

        //--BLOCK--//

        BlockReq1.text = "" + BlockBLP.Req1amount + " Stone [" + stone_small_count + "]";

        if (stone_small_count >= BlockBLP.Req1amount && InventorySystem.Instance.CheckSlotsAvailable(2))
        {
            craftBlockBTN.gameObject.SetActive(true);
        }
        else
        {
            craftBlockBTN.gameObject.SetActive(false);
        }

        //--FOUNDATION--//

        FoundationReq1.text = "" + FoundationBLP.Req1amount + " Plank [" + plank_count + "]";

        if (plank_count >= FoundationBLP.Req1amount && InventorySystem.Instance.CheckSlotsAvailable(1))
        {
            craftFoundationBTN.gameObject.SetActive(true);
        }
        else
        {
            craftFoundationBTN.gameObject.SetActive(false);
        }

        //--WALL--//

        WallReq1.text = "" + WallBLP.Req1amount + " Plank [" + plank_count + "]";

        if (plank_count >= WallBLP.Req1amount && InventorySystem.Instance.CheckSlotsAvailable(1))
        {
            craftWallBTN.gameObject.SetActive(true);
        }
        else
        {
            craftWallBTN.gameObject.SetActive(false);
        }

        //--SPECIALAREA--//

        specialAreaReq1.text = "" + SpecialAreaBLP.Req1amount + " Plank [" + plank_count + "]";
        specialAreaReq2.text = "" + SpecialAreaBLP.Req1amount + " Tree Log [" + tree_log_count + "]";
        if (plank_count >= SpecialAreaBLP.Req1amount && SpecialAreaBLP.Req2amount >= 0 && InventorySystem.Instance.CheckSlotsAvailable(1) && MissionSystem.Instance.itemToPlaced != "none")
        {
            //Debug.Log("item: ____ "+MissionSystem.Instance.itemToPlaced);
            craftSpecialAreaBTN.gameObject.SetActive(true);
        }
        else
        {
            craftSpecialAreaBTN.gameObject.SetActive(false);
        }

        //--ROAD--//
        Road_MidReq1.text = "" + Road_MidBLP.Req1amount + " Block [" + block_stone_count + "]";
        Road_EndReq1.text = "" + Road_EndBLP.Req1amount + " Block [" + block_stone_count + "]";
        Road_JunctionReq1.text = "" + Road_JunctionBLP.Req1amount + " Block [" + block_stone_count + "]";
        Road_CornerReq1.text = "" + Road_CornerBLP.Req1amount + " Block [" + block_stone_count + "]";
        Road_StairReq1.text = "" + Road_StairBLP.Req1amount + " Block [" + block_stone_count + "]";


        //--FENCE--//
        Fence_ColumnReq1.text = "" + Fence_ColumnBLP.Req1amount + " Block [" + block_stone_count + "]";
        Fence_MidReq1.text = "" + Fence_MidBLP.Req1amount + " Stone [" + stone_small_count + "]";
        Fence_MidReq2.text = "" + Fence_MidBLP.Req2amount + " Tree Log [" + tree_log_count + "]";
        Fence_GateReq1.text = "" + Fence_GateBLP.Req1amount + " Block [" + block_stone_count + "]";
        Fence_PoleReq1.text = "" + Fence_ColumnBLP.Req1amount + " Stone [" + stone_small_count + "]";
        Fence_PoleReq2.text = "" + Fence_ColumnBLP.Req1amount + " Tree Log [" + tree_log_count + "]";

        Fence_House1Req1.text = "" + Fence_ColumnBLP.Req1amount + " Tree Log [" + tree_log_count + "]";
        Fence_House2Req1.text = "" + Fence_ColumnBLP.Req1amount + " Tree Log [" + tree_log_count + "]";
        Fence_House3Req1.text = "" + Fence_ColumnBLP.Req1amount + " Block [" + block_stone_count + "]";
        //Fence_GuardReq1.text = "4 Block Stone [" + block_stone_count + "]";
        //Fence_GuardRoomReq1.text = "6 Block Stone [" + block_stone_count + "]";

        craftFence_GuardBTN.gameObject.SetActive(true);
        craftFence_GuardRoomBTN.gameObject.SetActive(true);

        if (block_stone_count >= Road_MidBLP.Req1amount && InventorySystem.Instance.CheckSlotsAvailable(1))
        {
            craftRoad_MidBTN.gameObject.SetActive(true);
            craftRoad_EndBTN.gameObject.SetActive(true);
            craftRoad_JunctionBTN.gameObject.SetActive(true);
            craftRoad_CornerBTN.gameObject.SetActive(true);
            craftRoad_StairBTN.gameObject.SetActive(true);
            craftFence_ColumnBTN.gameObject.SetActive(true);
            craftFence_GateBTN.gameObject.SetActive(true);
            craftFence_House3BTN.gameObject.SetActive(true);
        }
        else
        {
            craftRoad_MidBTN.gameObject.SetActive(false);
            craftRoad_EndBTN.gameObject.SetActive(false);
            craftRoad_JunctionBTN.gameObject.SetActive(false);
            craftRoad_CornerBTN.gameObject.SetActive(false);
            craftRoad_StairBTN.gameObject.SetActive(false);
            craftFence_ColumnBTN.gameObject.SetActive(false);
            craftFence_GateBTN.gameObject.SetActive(false);
            craftFence_House3BTN.gameObject.SetActive(false);
        }

        if (stone_small_count >= Fence_MidBLP.Req1amount && tree_log_count >= Fence_MidBLP.Req2amount && InventorySystem.Instance.CheckSlotsAvailable(1))
        {
            craftFence_MidBTN.gameObject.SetActive(true);
        }
        else
        {
            craftFence_MidBTN.gameObject.SetActive(false);
        }

        if (stone_small_count >= Fence_PoleBLP.Req1amount && tree_log_count >= Fence_PoleBLP.Req2amount && InventorySystem.Instance.CheckSlotsAvailable(1))
        {
            craftFence_PoleBTN.gameObject.SetActive(true);
        }
        else
        {
            craftFence_PoleBTN.gameObject.SetActive(false);
        }

        if (tree_log_count >= Fence_House1BLP.Req1amount && InventorySystem.Instance.CheckSlotsAvailable(1))
        {
            craftFence_House1BTN.gameObject.SetActive(true);
            craftFence_House2BTN.gameObject.SetActive(true);
        }
        else
        {
            craftFence_House1BTN.gameObject.SetActive(false);
            craftFence_House2BTN.gameObject.SetActive(false);
        }

        //--BASE--//
        Base_HugeReq1.text = "" + Base_HugeBLP.Req1amount + " Block [" + block_stone_count + "]";
        Base_HugeReq2.text = "" + Base_HugeBLP.Req2amount + " Plank [" + plank_count + "]";
        Base_LargeReq1.text = "" + Base_LargeBLP.Req1amount + " Block [" + block_stone_count + "]";
        Base_LargeReq2.text = "" + Base_LargeBLP.Req2amount + " Plank [" + plank_count + "]";
        Base_MediumReq1.text = "" + Base_MediumBLP.Req1amount + " Block [" + block_stone_count + "]";
        Base_MediumReq2.text = "" + Base_MediumBLP.Req2amount + " Plank [" + plank_count + "]";
        Base_SmallReq1.text = "" + Base_SmallBLP.Req1amount + " Block [" + block_stone_count + "]";
        Base_SmallReq2.text = "" + Base_SmallBLP.Req2amount + " Plank [" + plank_count + "]";
        Base_TreeReq1.text = "" + Base_TreeBLP.Req1amount + " Plank [" + block_stone_count + "]";

        //--MAIN--//
        Main_TownHallReq1.text = "" + Main_TownHallBLP.Req1amount + " Block [" + block_stone_count + "]";
        Main_TownHallReq2.text = "" + Main_TownHallBLP.Req2amount + " Plank [" + plank_count + "]";
        Main_SchoolReq1.text = "" + Main_SchoolBLP.Req1amount + " Block [" + block_stone_count + "]";
        Main_SchoolReq2.text = "" + Main_SchoolBLP.Req2amount + " Plank [" + plank_count + "]";
        Main_HospitalReq1.text = "" + Main_HospitalBLP.Req1amount + " Block [" + block_stone_count + "]";
        Main_HospitalReq2.text = "" + Main_HospitalBLP.Req2amount + " Plank [" + plank_count + "]";
        Main_ElectricityWaterReq1.text = "" + Main_ElectricityWaterBLP.Req1amount + " Block [" + block_stone_count + "]";
        Main_ElectricityWaterReq2.text = "" + Main_ElectricityWaterBLP.Req2amount + " Plank [" + plank_count + "]";
        Main_ElectricityFanReq1.text = "" + Main_ElectricityFanBLP.Req1amount + " Block [" + block_stone_count + "]";
        Main_ElectricityFanReq2.text = "" + Main_ElectricityFanBLP.Req2amount + " Plank [" + plank_count + "]";
        Main_WaterTowerReq1.text = "" + Main_WaterTowerBLP.Req1amount + " Block [" + block_stone_count + "]";
        Main_WaterTowerReq2.text = "" + Main_WaterTowerBLP.Req2amount + " Plank [" + plank_count + "]";
        Main_WaterWellReq1.text = "" + Main_WaterWellBLP.Req1amount + " Block [" + block_stone_count + "]";
        Main_WaterWellReq2.text = "" + Main_WaterWellBLP.Req2amount + " Plank [" + plank_count + "]";
        Main_LightHouseReq1.text = "" + Main_LightHouseBLP.Req1amount + " Block [" + block_stone_count + "]";
        Main_LightHouseReq2.text = "" + Main_LightHouseBLP.Req2amount + " Plank [" + plank_count + "]";

        //--BASIC--//
        Basic_House1Req1.text = "" + Basic_House1BLP.Req1amount + " Block [" + block_stone_count + "]";
        Basic_House1Req2.text = "" + Basic_House1BLP.Req2amount + " Plank [" + plank_count + "]";
        Basic_House2Req1.text = "" + Basic_House2BLP.Req1amount + " Block [" + block_stone_count + "]";
        Basic_House2Req2.text = "" + Basic_House2BLP.Req2amount + " Plank [" + plank_count + "]";
        Basic_House3Req1.text = "" + Basic_House3BLP.Req1amount + " Block [" + block_stone_count + "]";
        Basic_House3Req2.text = "" + Basic_House3BLP.Req2amount + " Plank [" + plank_count + "]";
        Basic_Factory1Req1.text = "" + Basic_Factory1BLP.Req1amount + " Block [" + block_stone_count + "]";
        Basic_Factory1Req2.text = "" + Basic_Factory1BLP.Req2amount + " Plank [" + plank_count + "]";
        Basic_Factory2Req1.text = "" + Basic_Factory2BLP.Req1amount + " Block [" + stone_small_count + "]";
        Basic_Factory2Req2.text = "" + Basic_Factory2BLP.Req2amount + " Plank [" + plank_count + "]";
        Basic_Hotel1Req1.text = "" + Basic_Hotel1BLP.Req1amount + " Block [" + block_stone_count + "]";
        Basic_Hotel1Req2.text = "" + Basic_Hotel1BLP.Req2amount + " Plank [" + plank_count + "]";

        if (block_stone_count >= Main_TownHallBLP.Req1amount && plank_count >= Main_TownHallBLP.Req2amount && InventorySystem.Instance.CheckSlotsAvailable(1))
        {
            craftMain_TownHallBTN.gameObject.SetActive(true);
        }
        else
        {
            craftMain_TownHallBTN.gameObject.SetActive(false);
        }

        if (block_stone_count >= Base_HugeBLP.Req1amount && plank_count >= Base_HugeBLP.Req2amount && InventorySystem.Instance.CheckSlotsAvailable(1))
        {
            craftBase_HugeBTN.gameObject.SetActive(true);
            craftBase_LargeBTN.gameObject.SetActive(true);
            craftBase_MediumBTN.gameObject.SetActive(true);
            craftBase_SmallBTN.gameObject.SetActive(true);
            craftMain_ElectricityWaterBTN.gameObject.SetActive(true);
            craftMain_ElectricityFanBTN.gameObject.SetActive(true);
            craftMain_WaterTowerBTN.gameObject.SetActive(true);
            craftMain_WaterWellBTN.gameObject.SetActive(true);
            craftBasic_House1BTN.gameObject.SetActive(true);
            craftBasic_House2BTN.gameObject.SetActive(true);
            craftBasic_House3BTN.gameObject.SetActive(true);
            craftBasic_Factory1BTN.gameObject.SetActive(true);
            craftBasic_Factory2BTN.gameObject.SetActive(true);
            craftBasic_Hotel1BTN.gameObject.SetActive(true);
        }
        else
        {
            craftBase_HugeBTN.gameObject.SetActive(false);
            craftBase_LargeBTN.gameObject.SetActive(false);
            craftBase_MediumBTN.gameObject.SetActive(false);
            craftBase_SmallBTN.gameObject.SetActive(false);
            craftMain_ElectricityWaterBTN.gameObject.SetActive(false);
            craftMain_ElectricityFanBTN.gameObject.SetActive(false);
            craftMain_WaterTowerBTN.gameObject.SetActive(false);
            craftMain_WaterWellBTN.gameObject.SetActive(false);
            craftBasic_House1BTN.gameObject.SetActive(false);
            craftBasic_House2BTN.gameObject.SetActive(false);
            craftBasic_House3BTN.gameObject.SetActive(false);
            craftBasic_Factory1BTN.gameObject.SetActive(false);
            craftBasic_Factory2BTN.gameObject.SetActive(false);
            craftBasic_Hotel1BTN.gameObject.SetActive(false);
        }

        if (block_stone_count >= Base_TreeBLP.Req1amount && InventorySystem.Instance.CheckSlotsAvailable(1))
        {
            craftBase_TreeBTN.gameObject.SetActive(true);
        }
        else
        {
            craftBase_TreeBTN.gameObject.SetActive(false);
        }

        if (block_stone_count >= Main_SchoolBLP.Req1amount && plank_count >= Main_SchoolBLP.Req2amount && InventorySystem.Instance.CheckSlotsAvailable(1))
        {
            craftMain_SchoolBTN.gameObject.SetActive(true);
            craftMain_HospitalBTN.gameObject.SetActive(true);
            craftMain_LightHouseBTN.gameObject.SetActive(true);
        }
        else
        {
            craftMain_SchoolBTN.gameObject.SetActive(false);
            craftMain_HospitalBTN.gameObject.SetActive(false);
            craftMain_LightHouseBTN.gameObject.SetActive(false);
        }

        //--TREE--//
        Conifer_TreeReq1.text = "" + Conifer_TreeBLP.Req1amount + " Seed [" + seed_count + "]";
        Cypress_TreeReq1.text = "" + Cypress_TreeBLP.Req1amount + " Seed [" + seed_count + "]";
        Pine_TreeReq1.text = "" + Pine_TreeBLP.Req1amount + "1 Seed [" + seed_count + "]";

        if (seed_count >= Conifer_TreeBLP.Req1amount && InventorySystem.Instance.CheckSlotsAvailable(1))
        {
            craftConifer_TreeBTN.gameObject.SetActive(true);
            craftCypress_TreeBTN.gameObject.SetActive(true);
            craftPine_TreeBTN.gameObject.SetActive(true);
        }
        else
        {
            craftConifer_TreeBTN.gameObject.SetActive(false);
            craftCypress_TreeBTN.gameObject.SetActive(false);
            craftPine_TreeBTN.gameObject.SetActive(false);
        }
    }
}