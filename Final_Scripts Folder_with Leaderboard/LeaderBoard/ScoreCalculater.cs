using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ScoreCalculater;

public class ScoreCalculater
{
    int score = 0;
    int maxScore = int.Parse(PlayerPrefs.GetString("PlayerMarks", "No playerMarks found"));
    int currentNetConsumptionMaxPoints = 10;
    int currentNetPowerConsumptionMaxPoints = 30;
    int energySavingActionsPoints = 10;

    // Calculate Total Score
    public int CalculateTotalScore(float currentCitizenHappiness, float maxCitizenHappiness, CurrentNetConsumptions currentNetConsumptions, int totalMaximumNetPowerConsumptionForSpecificDay)
    {
        // CalculateScore upon CitizenHappiness
        CalculateScoreUponCitizenHappiness(currentCitizenHappiness, maxCitizenHappiness);

        // CalculateScore upon all the currentNetConsumptions
        CalculateScoreUponNetConsumptions(currentNetConsumptions.currentNetCoinsConsumption);
        CalculateScoreUponNetConsumptions(currentNetConsumptions.currentNetEnergyPowerConsumption);
        CalculateScoreUponNetConsumptions(currentNetConsumptions.currentNetWaterCapacityConsumption);
        CalculateScoreUponNetConsumptions(currentNetConsumptions.currentNetFoodMassConsumption);

        // CalculateScore upon all NetMaximumAPIConsumptions according to all the currentNetConsumptions
        CalculateScoreUponNetConsumptionsAndAPI(currentNetConsumptions.currentNetEnergyPowerConsumption, totalMaximumNetPowerConsumptionForSpecificDay);
        CalculateScoreUponNetConsumptionsAndAPI(currentNetConsumptions.currentNetWaterCapacityConsumption, totalMaximumNetPowerConsumptionForSpecificDay);
        CalculateScoreUponNetConsumptionsAndAPI(currentNetConsumptions.currentNetFoodMassConsumption, totalMaximumNetPowerConsumptionForSpecificDay);

        return score;
    }

    // Calculate score based on citizen happiness value 
    public void CalculateScoreUponCitizenHappiness(float currentCitizenHappiness, float maxCitizenHappiness)
    {
        if (currentCitizenHappiness >= maxCitizenHappiness / 2)
        {
            Debug.Log("Calculate score based on citizen happiness value ");
            score += Mathf.RoundToInt(Mathf.Pow(currentCitizenHappiness, 2) / 20); // Using x^2 to increase more
        }
        if (currentCitizenHappiness == maxCitizenHappiness) // As a bonus score
        {
            Debug.Log("Calculate score based on citizen happiness value ");
            score += Mathf.RoundToInt(Mathf.Pow(currentCitizenHappiness, 2) / 20);
        }
    }

    // Calculate score based on NetConsumptions (coins, electricity, water, food) 
    public void CalculateScoreUponNetConsumptions(int currentNetConsumption)
    {
        if (currentNetConsumption < 0)
        {
            Debug.Log("Calculate score based on NetConsumptions (coins, electricity, water, food) ");
            score += Mathf.RoundToInt(currentNetConsumptionMaxPoints * (1 - Mathf.Exp((float)currentNetConsumption / 100))); // Using 1 - e^(-x) to decrease the increasing rate
        }
    }

    // Calculate score based on NetConsumptions and API Consumptions
    public void CalculateScoreUponNetConsumptionsAndAPI(int currentNetConsumption, int totalMaximumNetPowerConsumptionForSpecificDay)
    {
        if (totalMaximumNetPowerConsumptionForSpecificDay / 24 - currentNetConsumption > 0 && totalMaximumNetPowerConsumptionForSpecificDay / 24 - currentNetConsumption < currentNetPowerConsumptionMaxPoints)
        {
            Debug.Log("Calculate score based on NetConsumptions and API Consumptions");
            score += totalMaximumNetPowerConsumptionForSpecificDay / 24 - currentNetConsumption;
        }                                                                        
        else if (totalMaximumNetPowerConsumptionForSpecificDay / 24 - currentNetConsumption >= currentNetPowerConsumptionMaxPoints)
        {
            Debug.Log("Calculate score based on NetConsumptions and API Consumptions");
            score += currentNetPowerConsumptionMaxPoints;
        }
    } 

    // Add some points to score when using Renewable Energy
    public void IncreaseScoreUponRenewableEnergy(int totalRenewableEnergySources)
    {
        Debug.Log("Add some points to score when using Renewable Energy");
        score += totalRenewableEnergySources * energySavingActionsPoints ^ 2;
    }

    // Add some points to score when selling Assets to save energy
    public void IncreaseScoreUponSelledAssets(int selledAssetsAmount)
    {
        Debug.Log("Add some points to score when selling Assets to save energy");
        score += selledAssetsAmount * energySavingActionsPoints ^ 3;   
    }

    // For showing in leader the Max score of the player is saved
    public void SaveMaxScore(int currentScore)
    {
        // Save the max player score
        if (currentScore > maxScore)
        {
            PlayerPrefs.SetString("PlayerMarks", currentScore.ToString());
        }
    }

    public class CurrentNetConsumptions
    {
        public int currentNetCoinsConsumption;
        public int currentNetEnergyPowerConsumption;
        public int currentNetWaterCapacityConsumption;
        public int currentNetFoodMassConsumption;
    }
}
