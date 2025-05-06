using System.Collections.Generic;
using tuleeeeee.Managers;
using tuleeeeee.Misc;
using UnityEngine;

[DisallowMultipleComponent]
public class HealthUI : MonoBehaviour
{
    List<GameObject> healthHeartsList = new List<GameObject>();

    private void OnEnable()
    {
        GameManager.Instance.GetPlayer().HealthEvent.OnHealthChanged += HealthEvent_OnHealthChanged;
    }
    private void OnDisable()
    {
        GameManager.Instance.GetPlayer().HealthEvent.OnHealthChanged -= HealthEvent_OnHealthChanged;
    }
    private void HealthEvent_OnHealthChanged(HealthEvent healthEvent, HealthEventArgs healthEventArgs)
    {
        SetHealthBar(healthEventArgs);
    }

    private void ClearHealthBar()
    {
        foreach (GameObject heartIcon in healthHeartsList)
        {
            Destroy(heartIcon);
        }

        healthHeartsList.Clear();
    }

    private void SetHealthBar(HealthEventArgs healthEventArgs)
    {
        ClearHealthBar();

        int healthHearts = Mathf.CeilToInt(healthEventArgs.healthPercent * 5f);
        int fullHeart = Mathf.CeilToInt(GameManager.Instance.GetPlayer().PlayerDetails.playerHealthAmount / 20f);

        for (int i = 0; i < fullHeart; i++)
        {
            GameObject heart = Instantiate((i < healthHearts) ? GameResources.Instance.heartPrefab : GameResources.Instance.emptyHeartPrefab, transform);

            heart.GetComponent<RectTransform>().anchoredPosition = new Vector2(Settings.uiHeartSpacing * i, 0f);

            healthHeartsList.Add(heart);
        }
    }
}
