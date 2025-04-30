using System;
using System.Collections;
using System.Collections.Generic;
using tuleeeeee.Data;
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
    public Movement Movement { get => movement != null ? movement : Core.GetCoreComponent(ref movement); }

    private Movement movement;
    public StateManager StateManager { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerRollState RollState { get; private set; }
    public PlayerDetailsSO PlayerDetails { get; private set; }

    #region Components
    public Core Core { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; }
    public Health Health { get; private set; }
    public SpriteRenderer SpriteRenderer { get; private set; }
    public Animator Animator { get; private set; }
    #endregion

    private float moveSpeed;

    #region EVENTS
    [HideInInspector] public AimWeaponEvent AimWeaponEvent { get; private set; }
    [HideInInspector] public SetActiveWeaponEvent SetActiveWeaponEvent { get; private set; }
    [HideInInspector] public ActiveWeapon ActiveWeapon { get; private set; }
    [HideInInspector] public FireWeaponEvent FireWeaponEvent { get; private set; }
    [HideInInspector] public WeaponFiredEvent WeaponFiredEvent { get; private set; }
    [HideInInspector] public ReloadWeaponEvent ReloadWeaponEvent { get; private set; }
    [HideInInspector] public WeaponReloadedEvent WeaponReloadedEvent { get; private set; }
    [HideInInspector] public StopReloadWeaponEvent StopReloadWeaponEvent { get; private set; }
    #endregion

    public List<Weapon> weaponList = new List<Weapon>();
    private void Awake()
    {
        #region Compoments
        Core = GetComponentInChildren<Core>();
        InputHandler = GetComponent<PlayerInputHandler>();
        Health = GetComponent<Health>();
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
        #endregion

    }
    private void Start()
    {

    }
    private void Update()
    {
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

        moveSpeed = PlayerDetails.movementVelocity;

        CreatePlayerStartingWeapon();

        SetPlayerHealth();

        //  SetPlayerAnimationSpeed();


        IdleState = new PlayerIdleState(this, StateManager, PlayerDetails, "isIdle");
        MoveState = new PlayerMoveState(this, StateManager, PlayerDetails, "isMoving");
        RollState = new PlayerRollState(this, StateManager, PlayerDetails, "isRolling");

        StateManager.InitializePlayer(IdleState);
    }

    private void SetPlayerHealth()
    {
        Health.SetStartingHealth(PlayerDetails.playerHealthAmount);
    }

    private void SetPlayerAnimationSpeed()
    {
        Animator.speed = moveSpeed / Settings.baseSpeedForEnemyAnimation;
    }

    public Vector3 GetPlayerPosition()
    {
        return transform.position;
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
}
