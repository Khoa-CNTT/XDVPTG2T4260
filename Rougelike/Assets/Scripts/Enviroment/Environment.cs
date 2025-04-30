using System.Collections;
using tuleeeeee.Utilities;
using UnityEngine;

namespace tuleeeeee.environment
{
    public class Environment : MonoBehaviour
    {
        #region Header References
        [Space(10)]
        [Header("Reference")]
        #endregion
        public SpriteRenderer spriteRenderer;

        #region Validation
#if UNITY_EDITOR
        private void OnValidate()
        {
            HelperUtilities.ValidateCheckNullValue(this, nameof(spriteRenderer), spriteRenderer);
        }
#endif
        #endregion
    }
}