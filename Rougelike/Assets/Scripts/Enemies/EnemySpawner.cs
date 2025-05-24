using System.Collections;
using tuleeeeee.Dungeon;
using tuleeeeee.Enums;
using tuleeeeee.Managers;
using tuleeeeee.Misc;
using tuleeeeee.StaticEvent;
using UnityEngine;
using static tuleeeeee.StaticEvent.StaticEventHandler;

[DisallowMultipleComponent]
public class EnemySpawner : SingletonMonoBehaviour<EnemySpawner>
{
    private int enemiesToSpawn;
    private int currentEnemyCount;
    private int enemiesSpawedSoFar;
    private int enemyMaxConcurrentSpawnNumber;
    private int totalFirstWave;
    private Room currentRoom;
    private RoomEnemySpawnParameters roomEnemySpawnParameters;

    private void OnEnable()
    {
        OnRoomChanged += StaticEventHandler_OnRoomChanged;
    }
    private void OnDisable()
    {
        OnRoomChanged -= StaticEventHandler_OnRoomChanged;
    }

    private void StaticEventHandler_OnRoomChanged(RoomChangedEventArgs roomChangedEventArgs)
    {
        enemiesSpawedSoFar = 0;
        currentEnemyCount = 0;

        currentRoom = roomChangedEventArgs.room;

        MusicManager.Instance.PlayMusic(currentRoom.ambientMusic, 0.2f, 2f);

        if (currentRoom.roomNodeType.isCorridorEW || currentRoom.roomNodeType.isCorridorNS || currentRoom.roomNodeType.isEntrance) return;
        if (currentRoom.isClearedOfEnemies) return;

        DungeonLevelSO currentDungeonLevel = GameManager.Instance.GetCurrentDungeonLevel();
        enemiesToSpawn = currentRoom.GetNumberOfEnemiesToSpawn(currentDungeonLevel);
        roomEnemySpawnParameters = currentRoom.GetRoomEnemySpawnParameters(currentDungeonLevel);

        if (enemiesToSpawn == 0)
        {
            currentRoom.isClearedOfEnemies = true;
            return;
        }

        enemyMaxConcurrentSpawnNumber = GetConcurrentEnemies();

        totalFirstWave = roomEnemySpawnParameters.totalFirstWave;

        MusicManager.Instance.PlayMusic(currentRoom.battleMusic, 0.2f, 0.5f);



        currentRoom.instantiatedRoom.LockDoors();

        SpawnEnemies();

    }

    private void SpawnEnemies()
    {

        if (GameManager.Instance.gameState == GameState.bossStage)
        {
            GameManager.Instance.previousGameState = GameState.bossStage;
            GameManager.Instance.gameState = GameState.engagingBoss;
        }
        else if (GameManager.Instance.gameState == GameState.playingLevel)
        {
            GameManager.Instance.previousGameState = GameState.playingLevel;
            GameManager.Instance.gameState = GameState.engagingEnemies;
        }

        StartCoroutine(SpawnEnemiesRoutine());
    }

    private IEnumerator SpawnEnemiesRoutine()
    {
        Grid grid = currentRoom.instantiatedRoom.grid;

        RandomSpawnableObject<EnemyDetailsSO> randomEnemyHelperClass = new RandomSpawnableObject<EnemyDetailsSO>(currentRoom.enemiesByLevelList);

        if (currentRoom.spawnPositionArray.Length > 0)
        {
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                while (currentEnemyCount >= enemyMaxConcurrentSpawnNumber || (currentEnemyCount > 0 && enemiesSpawedSoFar == totalFirstWave))
                {
                    yield return null;
                }

                Vector3Int cellPosition = (Vector3Int)currentRoom.spawnPositionArray[Random.Range(0, currentRoom.spawnPositionArray.Length)];

                if (enemiesSpawedSoFar >= totalFirstWave)
                {
                    CreateEnemy(randomEnemyHelperClass.GetItem(), grid.CellToWorld(cellPosition), true);
                }
                else
                {
                    CreateEnemy(randomEnemyHelperClass.GetItem(), grid.CellToWorld(cellPosition), false);
                }

                yield return new WaitForSeconds(GetEnemySpawnInterval());
            }
        }
    }

    private float GetEnemySpawnInterval()
    {
        return Random.Range(roomEnemySpawnParameters.minSpawnInterval, roomEnemySpawnParameters.maxSpawnInterval);
    }

    private int GetConcurrentEnemies()
    {
        return Random.Range(roomEnemySpawnParameters.minConcurrentEnemies, roomEnemySpawnParameters.maxConcurrentEnemies);
    }

    private void CreateEnemy(EnemyDetailsSO enemyDetails, Vector3 position, bool materialize)
    {

        enemiesSpawedSoFar++;
        currentEnemyCount++;

        DungeonLevelSO currentDungeonLevel = GameManager.Instance.GetCurrentDungeonLevel();

        GameObject enemy = Instantiate(enemyDetails.enemyPrefab, position, Quaternion.identity, transform);

        enemy.GetComponent<Entity>().EnemyInitialization(enemyDetails, enemiesSpawedSoFar, currentDungeonLevel, materialize);

        // if (boss){
        //     GameManager.Instance.boss = enemy.GetComponent<Enemy>();
        // }

        enemy.GetComponent<DestroyedEvent>().OnDestroyed += Enemy_OnDestroyed;

    }

    private void Enemy_OnDestroyed(DestroyedEvent destroyedEvent, DestroyedEventArgs destroyedEventArgs)
    {
        destroyedEvent.OnDestroyed -= Enemy_OnDestroyed;

        currentEnemyCount--;

        StaticEventHandler.CallPointScoredEvent(destroyedEventArgs.points);

        if (currentEnemyCount <= 0 && enemiesSpawedSoFar == enemiesToSpawn)
        {
            currentRoom.isClearedOfEnemies = true;

            if (GameManager.Instance.gameState == GameState.engagingEnemies)
            {
                GameManager.Instance.previousGameState = GameState.engagingEnemies;
                GameManager.Instance.gameState = GameState.playingLevel;
            }
            else if (GameManager.Instance.gameState == GameState.engagingBoss)
            {
                GameManager.Instance.previousGameState = GameState.engagingBoss;
                GameManager.Instance.gameState = GameState.playingLevel;
            }

            currentRoom.instantiatedRoom.UnlockDoors(Settings.doorUnlockDelay);

            MusicManager.Instance.PlayMusic(currentRoom.ambientMusic, 0.2f, 2f);

            StaticEventHandler.CallRoomEnemiesDefeatedEvent(currentRoom);
        }

    }
}
