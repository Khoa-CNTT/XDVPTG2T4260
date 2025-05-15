using System.Collections;
using System.Collections.Generic;
using tuleeeeee.Dungeon;
using tuleeeeee.Managers;
using tuleeeeee.Misc;
using tuleeeeee.StateMachine;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;


public class MoveState_SlimeBlock : EnemyMoveState
{
    private Enemy enemy;
    private float currentEnemyPathRebuildCooldown;
    private Vector3 playerReferencePosition;
    private bool chasePlayer;

    public MoveState_SlimeBlock(Entity entity, StateManager stateManager, string animBoolName, MovementDetailsSO enemyDetails, Enemy enemy) :
        base(entity, stateManager, animBoolName, enemyDetails)
    {
        this.enemy = enemy;
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
        float chaseDistance = enemy.EnemyDetails.chaseDistance;
        Vector3 playerPosition = GameManager.Instance.GetPlayer().GetPlayerPosition();

        currentEnemyPathRebuildCooldown -= Time.deltaTime;

        if (!chasePlayer && Vector3.Distance(enemy.transform.position, playerPosition) < chaseDistance)
        {
            chasePlayer = true;
        }

        if (!chasePlayer) return;

        if (Time.frameCount % Settings.targetFrameRateToSpreadPathfindingOver != enemy.updateFrameNumber) return;

        if (currentEnemyPathRebuildCooldown <= 0f ||
            (Vector3.Distance(playerReferencePosition, playerPosition) > Settings.playerMoveDistanceToRebuildPath))
        {
            currentEnemyPathRebuildCooldown = Settings.enemyPathRebulidCooldown;

            playerReferencePosition = GameManager.Instance.GetPlayer().GetPlayerPosition();
        }

        if (enemy.movementSteps == null)
        {
            stateManager.ChangeState(enemy.IdleState);
        }
    }
}
