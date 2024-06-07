using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class RaycastWeapon : MonoBehaviour
{
    class Bullet
    {
        public float time;
        public Vector3 initialPosition;
        public Vector3 initialVelocity;
        public TrailRenderer tracer;
        public int bounce;
    }

    public ActiveWeapon.WeaponSlot weaponSlot;
    public bool isFiring = false;
    public int fireRate = 25;
    public float bulletSpeed = 1000.0f;
    public float bulletDrop = 0.0f;
    public int maxBounces = 0;
    public bool debug = false;
    public ParticleSystem[] muzzleFlash;
    public ParticleSystem[] hitEffect;
    public TrailRenderer tracerEffect;

    public int ammoCount;
    public int clipSize;
    public string weaponName;
    public Transform raycastOrigin;
    public Transform raycastDestination;
    public WeaponRecoil recoil;
    public GameObject magazine;

    Ray ray;
    public RaycastHit hitInfo;
    float accumulatedTime;
    List<Bullet> bullets = new List<Bullet>();
    float maxLifetime = 100.0f;

    bool canDealDamage;
    List<GameObject> hasDealtDamage;
    [SerializeField] float weaponDamage;

    private void Awake()
    {
        recoil = GetComponent<WeaponRecoil>();
    }

    private void Start()
    {
        canDealDamage = false;
        hasDealtDamage = new List<GameObject>();
    }

    Vector3 GetPosition(Bullet bullet)
    {
        Vector3 gravity = Vector3.down * bulletDrop;
        return (bullet.initialPosition) + (bullet.initialVelocity * bullet.time) + (0.5f * gravity * bullet.time * bullet.time);
    }

    Bullet CreateBullet(Vector3 position, Vector3 velocity)
    {
        Bullet bullet = new Bullet();
        bullet.initialPosition = position;
        bullet.initialVelocity = velocity;
        bullet.time = 0.0f;
        bullet.tracer = Instantiate(tracerEffect, position, Quaternion.identity);
        bullet.tracer.AddPosition(position);
        bullet.bounce = maxBounces;
        Color color = UnityEngine.Random.ColorHSV(0.46f, 0.61f);
        float intensity = 20.0f;
        Color rgb = new Color(color.r * intensity, color.g * intensity, color.b * intensity, color.a * intensity);
        bullet.tracer.material.SetColor("_EmissionColor", rgb);
        return bullet;
    }
    public void StartFiring()
    {
        isFiring = true;
        accumulatedTime = 0.0f;
        recoil.Reset();
    }

    public void UpdateWeapon(float deltaTime)
    {
        // Firing from the Guns
        if (!WeaponWheelController.Instance.weaponWheelSelected && !InventorySystem.Instance.isOpen && !CraftingSystem.Instance.isOpen && 
            !ConstructionManager.Instance.inConstructionMode && !RemoveConstruction.Instance.inRemoveConstructionMode && !DialogSystem.Instance.dialogUIActive && !BuyingSystem.Instance.isOpen)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                StartFiring();
            }
        }

        if (isFiring)
        {
            UpdateFiring(deltaTime);
        }
        UpdateBullets(deltaTime);
        if (Input.GetButtonUp("Fire1"))
        {
            StopFiring();
        } 
    }

    public void UpdateFiring(float deltaTime)
    {
        accumulatedTime += deltaTime;   
        float fireInterval = 1.0f / fireRate;
        while(accumulatedTime >= 0.0f)
        {
            FireBullet();
            accumulatedTime -= fireInterval;
        }
    }

    public void UpdateBullets(float deltaTime)
    {
        SimulateBullets(deltaTime);
        DestroyBullets();
    }

    // Moving the bullets
    void SimulateBullets(float deltaTime)
    {
        bullets.ForEach(bullet =>
        {
            Vector3 p0 = GetPosition(bullet);
            bullet.time += deltaTime;
            Vector3 p1 = GetPosition(bullet);
            RaycastSegment(p0, p1, bullet);
        });
    }

    void DestroyBullets()
    {
        bullets.RemoveAll(bullet => bullet.time >= maxLifetime);
    }

    // Show the Raycast for each buulets
    void RaycastSegment(Vector3 start, Vector3 end, Bullet bullet)
    {
        Vector3 direction = end - start;
        float distance = direction.magnitude;
        ray.origin = start;
        ray.direction = direction;

        Color debugColor = Color.green;

        if (Physics.Raycast(ray, out hitInfo, distance))
        {
            foreach (var particle in hitEffect)
            {
                particle.transform.position = hitInfo.point;
                particle.transform.forward = hitInfo.normal;
                SoundManager.Instance.gunParticleHitSound.Stop();
                SoundManager.Instance.PlaySound(SoundManager.Instance.gunParticleHitSound);
                particle.Emit(1);
            }

            if (hitInfo.transform.TryGetComponent(out Enemy enemy) && !hasDealtDamage.Contains(hitInfo.transform.gameObject))
            {
                enemy.TakeDamage(2);
                SoundManager.Instance.PlaySound(SoundManager.Instance.bloodSound);
                enemy.HitVFX(hitInfo.point);
            }

            bullet.time = maxLifetime;
            end = hitInfo.point;
            debugColor = Color.red;

            // Bullet ricochet
            if (bullet.bounce > 0)
            {
                bullet.time = 0;
                bullet.initialPosition = hitInfo.point;
                bullet.initialVelocity = Vector3.Reflect(bullet.initialVelocity, hitInfo.normal);
                bullet.bounce--;
            }

            // Collison impule
            var rb2d = hitInfo.collider.GetComponent<Rigidbody>();
            if (rb2d)
            {
                rb2d.AddForceAtPosition(ray.direction * 20, hitInfo.point, ForceMode.Impulse);
            }

        }

        if(bullet.tracer != null)
        {
            bullet.tracer.transform.position = end;
        }
        
    }

    private void FireBullet()
    {
        try
        {
            if (ammoCount <= 0)
            {
                return;
            }
            ammoCount--;

            foreach (var particle in muzzleFlash)
            {
                SoundManager.Instance.gunParticleEmitSound.Stop();
                SoundManager.Instance.PlaySound(SoundManager.Instance.gunParticleEmitSound);
                particle.Emit(1);
            }

            Vector3 velocity = (raycastDestination.position - raycastOrigin.position).normalized * bulletSpeed;
            var bullet = CreateBullet(raycastOrigin.position, velocity);
            bullets.Add(bullet);

            recoil.GenerateRecoil(weaponName);
        }
        catch (Exception ex)
        {
            Debug.LogError("Cannot Firing: " + ex.Message);
        }

    }

    public void StopFiring()
    {
        isFiring = false;
    }
}
