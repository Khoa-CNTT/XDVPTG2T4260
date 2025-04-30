using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using tuleeeeee.Data;
using tuleeeeee.StateMachine;


public class PlayerGroundedState : PlayerState
{
    protected Movement Movement { get => movement != null ? movement : Core.GetCoreComponent(ref movement); }

    private Movement movement;

    protected Vector2 moveInput;
    private bool dashInput;
    public PlayerGroundedState(Player player, StateManager stateManager, PlayerDetailsSO playerData, string animBoolName) : base(player, stateManager, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.RollState.ResetCanRoll();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        moveInput = player.InputHandler.RawMovementInput;
        dashInput = player.InputHandler.RollInput;

        if (moveInput != Vector2.zero && dashInput && player.RollState.CheckIfCanDash())
        {
            stateManager.ChangePlayerState(player.RollState);
        }
    }
}
