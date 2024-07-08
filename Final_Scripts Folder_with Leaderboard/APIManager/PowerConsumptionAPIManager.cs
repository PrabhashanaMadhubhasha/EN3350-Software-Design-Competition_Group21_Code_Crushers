using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class PowerConsumptionAPIManager : MonoBehaviour
{
    public static PowerConsumptionAPIManager Instance { get; set; }

    [Header("API Access")]
    private string baseUrl = "http://20.15.114.131:8080/api";
    private string baseurl_for_get_marks = "http://localhost:8080/getMarks/0";
    private string apiKey = "NjVkNDRmNWQ0ZjZjN2U1YTFhNDVhNDAwOjY1ZDQ0ZjVkNGY2YzdlNWExYTQ1YTNmNg";
    private string jwtToken;

    [Header("API Responses")]
    public string getYearlyPowerConsumptionRes;
    public string getPowerConsumptionForSpecificMonthRes;
    public string getDailyPowerConsumptionForSpecificMonthRes;

    [Header("Total Maximum Net Consumptions")]
    public int totalMaximumNetYearlyPowerConsumption;
    public int totalMaximumNetPowerConsumptionForSpecificMonth;
    public int totalMaximumNetPowerConsumptionForSpecificDay;

    [Header("Total Maximum Net Consumptions Texts")]
    public Text totalMaximumNetYearlyPowerConsumptionText;
    public Text totalMaximumNetPowerConsumptionForSpecificMonthText;
    public Text totalMaximumNetPowerConsumptionForSpecificDayText;

    // Create a list to store the daily values
    public List<int> dailyMaximumNetPowerConsumptionsForSpecificMonth = new List<int>();

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

    public void Start()
    {
        StartCoroutine(AuthenticatePlayerCoroutine());

        StartCoroutine(GetTotalMaximumNetYearlyPowerConsumption(TimeManager.Instance.yearInGame));
        StartCoroutine(GetTotalMaximumNetPowerConsumptionForSpecificMonth(TimeManager.Instance.yearInGame, TimeManager.Instance.Months[TimeManager.Instance.monthInGame - 1].monthName));
        StartCoroutine(GetMaximumNetDailyPowerConsumptionForSpecificMonth(TimeManager.Instance.yearInGame, TimeManager.Instance.Months[TimeManager.Instance.monthInGame - 1].monthName, TimeManager.Instance.dayInGame));

        //UnityWebRequest request = UnityWebRequest.Get(baseurl_for_get_marks);
        //StartCoroutine(GetQuizMarks(request));

    }

    // Get the JWTToken for Authentication
    private IEnumerator AuthenticatePlayerCoroutine()
    {
        // Create JSON request body
        string requestBody = "{\"apiKey\": \"" + apiKey + "\"}";

        // Create request
        using (UnityWebRequest request = UnityWebRequest.PostWwwForm(baseUrl + "/login", "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(requestBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            // Send request
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Authentication failed: " + request.error);
                yield break;
            }

            // Extract JWT token from response
            string responseText = request.downloadHandler.text;
            jwtToken = JsonUtility.FromJson<TokenResponse>(responseText).token;

            yield return StartCoroutine(FetchPowerConsumption());
        }
    }

    private IEnumerator FetchPowerConsumption()
    {
        // Yearly Power Consumption
        yield return StartCoroutine(GetYearlyPowerConsumption(2024));

        // Power Consumption for Specific Month
        yield return StartCoroutine(GetPowerConsumptionForSpecificMonth(2024, "JANUARY"));

        // Daily Power Consumption for Specific Month
        yield return StartCoroutine(GetDailyPowerConsumptionForSpecificMonth(2024, "JANUARY"));

        // Daily Power Consumption for Current Month
        yield return StartCoroutine(GetDailyPowerConsumptionForCurrentMonth());

        // Power Consumption for Current Month
        yield return StartCoroutine(GetPowerConsumptionForCurrentMonth());

        // All Power Consumption
        yield return StartCoroutine(GetAllPowerConsumption());
    }

    // GetYearlyPowerConsumption
    private IEnumerator GetYearlyPowerConsumption(int year)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(baseUrl + $"/power-consumption/yearly/view?year={year}"))
        {
            request.SetRequestHeader("Authorization", "Bearer " + jwtToken);

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to fetch yearly power consumption: " + request.error);
                yield break;
            }

            getYearlyPowerConsumptionRes = request.downloadHandler.text;
        }
    }

    // GetPowerConsumptionForSpecificMonth
    private IEnumerator GetPowerConsumptionForSpecificMonth(int year, string month)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(baseUrl + $"/power-consumption/month/view?year={year}&month={month}"))
        {
            request.SetRequestHeader("Authorization", "Bearer " + jwtToken);

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Failed to fetch power consumption for {month}: " + request.error);
                yield break;
            }

            getPowerConsumptionForSpecificMonthRes = request.downloadHandler.text;
        }
    }

    // GetDailyPowerConsumptionForSpecificMonth
    private IEnumerator GetDailyPowerConsumptionForSpecificMonth(int year, string month)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(baseUrl + $"/power-consumption/month/daily/view?year={year}&month={month}"))
        {
            request.SetRequestHeader("Authorization", "Bearer " + jwtToken);

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Failed to fetch daily power consumption for {month}: " + request.error);
                yield break;
            }

            getDailyPowerConsumptionForSpecificMonthRes = request.downloadHandler.text;
        }
    }

    // GetDailyPowerConsumptionForCurrentMonth
    private IEnumerator GetDailyPowerConsumptionForCurrentMonth()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(baseUrl + "/power-consumption/current-month/daily/view"))
        {
            request.SetRequestHeader("Authorization", "Bearer " + jwtToken);

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to fetch daily power consumption for current month: " + request.error);
                yield break;
            }

            string responseText = request.downloadHandler.text;
        }
    }

    // GetPowerConsumptionForCurrentMonth
    private IEnumerator GetPowerConsumptionForCurrentMonth()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(baseUrl + "/power-consumption/current-month/view"))
        {
            request.SetRequestHeader("Authorization", "Bearer " + jwtToken);

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to fetch power consumption for current month: " + request.error);
                yield break;
            }

            string responseText = request.downloadHandler.text;
        }
    }

    // GetAllPowerConsumption
    private IEnumerator GetAllPowerConsumption()
    {
        yield return new WaitForSeconds(0.5f);
        using (UnityWebRequest request = UnityWebRequest.Get(baseUrl + "/power-consumption/all/view"))
        {
            request.SetRequestHeader("Authorization", "Bearer " + jwtToken);

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to fetch all power consumption: " + request.error);
                yield break;
            }

            string responseText = request.downloadHandler.text;
        }
    }

    IEnumerator GetQuizMarks(UnityWebRequest request)
    {
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ConnectionError ||
            request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {

            string responseText = request.downloadHandler.text;
            Debug.Log("Response: " + responseText);
            ResponseMarks responseData = JsonUtility.FromJson<ResponseMarks>(responseText);
            if (responseData != null)
            {
                // Change the Gme environment Upon Quiz Marks 
                int marks = responseData.marks;
                BuyingSystem.Instance.AddFundsCoinsUponQuizMarks(marks);    
                MissionSystem.Instance.ActiveEnemiesUponQuestionnaireMarks(marks);
            }
            else
            {
                Debug.LogError("Failed to parse JSON response.");
            }
        }
    }

    // Get Total Maximum NetYearlyPowerConsumption
    public IEnumerator GetTotalMaximumNetYearlyPowerConsumption(int currentYear)
    {
        yield return new WaitForSeconds(1f);

        // Yearly Power Consumption
        yield return StartCoroutine(GetYearlyPowerConsumption(currentYear));

        // Deserialize JSON response
        YearlyPowerConsumption yearlyConsumption = JsonUtility.FromJson<YearlyPowerConsumption>(getYearlyPowerConsumptionRes);

        // Sum up the units
        float totalUnits = yearlyConsumption.units.JANUARY.units +
                           yearlyConsumption.units.FEBRUARY.units +
                           yearlyConsumption.units.MARCH.units +
                           yearlyConsumption.units.APRIL.units +
                           yearlyConsumption.units.MAY.units +
                           yearlyConsumption.units.JUNE.units +
                           yearlyConsumption.units.JULY.units +
                           yearlyConsumption.units.AUGUST.units +
                           yearlyConsumption.units.SEPTEMBER.units +
                           yearlyConsumption.units.OCTOBER.units +
                           yearlyConsumption.units.NOVEMBER.units +
                           yearlyConsumption.units.DECEMBER.units;

        totalMaximumNetYearlyPowerConsumption = Mathf.RoundToInt(totalUnits);

        totalMaximumNetYearlyPowerConsumptionText.text = $"{totalMaximumNetYearlyPowerConsumption}";

    }

    // Get Total Maximum NetPowerConsumption ForSpecificMonth
    public IEnumerator GetTotalMaximumNetPowerConsumptionForSpecificMonth(int currentYear, string currentMonth)
    {
        yield return new WaitForSeconds(1f);

        // Power Consumption for Specific Month
        yield return StartCoroutine(GetPowerConsumptionForSpecificMonth(currentYear, currentMonth));

        // Deserialize JSON response
        MonthlyPowerConsumptionResponse consumptionResponse = JsonUtility.FromJson<MonthlyPowerConsumptionResponse>(getPowerConsumptionForSpecificMonthRes);

        // Extract the units
        float units = consumptionResponse.monthlyPowerConsumptionView.units;

        totalMaximumNetPowerConsumptionForSpecificMonth = Mathf.RoundToInt(units);

        totalMaximumNetPowerConsumptionForSpecificMonthText.text = $"{totalMaximumNetPowerConsumptionForSpecificMonth}";

    }

    // GetMaximum NetDailyPowerConsumption ForSpecificMonth ForSpecificDay
    public IEnumerator GetMaximumNetDailyPowerConsumptionForSpecificMonth(int currentYear, string currentMonth, int currentDay)
    {
        yield return new WaitForSeconds(1f);

        // Daily Power Consumption for Specific Month
        yield return StartCoroutine(GetDailyPowerConsumptionForSpecificMonth(currentYear, currentMonth));
        Debug.Log(getDailyPowerConsumptionForSpecificMonthRes);

        // Deserialize JSON response into a temporary object
        DailyPowerConsumptionResponse tempResponse = JsonUtility.FromJson<DailyPowerConsumptionResponse>(getDailyPowerConsumptionForSpecificMonthRes);

        // Manually parse the dailyUnits dictionary from the response text
        string dailyUnitsJson = Regex.Match(getDailyPowerConsumptionForSpecificMonthRes, "\"dailyUnits\":\\{(.*?)\\}").Groups[1].Value;
        tempResponse.dailyPowerConsumptionView.dailyUnits = new Dictionary<int, float>();

        // Parse the dailyUnits JSON string
        string[] dailyUnitsArray = dailyUnitsJson.Split(',');
        foreach (string dailyUnit in dailyUnitsArray)
        {
            string[] keyValue = dailyUnit.Split(':');
            int day = int.Parse(keyValue[0].Trim('\"'));
            float units = float.Parse(keyValue[1]);

            tempResponse.dailyPowerConsumptionView.dailyUnits.Add(day, units);
        }

        // Create a list to store the daily values
        dailyMaximumNetPowerConsumptionsForSpecificMonth = new List<int>();

        // Add daily values to the list
        foreach (KeyValuePair<int, float> dailyUnit in tempResponse.dailyPowerConsumptionView.dailyUnits)
        {
            int dailyValue = Mathf.RoundToInt(dailyUnit.Value); // Convert float to int
            dailyMaximumNetPowerConsumptionsForSpecificMonth.Add(dailyValue);
        }

        totalMaximumNetPowerConsumptionForSpecificDay = dailyMaximumNetPowerConsumptionsForSpecificMonth[currentDay - 1];

        totalMaximumNetPowerConsumptionForSpecificDayText.text = $"{totalMaximumNetPowerConsumptionForSpecificDay}";

    }

    [Serializable]
    private class TokenResponse
    {
        public string token;
    }

    [System.Serializable]
    public class MonthlyUnits
    {
        public float units;
    }

    [System.Serializable]
    public class Units
    {
        public MonthlyUnits JANUARY;
        public MonthlyUnits FEBRUARY;
        public MonthlyUnits MARCH;
        public MonthlyUnits APRIL;
        public MonthlyUnits MAY;
        public MonthlyUnits JUNE;
        public MonthlyUnits JULY;
        public MonthlyUnits AUGUST;
        public MonthlyUnits SEPTEMBER;
        public MonthlyUnits OCTOBER;
        public MonthlyUnits NOVEMBER;
        public MonthlyUnits DECEMBER;
    }

    [System.Serializable]
    public class YearlyPowerConsumption
    {
        public Units units;
    }

    [System.Serializable]
    public class MonthlyPowerConsumptionView
    {
        public int year;
        public int month;
        public float units;
    }

    [System.Serializable]
    public class MonthlyPowerConsumptionResponse
    {
        public MonthlyPowerConsumptionView monthlyPowerConsumptionView;
    }

    [System.Serializable]
    public class DailyPowerConsumptionView
    {
        public int year;
        public int month;
        public Dictionary<int, float> dailyUnits = new Dictionary<int, float>();
    }

    [System.Serializable]
    public class DailyPowerConsumptionResponse
    {
        public DailyPowerConsumptionView dailyPowerConsumptionView;
    }

    public class ResponseMarks
    {
        public int marks;
    }
}

