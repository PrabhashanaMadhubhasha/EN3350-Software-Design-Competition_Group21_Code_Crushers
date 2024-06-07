using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class State // The Clas for State Standing, combat and attack for melee weapon related
{
    public Character character;
    public StateMachine stateMachine;

    public InputAction drawWeaponAction;
    public InputAction drawAxeAction;
    public InputAction attackAction;
    //public ActiveWeapon activeWeapon;
    public bool isCombatMode;

    public State(Character _character, StateMachine _stateMachine)
    {
        character = _character;
        stateMachine = _stateMachine;
        drawWeaponAction = character.playerInput.actions["DrawSword"];
        drawAxeAction = character.playerInput.actions["DrawAxe"];
        attackAction = character.playerInput.actions["Attack"];

    }

    public virtual void Enter()
    {

    }

    public virtual void HandleInput()
    {
    }

    public virtual void LogicUpdate()
    {
    }

    public virtual void PhysicsUpdate()
    {
    }

    public virtual void Exit()
    {
    }

}