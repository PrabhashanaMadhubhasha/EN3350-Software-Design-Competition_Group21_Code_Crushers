using UnityEngine;
using UnityEngine.TextCore.Text;

public class StandingState : State
{
    public bool drawWeapon;
    bool drawAxe;
    public StandingState(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
    {
        character = _character;
        stateMachine = _stateMachine;
    }

    public override void Enter()
    {   
        base.Enter();
        drawWeapon = false;
        drawAxe = false;
    }

    public override void HandleInput()
    {
        base.HandleInput();

        if (drawWeaponAction.triggered || character.isGettingSword)
        {
            drawWeapon = true;
            character.isChangingWeapon = false;
            character.isGettingSword = false;
            character.radialMenuSword = false;
        }
        else if ((drawAxeAction.triggered || character.isGettingAxe)) 
        {
            drawAxe = true;
            character.isChangingWeapon = false;
            character.isGettingAxe = false;
            character.radialMenuSword = false;
        }
    }

    public override void LogicUpdate()
    {
        if (drawWeapon)
        {
            character.isCombatMode = true;
            stateMachine.ChangeState(character.combatting);
            character.animator.SetTrigger("drawWeapon");
        }
        else if (drawAxe)
        {
            character.isCombatMode = true;
            stateMachine.ChangeState(character.combatting);
            character.animator.SetTrigger("drawAxe");
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

    }

    public override void Exit()
    {
        base.Exit();

    }

}
