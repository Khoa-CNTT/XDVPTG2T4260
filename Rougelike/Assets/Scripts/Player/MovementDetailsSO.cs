using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MovementDetail_", menuName = "Scriptable Objects/Movement/Movement Details")]
public class MovementDetailsSO : ScriptableObject
{
    #region Header STATEDETAILS
    [Header("Move State Details")]
    #endregion
    #region Tooltip
    [Tooltip("Player Movement Velocity")]
    #endregion
    public float movementVelocity = 10f;
    #region Tooltip
    [Tooltip("Player Roll")]
    [Header("Roll Details")]
    #endregion
    public float rollCooldown = .4f;
    public float maxHoldTime = 1f;
    public float holdTimeScale = 0.25f;
    public float rollTime = .4f;
    public float rollVelocity = 15f;
    public float drag = 10f;
    public float rollEndYMultiplier = 0.2f;
    public float distBetweenAfterImages = 0.5f;
}
