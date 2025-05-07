
using tuleeeeee.Dungeon;

public class Enemy : Entity
{
    public IdleState_SlimeBlock IdleState { get; private set; }
    public MoveState_SlimeBlock MoveState { get; private set; }


    public override void Awake()
    {
        base.Awake();
    }

    public override void EnemyInitialization(EnemyDetailsSO enemyDetails, int enemySpawnNumber, DungeonLevelSO dungeonLevel, bool materialize)
    {
        base.EnemyInitialization(enemyDetails, enemySpawnNumber, dungeonLevel, materialize);

        IdleState = new IdleState_SlimeBlock(this, StateManager, "isIdle", MovementDetails, this);
        MoveState = new MoveState_SlimeBlock(this, StateManager, "isMoving", MovementDetails, this);

        StateManager.Initialize(MoveState);
    }

    public override void Start()
    {
        base.Start();
    }

}
