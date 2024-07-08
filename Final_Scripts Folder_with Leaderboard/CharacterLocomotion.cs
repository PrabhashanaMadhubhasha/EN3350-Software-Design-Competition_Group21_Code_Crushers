using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLocomotion : MonoBehaviour
{
    public Animator rigController;
    public Character character;
    public float jumpHeight;
    public float gravity;
    public float stepDown;
    public float airControl;
    public float jumpDamp;
    public float groundSpeed;
    public float pushPower;

    Animator animator;
    CharacterController characterController;
    ActiveWeapon activeWeapon;
    ReloadWeapon reloadWeapon;
    CharacterAiming characterAiming;
    Vector2 input;

    Vector3 rootMotion;
    Vector3 velocity;
    public Vector3 positionDisplacement;
    bool isJumping;

    int isSprintingParam = Animator.StringToHash("isSprinting");
    int isWalkingParam = Animator.StringToHash("isWalking");

    public Vector2 lastPosition = new Vector2(0f, 0f);
    public bool isMoving;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();    
        characterController = GetComponent<CharacterController>();
        activeWeapon = GetComponent<ActiveWeapon>();  
        reloadWeapon = GetComponent<ReloadWeapon>(); 
        characterAiming = GetComponent<CharacterAiming>();
    }

    // Update is called once per frame
    void Update()
    {
        // Movement of the character
        if (!DialogSystem.Instance.dialogUIActive && !MenuManager.Instance.isMenuOpen)
        {
            input.x = Input.GetAxis("Horizontal");
            input.y = Input.GetAxis("Vertical");

            animator.SetFloat("InputX", input.x);
            animator.SetFloat("InputY", input.y);

            UpdateIsSprinting();
            UpdateIsWalking();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }

            if (lastPosition != new Vector2(input.x, input.y))
            {
                isMoving = true;
            }
            else
            {
                isMoving = false;
            }

            if (isMoving && !IsSprinting() && characterController.isGrounded)
            {
                SoundManager.Instance.PlaySound(SoundManager.Instance.grassWalkSound);
            }
            else
            {
                SoundManager.Instance.grassWalkSound.Stop();
            }
        }
        else
        {
            animator.SetFloat("InputX", 0);
            animator.SetFloat("InputY", 0);
        }

    }

    public bool IsSprinting()
    {
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        bool isFiring = activeWeapon.IsFiring();
        bool isReloading = reloadWeapon.isReloading;
        bool isChangingWeapon = activeWeapon.isChangingWeapon;
        bool isAiming = characterAiming.isAiming;
        return isSprinting && !isFiring && !isReloading && !isChangingWeapon && !isAiming;
    }

    bool IsWalking()
    {
        bool isIdle = activeWeapon.isIdle;
        return isIdle;
    }
    private void UpdateIsSprinting() // When running
    {
        bool isSprinting = IsSprinting(); 
        if(isSprinting && characterController.isGrounded)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.grassSprintSound);
        }
        else
        {
            SoundManager.Instance.grassSprintSound.Stop();
        }
        animator.SetBool(isSprintingParam, isSprinting);
        rigController.SetBool(isSprintingParam, isSprinting);   
    }

    private void UpdateIsWalking() // when walking
    {
        bool isWalking = IsWalking();

        animator.SetBool(isWalkingParam, isWalking);
        rigController.SetBool(isWalkingParam, isWalking);
    }

    private void OnAnimatorMove()
    {
        rootMotion += animator.deltaPosition;
    }

    private void FixedUpdate()
    {
        if (isJumping) // IsInAir State 
        {
            UpdateInAir();
        }
        else // IsGrounded State
        {
            UpdateOnGround();
        }

    }

    private void UpdateOnGround()
    {
        Vector3 stepForwardAmount = rootMotion * groundSpeed;
        Vector3 stepDownAmount = Vector3.down * stepDown;
        positionDisplacement = stepForwardAmount + stepDownAmount;
        characterController.Move(positionDisplacement);
        rootMotion = Vector3.zero;

        if (!characterController.isGrounded)
        {
            SetInAir(0);
        }
    }

    private void UpdateInAir()
    {
        velocity.y -= gravity * Time.fixedDeltaTime;
        Vector3 displacement = velocity * Time.fixedDeltaTime;
        displacement += CalculateAirControl();
        characterController.Move(displacement);
        isJumping = !characterController.isGrounded;
        rootMotion = Vector3.zero;
        animator.SetBool("isJumping", isJumping);
    }

    Vector3 CalculateAirControl()
    {
        return ((transform.forward * input.y) + (transform.right * input.x)) * (airControl / 100);  
    }

    void Jump()
    {
        if (!isJumping)
        {
            float jumpVelocity = Mathf.Sqrt(2 * gravity * jumpHeight);
            SetInAir(jumpVelocity);
        }
    }

    private void SetInAir(float jumpVelocity)
    {
        isJumping = true;
        velocity = animator.velocity * jumpDamp * groundSpeed;
        velocity.y = jumpVelocity;
        animator.SetBool("isJumping", true);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        // no rigidbody
        if (body == null || body.isKinematic)
            return;

        // We dont want to push objects below us
        if (hit.moveDirection.y < -0.3f)
            return;

        // Calculate push direction from move direction,
        // we only push objects to the sides never up and down
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        // If you know how fast your character is trying to move,
        // then you can also multiply the push velocity by that.

        // Apply the push
        body.velocity = pushDir * pushPower;
    }
}
