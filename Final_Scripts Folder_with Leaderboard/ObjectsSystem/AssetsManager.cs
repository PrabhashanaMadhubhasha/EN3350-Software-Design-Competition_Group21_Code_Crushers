using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;
using static ScoreCalculater;
using static UnityEngine.Rendering.DebugUI;

public class AssetsManager : MonoBehaviour
{
    public static AssetsManager Instance { get; set; }

    public GameObject assetsManagerUI;

    [Header("Bar Texts")]
    public TextMeshProUGUI coinsBar;
    public TextMeshProUGUI energyPowerBar;
    public TextMeshProUGUI waterCapacityBar;
    public TextMeshProUGUI foodBar;
    public TextMeshProUGUI taxRateBar;
    public TextMeshProUGUI fundCoinsBar;
    public TextMeshProUGUI citizenHappinessBar;
    public TextMeshProUGUI scoreBar;

    [Header("Buildings Counts")]
    public int totalHouse;
    public int totalFactory;
    public int totalHotel;
    public int totalSchool;
    public int totalHospital;
    public int totalHydroPlant;
    public int totalWindPlant;

    public GameObject TreesObject;
    public List<GameObject> treesList = new List<GameObject>();

    [Header("currentNetConsumptionForCurrentDayText")]
    public Text currentNetElectricityConsumptionForCurrentDayText;
    public Text currentNetWaterConsumptionForCurrentDayText;
    public Text currentNetFoodConsumptionForCurrentDayText;

    [Header("currentNetConsumptionForCurrentMonthText")]
    public Text currentNetElectricityConsumptionForCurrentMonthText;
    public Text currentNetWaterConsumptionForCurrentMonthText;
    public Text currentNetFoodConsumptionForCurrentMonthText;

    [Header("currentNetConsumptionForCurrentYearText")]
    public Text currentNetElectricityConsumptionForCurrentYearText;
    public Text currentNetWaterConsumptionForCurrentYearText;
    public Text currentNetFoodConsumptionForCurrentYearText;

    public int currentCoins = 1000;  // Gold

    [Header("currentConsumptions")]
    public int currentCoinsConsumption;  // Gold/hour
    public int currentEnergyPowerConsumption;  // MW
    public int currentWaterCapacityConsumption;  // Galloon/hour 
    public int currentFoodMassConsumption; // kg/hour

    [Header("currentProductions")]
    public int currentCoinsProduction;
    public int currentEnergyPowerProduction;
    public int currentWaterCapacityProduction;   
    public int currentFoodMassProduction;

    [Header("currentPowerRatios")]
    public float currentEnergyPowerRatio;
    public float currentWaterCapacityRatio;
    public float currentFoodMassRatio;

    [Header("totalNetPowerConsumptionForCurrentDay")]
    public int totalNetEnergyPowerConsumptionForCurrentDay;
    public int totalNetWaterCapacityConsumptionForCurrentDay;
    public int totalNetFoodMassConsumptionForCurrentDay;

    [Header("totalNetPowerConsumptionForCurrentMonth")]
    public int totalNetEnergyPowerConsumptionForCurrentMonth;
    public int totalNetWaterCapacityConsumptionForCurrentMonth;
    public int totalNetFoodMassConsumptionForCurrentMonth;

    [Header("totalNetPowerConsumptionForCurrentYear")]
    public int totalNetEnergyPowerConsumptionForCurrentYear;
    public int totalNetWaterCapacityConsumptionForCurrentYear;
    public int totalNetFoodMassConsumptionForCurrentYear;

    [Header("selledAssets")]
    public int selledElectricity;
    public int selledWaterCapacity;
    public int selledFoodMass;

    [Header("fundsFactorForPowerConsumption")]
    public float fundsFactorForEnergyPowerConsumption;
    public float fundsFactorForWaterCapacityConsumption;
    public float fundsFactorForFoodMassConsumption;

    [Header("Some Factors")]
    public float taxRateFactor;
    public float citizenHappinessChangingValueUponTaxRateFactor;
    public float assetRatioMultipleFactor;
    public float reduceConsumptionFactor;

    [Header("Citizen Happiness Related")]
    public float maxCitizenHappiness = 5.0f;
    public float currentCitizenHappiness;
    public float citizenHappinessChangingRate = 0.1f;
    public float citizenHappinessValueForPayingTax = 2.0f;
    public bool leaveCitizensFromAPI;

    public int currentHappinessStageIndex = 5;
    public List<string> happinessStages = new List<string> { "Very Unhappy", "Unhappy", "Neutral", "Happy", "Very Happy", "Excellent" };

    [Header("fundsCoins")]
    public int fundsCoins_Daily;
    public int fundsCoins_Monthly;
    public int fundsCoins_Yearly;

    public bool isOpen;

    [Header("isSelling")]
    public bool isSellingElectricity;
    public bool isSellingWaterCapacity;
    public bool isSellingFoodMass;

    [Header("Score Related")]
    public int score;
    public int rewardLevel;
    public int rewardCoins;
    ScoreCalculater scoreCalculater;
    CurrentNetConsumptions currentNetConsumptions;
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
        currentCitizenHappiness = maxCitizenHappiness;

        CalculateTotalNetPowerConsumptions();

        SetUITexts();

        scoreCalculater = new ScoreCalculater();

        currentNetConsumptions = new CurrentNetConsumptions();
        currentNetConsumptions.currentNetCoinsConsumption = currentCoinsConsumption - currentCoinsProduction;
        currentNetConsumptions.currentNetEnergyPowerConsumption = currentEnergyPowerConsumption - currentEnergyPowerProduction;
        currentNetConsumptions.currentNetWaterCapacityConsumption = currentWaterCapacityConsumption - currentWaterCapacityProduction;
        currentNetConsumptions.currentNetFoodMassConsumption = currentFoodMassConsumption - currentFoodMassProduction;
    }

    private void Update()
    {
        // Open the Assets Screen
        if (!WeaponWheelController.Instance.weaponWheelSelected)
        {
            if (Input.GetKeyDown(KeyCode.P) && !isOpen && !InventorySystem.Instance.isOpen && !ConstructionManager.Instance.inConstructionMode && !RemoveConstruction.Instance.inRemoveConstructionMode 
                && !DialogSystem.Instance.dialogUIActive && !CraftingSystem.Instance.isOpen && !BuyingSystem.Instance.isOpen && !MissionObjectMenuController.Instance.isOpen && !MenuManager.Instance.isMenuOpen)
            {
                assetsManagerUI.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                SelectionManager.instance.DisableSelection();
                SelectionManager.instance.GetComponent<SelectionManager>().enabled = false;
                isOpen = true;

            }
            else if (Input.GetKeyDown(KeyCode.P) && isOpen)
            {
                assetsManagerUI.SetActive(false);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;

                SelectionManager.instance.EnableSelection();
                SelectionManager.instance.GetComponent<SelectionManager>().enabled = true;
                isOpen = false;
            }
        }
    }

    public void SetCurrentCoins()
    {
        coinsBar.text = $"{currentCoins}";
    }

    public void SetCurrentFundCoins()
    {
        fundCoinsBar.text = $"{BuyingSystem.Instance.fundsAmount}";
    }

    // Update the Assets by each Hour
    public void SetAssets()
    {
        if(rewardCoins > 0)
        {
            currentCoins += rewardCoins;
            rewardCoins = 0;
        }
        CalculationsUponReduceConsumptionRate();

        if (isSellingElectricity || isSellingWaterCapacity || isSellingFoodMass)
        {
            currentCoins += Mathf.RoundToInt(BuyingSystem.Instance.factorForEnergyPowerSelling * selledElectricity + BuyingSystem.Instance.factorForWaterCapacitySelling * selledWaterCapacity 
                + BuyingSystem.Instance.factorForFoodMassSelling * selledFoodMass);
        }

        CalculateCurrentCoins();

        if(MissionSystem.Instance.missionObjectIndex >= 8)    // after Mision 7 citizen happiness will be updated
        {
            CalculateCitizenHappiness();
        }
        
        CalculateTotalNetPowerConsumptions();

        ActiveTreesWhenUsingRenewableEnergy();

        currentNetConsumptions.currentNetCoinsConsumption = currentCoinsConsumption - currentCoinsProduction;
        currentNetConsumptions.currentNetEnergyPowerConsumption = currentEnergyPowerConsumption - currentEnergyPowerProduction;
        currentNetConsumptions.currentNetWaterCapacityConsumption = currentWaterCapacityConsumption - currentWaterCapacityProduction;
        currentNetConsumptions.currentNetFoodMassConsumption = currentFoodMassConsumption - currentFoodMassProduction;

        Debug.Log(currentNetConsumptions.currentNetCoinsConsumption);

        score += scoreCalculater.CalculateTotalScore(currentCitizenHappiness, maxCitizenHappiness, currentNetConsumptions, PowerConsumptionAPIManager.Instance.totalMaximumNetPowerConsumptionForSpecificDay);
        scoreCalculater.SaveMaxScore(score);
        scoreCalculater = new ScoreCalculater();

        SetUITexts();
    }

    // Player can reduce power consumption from assets UI and then the result will be calculated as follows
    void CalculationsUponReduceConsumptionRate()
    {
        currentEnergyPowerConsumption -= Mathf.RoundToInt((currentEnergyPowerConsumption / 2) * reduceConsumptionFactor);
        currentWaterCapacityConsumption -= Mathf.RoundToInt((currentWaterCapacityConsumption / 2) * reduceConsumptionFactor);
        currentFoodMassConsumption -= Mathf.RoundToInt((currentFoodMassConsumption / 2) * reduceConsumptionFactor);

        currentCitizenHappiness -= (currentCitizenHappiness / (happinessStages.Count - 1)) * reduceConsumptionFactor;  

    }

    // Calculate Totl Net power consumptions
    void CalculateTotalNetPowerConsumptions()
    {
        totalNetEnergyPowerConsumptionForCurrentDay += currentEnergyPowerConsumption - currentEnergyPowerProduction;
        totalNetWaterCapacityConsumptionForCurrentDay += currentWaterCapacityConsumption - currentWaterCapacityProduction;
        totalNetFoodMassConsumptionForCurrentDay += currentFoodMassConsumption - currentFoodMassProduction;

        totalNetEnergyPowerConsumptionForCurrentMonth += currentEnergyPowerConsumption - currentEnergyPowerProduction;
        totalNetWaterCapacityConsumptionForCurrentMonth += currentWaterCapacityConsumption - currentWaterCapacityProduction;
        totalNetFoodMassConsumptionForCurrentMonth += currentFoodMassConsumption - currentFoodMassProduction;

        totalNetEnergyPowerConsumptionForCurrentYear += currentEnergyPowerConsumption - currentEnergyPowerProduction;
        totalNetWaterCapacityConsumptionForCurrentYear += currentWaterCapacityConsumption - currentWaterCapacityProduction;
        totalNetFoodMassConsumptionForCurrentYear += currentFoodMassConsumption - currentFoodMassProduction;
    }

    // Set UI Texts
    void SetUITexts()
    {
        coinsBar.text = $"{currentCoins}";
        energyPowerBar.text = $"{currentEnergyPowerConsumption} / {currentEnergyPowerProduction}";
        waterCapacityBar.text = $"{currentWaterCapacityConsumption} / {currentWaterCapacityProduction}";
        foodBar.text = $"{currentFoodMassConsumption} / {currentFoodMassProduction}";
        taxRateBar.text = $"{Mathf.RoundToInt(currentCoinsConsumption * taxRateFactor)}";
        fundCoinsBar.text = $"{BuyingSystem.Instance.fundsAmount}";
        citizenHappinessBar.text = $"{happinessStages[currentHappinessStageIndex]}";
        scoreBar.text = $"{score}";

        currentNetElectricityConsumptionForCurrentDayText.text = $"{totalNetEnergyPowerConsumptionForCurrentDay}";
        currentNetWaterConsumptionForCurrentDayText.text = $"{totalNetWaterCapacityConsumptionForCurrentDay}";
        currentNetFoodConsumptionForCurrentDayText.text = $"{totalNetFoodMassConsumptionForCurrentDay}";

        currentNetElectricityConsumptionForCurrentMonthText.text = $"{totalNetEnergyPowerConsumptionForCurrentMonth}";
        currentNetWaterConsumptionForCurrentMonthText.text = $"{totalNetWaterCapacityConsumptionForCurrentMonth}";
        currentNetFoodConsumptionForCurrentMonthText.text = $"{totalNetFoodMassConsumptionForCurrentMonth}";

        currentNetElectricityConsumptionForCurrentYearText.text = $"{totalNetEnergyPowerConsumptionForCurrentYear}";
        currentNetWaterConsumptionForCurrentYearText.text = $"{totalNetWaterCapacityConsumptionForCurrentYear}";
        currentNetFoodConsumptionForCurrentYearText.text = $"{totalNetFoodMassConsumptionForCurrentYear}";
    }

    // Current coins will be calulated by hour from coins consumption and producing
    void CalculateCurrentCoins()
    {
        if(currentCitizenHappiness < citizenHappinessValueForPayingTax)
        {
            if (currentCoins + (currentCoinsProduction - currentCoinsConsumption) >= 0)
            {
                currentCoins += (currentCoinsProduction - currentCoinsConsumption);
            }
            else
            {
                currentCoins = 0;
            }
        }
        else
        {
            if (currentCoins + (currentCoinsProduction - (currentCoinsConsumption - Mathf.RoundToInt(currentCoinsConsumption * taxRateFactor))) >= 0)
            {
                currentCoins += (currentCoinsProduction - (currentCoinsConsumption - Mathf.RoundToInt(currentCoinsConsumption * taxRateFactor)));
            }
            else
            {
                currentCoins = 0;
            }
        }
    }

    // citizen happiness calculation
    void CalculateCitizenHappiness()
    {
        // if current net productions is reduced under 0 then object will appear in mission and object UI
        if (currentEnergyPowerConsumption - currentEnergyPowerProduction >= 0)
        {
            CitizenHappinessDecrement();
            StartCoroutine(MissionObjectMenuController.Instance.ActivateDeactiveObjectsFromObjectIndex(0, true));
        }
        else
        {
            CitizenHappinessIncrement();
            StartCoroutine(MissionObjectMenuController.Instance.ActivateDeactiveObjectsFromObjectIndex(0, false));
        }

        if (currentWaterCapacityConsumption - currentWaterCapacityProduction >= 0)
        {
            CitizenHappinessDecrement();
            StartCoroutine(MissionObjectMenuController.Instance.ActivateDeactiveObjectsFromObjectIndex(1, true));
        }
        else
        {
            CitizenHappinessIncrement();
            StartCoroutine(MissionObjectMenuController.Instance.ActivateDeactiveObjectsFromObjectIndex(1, false));
        }

        if (currentFoodMassConsumption - currentFoodMassProduction >= 0)
        {
            CitizenHappinessDecrement();
            StartCoroutine(MissionObjectMenuController.Instance.ActivateDeactiveObjectsFromObjectIndex(2, true));
        }
        else
        {
            CitizenHappinessIncrement();
            StartCoroutine(MissionObjectMenuController.Instance.ActivateDeactiveObjectsFromObjectIndex(2, false));
        }

        CitizenHappinessUponTaxRateFactorFormula();
        CitizenHappinessUponUncompletedConstruction();

        if(currentCitizenHappiness < maxCitizenHappiness / 2) // if current citizen happines is reduced under half of max then object will appear in mission and object UI
        {
            StartCoroutine(MissionObjectMenuController.Instance.ActivateDeactiveObjectsFromObjectIndex(6, true));
        }
        else
        {
            StartCoroutine(MissionObjectMenuController.Instance.ActivateDeactiveObjectsFromObjectIndex(6, false));
        }

        currentHappinessStageIndex = Mathf.RoundToInt(currentCitizenHappiness / (maxCitizenHappiness / (happinessStages.Count - 1)));
    }

    void CitizenHappinessUponTaxRateFactorFormula() // Citizen happiness chnge on the finds fctor that change by player in assets UI screen
    {
        currentEnergyPowerRatio = (float)currentEnergyPowerConsumption / currentEnergyPowerProduction;
        currentWaterCapacityRatio = (float)currentWaterCapacityConsumption / currentWaterCapacityProduction;
        currentFoodMassRatio = (float)currentFoodMassConsumption / currentFoodMassProduction;

        if (currentEnergyPowerProduction != 0 && currentWaterCapacityProduction != 0 && currentFoodMassProduction != 0)
        {
            citizenHappinessChangingValueUponTaxRateFactor = citizenHappinessChangingRate + (-taxRateFactor + assetRatioMultipleFactor * (1 / (currentEnergyPowerRatio + currentWaterCapacityRatio + currentFoodMassRatio)));
        }
        else
        {
            citizenHappinessChangingValueUponTaxRateFactor = citizenHappinessChangingRate + (-taxRateFactor);
        }
            
        if (currentCitizenHappiness + citizenHappinessChangingValueUponTaxRateFactor <= maxCitizenHappiness && currentCitizenHappiness + citizenHappinessChangingValueUponTaxRateFactor >= 0.0f)
        {
            currentCitizenHappiness += citizenHappinessChangingValueUponTaxRateFactor;
        }
        else
        {
            if(currentCitizenHappiness + citizenHappinessChangingValueUponTaxRateFactor > maxCitizenHappiness)
            {
                currentCitizenHappiness = maxCitizenHappiness;
            }
            else
            {
                currentCitizenHappiness = 0.0f;
            }
        }
    }

    void CitizenHappinessUponUncompletedConstruction() // citizens happiness change on uncompled constructions and 
    {
        // Appear objects in misson and object UI
        if (totalHouse > 10 * totalSchool)
        {
            CitizenHappinessDecrement();
            StartCoroutine(MissionObjectMenuController.Instance.ActivateDeactiveObjectsFromObjectIndex(3, true)); 
        }
        else
        {
            StartCoroutine(MissionObjectMenuController.Instance.ActivateDeactiveObjectsFromObjectIndex(3, false));
        }
        if(totalHouse + totalFactory + totalHotel > 15 * totalHospital)
        {
            CitizenHappinessDecrement();
            StartCoroutine(MissionObjectMenuController.Instance.ActivateDeactiveObjectsFromObjectIndex(4, true));
        }
        else
        {
            StartCoroutine(MissionObjectMenuController.Instance.ActivateDeactiveObjectsFromObjectIndex(4, false));
        }
        if(totalHouse + totalFactory > 10 * totalHotel)
        {
            CitizenHappinessDecrement();
            StartCoroutine(MissionObjectMenuController.Instance.ActivateDeactiveObjectsFromObjectIndex(5, true));
        }
        else
        {
            StartCoroutine(MissionObjectMenuController.Instance.ActivateDeactiveObjectsFromObjectIndex(5, false));
        }
    }

    // Factor citizen leaving if cannot maintain demand upon coreland API net consumptions
    public void CitizenLeaveUponAPI(int totalMaximumNetPowerConsumptionForSpecificDay)
    {
        if (2 * totalMaximumNetPowerConsumptionForSpecificDay < totalNetEnergyPowerConsumptionForCurrentDay || 2 * totalMaximumNetPowerConsumptionForSpecificDay < totalNetWaterCapacityConsumptionForCurrentDay || 2 * totalMaximumNetPowerConsumptionForSpecificDay < totalNetFoodMassConsumptionForCurrentDay)
        {
            leaveCitizensFromAPI = true;
        }
        else
        {
            leaveCitizensFromAPI = false;
        }
    }

    public void CitizenHappinessIncrement()
    {
        if (currentCitizenHappiness + citizenHappinessChangingRate <= maxCitizenHappiness)
        {
            currentCitizenHappiness += citizenHappinessChangingRate;
        }
        else
        {
            currentCitizenHappiness = maxCitizenHappiness;
        }
    }

    public void CitizenHappinessDecrement()
    {
        if (currentCitizenHappiness - citizenHappinessChangingRate >= 0.0f)
        {
            currentCitizenHappiness -= citizenHappinessChangingRate;
        }
        else
        {
            currentCitizenHappiness = 0.0f;
        }
    }
    // Funds will be calculated upon API called power consumptions
    public void CalculateFundsForDay(int totalMaximumNetPowerConsumptionForSpecificDay)
    {
        StartCoroutine(CalculateFunds(fundsCoins_Daily, totalNetEnergyPowerConsumptionForCurrentDay, totalNetWaterCapacityConsumptionForCurrentDay, totalNetFoodMassConsumptionForCurrentDay, totalMaximumNetPowerConsumptionForSpecificDay));

        if (totalMaximumNetPowerConsumptionForSpecificDay < totalNetEnergyPowerConsumptionForCurrentDay || totalMaximumNetPowerConsumptionForSpecificDay < totalNetWaterCapacityConsumptionForCurrentDay || totalMaximumNetPowerConsumptionForSpecificDay < totalNetFoodMassConsumptionForCurrentDay)
        {
            StartCoroutine(MissionObjectMenuController.Instance.ActivateDeactiveObjectsFromObjectIndex(7, true)); // Appear objects in misson and object UI
        }
        else
        {
            StartCoroutine(MissionObjectMenuController.Instance.ActivateDeactiveObjectsFromObjectIndex(7, false));
        }

        totalNetEnergyPowerConsumptionForCurrentDay = 0;
        totalNetWaterCapacityConsumptionForCurrentDay = 0;
        totalNetFoodMassConsumptionForCurrentDay = 0;
    }

    public void CalculateFundsForMonth(int totalMaximumNetPowerConsumptionForSpecificMonth)
    {
        StartCoroutine(CalculateFunds(fundsCoins_Monthly, totalNetEnergyPowerConsumptionForCurrentMonth, totalNetWaterCapacityConsumptionForCurrentMonth, totalNetFoodMassConsumptionForCurrentMonth, totalMaximumNetPowerConsumptionForSpecificMonth));

        totalNetEnergyPowerConsumptionForCurrentMonth = 0;
        totalNetWaterCapacityConsumptionForCurrentMonth = 0;
        totalNetFoodMassConsumptionForCurrentMonth = 0;
    }

    public void CalculateFundsForYear(int totalMaximumNetYearlyPowerConsumption)
    {
        StartCoroutine(CalculateFunds(fundsCoins_Yearly, totalNetEnergyPowerConsumptionForCurrentYear, totalNetWaterCapacityConsumptionForCurrentYear, totalNetFoodMassConsumptionForCurrentYear, totalMaximumNetYearlyPowerConsumption));

        totalNetEnergyPowerConsumptionForCurrentYear = 0;
        totalNetWaterCapacityConsumptionForCurrentYear = 0;
        totalNetFoodMassConsumptionForCurrentYear = 0;
    }

    public IEnumerator CalculateFunds(int fundsCoins, int totalNetEnergyPowerConsumption, int totalNetWaterCapacityConsumption, int totalNetFoodMassConsumption, int totalMaximumNetPowerConsumption)
    {
        yield return new WaitForSeconds(0.3f);

        BuyingSystem.Instance.fundsAmount += fundsCoins + Mathf.RoundToInt((totalMaximumNetPowerConsumption - totalNetEnergyPowerConsumption) * fundsFactorForEnergyPowerConsumption +
            (totalMaximumNetPowerConsumption - totalNetWaterCapacityConsumption) * fundsFactorForWaterCapacityConsumption + (totalMaximumNetPowerConsumption - totalNetFoodMassConsumption) * fundsFactorForFoodMassConsumption);
        if (BuyingSystem.Instance.fundsAmount < 0)
        {
            BuyingSystem.Instance.fundsAmount = 0;
        }

        SetCurrentFundCoins();
    }

    // After ending the day reset power to our city
    public void ResetSelledAssets()
    {
        currentEnergyPowerProduction += selledElectricity;
        currentWaterCapacityProduction += selledWaterCapacity;
        currentFoodMassProduction += selledFoodMass;

        selledElectricity = 0;
        selledWaterCapacity = 0;
        selledFoodMass = 0;
    }

    public void SellElectricity()
    {
        if(currentEnergyPowerProduction - currentEnergyPowerConsumption > 0)
        {
            selledElectricity = currentEnergyPowerProduction - currentEnergyPowerConsumption;
            currentEnergyPowerProduction -= selledElectricity;
            isSellingElectricity = true;

            scoreCalculater.IncreaseScoreUponSelledAssets(selledElectricity);
        }
        else
        {
            selledElectricity = 0;
        }
    }

    public void SellWaterCapacity()
    {
        if (currentWaterCapacityProduction - currentWaterCapacityConsumption > 0)
        {
            selledWaterCapacity = currentWaterCapacityProduction - currentWaterCapacityConsumption;
            currentWaterCapacityProduction -= selledWaterCapacity;
            isSellingWaterCapacity = true;

            scoreCalculater.IncreaseScoreUponSelledAssets(selledElectricity);
        }
        else
        {
            selledWaterCapacity = 0;
        }
    }

    public void SellFoodMass()
    {
        if (currentFoodMassProduction - currentFoodMassConsumption > 0)
        {
            selledFoodMass = currentFoodMassProduction - currentFoodMassConsumption;
            currentFoodMassProduction -= selledFoodMass;
            isSellingFoodMass = true;

            scoreCalculater.IncreaseScoreUponSelledAssets(selledElectricity);
        }
        else
        {
            selledFoodMass = 0;
        }
    }

    public void ActiveTreesWhenUsingRenewableEnergy()
    {
        try
        {
            int totalRenewableEnergySources = totalHydroPlant + totalWindPlant;
            foreach (Transform child in TreesObject.transform)
            {
                if (child.CompareTag("Tree"))
                {
                    treesList.Add(child.gameObject);
                }
            }

            for (int i = 0; i < totalRenewableEnergySources; i++)
            {
                if (treesList[i] != null)
                {
                    scoreCalculater.IncreaseScoreUponRenewableEnergy(totalRenewableEnergySources);

                    treesList[i].SetActive(true);

                    // Citizen Happines increment upon renewablw enrgy
                    if (currentCitizenHappiness + citizenHappinessChangingRate / 5 <= maxCitizenHappiness)
                    {
                        currentCitizenHappiness += citizenHappinessChangingRate / 5;
                    }
                    else
                    {
                        currentCitizenHappiness = maxCitizenHappiness;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Unable to Plant Trees: " + ex.Message);
        }
    }

    // Add the reward coins to the current coins amount
    public void AddRewardCoins(int slotNumber, int rewardAmount)
    {
        rewardCoins = rewardAmount;
        PlayerPrefs.SetString("slot_" + slotNumber.ToString() + "_rewardCoins", "0");
    }
}
