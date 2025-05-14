
using tuleeeeee.StateMachine;

public class PlayerDeadState : PlayerGroundedState
{
    public PlayerDeadState(Player player, StateManager stateManager, MovementDetailsSO playerData, int animBoolID) : base(player, stateManager, playerData, animBoolID)
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
