using System.Collections;
using System.Collections.Generic;
using tuleeeeee.Enums;
using UnityEngine;


[RequireComponent(typeof(AimWeaponEvent))]
[DisallowMultipleComponent]
public class AimWeapon : MonoBehaviour
{
    #region Tooltip
    [Tooltip("Populate with the Transform from the child WeaponeRotationPoint gameobject")]
    #endregion
    [SerializeField] private Transform weaponRotationPointTransform;

    private AimWeaponEvent aimWeaponEvent;
    private Player player;

    private void Awake()
    {
        aimWeaponEvent = GetComponent<AimWeaponEvent>();
        player = GetComponent<Player>();
    }

    private void OnEnable()
    {
        aimWeaponEvent.OnWeaponAim += AimWeaponEvent_OnWeaponAim;
    }

    private void OnDisable()
    {
        aimWeaponEvent.OnWeaponAim -= AimWeaponEvent_OnWeaponAim;
    }

    private void AimWeaponEvent_OnWeaponAim(AimWeaponEvent aimWeaponEvent, AimWeaponEventArgs aimWeaponEventArgs)
    {
        Aim(aimWeaponEventArgs.aimDirection, aimWeaponEventArgs.aimAngle);
    }

    private void Aim(Direction aimDirection, float aimAngle)
    {
        //Set angle
        weaponRotationPointTransform.eulerAngles = new Vector3(0f, 0f, aimAngle);

        // flipping logic
        bool isFacingLeft = aimAngle > 90f || aimAngle < -90f;

        float xScale = isFacingLeft ? -1f : 1f;

        if (!player.RollState.IsRolling)
        {
            transform.localScale = new Vector3(xScale, transform.localScale.y, transform.localScale.z);
        }
        // Flip the armPivot
        weaponRotationPointTransform.localScale = new Vector3(xScale, isFacingLeft ? -1 : 1, weaponRotationPointTransform.localScale.z);

    }

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        //HelperUtilities.ValidateCheckNullValue(this, nameof(weaponRotationPointTransform), weaponRotationPointTransform);
    }

#endif
    #endregion


}
