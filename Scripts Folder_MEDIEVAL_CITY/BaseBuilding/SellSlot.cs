using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class SellSlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{

    public GameObject sellAlertUI;

    private Text textToModify;

    public Sprite sell_closed;
    public Sprite sell_opened;

    private Image imageComponent;

    Button YesBTN, NoBTN;

    GameObject draggedItem
    {
        get
        {
            return DragDrop.itemBeingDragged;
        }
    }

    GameObject itemToBeSelled;



    public string itemName
    {
        get
        {
            string name = itemToBeSelled.name;
            string toRemove = "(Clone)";
            string result = name.Replace(toRemove, "");
            return result;
        }
    }



    void Start()
    {
        imageComponent = transform.Find("background").GetComponent<Image>();

        textToModify = sellAlertUI.transform.Find("Text").GetComponent<Text>();

        YesBTN = sellAlertUI.transform.Find("yes").GetComponent<Button>();
        YesBTN.onClick.AddListener(delegate { SellItem(); });

        NoBTN = sellAlertUI.transform.Find("no").GetComponent<Button>();
        NoBTN.onClick.AddListener(delegate { CancelSelling(); });

    }

    public void OnDrop(PointerEventData eventData)
    {
        if (draggedItem.GetComponent<InventoryItem>().isSellable == true)
        {
            itemToBeSelled = draggedItem.gameObject;

            StartCoroutine(notifyBeforeSelling());
        }

    }

    // Apear some alert wehn drop the item to sell slot
    IEnumerator notifyBeforeSelling()
    {
        sellAlertUI.SetActive(true);
        textToModify.text = "Sell this " + itemName + "?";
        yield return new WaitForSeconds(1f);
    }

    private void CancelSelling()
    {
        imageComponent.sprite = sell_closed;
        sellAlertUI.SetActive(false);
    }

    // For selling the item
    private void SellItem()
    {
        try
        {
            imageComponent.sprite = sell_closed;
            AssetsManager.Instance.currentCoins += itemToBeSelled.GetComponent<InventoryItem>().sellRevenue;
            DestroyImmediate(itemToBeSelled.gameObject);
            InventorySystem.Instance.ReCalculateList();
            CraftingSystem.Instance.RefreshNeededItems();
            sellAlertUI.SetActive(false);
        }
        catch (Exception ex)
        {
            Debug.LogError("Not success the selling: " + ex.Message);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        if (draggedItem != null && draggedItem.GetComponent<InventoryItem>().isSellable == true)
        {
            imageComponent.sprite = sell_opened;
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (draggedItem != null && draggedItem.GetComponent<InventoryItem>().isSellable == true)
        {
            imageComponent.sprite = sell_closed;
        }
    }

}