using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class LoadSlot : MonoBehaviour
{
    private Button button;
    private TextMeshProUGUI buttonText;

    public int slotNumber;

    private void Awake()
    {
        button = GetComponent<Button>();
        buttonText = transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();

    }
    public void Update()
    {
        if (MenuController.Instance.IsSlotEmpty(slotNumber))
        {
            buttonText.text = "Empty";

        }
        else
        {
            buttonText.text = PlayerPrefs.GetString("slot" + slotNumber + "Description");
        }
    }

    private void Start()
    {
        button.onClick.AddListener(() =>
        {
            if (MenuController.Instance.IsSlotEmpty(slotNumber) == false)
            {
                MenuController.Instance.StartedLoadedGame(slotNumber);
                MenuController.Instance.DeselectButton();
            }
            else
            {
                //Empty
            }
        });
    }
}
