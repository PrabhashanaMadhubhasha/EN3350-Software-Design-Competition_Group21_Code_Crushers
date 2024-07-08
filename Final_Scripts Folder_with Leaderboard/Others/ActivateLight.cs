using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateLight : MonoBehaviour
{
    Light lightComponent;

    public int activeHour;
    public int deactiveHour;

    public bool changeOnReduceConsumptionFactor;

    private void Start()
    {
        lightComponent = GetComponent<Light>();
    }

    void Update()
    {
        // Light on off according to the Day time of player and Pole
        if (DayNightSystem.Instance.currentHour >= activeHour && DayNightSystem.Instance.currentHour <= deactiveHour)
        {
            lightComponent.enabled = false;
        }
        else
        {
            lightComponent.enabled = true;
        }

        if (changeOnReduceConsumptionFactor && AssetsManager.Instance.reduceConsumptionFactor > 0.2) // Power reduce factor affect for the pole
        {
            lightComponent.enabled = false;
        }
    }
}
