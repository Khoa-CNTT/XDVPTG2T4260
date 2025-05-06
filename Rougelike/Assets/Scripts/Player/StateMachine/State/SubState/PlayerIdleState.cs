using tuleeeeee.Data;
using tuleeeeee.StateMachine;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player player, StateManager stateManager, MovementDetailsSO playerData, string animBoolName) : base(player, stateManager, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Movement.SetVelocityZero();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        moveInput = player.InputHandler.RawMovementInput;

        if (moveInput != Vector2.zero && !isExitingState)
        {
            stateManager.ChangeState(player.MoveState);
        }
    }
   
}
