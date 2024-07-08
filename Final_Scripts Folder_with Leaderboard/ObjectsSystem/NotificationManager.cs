using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    GameObject notificationUI;
    public List<GameObject> notificationList = new List<GameObject>();

    public int activeNotificationIndex = 0;
    public bool isOpen;
    void Start()
    {
        notificationUI = gameObject.transform.GetChild(0).gameObject;
        PopulateNotificationtList();
    }

    private void PopulateNotificationtList()
    {
        foreach (Transform child in notificationUI.transform)
        {
            if (child.CompareTag("Notification"))
            {
                notificationList.Add(child.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Comma) && isOpen && !MenuManager.Instance.isMenuOpen) // Bckward the notification UI 
        {
            notificationList[activeNotificationIndex].SetActive(false);
            if (activeNotificationIndex - 1 >= 0)
            {
                activeNotificationIndex--;
            }
            else
            {
                activeNotificationIndex = notificationList.Count - 1;   
            }
            notificationList[activeNotificationIndex].SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Period) && isOpen && !MenuManager.Instance.isMenuOpen) // Forward the notification UI 
        {
            notificationList[activeNotificationIndex].SetActive(false);
            if (activeNotificationIndex + 1 < notificationList.Count)
            {
                activeNotificationIndex++;
            }
            else
            {
                activeNotificationIndex = 0;
            }
            notificationList[activeNotificationIndex].SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Slash) && !MenuManager.Instance.isMenuOpen) // Display notification UI
        {
            if(!isOpen)
            {
                isOpen = true;
                notificationList[activeNotificationIndex].SetActive(true);
            }
            else
            {
                isOpen = false;
                notificationList[activeNotificationIndex].SetActive(false);
            }
            
        }

        // In construction Mode special notification will appear
        if (ConstructionManager.Instance.inConstructionMode)
        {
            isOpen = true;
            notificationList[activeNotificationIndex].SetActive(false);
            activeNotificationIndex = 1;
            notificationList[activeNotificationIndex].SetActive(true);
        }
        // In remove construction Mode special notification will appear
        if (RemoveConstruction.Instance.inRemoveConstructionMode)
        {
            isOpen = true;
            notificationList[activeNotificationIndex].SetActive(false);
            activeNotificationIndex = 2;
            notificationList[activeNotificationIndex].SetActive(true);
        }
        // After end day common special notification will appear
        if (DayNightSystem.Instance.previousHour != DayNightSystem.Instance.currentHour)
        {
            isOpen = true;
            notificationList[activeNotificationIndex].SetActive(false);
            activeNotificationIndex = 0;
            notificationList[activeNotificationIndex].SetActive(true);
        }
    }
}
