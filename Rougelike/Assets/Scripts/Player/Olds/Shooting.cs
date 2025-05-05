using System.Collections;
using System.Collections.Generic;
using tuleeeeee.MyInput;
using tuleeeeee.Utilities;
using UnityEngine;
using UnityEngine.UIElements;

public class Shooting : MonoBehaviour
{
  /*  [Header("Parameters")]

    private Vector2 _aimDirection;

    private Player player;
    private PlayerInputHandler inputHandler;

    private ActiveWeapon activeWeapon;

    private int currentWeaponIndex = 1;

    private float firePreChargeTimer = 0f;
    private float fireRateCoolDownTimer = 0f;
    private void Awake()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
        activeWeapon = GetComponent<ActiveWeapon>();
        player = GetComponent<Player>();
    }

    private void Start()
    {
        SetStartingWeapon();
        //inputHandler.AttackEvent.AddListener(OnShoot);
        //inputHandler.LookEvent.AddListener(OnAim);
    }
    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        
    }
    /// <summary>
    ///     To call when the aim changes direction
    /// </summary>
    /// <param name="newAimDirection"> The new aim direction </param>
    public void OnAim(Vector2 newAimDirection)
    {
        _aimDirection = newAimDirection.normalized;
    }
    public void OnShoot()
    {
        Vector2 playerDirection = _aimDirection;

        Vector2 weaponDirection = _aimDirection;

        float playerAngleDegrees = HelperUtilities.GetAngleFromVector(playerDirection);

        float weaponAimAngle = HelperUtilities.GetAngleFromVector(weaponDirection);

        FireAmmo(playerAngleDegrees, weaponAimAngle, weaponDirection);

    }
    private void FireAmmo(float aimAngle, float weaponAimAngle, Vector2 weaponAimDirectionVector)
    {
        AmmoDetailsSO currentAmmo = activeWeapon.GetCurrentAmmo();

        if (currentAmmo != null)
        {
            GameObject ammoPrefab = currentAmmo.ammoPrefabArray[Random.Range(0, currentAmmo.ammoPrefabArray.Length)];

            float ammoSpeed = Random.Range(currentAmmo.ammoSpeedMin, currentAmmo.ammoSpeedMax);

            IFireable ammo = (IFireable)PoolManager.Instance.ReuseComponent(ammoPrefab, activeWeapon.GetShootPosition(), Quaternion.identity);

            ammo.InitialiseAmmo(currentAmmo, aimAngle, weaponAimAngle, ammoSpeed, weaponAimDirectionVector);

            Weapon currentWeapon = activeWeapon.GetCurrentWeapon();
            if (!currentWeapon.weaponDetails.hasInfiniteClipCapacity)
            {
                currentWeapon.weaponClipRemainingAmmo--;
                currentWeapon.weaponRemainingAmmo--;
            }
        }
    }
    private void ResetCoolDownTimer()
    {
        fireRateCoolDownTimer = activeWeapon.GetCurrentWeapon().weaponDetails.weaponFireRate;
    }
    private static Vector2 RotateVector2(Vector2 v, float degrees)
    {
        return Quaternion.Euler(0, 0, degrees) * v;
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
        if (weaponIndex - 1 < player.weaponList.Count)
        {
            currentWeaponIndex = weaponIndex;
            Weapon weapon = player.weaponList[weaponIndex - 1];
            player.SetActiveWeaponEvent.CallSetActiveWeaponEvent(weapon);
        }
    }*/
}
