using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    public static DialogSystem Instance { get; set; }

    public Text dialogText;

    public Button option1BTN;
    public Button option2BTN;

    public Canvas dialogUI;

    public bool dialogUIActive;

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

    // Open Dialog UI when starting the conversation
    public void OpenDialogUI()
    {
        dialogUI.gameObject.SetActive(true);
        dialogUIActive = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;  
    }

    // Close Dialog UI when finishing the conversation
    public void CloseDialogUI()
    {
        dialogUI.gameObject.SetActive(false);
        dialogUIActive = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
