using tuleeeeee.Dungeon;
using UnityEditor.EditorTools;
using UnityEngine;

[System.Serializable]
public class RoomEnemySpawnParameters 
{
    [Tooltip("")]
    public DungeonLevelSO dungeonLevel;
    public int minTotalEnemiesToSpawn;
    public int maxTotalEnemiesToSpawn;
    public int minConcurrentEnemies;
    public int maxConcurrentEnemies;
    public int minSpawnInterval;
    public int maxSpawnInterval;
    public int totalFirstWave;
}
