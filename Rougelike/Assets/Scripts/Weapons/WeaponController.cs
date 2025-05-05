using System.Collections;
using System.Collections.Generic;
using tuleeeeee.Enums;
using tuleeeeee.Misc;
using tuleeeeee.MyInput;
using tuleeeeee.Utilities;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private Vector2 aimDirection;
    private Vector2 scrollValue;
    private int weaponSlotIndex;
    private bool fastSwitchWeapon;
    private bool isShooting;

    private PlayerInputHandler inputHandler;
    private Player player;

    private Vector3 currentWeaponDirection;
    private float currentWeaponAngle;
    private float currentPlayerAngle;
    private Direction currentAimDirection;

    private bool leftMouseDownPreviousFrame = false;

    private float fireTime = 0;

    private bool reverse = false;

    private int currentWeaponIndex = 1;
    private int previousWeaponIndex = 0;

    private void Awake()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
        player = GetComponent<Player>();
    }
    private void Start()
    {
        SetStartingWeapon();
    }
    private void OnEnable()
    {
        inputHandler.LookEvent.AddListener(OnAim);
        inputHandler.AttackEvent.AddListener(OnShoot);
        inputHandler.ScrollEvent.AddListener(OnScroll);
        inputHandler.SelectWeaponEvent.AddListener(OnSelectWeapon);
        inputHandler.FastSwitchWeaponEvent.AddListener(OnFastSwitch);
    }
    private void OnDisable()
    {
        inputHandler.LookEvent.RemoveListener(OnAim);
        inputHandler.AttackEvent.RemoveListener(OnShoot);
        inputHandler.ScrollEvent.RemoveListener(OnScroll);
        inputHandler.SelectWeaponEvent.RemoveListener(OnSelectWeapon);
        inputHandler.FastSwitchWeaponEvent.RemoveListener(OnFastSwitch);
    }
    private void Update()
    {
        if (isShooting)
        {
            fireTime += Time.deltaTime;
            FireWeaponInput(isShooting, currentWeaponDirection, currentWeaponAngle, currentPlayerAngle, currentAimDirection);
        }
    }
    public void OnShoot(bool shootInput)
    {
        isShooting = shootInput;
        // FireWeaponInput(isShooting, currentWeaponDirection, currentWeaponAngle, currentPlayerAngle, currentAimDirection);
    }

    public void OnAim(Vector2 newAimDirection)
    {
        aimDirection = newAimDirection;
        CalculateAimParameters();
        AimWeapon(currentWeaponDirection, currentWeaponAngle, currentPlayerAngle, currentAimDirection);
    }
    public void OnScroll(Vector2 newScrollValue)
    {
        scrollValue = newScrollValue;
        SwitchWeaponInput(scrollValue);
    }
    public void OnSelectWeapon(int newWeaponSlotIndex)
    {
        weaponSlotIndex = newWeaponSlotIndex;
        SetWeaponByIndex(weaponSlotIndex);
    }
    public void OnFastSwitch(bool isClicked)
    {
        fastSwitchWeapon = isClicked;
        Debug.Log("FastSwitch");
        FastSwitchWeapon();
    }
    private void CalculateAimParameters()
    {
        currentWeaponDirection = aimDirection;
        currentWeaponAngle = HelperUtilities.GetAngleFromVector(currentWeaponDirection);
        currentPlayerAngle = HelperUtilities.GetAngleFromVector(aimDirection);
        currentAimDirection = HelperUtilities.GetDirection(currentPlayerAngle);
    }
    private void AimWeapon(Vector3 weaponDirection, float weaponAngle, float playerAngle, Direction playerAimDirection)
    {
        player.AimWeaponEvent.CallAimWeaponEvent(playerAimDirection, playerAngle, weaponAngle, weaponDirection);
    }

    private void FireWeaponInput(bool isShoot, Vector3 weaponDirection, float weaponAngle, float playerAngle, Direction playerAimDirection)
    {
        if (isShoot && !player.RollState.IsRolling)
        {
            player.FireWeaponEvent.CallFireWeaponEvent(true, leftMouseDownPreviousFrame, playerAimDirection, playerAngle, weaponAngle, weaponDirection, fireTime);
            leftMouseDownPreviousFrame = true;
        }
        else
        {
            fireTime = 0;
            leftMouseDownPreviousFrame = false;
        }
    }

    private void SwitchWeaponInput(Vector2 newScrollValue)
    {
        if (newScrollValue.y < 0f)
        {
            PreviousWeapon();
        }
        if (newScrollValue.y > 0f)
        {
            NextWeapon();
        }
    }


    private void SetStartingWeapon()
    {
        int index = 1;
        foreach (Weapon weapon in player.weaponList)
        {
            if (weapon.weaponDetails == player.PlayerDetails.startingWeapon)
            {
                SetWeaponByIndex(index);
                break;
            }
            index++;
        }
    }
    private void SetWeaponByIndex(int weaponIndex)
    {
        Weapon currentWeapon = player.ActiveWeapon.GetCurrentWeapon();

        if (weaponIndex - 1 < player.weaponList.Count)
        {
            previousWeaponIndex = currentWeaponIndex;
            currentWeaponIndex = weaponIndex;
            Weapon weapon = player.weaponList[weaponIndex - 1];
            if (weapon == currentWeapon) return;
            player.StopReloadWeaponEvent.CallStopReloadWeapon(currentWeapon);
            player.SetActiveWeaponEvent.CallSetActiveWeaponEvent(weapon);
        }
    }

    private void PreviousWeapon()
    {
        currentWeaponIndex--;
        if (currentWeaponIndex < 1)
        {
            currentWeaponIndex = player.weaponList.Count;
        }
        SetWeaponByIndex(currentWeaponIndex);

    }

    private void NextWeapon()
    {
        currentWeaponIndex++;
        if (currentWeaponIndex > player.weaponList.Count)
        {
            currentWeaponIndex = 1;
        }
        SetWeaponByIndex(currentWeaponIndex);
    }

    private void SwitchBetweenTwoWeapon()
    {
        if (!reverse)
        {
            NextWeapon();
        }
        else
        {
            PreviousWeapon();
        }
        reverse = !reverse;
    }

    private void FastSwitchWeapon()
    {
        SetWeaponByIndex(previousWeaponIndex);
    }

    private void SetCurrentWeaponToFirstInTheList()
    {
        List<Weapon> tempWeaponList = new List<Weapon>();

        Weapon currentWeapon = player.weaponList[currentWeaponIndex - 1];
        currentWeapon.weaponListPosition = 1;
        tempWeaponList.Add(currentWeapon);

        int index = 2;
        foreach (Weapon weapon in player.weaponList)
        {
            if (weapon == currentWeapon) continue;
            tempWeaponList.Add(weapon);
            weapon.weaponListPosition = index;
            index++;
        }

        player.weaponList = tempWeaponList;

        currentWeaponIndex = 1;

        SetWeaponByIndex(currentWeaponIndex);
    }

    #region ANIMATION
    private void AimAnimation(Vector2 direction)
    {
        float angle = HelperUtilities.GetAngleFromVector(direction);
        Direction aimDirection = HelperUtilities.GetDirection(angle);

        InitializeAimAnimationParameters();
        SetAimWeaponAnimationParamters(aimDirection);
    }
    private void InitializeAimAnimationParameters()
    {
        player.Animator.SetBool(Settings.aimUp, false);
        player.Animator.SetBool(Settings.aimUpRight, false);
        player.Animator.SetBool(Settings.aimRight, false);
        player.Animator.SetBool(Settings.aimDown, false);
    }
    private void SetAimWeaponAnimationParamters(Direction aimDirection)
    {
        switch (aimDirection)
        {
            case Direction.up:
                player.Animator.SetBool(Settings.aimUp, true);
                break;
            case Direction.upleft:
            case Direction.upright:
                player.Animator.SetBool(Settings.aimUpRight, true);
                break;
            case Direction.left:
            case Direction.right:
                player.Animator.SetBool(Settings.aimRight, true);
                break;
            case Direction.down:
                player.Animator.SetBool(Settings.aimDown, true);
                break;
        }
    }
    #endregion

}
