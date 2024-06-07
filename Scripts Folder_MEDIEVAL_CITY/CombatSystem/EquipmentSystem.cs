using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSystem : MonoBehaviour
{
    public static EquipmentSystem Instance { get; set; }

    [SerializeField] GameObject swordHolder;
    [SerializeField] GameObject sword;
    [SerializeField] GameObject swordSheath;

    [SerializeField] GameObject axeHolder;
    [SerializeField] GameObject axe;
    [SerializeField] GameObject axeSheath;


    GameObject currentWeaponInHand;
    GameObject currentWeaponInSheath1;

    GameObject currentWeaponInSheath2;

    public bool drawAxe;
    public bool drawSword;

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
        currentWeaponInSheath1 = Instantiate(sword, swordSheath.transform); 
    }

    void Update()
    {
        if (CraftingSystem.Instance.isAxeInInventory && currentWeaponInSheath2 == null && !drawAxe)
        {
            currentWeaponInSheath2 = Instantiate(axe, axeSheath.transform);
        }
    }

    // Following functions were assigned to the animations of weapons
    public void DrawWeapon()
    {
        drawSword = true;
        SoundManager.Instance.PlaySound(SoundManager.Instance.meleeWeaponDrawSound);
        currentWeaponInHand = Instantiate(sword, swordHolder.transform);
        Destroy(currentWeaponInSheath1);
    }

    public void SheathWeapon()
    {
        drawSword = false;
        SoundManager.Instance.PlaySound(SoundManager.Instance.meleeWeaponDrawSound);
        currentWeaponInSheath1 = Instantiate(sword, swordSheath.transform);
        Destroy(currentWeaponInHand);
    }

    public void DrawAxe()
    {
        drawAxe = true;
        SoundManager.Instance.PlaySound(SoundManager.Instance.meleeWeaponDrawSound);
        currentWeaponInHand = Instantiate(axe, axeHolder.transform);
        Destroy(currentWeaponInSheath2);
    }

    public void SheathAxe()
    {
        drawAxe = false;
        SoundManager.Instance.PlaySound(SoundManager.Instance.meleeWeaponDrawSound);
        currentWeaponInSheath2 = Instantiate(axe, axeSheath.transform);
        Destroy(currentWeaponInHand);
    }

    public void StartDealDamage()
    {
        currentWeaponInHand.GetComponentInChildren<DamageDealer>().StartDealDamage();
    }
    public void EndDealDamage()
    {
        currentWeaponInHand.GetComponentInChildren<DamageDealer>().EndDealDamage();
    }
}