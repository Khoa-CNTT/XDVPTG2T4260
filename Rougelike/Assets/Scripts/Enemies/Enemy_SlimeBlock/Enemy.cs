

using System.Collections;
using System.Collections.Generic;
using tuleeeeee.Dungeon;
using tuleeeeee.Managers;
using tuleeeeee.Misc;
using UnityEngine;

public class Enemy : Entity
{
    public Stack<Vector3> movementSteps { get; private set; }
    public Coroutine moveEnemyRoutine;
    private List<Vector2Int> surroundingPositionList = new List<Vector2Int>();

    public IdleState_SlimeBlock IdleState { get; private set; }
    public MoveState_SlimeBlock MoveState { get; private set; }

    public override void EnemyInitialization(EnemyDetailsSO enemyDetails, int enemySpawnNumber, DungeonLevelSO dungeonLevel, bool materialize)
    {
        base.EnemyInitialization(enemyDetails, enemySpawnNumber, dungeonLevel, materialize);
        IdleState = new IdleState_SlimeBlock(this, StateManager, "isIdle", MovementDetails, this);
        MoveState = new MoveState_SlimeBlock(this, StateManager, "isMoving", MovementDetails, this);
        StateManager.Initialize(IdleState);

    }
    public override void Update()
    {
        base.Update();

        if (Time.frameCount % Settings.targetFrameRateToSpreadPathfindingOver != updateFrameNumber) return;
        CreatePath();
        StartCoroutine(MoveEnemyRoutine(movementSteps));

#if UNITY_EDITOR
        DrawPathInScene(movementSteps, Color.white);
#endif
    }
    public IEnumerator MoveEnemyRoutine(Stack<Vector3> movementSteps)
    {
        while (movementSteps.Count > 0)
        {
            Vector3 nextPosition = movementSteps.Pop();

            while (Vector3.Distance(nextPosition, transform.position) > 0.2f)
            {
                Vector3 direction = (nextPosition - transform.position).normalized;
                Movement.SetVelocity(MoveSpeed, direction);
                yield return waitForFixedUpdate;
            }

            yield return waitForFixedUpdate;

            moveEnemyRoutine = null;
            Movement.SetVelocityZero();
        }
    }
#if UNITY_EDITOR
    public void DrawPathInScene(Stack<Vector3> path, Color color)
    {
        if (path == null || path.Count == 0) return;

        Vector3 previous = path.Peek();
        foreach (Vector3 current in path)
        {
            Debug.DrawLine(previous, current, color, 5f);
            previous = current;
        }
    }
#endif
    public void CreatePath()
    {
        Room currentRoom = GameManager.Instance.GetCurrentRoom();
        Vector3 playerPosition = GameManager.Instance.GetPlayer().GetPlayerPosition();
        Grid grid = currentRoom.instantiatedRoom.grid;

        Vector3Int enemyGridPosition = grid.WorldToCell(transform.position);
        Vector3Int playerCellPosition = grid.WorldToCell(playerPosition);
        Vector3Int playerGridPosition = GetNearestNonObstaclePlayerPosition(currentRoom, playerPosition, playerCellPosition);

        movementSteps = AStar.BuildPath(currentRoom, enemyGridPosition, playerGridPosition);

        if (movementSteps != null)
        {
            movementSteps.Pop();
        }
        /*else
        {
            enemy.idleEvent.CallIdleEvent();
        }*/
    }
    public Vector3Int GetNearestNonObstaclePlayerPosition(Room currentRoom, Vector3 playerPosition, Vector3Int playerCellPosition)
    {

        Vector2Int adjustedPlayerCellPosition = new Vector2Int(playerCellPosition.x - currentRoom.templateLowerBounds.x,
           playerCellPosition.y - currentRoom.templateLowerBounds.y);
        int obstacle = Mathf.Min(currentRoom.instantiatedRoom.aStarMovementPenalty[adjustedPlayerCellPosition.x, adjustedPlayerCellPosition.y],
            currentRoom.instantiatedRoom.aStarItemObstacles[adjustedPlayerCellPosition.x, adjustedPlayerCellPosition.y]);

        if (obstacle == 0)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (j == 0 && i == 0) continue;

                    try
                    {
                        obstacle = currentRoom.instantiatedRoom.aStarMovementPenalty[adjustedPlayerCellPosition.x + i, adjustedPlayerCellPosition.y + j];
                        if (obstacle != 0)
                        {
                            return new Vector3Int(playerCellPosition.x + i, playerCellPosition.y + j, 0);
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
            }

        }


        /* if (obstacle == 0)
         {
             surroundingPositionList.Clear();

             for (int i = -1; i <= 1; i++)
             {
                 for (int j = -1; j <= 1; j++)
                 {
                     if (j == 0 && i == 0) continue;

                     surroundingPositionList.Add(new Vector2Int(i, j));
                 }
             }

             for (int l = 0; l < 8; l++)
             {
                 int index = Random.Range(0, surroundingPositionList.Count);

                 try
                 {
                     obstacle = Mathf.Min(currentRoom.instantiatedRoom.aStarMovementPenalty[
                         adjustedPlayerCellPosition.x + surroundingPositionList[index].x,
                         adjustedPlayerCellPosition.y + surroundingPositionList[index].y],
                         currentRoom.instantiatedRoom.aStarItemObstacles[
                         adjustedPlayerCellPosition.x + surroundingPositionList[index].x,
                         adjustedPlayerCellPosition.y + surroundingPositionList[index].y]);

                     if (obstacle != 0)
                     {
                         return new Vector3Int(playerCellPosition.x + surroundingPositionList[index].x,
                             playerCellPosition.y + surroundingPositionList[index].y);
                     }
                 }
                 catch
                 {

                 }

                 surroundingPositionList.RemoveAt(index);
             }
             return (Vector3Int)currentRoom.spawnPositionArray[Random.Range(0, currentRoom.spawnPositionArray.Length)];
         }*/

        return playerCellPosition;
    }

}
