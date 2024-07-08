using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;


public class CharacterAiming : MonoBehaviour
{
    public float turnSpeed = 15;
    public float aimDuration = 0.3f;

    public Transform cameraLookAt;
    public Cinemachine.AxisState xAxis;
    public Cinemachine.AxisState yAxis;

    Camera mainCamera;
    Animator animator;
    ActiveWeapon activeWeapon;
    int isAimingParam = Animator.StringToHash("isAiming");
    public bool isAiming;
    public bool isPermanantAimed;

    public float recoilModifier_Aiming = 0.3f;
    public float recoilModifier_notAiming = 1.0f;

    RaycastWeapon weapon;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        weapon = GetComponentInChildren<RaycastWeapon>();
        animator = GetComponent<Animator>();  
        activeWeapon = GetComponent<ActiveWeapon>();
    }

    void Update()
    {
        // We can change the view of the 3RD peron character
        if(!CraftingSystem.Instance.isOpen && !InventorySystem.Instance.isOpen && !ConstructionManager.Instance.inConstructionMode && !BuyingSystem.Instance.isOpen)
        {

            if (isPermanantAimed)
            {
                if (Input.GetKeyDown(KeyCode.V))
                {
                    isAiming = !isAiming;
                    animator.SetBool(isAimingParam, isAiming);
                    isPermanantAimed = false;   
                }
            }
            else if(Input.GetMouseButton(1))
            {
                isAiming = Input.GetMouseButton(1);
                animator.SetBool(isAimingParam, isAiming);
            }
            else
            {
                
                if (Input.GetKeyDown(KeyCode.V))
                {
                    isAiming = !isAiming;
                    animator.SetBool(isAimingParam, isAiming);
                    isPermanantAimed = true;
                }
                else
                {
                    isAiming = Input.GetMouseButton(1);
                    animator.SetBool(isAimingParam, isAiming);
                }
            }
  
        }
        
        var weapon = activeWeapon.GetActiveWeapon();
        if (weapon)
        {
            weapon.recoil.recoilModifier = isAiming ? recoilModifier_Aiming : recoilModifier_notAiming;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Roatating the character
        if (!InventorySystem.Instance.isOpen && !CraftingSystem.Instance.isOpen && !WeaponWheelController.Instance.weaponWheelSelected && !DialogSystem.Instance.dialogUIActive && !BuyingSystem.Instance.isOpen && !AssetsManager.Instance.isOpen && !MissionObjectMenuController.Instance.isOpen && !MenuManager.Instance.isMenuOpen)
        {
            xAxis.Update(Time.fixedDeltaTime);
            yAxis.Update(Time.fixedDeltaTime);

            cameraLookAt.eulerAngles = new Vector3(yAxis.Value, xAxis.Value, 0);

            float yCamera = mainCamera.transform.rotation.eulerAngles.y;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yCamera, 0), turnSpeed * Time.fixedDeltaTime);
        }
    }

    private void LateUpdate()
    {
        // Firing
        if (weapon)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                weapon.StartFiring();
            }
            if (weapon.isFiring)
            {
                weapon.UpdateFiring(Time.deltaTime);
            }
            weapon.UpdateBullets(Time.deltaTime);
            if (Input.GetButtonUp("Fire1"))
            {
                weapon.StopFiring();
            }
        }

    }
}
