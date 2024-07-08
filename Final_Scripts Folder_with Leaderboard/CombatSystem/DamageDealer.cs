using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    bool canDealDamage;
    List<GameObject> hasDealtDamage;

    [SerializeField] float weaponLength;
    float weaponWidth1 = 0.4f;
    float weaponWidth2 = 0.2f;
    [SerializeField] float weaponDamage;
    public int meleeWeaponHitCount;
    Vector3 weaponSize;
    void Start()
    {
        canDealDamage = false;
        hasDealtDamage = new List<GameObject>();
    }

    void Update()
    {
        if (canDealDamage)
        {
            RaycastHit hit;

            int layerMask = 1 << 9;
            if (Physics.Raycast(transform.position, -transform.up, out hit, weaponLength, layerMask))
            {
                if (hit.transform.TryGetComponent(out Enemy enemy) && !hasDealtDamage.Contains(hit.transform.gameObject)) // Enemy get damage
                {
                    meleeWeaponHitCount++;
                    
                    enemy.TakeDamage(weaponDamage);
                    SoundManager.Instance.PlaySound(SoundManager.Instance.meleeWeaponHitSound);
                    SoundManager.Instance.PlaySound(SoundManager.Instance.bloodSound);
                    enemy.HitVFX(hit.point);
                    hasDealtDamage.Add(hit.transform.gameObject);
                }
            }
        }
    }

    // Assigned this to Axe and Sword
    public void StartDealDamage()
    {
        canDealDamage = true;
        hasDealtDamage.Clear();
    }
    public void EndDealDamage()
    {
        canDealDamage = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        weaponSize = new Vector3(weaponWidth1, weaponWidth2, weaponLength);
        Gizmos.DrawWireCube(transform.position, weaponSize);
    }
}