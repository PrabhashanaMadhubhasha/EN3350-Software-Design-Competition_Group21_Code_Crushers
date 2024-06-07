using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constructable : MonoBehaviour
{
    [Header("Validations")]
    public bool isGrounded;
    public bool isOverlappingItems;
    public bool isValidToBeBuilt = true;
    public bool detectedGhostMemeber;

    [Header("Material Related")]
    private Renderer mRenderer;
    public Renderer mRendererChild;
    public Material redMaterial;
    public Material greenMaterial;
    public Material defaultMaterial;
    public bool changeMaterials;

    public List<GameObject> ghostList = new List<GameObject>();

    public BoxCollider solidCollider; // We need to drag this collider manualy into the inspector

    public Vector2Int area = new Vector2Int(30, 30);

    public int currentPlacedItemCount;

    [Header("Consumptions")]
    public int coinsConsumption = 1000;  // Gold/hour
    public int energyPowerConsumption;  // MW
    public int waterCapacityConsumption;  // Galloon/hour
    public int foodMassConsumption; // kg/hour

    [Header("Productions")]
    public int coinsProduction;  // Gold/hour
    public int energyPowerProduction;
    public int waterCapacityProduction;
    public int foodMassProduction;

    [Header("TypesOfTheConsumptionProduction")]
    public bool isCoinsConsumption;
    public bool isEnergyPowerConsumption;
    public bool isWaterCapacityConsumption;
    public bool isFoodMassConsumption;
    public bool isTaxRate;
    public bool isEnergyPowerProduction;
    public bool isWaterCapacityProduction;
    public bool isFoodMassProduction;

    [Header("TypeOfTheConstructionByName")]
    public bool isHouse;
    public bool isFactory;
    public bool isHotel;
    public bool isSchool;
    public bool isHospital;
    public bool isHydroPlant;
    public bool isWindPlant;

    private void Start()
    {
        IncrementTotalConstructions();

        ActiveConsumptionProduction();

        mRenderer = GetComponent<Renderer>();

        SetMaterial(defaultMaterial);

        foreach (Transform child in transform)
        {
            GhostItem ghostItem = child.GetComponent<GhostItem>();
            if(ghostItem != null)
            {
                ghostList.Add(child.gameObject);
            }       
        }

    }
    void Update()
    {
        // Check is valid to placed the base construction 
        if (ConstructionManager.Instance.inConstructionMode)
        {
            if (isGrounded && isOverlappingItems == false)
            {
                isValidToBeBuilt = true;
            }
            else
            {
                isValidToBeBuilt = false;
            }
        }  
    }

    private void OnTriggerEnter(Collider other)
    {
        // Get validation by compare the Tag of collided objects
        if (other.CompareTag("Ground") && gameObject.CompareTag("activeConstructable"))
        {
            isGrounded = true;
        }

        if (other.CompareTag("Tree") || other.CompareTag("pickable") && gameObject.CompareTag("activeConstructable"))
        {

            isOverlappingItems = true;
        }

        if (other.gameObject.CompareTag("ghost") && gameObject.CompareTag("activeConstructable"))
        {
            detectedGhostMemeber = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Get validation by compare the Tag of collided objects
        if (other.CompareTag("Ground") && gameObject.CompareTag("activeConstructable"))
        {
            isGrounded = false;
        }

        if (other.CompareTag("Rock") || other.CompareTag("Tree") || other.CompareTag("pickable") && gameObject.CompareTag("activeConstructable"))
        {
            isOverlappingItems = false;
        }

        if (other.gameObject.CompareTag("ghost") && gameObject.CompareTag("activeConstructable"))
        {
            detectedGhostMemeber = false;
        }
    }


    // Chnge the material of the construction according to th validation
    public void SetInvalidColor()
    {
        SetMaterial(redMaterial);
    }

    public void SetValidColor()
    {
        SetMaterial(greenMaterial);
    }

    public void SetDefaultColor()
    {
        SetMaterial(defaultMaterial);
    }

    // Get Ghost Object from Constrcted Object
    public void ExtractGhostMembers()
    {
        try
        {
            currentPlacedItemCount = ConstructionManager.Instance.placedItemCount;
            foreach (GameObject item in ghostList)
            {
                item.gameObject.name += $"${currentPlacedItemCount}";
                item.transform.SetParent(transform.parent, true); /////////

                item.gameObject.GetComponent<GhostItem>().isPlaced = true;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Unable to extrct ghost members from the base construction : " + ex.Message);
        }
    }

    // Change the material of the construction
    public void SetMaterial(Material materialToSet)
    {
        try
        {
            if (changeMaterials)
            {
                if (mRenderer != null)
                {
                    SetMaterialOfRenderer(materialToSet, mRenderer);
                }
                else
                {
                    for (int i = 0; i < gameObject.transform.childCount; i++)
                    {

                        GameObject child = gameObject.transform.GetChild(i).gameObject;
                        mRendererChild = child.GetComponent<Renderer>();
                        GhostItem ghostItem = child.GetComponent<GhostItem>();
                        if (ghostItem == null && mRendererChild != null)
                        {
                            SetMaterialOfRenderer(materialToSet, mRendererChild);
                        }

                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Cannot change the material of Renderer: " + ex.Message);
        }

    }


    public void SetMaterialOfRenderer(Material materialToSet, Renderer renderer)
    {
        Material[] materials = renderer.materials;
        if (materials.Length == 1)
        {
            renderer.material = materialToSet; //Green
        }
        else
        {
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = materialToSet;
            }
            renderer.materials = materials;
        }
    }

    // Activation the choosed consumptions and productions when placing the construction
    public void ActiveConsumptionProduction()
    {
        AssetsManager assetsManager = AssetsManager.Instance;

        if (isCoinsConsumption)
        {
            assetsManager.currentCoinsConsumption += coinsConsumption;
        }
        if (isEnergyPowerConsumption)
        {
            assetsManager.currentEnergyPowerConsumption += energyPowerConsumption;
        }
        if (isWaterCapacityConsumption)
        {
            assetsManager.currentWaterCapacityConsumption += waterCapacityConsumption;
        }
        if (isFoodMassConsumption)
        {
            assetsManager.currentFoodMassConsumption += foodMassConsumption;
        }
        if (isTaxRate)
        {
            assetsManager.currentCoinsProduction += coinsProduction;
        }
        if (isEnergyPowerProduction)
        {
            assetsManager.currentEnergyPowerProduction += energyPowerProduction;
        }
        if (isWaterCapacityProduction)
        {
            assetsManager.currentWaterCapacityProduction += waterCapacityProduction;
        }
        if (isFoodMassProduction)
        {
            assetsManager.currentFoodMassProduction += foodMassProduction;
        }
    }

    // Deactivate the choosed consumptions and productions when removing the construction
    public void DeactiveConsumptionProduction()
    {
        AssetsManager assetsManager = AssetsManager.Instance;

        if (isCoinsConsumption)
        {
            assetsManager.currentCoinsConsumption -= coinsConsumption;
        }
        if (isEnergyPowerConsumption)
        {
            assetsManager.currentEnergyPowerConsumption -= energyPowerConsumption;
        }
        if (isWaterCapacityConsumption)
        {
            assetsManager.currentWaterCapacityConsumption -= waterCapacityConsumption;
        }
        if (isFoodMassConsumption)
        {
            assetsManager.currentFoodMassConsumption -= foodMassConsumption;
        }
        if (isTaxRate)
        {
            assetsManager.currentCoinsProduction -= coinsProduction;
        }
        if (isEnergyPowerProduction)
        {
            assetsManager.currentEnergyPowerProduction -= energyPowerProduction;
        }
        if (isWaterCapacityProduction)
        {
            assetsManager.currentWaterCapacityProduction -= waterCapacityProduction;
        }
        if (isFoodMassProduction)
        {
            assetsManager.currentFoodMassProduction -= foodMassProduction;
        }
    }

    // Increment the total count of building according to the building type when placing the construction
    void IncrementTotalConstructions()
    {
        if (isHouse)
        {
            AssetsManager.Instance.totalHouse++;
        }else if (isFactory)
        {
            AssetsManager.Instance.totalFactory++;
        }else if (isHotel)
        {
            AssetsManager.Instance.totalHotel++;
        }else if (isSchool)
        {
            AssetsManager.Instance.totalSchool++;
        }else if (isHospital)
        {
            AssetsManager.Instance.totalHospital++;
        }else if (isHydroPlant)
        {
            AssetsManager.Instance.totalHydroPlant++;
        }else if (isWindPlant)
        {
            AssetsManager.Instance.totalWindPlant++;
        }
    }

    // Decrement the total count of building according to the building type when removing the construction
    void DecrementTotalConstructions()
    {
        if (isHouse)
        {
            AssetsManager.Instance.totalHouse--;
        }
        else if (isFactory)
        {
            AssetsManager.Instance.totalFactory--;
        }
        else if (isHotel)
        {
            AssetsManager.Instance.totalHotel--;
        }
        else if (isSchool)
        {
            AssetsManager.Instance.totalSchool--;
        }
        else if (isHospital)
        {
            AssetsManager.Instance.totalHospital--;
        }
        else if (isHydroPlant)
        {
            AssetsManager.Instance.totalHydroPlant--;
        }
        else if (isWindPlant)
        {
            AssetsManager.Instance.totalWindPlant--;
        }
    }

    private void OnDestroy()
    {
        DecrementTotalConstructions();

        DeactiveConsumptionProduction();
    }
}