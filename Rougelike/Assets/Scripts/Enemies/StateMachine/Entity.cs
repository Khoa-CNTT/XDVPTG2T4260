
using System.Collections;
using System.Collections.Generic;
using tuleeeeee.Dungeon;
using tuleeeeee.Managers;
using tuleeeeee.Misc;
using tuleeeeee.StateMachine;
using UnityEngine;


public class Entity : MonoBehaviour
{
    public Health Health { get => health != null ? health : Core.GetCoreComponent(ref health); }
    private Health health;
    public Core Core { get; private set; }
    public StateManager StateManager { get; private set; }

    public EnemyDetailsSO EnemyDetails { get; private set; }
    public MovementDetailsSO MovementDetails;

    private FireWeapon fireWeapon;
    private MaterializeEffect materializeEffect;

    #region COMPONENTS
    public Rigidbody2D RB { get; private set; }
    public Animator Animator { get; private set; }
    public SpriteRenderer[] spriteRendererArray { get; private set; }
    public CircleCollider2D CircleCollider { get; private set; }
    public BoxCollider2D BoxCollider { get; private set; }
    #endregion
    #region EVENTS
    public AimWeaponEvent AimWeaponEvent { get; private set; }
    public SetActiveWeaponEvent SetActiveWeaponEvent { get; private set; }
    public ActiveWeapon ActiveWeapon { get; private set; }
    public FireWeaponEvent FireWeaponEvent { get; private set; }
    public WeaponFiredEvent WeaponFiredEvent { get; private set; }
    public ReloadWeaponEvent ReloadWeaponEvent { get; private set; }
    public WeaponReloadedEvent WeaponReloadedEvent { get; private set; }
    public StopReloadWeaponEvent StopReloadWeaponEvent { get; private set; }
    public HealthEvent HealthEvent { get; private set; }
    #endregion

    public float MoveSpeed { get; private set; }    

    public int updateFrameNumber = 1;
    private List<Vector2Int> surroundingPositionList = new List<Vector2Int>();

    private bool isEnemyMovementDisabled;

    public virtual void OnEnable()
    {
        HealthEvent.OnHealthChanged += HealthEvent_OnHealthChanged;
    }
    public virtual void OnDisable()
    {
        HealthEvent.OnHealthChanged -= HealthEvent_OnHealthChanged;
    }
    public virtual void Awake()
    {
        #region COMPONENTS
        Core = GetComponentInChildren<Core>();
        Animator = GetComponent<Animator>();
        spriteRendererArray = GetComponentsInChildren<SpriteRenderer>();
        CircleCollider = GetComponent<CircleCollider2D>();
        BoxCollider = GetComponent<BoxCollider2D>();
        StateManager = new StateManager();
        #endregion
        fireWeapon = GetComponent<FireWeapon>();
        materializeEffect = GetComponent<MaterializeEffect>();
        #region EVENTS
        AimWeaponEvent = GetComponent<AimWeaponEvent>();
        SetActiveWeaponEvent = GetComponent<SetActiveWeaponEvent>();
        ActiveWeapon = GetComponent<ActiveWeapon>();
        FireWeaponEvent = GetComponent<FireWeaponEvent>();
        WeaponFiredEvent = GetComponent<WeaponFiredEvent>();
        ReloadWeaponEvent = GetComponent<ReloadWeaponEvent>();
        WeaponReloadedEvent = GetComponent<WeaponReloadedEvent>();
        StopReloadWeaponEvent = GetComponent<StopReloadWeaponEvent>();
        HealthEvent = GetComponentInChildren<HealthEvent>();
        #endregion
    }
    public virtual void Start()
    {

    }
    public virtual void Update()
    {
        if (isEnemyMovementDisabled) return;
        Core.LogicUpdate();
        StateManager.CurrentEnemyState.LogicUpdate();
    }

    public virtual void EnemyInitialization(EnemyDetailsSO enemyDetails, int enemySpawnNumber, DungeonLevelSO dungeonLevel, bool materialize)
    {
        this.EnemyDetails = enemyDetails;

        MoveSpeed = MovementDetails.GetMoveSpeed();

        SetUpdateFrameNumber(enemySpawnNumber % Settings.targetFrameRateToSpreadPathfindingOver);

        SetEnemyStartingHealth(dungeonLevel);

        SetEnemyStartingWeapon();

        StartCoroutine(MaterializeEnemy());

    }
    private void HealthEvent_OnHealthChanged(HealthEvent healthEvent, HealthEventArgs healthEventArgs)
    {
        if (healthEventArgs.healthAmount <= 0)
        {
            EnemyDestroyed();
        }
    }

    private void EnemyDestroyed()
    {
        DestroyedEvent destroyedEvent = GetComponent<DestroyedEvent>();
        destroyedEvent.CallDestroyedEvent(false, health.GetStartingHealth());
    }

    private void SetEnemyStartingHealth(DungeonLevelSO dungeonLevel)
    {
        EnemyHealthDetails[] enemyHealthDetailsArray = EnemyDetails.enemyHealthDetailsArray;
        foreach (EnemyHealthDetails enemyHealthDetails in enemyHealthDetailsArray)
        {
            if (enemyHealthDetails.dungeonLevel == dungeonLevel)
            {
                Health.SetStartingHealth(enemyHealthDetails.enemyHealthAmount);
                return;
            }
        }
    }
    private void SetEnemyStartingWeapon()
    {
        if (EnemyDetails.enemyWeapon != null)
        {
            Weapon weapon = new Weapon()
            {
                weaponDetails = EnemyDetails.enemyWeapon,
                weaponReloadTimer = 0f,
                weaponClipRemainingAmmo = EnemyDetails.enemyWeapon.weaponClipAmmoCapacity,
                weaponRemainingAmmo = EnemyDetails.enemyWeapon.weaponAmmoCapacity,
                isWeaponReloading = false
            };

            SetActiveWeaponEvent.CallSetActiveWeaponEvent(weapon);
        }
    }

    private IEnumerator MaterializeEnemy()
    {
        EnemyEnable(false);

        yield return StartCoroutine(materializeEffect.MaterializeRoutine(EnemyDetails.enemyMaterializeShader,
            EnemyDetails.enemyMaterializeColor, EnemyDetails.enemyMaterializeTime, spriteRendererArray, EnemyDetails.enemyStandardMaterial));

        EnemyEnable(true);
    }

    private void EnemyEnable(bool isEnabled)
    {
        CircleCollider.enabled = isEnabled;
        BoxCollider.enabled = isEnabled;
        isEnemyMovementDisabled = isEnabled;
        fireWeapon.enabled = isEnabled;
    }
    public Stack<Vector3> CreatePath()
    {
        Room currentRoom = GameManager.Instance.GetCurrentRoom();
        Vector3 playerPosition = GameManager.Instance.GetPlayer().GetPlayerPosition();
        Grid grid = currentRoom.instantiatedRoom.grid;

        Vector3Int enemyGridPosition = grid.WorldToCell(transform.position);
        Vector3Int playerCellPosition = grid.WorldToCell(playerPosition);
        Vector3Int playerGridPosition = GetNearestNonObstaclePlayerPosition(currentRoom, playerPosition, playerCellPosition);

        return AStar.BuildPath(currentRoom, enemyGridPosition, playerGridPosition);
    }
    public Vector3Int GetNearestNonObstaclePlayerPosition(Room currentRoom, Vector3 playerPosition, Vector3Int playerCellPosition)
    {

        Vector2Int adjustedPlayerCellPosition = new Vector2Int(playerCellPosition.x - currentRoom.templateLowerBounds.x,
            playerCellPosition.y - currentRoom.templateLowerBounds.y);
        int obstacle = Mathf.Min(currentRoom.instantiatedRoom.aStarMovementPenalty[adjustedPlayerCellPosition.x, adjustedPlayerCellPosition.y],
            currentRoom.instantiatedRoom.aStarItemObstacles[adjustedPlayerCellPosition.x, adjustedPlayerCellPosition.y]);

        if (obstacle == 0)
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
        }

        return playerCellPosition;
    }

    public void SetUpdateFrameNumber(int updateFrameNumber)
    {
        this.updateFrameNumber = updateFrameNumber;
    }

}
