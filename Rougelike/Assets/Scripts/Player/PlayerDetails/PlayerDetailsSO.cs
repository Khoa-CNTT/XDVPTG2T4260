using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using tuleeeeee.Utilities;

namespace tuleeeeee.Data
{
    [CreateAssetMenu(fileName = "Player Details", menuName = "Scriptable Objects/Player/Player Details")]
    public class PlayerDetailsSO : ScriptableObject
    {
        #region Header PLAYER BASE DETAILS
        [Space(10)]
        [Header("PLAYER BASE DETAILS")]
        #endregion
        #region Tooltip
        [Tooltip("Player character name.")]
        #endregion
        public string playerCharacterName;

        #region Tooltip
        [Tooltip("Prefab gameobject for the player")]
        #endregion
        public GameObject playerPrefab;

        #region Tooltip
        [Tooltip("Player runtime animator controller")]
        #endregion
        public RuntimeAnimatorController runtimeAnimatorController;

        #region Header HEALTH
        [Space(10)]
        [Header("HEALTH")]
        #endregion
        #region Tooltip
        [Tooltip("Player starting health amount")]
        #endregion
        public int playerHealthAmount;
        public bool isImmuneAfterHit = false;
        public float hitImmunityTime;

        #region Header WEAPON
        [Space(10)]
        [Header("WEAPON")]
        #endregion
        public WeaponDetailsSO startingWeapon;
        public List<WeaponDetailsSO> startingWeaponList;

        #region Header OTHER
        [Space(10)]
        [Header("OTHER")]
        #endregion
        #region Tooltip
        [Tooltip("Player icon sprite to be used in the minimap")]
        #endregion
        public Sprite playerMiniMapIcon;

        #region Tooltip
        [Tooltip("Player ahnd sprite")]
        #endregion
        public Sprite playerHandSprite;

        #region Validation
#if UNITY_EDITOR

        private void OnValidate()
        {
            HelperUtilities.ValidateCheckEmptyString(this, nameof(playerCharacterName), playerCharacterName);
            HelperUtilities.ValidateCheckNullValue(this, nameof(playerPrefab), playerPrefab);
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(playerHealthAmount), playerHealthAmount, false);
            /*HelperUtilities.ValidateCheckNullValue(this, nameof(startingWeapon), startingWeapon);
            HelperUtilities.ValidateCheckEnumerableValues(this, nameof(startingWeaponList), startingWeaponList);*/
            HelperUtilities.ValidateCheckNullValue(this, nameof(playerMiniMapIcon), playerMiniMapIcon);
            HelperUtilities.ValidateCheckNullValue(this, nameof(playerHandSprite), playerHandSprite);
            HelperUtilities.ValidateCheckNullValue(this, nameof(runtimeAnimatorController), runtimeAnimatorController);
            if (isImmuneAfterHit)
            {
                // HelperUtilities.ValidateCheckPositiveValue(this, nameof(hitImmunityTime), hitImmunityTime, false);
            }
        }
#endif
        #endregion Validation
    }
}
