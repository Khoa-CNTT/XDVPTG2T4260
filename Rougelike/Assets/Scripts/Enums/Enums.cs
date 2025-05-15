

namespace tuleeeeee.Enums
{
    public enum Orientation
    {
        east,
        west,
        south,
        north,
        none
    }
    public enum ChestSpawnEvent
    {
        onRoomEntry,
        onEnemiesDefeated
    }
    public enum ChestSpawnPosition
    {
        atSpawnerPosition,
        atPlayerPosition
    }
    public enum Direction
    {
        up,
        upleft,
        upright,
        left, 
        right,
        down,
    }
    public enum GameState
    {
        gameStarted,
        playingLevel,
        engagingEnemies,
        bossStage,
        engagingBoss,
        levelCompleted,
        gameWon,
        gameLost,
        gamePaused,
        dungeonOverviewMap,
        restartGame
    }
    public enum ComponentType
    {
        IFireable,
        WeaponShootEffect,
        AmmoHitEffect,
        SoundEffect,
    }
}