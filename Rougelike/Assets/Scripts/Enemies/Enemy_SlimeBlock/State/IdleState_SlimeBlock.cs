using tuleeeeee.Managers;
using tuleeeeee.StateMachine;
using UnityEngine;

public class IdleState_SlimeBlock : EnemyIdleState
{
    private Enemy enemy;
    public IdleState_SlimeBlock(Entity entity, StateManager stateManager, string animBoolName, MovementDetailsSO enemyDetails, Enemy enemy) : base(entity, stateManager, animBoolName, enemyDetails)
    {
        this.enemy = enemy;
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isIdleTimeOver)
        {
            stateManager.ChangeState(enemy.MoveState);
        }
    }
}
