using System.Collections;
using System.Collections.Generic;
using tuleeeeee.Enums;
using tuleeeeee.Misc;
using UnityEngine;

[DisallowMultipleComponent]
public class AnimateEnemy : MonoBehaviour
{
    private Entity entity;

    private void Awake()
    {
        entity = GetComponent<Entity>();
    }

    private void OnEnable()
    {
        entity.AimWeaponEvent.OnWeaponAim += AimWeaponEvent_OnWeaponAim;
    }

    private void OnDisable()
    {

        entity.AimWeaponEvent.OnWeaponAim -= AimWeaponEvent_OnWeaponAim;
    }

    private void AimWeaponEvent_OnWeaponAim(AimWeaponEvent aimWeaponEvent, AimWeaponEventArgs aimWeaponEventArgs)
    {
        InitialiseAimAnimationParameters();
        SetAimWeaponAnimationParameters(aimWeaponEventArgs.aimDirection);
    }

    private void InitialiseAimAnimationParameters()
    {
        entity.Animator.SetBool(Settings.aimUp, false);
        entity.Animator.SetBool(Settings.aimUpRight, false);
        entity.Animator.SetBool(Settings.aimRight, false);
        entity.Animator.SetBool(Settings.aimDown, false);
    }

    private void SetAimWeaponAnimationParameters(Direction aimDirection)
    {
        switch (aimDirection)
        {
            case Direction.up:
                entity.Animator.SetBool(Settings.aimUp, true);
                break;
            case Direction.upleft:
            case Direction.upright:
                entity.Animator.SetBool(Settings.aimUpRight, true);
                break;
            case Direction.left:
            case Direction.right:
                entity.Animator.SetBool(Settings.aimRight, true);
                break;
            case Direction.down:
                entity.Animator.SetBool(Settings.aimDown, true);
                break;
        }
    }

}
