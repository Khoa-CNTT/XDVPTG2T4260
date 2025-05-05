using System.Collections.Generic;
using tuleeeeee.Dungeon;

[System.Serializable]

public class SpawnableObjectByLevel<T>
{
    public DungeonLevelSO dungeonLevel;
    public List<SpawnableObjectRatio<T>> spawnableObjectRatioList;
}
