using System;
using System.Collections;
using System.Collections.Generic;
using tuleeeeee.Utilities;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Health : CoreComponent
{
    #region Header References
    [Space(10)]
    [Header("Reference")]
    #endregion
    [SerializeField] private HealthBar healthBar;

    private Player player;
    private Entity entity;

    private Coroutine immunityCoroutine;
    private bool isImmuneAfterHit = false;
    private float immunityTime = 0f;
    private SpriteRenderer[] spriteRendererArray = null;
    private const float spriteFlashInterval = 0.2f;
    private WaitForSeconds WaitForSecondsSpriteFlashInterval = new WaitForSeconds(spriteFlashInterval);

    [HideInInspector] public bool isDamageable = true;
    [HideInInspector] public Enemy enemy;

    private int startingHealth;
    private int currentHealth;

    private HealthEvent healthEvent;
    protected override void Awake()
    {
        base.Awake();

        healthEvent = GetComponent<HealthEvent>();
    }
    private void Start()
    {
        CallHealthEvent(0);
        player = GetComponentInParent<Player>();
        entity = GetComponentInParent<Entity>();

        if (player != null)
        {
            if (player.PlayerDetails.isImmuneAfterHit)
            {
                isImmuneAfterHit = true;
                immunityTime = player.PlayerDetails.hitImmunityTime;
                spriteRendererArray = player.SpriteRendererArray;
            }
        }
        else if (entity != null)
        {
            if (entity.EnemyDetails.isImmuneAfterHit)
            {
                isImmuneAfterHit = true;
                immunityTime = entity.EnemyDetails.hitImmunityTime;
                spriteRendererArray = entity.spriteRendererArray;
            }
        }

        if (entity != null && entity.EnemyDetails.isHealthBarDisplayed == true && healthBar != null)
        {
            healthBar.EnableHealthBar();

        }
        else if (healthBar != null)
        {
            healthBar.DisableHealthBar();
        }
    }
    private void CallHealthEvent(int damageAmount)
    {
        healthEvent.CallHealthChangedEvent(((float)currentHealth / (float)startingHealth), currentHealth, damageAmount);
    }
    public void TakeDamge(int damageAmount)
    {
        bool isRolling = false;

        if (player != null)
        {
            isRolling = player.RollState.IsRolling;
        }

        if (isDamageable && !isRolling)
        {
            currentHealth -= damageAmount;
            CallHealthEvent(damageAmount);


            if (healthBar != null)
            {
                healthBar.SetHealthBarValue((float)currentHealth / (float)startingHealth);
            }

            PostHitImmunity();
        }
    }
    private void PostHitImmunity()
    {
        if (gameObject.activeSelf == false)
        {
            return;
        }

        if (isImmuneAfterHit)
        {
            if (immunityCoroutine != null)
            {
                StopCoroutine(immunityCoroutine);
            }
            Debug.Log("PostHitImmunity");
            immunityCoroutine = StartCoroutine(PostHitImmunityRoutine(immunityTime, spriteRendererArray));
        }
    }
    private IEnumerator PostHitImmunityRoutine(float immunityTime, SpriteRenderer[] spriteRenderers)
    {
        int iterations = Mathf.RoundToInt(immunityTime / spriteFlashInterval / 2f);
        isDamageable = false;

        bool arraysInitialized = false;

        Color[] originalColors = new Color[spriteRenderers.Length];
        Color[] transparentColors = new Color[spriteRenderers.Length];

        // Initialize color arrays only once
        if (!arraysInitialized || originalColors.Length != spriteRenderers.Length)
        {
            originalColors = new Color[spriteRenderers.Length];
            transparentColors = new Color[spriteRenderers.Length];

            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                originalColors[i] = spriteRenderers[i].color;
                transparentColors[i] = originalColors[i];
                transparentColors[i].a = 0f;
            }

            arraysInitialized = true;
        }

        while (iterations > 0)
        {
            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                spriteRenderers[i].color = transparentColors[i];
            }
            yield return WaitForSecondsSpriteFlashInterval;
            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                spriteRenderers[i].color = originalColors[i];
            }
            yield return WaitForSecondsSpriteFlashInterval;
            iterations--;
            yield return null;
        }

        // Ensure end with original colors
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].color = originalColors[i];
        }

        isDamageable = true;
    }
    public void SetStartingHealth(int startingHealth)
    {
        this.startingHealth = startingHealth;
        currentHealth = startingHealth;
    }

    public int GetStartingHealth()
    {
        return startingHealth;
    }
    public void AddHealth(int healthPercent)
    {
        int healthIncrease = Mathf.RoundToInt((startingHealth * healthPercent) / 100f);

        int totalHealth = currentHealth + healthIncrease;

        if (totalHealth > startingHealth)
        {
            currentHealth = startingHealth;
        }
        else
        {
            currentHealth = totalHealth;
        }

        CallHealthEvent(0);
    }


}
