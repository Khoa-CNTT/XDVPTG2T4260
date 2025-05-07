using System;
using System.Collections;
using System.Collections.Generic;
using tuleeeeee.Data;
using tuleeeeee.Managers;
using tuleeeeee.Misc;
using tuleeeeee.MyInput;
using tuleeeeee.StateMachine;
using UnityEngine;

/*[RequireComponent(typeof(Health))]
[RequireComponent(typeof(InputHandler))]
[RequireComponent(typeof(SortingGroup))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[DisallowMultipleComponent]*/
public class Player : MonoBehaviour
{
    public Health Health { get => health != null ? health : Core.GetCoreComponent(ref health); }
    private Health health;
    public StateManager StateManager { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerRollState RollState { get; private set; }
    public PlayerDeadState DeadState { get; private set; }
    public PlayerDetailsSO PlayerDetails { get; private set; }

    public MovementDetailsSO MovementDetails;

    #region Components
    public Core Core { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; }
    public SpriteRenderer SpriteRenderer { get; private set; }
    public Animator Animator { get; private set; }
    #endregion

    public float MoveSpeed { get; private set; }
    private bool isPlayerMovementDisabled;

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
    public DestroyedEvent DestroyedEvent { get; private set; }
    #endregion

    public List<Weapon> weaponList = new List<Weapon>();
    private void OnEnable()
    {
        HealthEvent.OnHealthChanged += HealthEvent_OnHealthChanged;
    }
    private void OnDisable()
    {
        HealthEvent.OnHealthChanged -= HealthEvent_OnHealthChanged;
    }
    private void Awake()
    {
        #region Compoments
        Core = GetComponentInChildren<Core>();
        InputHandler = GetComponent<PlayerInputHandler>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        Animator = GetComponent<Animator>();
        StateManager = new StateManager();
        #endregion
        #region Events
        AimWeaponEvent = GetComponent<AimWeaponEvent>();
        SetActiveWeaponEvent = GetComponent<SetActiveWeaponEvent>();
        ActiveWeapon = GetComponent<ActiveWeapon>();
        FireWeaponEvent = GetComponent<FireWeaponEvent>();
        WeaponFiredEvent = GetComponent<WeaponFiredEvent>();
        ReloadWeaponEvent = GetComponent<ReloadWeaponEvent>();
        WeaponReloadedEvent = GetComponent<WeaponReloadedEvent>();
        StopReloadWeaponEvent = GetComponent<StopReloadWeaponEvent>();
        HealthEvent = GetComponentInChildren<HealthEvent>();
        DestroyedEvent = GetComponent<DestroyedEvent>();
        #endregion

    }
    private void Start()
    {

    }
    private void Update()
    {
        if (isPlayerMovementDisabled) return;
        Core.LogicUpdate();
        StateManager.CurrentPlayerState.LogicUpdate();
    }
    private void FixedUpdate()
    {
        StateManager.CurrentPlayerState.PhysicUpdate();
    }
    public void Initialize(PlayerDetailsSO playerDetailsSO)
    {
        this.PlayerDetails = playerDetailsSO;

        MoveSpeed = MovementDetails.GetMoveSpeed();

        SetPlayerHealth();

        CreatePlayerStartingWeapon();

        //  SetPlayerAnimationSpeed();

        IdleState = new PlayerIdleState(this, StateManager, MovementDetails, "isIdle");
        MoveState = new PlayerMoveState(this, StateManager, MovementDetails, "isMoving");
        RollState = new PlayerRollState(this, StateManager, MovementDetails, "isRolling");
        DeadState = new PlayerDeadState(this, StateManager, MovementDetails, "isDead");

        StateManager.Initialize(IdleState);
    }

    private void HealthEvent_OnHealthChanged(HealthEvent healthEvent, HealthEventArgs healthEventArgs)
    {

        if (healthEventArgs.healthAmount <= 0f)
        {
            StateManager.ChangeState(DeadState);
        }
    }

    private void SetPlayerHealth()
    {
        Health.SetStartingHealth(PlayerDetails.playerHealthAmount);
    }

    private void SetPlayerAnimationSpeed()
    {
        Animator.speed = MoveSpeed / Settings.baseSpeedForEnemyAnimation;
    }

    private void CreatePlayerStartingWeapon()
    {
        weaponList.Clear();

        foreach (WeaponDetailsSO weaponDetails in PlayerDetails.startingWeaponList)
        {
            AddWeaponToPlayer(weaponDetails);
        }
    }

    private Weapon AddWeaponToPlayer(WeaponDetailsSO weaponDetails)
    {
        Weapon weapon = new Weapon()
        {
            weaponDetails = weaponDetails,
            weaponReloadTimer = 0f,
            weaponClipRemainingAmmo = weaponDetails.weaponClipAmmoCapacity,
            weaponRemainingAmmo = weaponDetails.weaponAmmoCapacity,
            isWeaponReloading = false
        };

        weaponList.Add(weapon);

        weapon.weaponListPosition = weaponList.Count;

        SetActiveWeaponEvent.CallSetActiveWeaponEvent(weapon);

        return weapon;
    }

    public Vector3 GetPlayerPosition()
    {
        return transform.position;
    }

    public void EnablePlayer()
    {
        isPlayerMovementDisabled = false;
    }

    public void DisablePlayer()
    {
        isPlayerMovementDisabled = true;
        StateManager.ChangeState(IdleState);
    }
    public void AnimationTrigger() => StateManager.CurrentPlayerState.AnimationTrigger();

    public void AnimationFinishedTrigger() => StateManager.CurrentPlayerState.AnimationFinishedTrigger();
}
