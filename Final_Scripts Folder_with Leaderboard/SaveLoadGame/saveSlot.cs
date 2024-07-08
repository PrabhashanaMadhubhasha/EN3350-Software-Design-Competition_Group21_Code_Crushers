using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class saveSlot : MonoBehaviour
{
    private Button button;
    private TextMeshProUGUI buttonText;

    public int slotNumber;

    public GameObject alertUI;
    Button yesBTN;
    Button noBTN;

    private void Awake()
    {
        button = GetComponent<Button>();
        buttonText = transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();

        yesBTN = alertUI.transform.Find("Yes Button").GetComponent<Button>();
        noBTN =  alertUI.transform.Find("No Button").GetComponent<Button>();
    }

    public void Start()
    {
        button.onClick.AddListener(() =>
        {
            if (MenuController.Instance.IsSlotEmpty(slotNumber))
            {
                SaveGameConfirmed();
            }
            else
            {
                DisplayOverrideWarning();
            }
        });
    }

    public void Update()
    {
        if(MenuController.Instance.IsSlotEmpty(slotNumber))
        {
            buttonText.text = "Empty";
        }
        else
        {
            buttonText.text = PlayerPrefs.GetString("slot" + slotNumber + "Description");
        }
    }

    public void DisplayOverrideWarning()
    {
        alertUI.SetActive(true);

        yesBTN.onClick.AddListener(() =>
        {
            SaveGameConfirmed();
            alertUI.SetActive(false);
        });

        noBTN.onClick.AddListener(() =>
        {
            alertUI.SetActive(false);
        });
    }

    private void SaveGameConfirmed()
    {
        MenuController.Instance.SaveGame(slotNumber);
        DateTime dt = DateTime.Now;
        string time = dt.ToString("yyy-MM-dd HH:mm");

        string description = "Saved Game " + slotNumber + " | " + time;

        buttonText.text = description;
        PlayerPrefs.SetString("slot" + slotNumber + "Description", description);

        MenuController.Instance.DeselectButton();
    }
}
