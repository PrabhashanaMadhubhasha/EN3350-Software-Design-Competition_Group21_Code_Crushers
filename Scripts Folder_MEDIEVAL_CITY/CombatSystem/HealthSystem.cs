using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] public float health = 2;
    [SerializeField] GameObject hitVFX;
    [SerializeField] GameObject ragdoll;

    Animator animator;
    InMainMenu InMainMenu;
    void Start()
    {
        InMainMenu = GetComponent<InMainMenu>();    
        animator = GetComponent<Animator>();
    }

    // Player Take damage
    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        animator.SetTrigger("damage");

        if (health <= 0)
        {
            Die();
        }
    }

    // player Die and initiate ragdoll and go to the Main menu
    void Die()
    {
        Instantiate(ragdoll, transform.position, transform.rotation);
        InMainMenu.BackToMainMenu();
        gameObject.SetActive(false);
    }
    public void HitVFX(Vector3 hitPosition)
    {
        GameObject hit = Instantiate(hitVFX, hitPosition, Quaternion.identity);
        Destroy(hit, 3f);

    }
}