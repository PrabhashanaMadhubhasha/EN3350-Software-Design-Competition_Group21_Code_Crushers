using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalState : MonoBehaviour
{
    public static GlobalState Instance { get; set; }

    public GameObject chopHolder;
    public float resourceHealth;
    public float resourceMaxHealth; 
    public string objectName;

    public Image centerDotImage;


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



}
