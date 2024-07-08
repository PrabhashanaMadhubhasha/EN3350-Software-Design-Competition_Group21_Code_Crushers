using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;
//using UnityEditor.AssetDatabase.SaveAssets();

public class ActiveWeapon : MonoBehaviour
{
    public enum WeaponSlot
    {
        Primary = 0,
        Secondary = 1
    }
    public Transform crossHairTarget;
    public Rig handIk;
    public Transform[] weaponSlots;
    public Transform weaponLeftGrip;
    public Transform weaponRightGrip;
    public Transform weaponLeftForeArm;
    public Transform weaponRightForeArm;
    public Animator rigController;

    public CharacterAiming characterAiming;
    RaycastWeapon[] equipped_weapons = new RaycastWeapon[2];
    public AmmoWidget ammoWidget;
    int activeWeaponIndex;
    bool isHolstered;
    public bool isChangingWeapon;
    public bool isIdle = true;
    public Character character;
    public bool isCombatMode;

    public bool radialMenuRifle;
    public bool radialMenuPistol;

    // Start is called before the first frame update
    void Start()
    {
        RaycastWeapon existingWeapon = GetComponentInChildren<RaycastWeapon>();
        if (existingWeapon)
        {
            Equip(existingWeapon);
        }
    }

    public bool IsFiring()
    {
        RaycastWeapon currentWeapon = GetActiveWeapon();
        if (!currentWeapon)
        {
            return false;
        }
        return currentWeapon.isFiring;
    }

    // Get the Gun
    public RaycastWeapon GetActiveWeapon()
    {
        return GetWeapon(activeWeaponIndex);
    }

    RaycastWeapon GetWeapon(int index)
    {
        if (index < 0 || index >= equipped_weapons.Length)
        {
            return null;
        }
        return equipped_weapons[index];
    }

    // Update is called once per frame
    void Update()
    {
        if (isIdle)
        {
            ammoWidget.gameObject.SetActive(false);
        }
        else
        {
            ammoWidget.gameObject.SetActive(true);
        }

        isCombatMode = character.isCombatMode;
        var weapon = GetWeapon(activeWeaponIndex);
        bool notSprinting = rigController.GetCurrentAnimatorStateInfo(2).shortNameHash == Animator.StringToHash("notSprinting");

        if (weapon && !isHolstered && notSprinting)
        {
            weapon.UpdateWeapon(Time.deltaTime);
        }

        // Holster or Active the current  Weapon
        if (Input.GetKeyDown(KeyCode.X) && !BuyingSystem.Instance.isOpen)
        {
            ToggleActiveWeapon();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1) || radialMenuRifle)
        {
            if(activeWeaponIndex == (int)WeaponSlot.Primary)
            {
                ToggleActiveWeapon();
            }
            else
            {
                SetActiveWeapon(WeaponSlot.Primary);
            }

            radialMenuRifle = false;
           
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) || radialMenuPistol)
        {
            if (activeWeaponIndex == (int)WeaponSlot.Secondary)
            {
                ToggleActiveWeapon();
            }
            else
            {
                SetActiveWeapon(WeaponSlot.Secondary);
            }

            radialMenuPistol = false;   
           
        }

    }

    public void Equip(RaycastWeapon newWeapon)
    {
        int weaponSlotIndex = (int)newWeapon.weaponSlot;
        var weapon = GetWeapon(weaponSlotIndex);
        if (weapon)
        {
            Destroy(weapon.gameObject);
        }
        weapon = newWeapon;
        weapon.raycastDestination = crossHairTarget;
        weapon.recoil.characterAiming = characterAiming;
        weapon.recoil.rigController = rigController;
        weapon.transform.SetParent(weaponSlots[weaponSlotIndex], false);
        equipped_weapons[weaponSlotIndex] = weapon;

        ammoWidget.Refresh(weapon.ammoCount);
        ammoWidget.SetTotalAmmoCount(BuyingSystem.Instance.totalAmmoCount);

    }

    // Toggle the current wepon
    public void ToggleActiveWeapon()
    {
        bool isHolstered = rigController.GetBool("holster_weapon");
        if (isHolstered)
        {
            StartCoroutine(ActivateWeapon(activeWeaponIndex));
        }
        else
        {
            StartCoroutine(HolsterWeapon(activeWeaponIndex));
        }

    }
    void SetActiveWeapon(WeaponSlot weaponSlot)
    {

        int holsterIndex = activeWeaponIndex;
        int activeIndex = (int)weaponSlot;

        if (holsterIndex == activeIndex)
        {
            holsterIndex = -1;
        }
        StartCoroutine(SwitchWeapon(holsterIndex, activeIndex));
    }

    IEnumerator SwitchWeapon(int holsterIndex, int activeIndex)
    {
        rigController.SetInteger("weapon_index", activeIndex);
        yield return StartCoroutine(HolsterWeapon(holsterIndex));
        yield return StartCoroutine(ActivateWeapon(activeIndex));
        activeWeaponIndex = activeIndex;
    }

    // Holster weapon and show Rig Animations
    IEnumerator HolsterWeapon(int index)
    {
        isChangingWeapon = true;
        isHolstered = true;

        var weapon = GetWeapon(index);
        if (weapon)
        {
            rigController.SetBool("holster_weapon", true);
            do
            {
                yield return new WaitForEndOfFrame();
            } while (rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
            isIdle = true;
        }
        isChangingWeapon = false;
    }

    // Activate weapon and show Rig Animations
    IEnumerator ActivateWeapon(int index)
    {
        if (character.isCombatMode)
        {            
            if (EquipSystem.Instance.isAxeIsEquipped)
            {
                EquipSystem.Instance.isAxeIsEquipped = false;
                EquipSystem.Instance.SelectCurrentQuickSlot();
            }
            character.sheathWeapon = true;

            do
            {
                yield return new WaitForEndOfFrame();
            } while (character.animator.GetCurrentAnimatorStateInfo(1).normalizedTime < 1.0f);


        }
        isChangingWeapon = true;
        var weapon = GetWeapon(index);
        if (weapon)
        {
            isIdle = false;
            rigController.SetBool("holster_weapon", false);
            rigController.Play("equip_" + weapon.weaponName);
            do
            {
                yield return new WaitForEndOfFrame();
            } while (rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
            isHolstered = false;
        }
        isChangingWeapon = false;
    }
}
