using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoWidget : MonoBehaviour
{
    public TMPro.TMP_Text ammoText;
    public TMPro.TMP_Text totalAmmoText;

    public void Refresh(int ammoCount) // Show current count in Magazine
    {
        ammoText.text = ammoCount.ToString();
    }

    public void SetTotalAmmoCount(int totalAmmoCount) // Show current total Ammo
    {
        totalAmmoText.text = totalAmmoCount.ToString();
    }
}
