using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ReloadWeapon : MonoBehaviour
{
    public Animator rigController;
    public WeaponAnimationEvents animationEvents;
    public ActiveWeapon activeWeapon;
    public Transform leftHand;
    public AmmoWidget ammoWidget;
    public bool isReloading;

    GameObject magazineHand;

    private void Start()
    {
        animationEvents.WeaponAnimationEvent.AddListener(OnAnimationEvent);
    }
    void Update()
    {
        RaycastWeapon weapon = activeWeapon.GetActiveWeapon();
        if (weapon) // Reloading the Gun
        {
            if (((weapon.ammoCount < weapon.clipSize && Input.GetKeyDown(KeyCode.R)) || weapon.ammoCount <= 0) && BuyingSystem.Instance.totalAmmoCount > 0)
            {
                isReloading = true;
                rigController.SetTrigger("reload_weapon");
            }

            if(weapon.isFiring)
            {
                ammoWidget.Refresh(weapon.ammoCount);
            }
        }
        
    }

    void OnAnimationEvent(string eventName) // Rig Animations for Reloading 
    {
        Debug.Log(eventName);
        switch (eventName)
        {
            case "detach_magazine":
                DetachMagazine();
                break;
            case "drop_magazine":
                DropMagazine();
                break;
            case "refill_magazine":
                RefillMagazine();
                break;
            case "attach_magazine":
                AttachMagazine();
                break;
            case "after_reloading":
                AfterReloading();
                break;
        }
    }

    void DetachMagazine() // Remove current magaine
    {
        RaycastWeapon weapon = activeWeapon.GetActiveWeapon();
        magazineHand = Instantiate(weapon.magazine, leftHand, true);
        weapon.magazine.SetActive(false);
    }

    void DropMagazine()
    {
        GameObject droppedMagazine = Instantiate(magazineHand, magazineHand.transform.position, magazineHand.transform.rotation);
        droppedMagazine.AddComponent<Rigidbody>();
        droppedMagazine.AddComponent<BoxCollider>();
        BoxCollider boxCollider =  droppedMagazine.GetComponent<BoxCollider>();
        boxCollider.center = new Vector3(0, (float)0.05, (float)0.006);
        boxCollider.size = new Vector3((float)0.02, (float)0.1, (float)0.004);
        magazineHand.SetActive(false);
    }

    void RefillMagazine()
    {
        magazineHand.SetActive(true);
    }

    void AttachMagazine()
    {
        RaycastWeapon weapon = activeWeapon.GetActiveWeapon();
        weapon.magazine.SetActive(true);
        Destroy(magazineHand);

        BuyingSystem.Instance.totalAmmoCount -= (weapon.clipSize - weapon.ammoCount);

        if (BuyingSystem.Instance.totalAmmoCount >= weapon.clipSize)
        {
            weapon.ammoCount = weapon.clipSize;
        }
        else
        {
            weapon.ammoCount = BuyingSystem.Instance.totalAmmoCount;
        }

        rigController.ResetTrigger("reload_weapon");

        ammoWidget.Refresh(weapon.ammoCount);
        ammoWidget.SetTotalAmmoCount(BuyingSystem.Instance.totalAmmoCount);
        //sReloading = false;
    }

    void AfterReloading()
    {
        isReloading = false;
    }
}
