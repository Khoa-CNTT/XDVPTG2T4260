
using System.Collections;
using System.Collections.Generic;
using tuleeeeee.Dungeon;
using tuleeeeee.Managers;
using tuleeeeee.Misc;
using tuleeeeee.StateMachine;
using UnityEngine;


public class Entity : MonoBehaviour
{
    protected Movement Movement { get => movement != null ? movement : Core.GetCoreComponent(ref movement); }

    private Movement movement;
    public Health Health { get => health != null ? health : Core.GetCoreComponent(ref health); }
    private Health health;
    public Core Core { get; private set; }
    public StateManager StateManager { get; private set; }

    public EnemyDetailsSO EnemyDetails { get; private set; }
    public MovementDetailsSO MovementDetails;

    private FireWeapon fireWeapon;
    private MaterializeEffect materializeEffect;

    public WaitForFixedUpdate waitForFixedUpdate { get; private set; }

    #region COMPONENTS
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

    private bool isEnemyMovementDisabled;

    public Stack<Vector3> movementsList = new Stack<Vector3>();

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
        waitForFixedUpdate = new WaitForFixedUpdate();
    }
    public virtual void Update()
    {
        if (isEnemyMovementDisabled) return;
        Core.LogicUpdate();
        StateManager.CurrentEnemyState.LogicUpdate();

    }
    public virtual void FixedUpdate()
    {
        StateManager.CurrentEnemyState.PhysicsUpdate();
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
    public void SetUpdateFrameNumber(int updateFrameNumber)
    {
        this.updateFrameNumber = updateFrameNumber;
    }
}
