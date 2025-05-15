using System.Collections;
using System.Collections.Generic;
using tuleeeeee.Chests;
using tuleeeeee.StateMachine;
using UnityEditor.AnimatedValues;
using UnityEngine;

public class ChestOpenState : ChestState
{
    public ChestOpenState(Chest chest, StateManager stateManager, int animBoolID) : base(chest, stateManager, animBoolID)
    {
    }

    public override void Enter()
    {
        base.Enter();
        UpdateChestState();
        chest.PlayOpenSound();
    }

    private void UpdateChestState()
    {
        if (chest.healthPercent != 0)
        {
            stateManager.ChangeState(chest.ChestHealthItemState);
            chest.ChestHealthItemState.InstantiateHealthItem();
        }
        else if (chest.ammoPercent != 0)
        {
            stateManager.ChangeState(chest.ChestAmmoItemState);
            chest.ChestAmmoItemState.InstatiateAmmoItem();
        }
        else if (chest.weaponDetails != null)
        {
            stateManager.ChangeState(chest.ChestWeaponItemState);
            chest.ChestWeaponItemState.InstatiateWeaponItem();
        }
    }
}
