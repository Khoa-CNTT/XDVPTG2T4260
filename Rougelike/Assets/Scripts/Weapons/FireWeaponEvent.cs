using System;
using System.Collections;
using System.Collections.Generic;
using tuleeeeee.Enums;
using UnityEngine;

public class FireWeaponEvent : MonoBehaviour
{
    public event Action<FireWeaponEvent, FireWeaponEventArgs> OnFireWeapon;

    public void CallFireWeaponEvent(bool fire, bool firePreviousFrame, Direction aimDirection, float aimAngle,
        float weaponAimAngle, Vector3 weaponAimDirectionVector, float fireTime = 0)
    {
        OnFireWeapon?.Invoke(this, new FireWeaponEventArgs()
        {
            fire = fire,
            firePreviousFrame = firePreviousFrame,
            aimDirection = aimDirection,
            aimAngle = aimAngle,
            weaponAimAngle = weaponAimAngle,
            weaponAimDirectionVector = weaponAimDirectionVector,
            fireTime = fireTime
        });
    }
}
public class FireWeaponEventArgs : EventArgs
{
    public bool fire;
    public bool firePreviousFrame; // Holding fire
    public Direction aimDirection;
    public float aimAngle;
    public float weaponAimAngle;
    public Vector3 weaponAimDirectionVector;
    public float fireTime;
}
