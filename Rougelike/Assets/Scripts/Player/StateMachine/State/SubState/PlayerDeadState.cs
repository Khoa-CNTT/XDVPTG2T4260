using System.Collections;
using System.Collections.Generic;
using tuleeeeee.Data;
using tuleeeeee.StateMachine;
using UnityEngine;

public class PlayerDeadState : PlayerGroundedState
{
    public PlayerDeadState(Player player, StateManager stateManager, MovementDetailsSO playerData, string animBoolName) : base(player, stateManager, playerData, animBoolName)
    {
    }

    public override void AnimationFinishedTrigger()
    {
        base.AnimationFinishedTrigger();
        player.DestroyedEvent.CallDestroyedEvent(true, 0);
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
    }

    public override void Enter()
    {
        base.Enter();
        Movement.SetVelocityZero();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
