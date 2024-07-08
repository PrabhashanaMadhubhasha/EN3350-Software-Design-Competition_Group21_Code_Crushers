using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; set; }

    [Header("Date")]
    public int dayInGame = 1;
    public int monthInGame = 1;
    public int yearInGame = 2023;

    public bool tenSecondTriggered;

    public List<DaysForMonth> Months;
    public int monthForAYear;

    public TextMeshProUGUI dayUI;

    private void Start()
    {
        monthForAYear = Months.Count;
        dayUI.text = $"{yearInGame} : {Months[monthInGame - 1].monthName} : {dayInGame}";
    }
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
    private void Update()
    {
        if (tenSecondTriggered) // If ten second triggerd then power consumption API will call
        {
            tenSecondTriggered = false;
            
            StartCoroutine(PowerConsumptionAPIManager.Instance.GetTotalMaximumNetYearlyPowerConsumption(yearInGame));
            StartCoroutine(PowerConsumptionAPIManager.Instance.GetTotalMaximumNetPowerConsumptionForSpecificMonth(yearInGame, Months[monthInGame - 1].monthName));
            StartCoroutine(PowerConsumptionAPIManager.Instance.GetMaximumNetDailyPowerConsumptionForSpecificMonth(yearInGame, Months[monthInGame - 1].monthName, dayInGame));

            AssetsManager.Instance.CitizenLeaveUponAPI(PowerConsumptionAPIManager.Instance.totalMaximumNetPowerConsumptionForSpecificDay);
        }
    }

    // After cuurent going to 0 the next day coming
    public void TriggerNextday()
    {
        dayInGame += 1; 

        if (dayInGame > Months[monthInGame - 1].noOfDays) // Check month is to be updated
        {
            monthInGame += 1;
            dayInGame = 1;

            if (monthInGame > monthForAYear) // check year is to be updated
            {
                yearInGame += 1;
                monthInGame = 1;

                // Get funds for the year according to the power consumption API
                AssetsManager.Instance.CalculateFundsForYear(PowerConsumptionAPIManager.Instance.totalMaximumNetYearlyPowerConsumption);
            }
            // Get funds for the month according to the power consumption API
            AssetsManager.Instance.CalculateFundsForMonth(PowerConsumptionAPIManager.Instance.totalMaximumNetPowerConsumptionForSpecificMonth);
        }

        // After selling these are shouldbe reset
        AssetsManager.Instance.isSellingElectricity = false;
        AssetsManager.Instance.isSellingWaterCapacity = false;
        AssetsManager.Instance.isSellingFoodMass = false;

        AssetsManager.Instance.ResetSelledAssets();

        // Get funds for the day according to the power consumption API
        AssetsManager.Instance.CalculateFundsForDay(PowerConsumptionAPIManager.Instance.totalMaximumNetPowerConsumptionForSpecificDay);

        dayUI.text = $"Date: {yearInGame} : {Months[monthInGame - 1].monthName} : {dayInGame}";
    }
}

[System.Serializable]
public class DaysForMonth
{
    public string monthName;
    public int noOfDays;
}
