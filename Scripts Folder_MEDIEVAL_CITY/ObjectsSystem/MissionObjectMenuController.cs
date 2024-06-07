using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MissionObjectMenuController : MonoBehaviour
{
    public static MissionObjectMenuController Instance { get; set; }

    public GameObject missionObjectUI;
    public GameObject contentScreenUI;

    public List<GameObject> missionList = new List<GameObject>();
    public List<GameObject> objectList = new List<GameObject>();

    public bool isOpen;

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
    void Start()
    {
        isOpen = false;

        PopulateMissionObjectLists();
    }

    private void PopulateMissionObjectLists()
    {
        foreach (Transform child in contentScreenUI.transform)
        {
            if (child.CompareTag("Mission"))
            {
                missionList.Add(child.gameObject);
            }
            if (child.CompareTag("Object"))
            {
                objectList.Add(child.gameObject);
            }
        }
    }

    // Activate Initial Missions
    public IEnumerator ActivateMissionsFromMissionIndex(int startIndex)
    {
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < startIndex; i++)
        {
            if (missionList[i] != null)
            {
                missionList[i].SetActive(false);
            }
            else
            {
                Debug.LogWarning($"GameObject at index {i} is null.");
            }
        }

        for (int i = startIndex; i < missionList.Count; i++)
        {
            if (missionList[i] != null)
            {
                missionList[i].SetActive(true);
            }
            else
            {
                Debug.LogWarning($"GameObject at index {i} is null.");
            }
        }
    }

    // Activate in real time objectives
    public IEnumerator ActivateDeactiveObjectsFromObjectIndex(int index, bool action)
    {
        yield return new WaitForSeconds(0.5f);

        if (objectList[index] != null)
        {
            Debug.Log("Active                sffffffffffffff");
            objectList[index].SetActive(action);
        }
        else
        {
            Debug.LogWarning($"GameObject at index {index} is null.");
        }

    }

    private void Update()
    {
        // Open Mission & Object UI
        if (!WeaponWheelController.Instance.weaponWheelSelected)
        {
            if (Input.GetKeyDown(KeyCode.M) && !isOpen && !InventorySystem.Instance.isOpen && !ConstructionManager.Instance.inConstructionMode && !RemoveConstruction.Instance.inRemoveConstructionMode && 
                !DialogSystem.Instance.dialogUIActive && !CraftingSystem.Instance.isOpen && !BuyingSystem.Instance.isOpen && !AssetsManager.Instance.isOpen && !MenuManager.Instance.isMenuOpen)
            {
                missionObjectUI.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                SelectionManager.instance.DisableSelection();
                SelectionManager.instance.GetComponent<SelectionManager>().enabled = false;
                isOpen = true;

            }
            else if (Input.GetKeyDown(KeyCode.M) && isOpen)
            {
                missionObjectUI.SetActive(false);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;

                SelectionManager.instance.EnableSelection();
                SelectionManager.instance.GetComponent<SelectionManager>().enabled = true;
                isOpen = false;
            }
        }
    }
}
