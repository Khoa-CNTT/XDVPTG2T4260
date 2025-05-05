
using tuleeeeee.StateMachine;

public class EnemyMoveState : EnemyState
{
    protected Movement Movement { get => movement != null ? movement : Core.GetCoreComponent(ref movement); }

    private Movement movement;

    public EnemyMoveState(Entity entity, StateManager stateManager, string animBoolName, EnemyDetailsSO enemyDetails) : base(entity, stateManager, enemyDetails, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
   
}
