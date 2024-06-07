using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponWheelController : MonoBehaviour
{
    public static WeaponWheelController Instance { get; set; }

    public Animator anim;
    public bool weaponWheelSelected;

    public static int weaponID;
    
    public ActiveWeapon activeWeapon;
    public Character character;

    public Text itemSelectedText;

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
        // Open the Weapon Wheel
        if (!InventorySystem.Instance.isOpen && !CraftingSystem.Instance.isOpen && !RemoveConstruction.Instance.inRemoveConstructionMode && !DialogSystem.Instance.dialogUIActive 
            && !BuyingSystem.Instance.isOpen && !AssetsManager.Instance.isOpen && !MissionObjectMenuController.Instance.isOpen && !MenuManager.Instance.isMenuOpen)
        {
            if (Input.GetKeyUp(KeyCode.Tab))
            {
                weaponWheelSelected = !weaponWheelSelected;
            }

            if (weaponWheelSelected)
            {
                SelectionManager.instance.DisableSelection();
                SelectionManager.instance.GetComponent<SelectionManager>().enabled = false;

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                itemSelectedText.gameObject.SetActive(true);
                anim.SetBool("OpenWeaponWheel", true);
            }
            else
            {
                SelectionManager.instance.EnableSelection();
                SelectionManager.instance.GetComponent<SelectionManager>().enabled = true;

                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;

                itemSelectedText.gameObject.SetActive(false);
                anim.SetBool("OpenWeaponWheel", false);
            }

            switch (weaponID)
            {
                case 0:
                    break;
                case 1:
                    Debug.Log("Rifle");
                    activeWeapon.radialMenuRifle = true;
                    weaponID = 0;
                    break;
                case 2:
                    Debug.Log("Pistol");
                    activeWeapon.radialMenuPistol = true;
                    weaponID = 0;
                    break;
                case 3:
                    Debug.Log("Sword");
                    character.radialMenuSword = true;
                    weaponID = 0;
                    break;
                case 4:
                    Debug.Log("Axe");
                    character.radialMenuAxe = true;
                    weaponID = 0;
                    break;
                case 5:
                    Debug.Log("Dagger");
                    break;
                case 6:
                    Debug.Log("Rifle");
                    break;
                case 7:
                    Debug.Log("Fists");
                    break;
            }
        }    
        
    }

}
