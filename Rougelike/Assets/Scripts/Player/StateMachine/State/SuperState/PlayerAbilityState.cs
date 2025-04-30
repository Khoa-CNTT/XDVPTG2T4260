using System.Collections;
using System.Collections.Generic;
using tuleeeeee.Data;
using tuleeeeee.StateMachine;
using UnityEngine;

public class PlayerAbilityState : PlayerState
{
    protected Movement Movement { get => movement != null ? movement : Core.GetCoreComponent(ref movement); }

    private Movement movement;

    protected bool isAbilityDone;
    protected int xInput;
    protected Vector2 moveInput;
    public PlayerAbilityState(Player player, StateManager stateManager, PlayerDetailsSO playerData, string animBoolName) : base(player, stateManager, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        isAbilityDone = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        xInput = player.InputHandler.NormInputX;
        moveInput = player.InputHandler.RawMovementInput;

        if (isAbilityDone)
        {

            stateManager.ChangePlayerState(player.IdleState);

        }
    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
    }
}
