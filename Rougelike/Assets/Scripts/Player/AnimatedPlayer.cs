using System.Collections;
using System.Collections.Generic;
using tuleeeeee.Enums;
using tuleeeeee.Misc;
using UnityEngine;

public class AnimatedPlayer : MonoBehaviour
{
    private Player player;
    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void OnEnable()
    {
        player.AimWeaponEvent.OnWeaponAim += AimWeaponEvent_OnWeaponAim;
    }

    private void OnDisable()
    {
        player.AimWeaponEvent.OnWeaponAim -= AimWeaponEvent_OnWeaponAim;
    }

    private void AimWeaponEvent_OnWeaponAim(AimWeaponEvent aimWeaponEvent, AimWeaponEventArgs aimWeaponEventArgs)
    {
        InitializeAimAnimationParameters();

        SetAimWeaponAnimationParamters(aimWeaponEventArgs.aimDirection);
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
}
