using tuleeeeee.Managers;
using tuleeeeee.StateMachine;

namespace tuleeeeee.Chests
{
    public class ChestHealthItemState : ChestState
    {
        public ChestHealthItemState(Chest chest, StateManager stateManager, int animBoolID) : base(chest, stateManager, animBoolID)
        {
        }
        public override void HandleUse()
        {
            base.HandleUse();
            CollectHealthItem();
        }

        public void InstantiateHealthItem()
        {
            chest.InstantiateItem();

            chest.chestItem.Initialize(GameResources.Instance.heartIcon, chest.healthPercent.ToString() + "%", chest.itemSpawnPoint.position, chest.materializeColor);
        }

        public void CollectHealthItem()
        {
            if (chest.chestItem == null || !chest.chestItem.isItemMaterialized) return;

            GameManager.Instance.GetPlayer().Health.AddHealth(chest.healthPercent);

            SoundEffectManager.Instance.PlaySoundEffect(GameResources.Instance.healthPickUp);

            chest.healthPercent = 0;

            chest.DestroyItem();

            chest.UpdateChestState();
        }

    }
}