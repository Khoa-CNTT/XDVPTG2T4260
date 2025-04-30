using System;
using tuleeeeee.Enums;
using UnityEngine;

[DisallowMultipleComponent]
public class AimWeaponEvent : MonoBehaviour {
    public event Action<AimWeaponEvent, AimWeaponEventArgs> OnWeaponAim;
    
    public void CallAimWeaponEvent(Direction aimDirection, float aimAngle, float weaponAimAngle, Vector3 weaponAimDirectionVector){
        OnWeaponAim?.Invoke(this, new AimWeaponEventArgs(){
            aimDirection = aimDirection,
            aimAngle = aimAngle,
            weaponAimAngle = weaponAimAngle,
            weaponAimDirectionVector = weaponAimDirectionVector
        });
    }
}

public class AimWeaponEventArgs : EventArgs {
    public Direction aimDirection;
    public float aimAngle;
    public float weaponAimAngle;
    public Vector3 weaponAimDirectionVector;
}
