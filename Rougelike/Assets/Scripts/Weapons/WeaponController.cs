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
    private bool isShooting;

    private PlayerInputHandler inputHandler;
    private Player player;

    private Vector3 currentWeaponDirection;
    private float currentWeaponAngle;
    private float currentPlayerAngle;
    private Direction currentAimDirection;

    private void Awake()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
        player = GetComponent<Player>();
    }

    private void OnEnable()
    {
        inputHandler.LookEvent.AddListener(OnAim);
        inputHandler.AttackEvent.AddListener(OnShoot);
    }
    private void OnDisable()
    {
        inputHandler.LookEvent.RemoveListener(OnAim);
        inputHandler.AttackEvent.RemoveListener(OnShoot);
    }
    public void OnShoot(bool shootInput)
    {
        isShooting = shootInput;
        FireWeaponInput(isShooting, currentWeaponDirection, currentWeaponAngle, currentPlayerAngle, currentAimDirection);
    }

    public void OnAim(Vector2 newAimDirection)
    {
        aimDirection = newAimDirection;
        CalculateAimParameters();
        player.AimWeaponEvent.CallAimWeaponEvent(currentAimDirection, currentPlayerAngle, currentWeaponAngle, currentWeaponDirection);
    }

    private void FireWeaponInput(bool isShoot, Vector3 weaponDirection, float weaponAngle, float playerAngle, Direction playerAimDirection)
    {
        if (isShoot && !player.RollState.IsRolling)
        {
            player.FireWeaponEvent.CallFireWeaponEvent(true, playerAimDirection, playerAngle, weaponAngle, weaponDirection);
        }
    }

    private void CalculateAimParameters()
    {
        currentWeaponDirection = aimDirection;
        currentWeaponAngle = HelperUtilities.GetAngleFromVector(currentWeaponDirection);
        currentPlayerAngle = HelperUtilities.GetAngleFromVector(aimDirection);
        currentAimDirection = HelperUtilities.GetDirection(currentPlayerAngle);
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
