using System.Collections;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
public class CombatState : State
{
    bool sheathWeapon;
    bool attack;
    public CombatState(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
    {
        character = _character;
        stateMachine = _stateMachine;
    }

    public override void Enter()
    {
        base.Enter();

        sheathWeapon = false;
        attack = false;

    }

    public override void HandleInput()
    {
        base.HandleInput();

        if (drawWeaponAction.triggered || drawAxeAction.triggered)
        {
            sheathWeapon = true;
        }

        if (attackAction.triggered)
        {
            attack = true;
        }

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (sheathWeapon && EquipSystem.Instance.isAxeIsEquipped) // Sheath Axe and Draw Sword
        {
            character.swapAxeToSword = true;
            character.animator.SetTrigger("sheathWeapon");

            stateMachine.ChangeState(character.standing);

            character.isCombatMode = false;

            EquipSystem.Instance.isAxeIsEquipped = false;
            EquipSystem.Instance.SelectCurrentQuickSlot();

            character.isGettingSword = true;


            if (character.sheathWeapon)
            {
                character.sheathWeapon = false;
                sheathWeapon = true;
            }

        }

        else if (sheathWeapon || character.sheathWeapon || (!EquipSystem.Instance.isAxeIsEquipped && EquipmentSystem.Instance.drawAxe && !character.swapAxeToSword)) // Sheath Weapon or Sheath sword and drw Axe
        {
            character.swapAxeToSword = false;
            character.animator.SetTrigger("sheathWeapon");

            stateMachine.ChangeState(character.standing);

            character.isCombatMode = false;

            if (character.sheathWeapon)
            {
                character.sheathWeapon = false;
                sheathWeapon = true;
            }
   
        }

        else if (EquipSystem.Instance.isAxeIsEquipped && EquipmentSystem.Instance.drawSword)
        {
            character.swapAxeToSword = false;
            EquipmentSystem.Instance.drawSword = false;

            character.animator.SetTrigger("sheathWeapon");

            stateMachine.ChangeState(character.standing);

            character.isCombatMode = false;

            sheathWeapon = false;

        }

        if (attack && !EquipSystem.Instance.haveDamagedAxe && !InventorySystem.Instance.isOpen && !CraftingSystem.Instance.isOpen && !RemoveConstruction.Instance.inRemoveConstructionMode && !WeaponWheelController.Instance.weaponWheelSelected)
        {
            character.animator.SetTrigger("attack");
            stateMachine.ChangeState(character.attacking);
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