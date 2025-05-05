using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : CoreComponent
{
    private Player player;

    [HideInInspector] public bool isDamageable = true;

    private int startingHealth;
    private int currentHealth;

    private HealthEvent healthEvent;
    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<Player>();
        healthEvent = GetComponent<HealthEvent>();
    }
    private void Start()
    {
        CallHealthEvent(0);
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
        }
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
