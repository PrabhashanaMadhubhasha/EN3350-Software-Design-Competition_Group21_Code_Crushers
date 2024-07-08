using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))]
public class EquipableItem : MonoBehaviour
{
    public Animator animator;
    public DamageDealer damageDealer;
    public int meleeWeaponHitCount;
    public int maxHit = 500;

    // Start is called before the first frame update
    void Start()
    {
        //damageDealer = GetComponentInChildren<DamageDealer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // if Some UIs appear unable to attck from Axe
        if (Input.GetMouseButtonDown(0) && !InventorySystem.Instance.isOpen && !CraftingSystem.Instance.isOpen && !SelectionManager.instance.handIsVisible && !ConstructionManager.Instance.inConstructionMode)
        {
            animator.SetTrigger("hit");
        }
    }

    // Get hit from the Axe
    public void GetHit()
    {
        GameObject selectedTree = SelectionManager.instance.selectedTree;

        if (selectedTree != null) // To if axe is equipped
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.chopSound);

            selectedTree.GetComponent<ChoppableTree>().GetHit();
            meleeWeaponHitCount++;
            
            if ((meleeWeaponHitCount + gameObject.GetComponentInChildren<DamageDealer>().meleeWeaponHitCount) >= maxHit)
            {
                EquipSystem.Instance.haveDamagedAxe = true;
                meleeWeaponHitCount = 0;
                gameObject.GetComponentInChildren<DamageDealer>().meleeWeaponHitCount = 0;
            }
        }
    }
}
