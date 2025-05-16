using tuleeeeee.StateMachine;

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
