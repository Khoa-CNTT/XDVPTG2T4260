using tuleeeeee.Managers;
using UnityEngine.UI;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    #region Header GameObject Reference
    [Space(10)]
    [Header("GameObject References")]
    #endregion
    [SerializeField] private Image healthBar;
    public void EnableHealthBar(){
        gameObject.SetActive(true);
    }

    public void DisableHealthBar(){
        gameObject.SetActive(false);
    }

    public void SetHealthBarValue(float healthPerCent){
        healthBar.fillAmount = healthPerCent;
    }
}
