using System.Collections;
using System.Collections.Generic;
using tuleeeeee.Managers;
using tuleeeeee.Misc;
using tuleeeeee.StateMachine;
using UnityEngine;


public class MoveState_SlimeBlock : EnemyMoveState
{
    private Stack<Vector3> movementSteps;

    private float moveSpeed = 1f;
    private float chaseDistance;
    private bool isChasing = false;

    private Coroutine moveEnemyRoutine;

    private float pathUpdateCooldown = 0.5f;
    private float lastPathUpdateTime;

    private Vector3 playerReferencePosition;


    private float currentEnemyPathRebuildCooldown;

    private WaitForFixedUpdate waitForFixedUpdate;

    private Enemy enemy;
    public MoveState_SlimeBlock(Entity entity, StateManager stateManager, string animBoolName, EnemyDetailsSO enemyDetails, Enemy enemy) :
        base(entity, stateManager, animBoolName, enemyDetails)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        waitForFixedUpdate = new WaitForFixedUpdate();
        playerReferencePosition = GameManager.Instance.GetPlayer().GetPlayerPosition();
        chaseDistance = enemyDetails.chaseDistance;
        moveSpeed = 3f;
    }

    public override void Exit()
    {
        base.Exit();
        Movement.SetVelocityZero();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        Vector3 playerPosition = GameManager.Instance.GetPlayer().GetPlayerPosition();
        currentEnemyPathRebuildCooldown -= Time.deltaTime;
        // Check if player is in chase range
        if (!isChasing && Vector3.Distance(enemy.transform.position, playerPosition) < chaseDistance)
        {
            isChasing = true;
        }

        if (!isChasing) return;

        // Update path occasionally
        if (Time.frameCount % Settings.targetFrameRateToSpreadPathfindingOver != entity.updateFrameNumber) return;

        if (currentEnemyPathRebuildCooldown <= 0f ||
         (Vector3.Distance(playerReferencePosition, playerPosition) > Settings.playerMoveDistanceToRebuildPath))
        {
            currentEnemyPathRebuildCooldown = Settings.enemyPathRebulidCooldown;
            playerReferencePosition = GameManager.Instance.GetPlayer().GetPlayerPosition();

            RebuildPath();
        }
    }
    private void RebuildPath()
    {
        playerReferencePosition = GameManager.Instance.GetPlayer().GetPlayerPosition();

        var newPath = entity.CreatePath();

        if (newPath != null && newPath.Count > 0)
        {
            // Stop previous movement if any
            if (moveEnemyRoutine != null)
            {
                enemy.StopCoroutine(moveEnemyRoutine);
            }

            movementSteps = newPath;
            moveEnemyRoutine = enemy.StartCoroutine(MoveEnemyRoutine());
        }
    }

    private IEnumerator MoveEnemyRoutine()
    {
        enemy.StateManager.ChangeEnemyState(enemy.MoveState);

        while (movementSteps.Count > 0)
        {
            Vector3 nextPosition = movementSteps.Peek(); // Don't pop yet
            Vector3 direction = (nextPosition - enemy.transform.position).normalized;

            Movement.SetVelocity(moveSpeed, direction);

            // Check if reached the waypoint
            if (Vector3.Distance(enemy.transform.position, nextPosition) <= 0.2f)
            {
                movementSteps.Pop(); // Only remove after reaching
            }

            yield return waitForFixedUpdate;
        }

        // Path complete
        enemy.StateManager.ChangeEnemyState(enemy.IdleState);
        moveEnemyRoutine = null;
    }
}
