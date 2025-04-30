using tuleeeeee.Data;
using tuleeeeee.StateMachine;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player player, StateManager stateManager, PlayerDetailsSO playerData, string animBoolName) : base(player, stateManager, playerData, animBoolName)
    {
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        moveInput = player.InputHandler.RawMovementInput;

        if (moveInput == Vector2.zero && !isExitingState)
        {
            stateManager.ChangePlayerState(player.IdleState);
        }
    }
    public override void PhysicUpdate()
    {
        base.PhysicUpdate();

        ApplyMovement(moveInput);

    }
    private void ApplyMovement(Vector2 direction)
    {
        Movement.SetVelocity(playerData.movementVelocity, direction);
    }

}
