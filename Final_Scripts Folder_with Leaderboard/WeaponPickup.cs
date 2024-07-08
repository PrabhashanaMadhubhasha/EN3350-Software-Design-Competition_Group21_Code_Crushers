using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public RaycastWeapon weaponFab;

    private void OnTriggerEnter(Collider other) // Can be pickup weapon from this
    {
        ActiveWeapon activeWeapon = other.gameObject.GetComponent<ActiveWeapon>();
        if (activeWeapon)
        {
            Debug.Log("weapon");
            RaycastWeapon newWeapon = Instantiate(weaponFab);
            activeWeapon.Equip(newWeapon);
        }
    }
}
