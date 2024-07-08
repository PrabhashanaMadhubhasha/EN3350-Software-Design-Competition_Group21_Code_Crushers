using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DayNightSystem : MonoBehaviour
{
    public static DayNightSystem Instance { get; set; }

    public Light directionlLight;

    [Header("Hour Related")]
    public float dayDurationInSeconds = 24.0f;
    public int currentHour;
    public int previousHour = -1;
    float currentTimeOfDay = 0.35f; // About 8 in the morning
    float tenSecondTime = 0.0f;

    public List<SkyboxTimeMapping> timeMappings;
    float blendedValue = 0.0f;

    bool lockNextDayTrigger = false;
    

    public TextMeshProUGUI timeUI;

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

    // Update is called once per frame
    void Update()
    {
        // for calling to the Power consumption APIs check 10s going
        tenSecondTime += Time.deltaTime;
        if(tenSecondTime >= 10.0f)
        {
            TimeManager.Instance.tenSecondTriggered = true;  
            tenSecondTime = 0.0f;
        }
        // Calculate the current time of the day based on the game time
        currentTimeOfDay += Time.deltaTime / dayDurationInSeconds;
        currentTimeOfDay %= 1; // Ensure it stays betweeen 0 and 1

        currentHour = Mathf.FloorToInt(currentTimeOfDay * 24);
        if(previousHour != currentHour)
        {
            AssetsManager.Instance.SetAssets();
            previousHour = currentHour; 
        }

        timeUI.text = $"{currentHour}:00";

        // Update the directional lights's rotation
        directionlLight.transform.rotation = Quaternion.Euler(new Vector3((currentTimeOfDay * 360) - 90, 170, 0));

        // Update the skybox material based on  the time of day
        UpdateSkybox();

    }

    // Change the Sky materila based on the currentHour
    private void UpdateSkybox()
    {
        try
        {
            // Find the appropriate skybox material for the current hour
            Material currentSkybox = null;
            foreach (SkyboxTimeMapping mapping in timeMappings)
            {
                if (currentHour == mapping.hour)
                {
                    currentSkybox = mapping.skyboxMaterial;

                    if (currentSkybox.shader != null)
                    {
                        if (currentSkybox.shader.name == "Custom/SkyboxTransition")
                        {
                            blendedValue += Time.deltaTime;
                            blendedValue = Mathf.Clamp01(blendedValue);

                            currentSkybox.SetFloat("_TransitionFactor", blendedValue);
                        }
                        else
                        {
                            blendedValue = 0;
                        }
                    }
                    break;
                }
            }

            if (currentHour == 0 && lockNextDayTrigger == false)
            {
                TimeManager.Instance.TriggerNextday();
                lockNextDayTrigger = true;
            }

            if (currentHour != 0)
            {
                lockNextDayTrigger = false;
            }

            if (currentSkybox != null)
            {
                RenderSettings.skybox = currentSkybox;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Unable to chnge the SkyBox: " + ex.Message);
        }
    }
}


[System.Serializable]
public class SkyboxTimeMapping
{
    public string phaseName;
    public int hour; // The hour of the day (0 - 23)
    public Material skyboxMaterial; // The corresponding sky box material for the hour
}
