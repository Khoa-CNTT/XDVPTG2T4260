using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Health : CoreComponent
{
    private Player player;
    private Entity entity;

    private Coroutine immunityCoroutine;
    private bool isImmuneAfterHit = false;
    private float immunityTime = 0f;
    private SpriteRenderer spriteRenderer = null;
    private const float spriteFlashInterval = 0.2f;
    private WaitForSeconds WaitForSecondsSpriteFlashInterval = new WaitForSeconds(spriteFlashInterval);

    [HideInInspector] public bool isDamageable = true;

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
                spriteRenderer = player.SpriteRenderer;
            }
        }
        else if (entity != null)
        {
            if (entity.EnemyDetails.isImmuneAfterHit)
            {
                isImmuneAfterHit = true;
                immunityTime = entity.EnemyDetails.hitImmunityTime;
                spriteRenderer = entity.spriteRendererArray[0];
            }
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
            immunityCoroutine = StartCoroutine(PostHitImmunityRoutine(immunityTime, spriteRenderer));
        }
    }
    private IEnumerator PostHitImmunityRoutine(float immunityTime, SpriteRenderer spriteRenderer)
    {
        int iterations = Mathf.RoundToInt(immunityTime / spriteFlashInterval / 2f);
        isDamageable = false;

        while (iterations > 0)
        {
            spriteRenderer.color = Color.red;
            yield return WaitForSecondsSpriteFlashInterval;
            spriteRenderer.color = Color.white;
            yield return WaitForSecondsSpriteFlashInterval;
            iterations--;
            yield return null;
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

}
