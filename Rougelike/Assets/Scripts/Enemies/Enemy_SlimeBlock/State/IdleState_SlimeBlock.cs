using System.Collections;
using System.Collections.Generic;
using tuleeeeee.StateMachine;
using UnityEngine;

public class IdleState_SlimeBlock : EnemyIdleState
{
    private Enemy enemy;

    public float cooldownTime = 2f; // cooldown duration in seconds
    private float nextReadyTime = 0f;
    public IdleState_SlimeBlock(Entity entity, StateManager stateManager, string animBoolName, MovementDetailsSO enemyDetails, Enemy enemy) : base(entity, stateManager, animBoolName, enemyDetails)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        nextReadyTime = 0;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(Time.time >= nextReadyTime)
        {
            enemy.StateManager.ChangeState(enemy.MoveState);
            nextReadyTime = Time.time + cooldownTime;
        }
    }
}
