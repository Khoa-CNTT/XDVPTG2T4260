using tuleeeeee.StateMachine;
using UnityEngine;

public class EnemyIdleState : EnemyState
{
    protected Movement Movement { get => movement != null ? movement : Core.GetCoreComponent(ref movement); }

    private Movement movement;

    protected bool isIdleTimeOver;

    private float idleTime;
    public EnemyIdleState(Entity entity, StateManager stateManager, string animBoolName, MovementDetailsSO enemyDetails) : base(entity, stateManager, enemyDetails, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        isIdleTimeOver = false;
        SetRandomIdleTime();
        Movement.SetVelocityZero();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        Movement.SetVelocityZero();
        if (Time.time >= StartTime + idleTime)
        {
            isIdleTimeOver = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
    private void SetRandomIdleTime()
    {
        idleTime = Random.Range(1f, 1f);
    }
}
