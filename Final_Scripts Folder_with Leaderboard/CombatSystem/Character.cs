using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour
{
    [Header("State")]
    public StateMachine movementSM;
    public StandingState standing;
    public CombatState combatting;
    public AttackState attacking;

    [HideInInspector]
    public PlayerInput playerInput;
    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public ActiveWeapon activeWeapon;

    public InputAction drawWeaponAction;
    public InputAction drawAxeAction;


    public Vector3 playerVelocity;

    [Header("Conditions")]
    public bool isCombatMode;
    public bool sheathWeapon;

    public bool swapAxeToSword;
    public bool isChangingWeapon = false;

    public bool isGettingSword = false;
    public bool isGettingAxe = false;

    public bool isEquipedAxe = false;

    public bool radialMenuSword = false;
    public bool radialMenuAxe = false;


    // Start is called before the first frame update
    private void Start()
    {
        activeWeapon = GetComponent<ActiveWeapon>();
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        movementSM = new StateMachine();
        standing = new StandingState(this, movementSM);
        combatting = new CombatState(this, movementSM);
        attacking = new AttackState(this, movementSM);

        movementSM.Initialize(standing);

        drawWeaponAction = playerInput.actions["DrawSword"];

        drawAxeAction = playerInput.actions["DrawAxe"];

        
    }

    private void Update()
    {

        if (activeWeapon.isIdle) // if the player not get the rifle or pistol
        {
            StartCoroutine(DrawMeleeWeaponIdle());

            movementSM.currentState.HandleInput();

            movementSM.currentState.LogicUpdate();

        }
        else
        {
            StartCoroutine(DrawMeleeWeaponNotIdle());
        }
    }

    // Draw melle weapon (sword or axe) when in Idle mode
    IEnumerator DrawMeleeWeaponIdle()
    {

        if (radialMenuSword)
        {
            isGettingSword = true;
        }
        if ((radialMenuAxe || isEquipedAxe) && !isCombatMode)
        {
            isEquipedAxe = false;
            radialMenuAxe = false;
            yield return new WaitForSeconds(0.5f);
            isGettingAxe = true;
        }
    }

    // Draw melle weapon (sword or axe) when not in Idle mode
    IEnumerator DrawMeleeWeaponNotIdle()
    {
        if (drawWeaponAction.triggered || radialMenuSword)
        {
            radialMenuSword = false;
            activeWeapon.ToggleActiveWeapon();
            yield return new WaitForSeconds(1f);
            isGettingSword = true;

        }
        if (drawAxeAction.triggered || radialMenuAxe || isEquipedAxe)
        {
            isEquipedAxe = false;
            radialMenuAxe = false;
            activeWeapon.ToggleActiveWeapon();
            yield return new WaitForSeconds(1f);
            isGettingAxe = true;

        }
    }


    private void FixedUpdate()
    {
        movementSM.currentState.PhysicsUpdate();
    }
}