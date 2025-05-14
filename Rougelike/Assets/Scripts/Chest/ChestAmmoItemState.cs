using tuleeeeee.Managers;
using tuleeeeee.StateMachine;

namespace tuleeeeee.Chests
{
    public class ChestAmmoItemState : ChestState
    {
        public ChestAmmoItemState(Chest chest, StateManager stateManager, int animBoolID) : base(chest, stateManager, animBoolID)
        {
        }

        public void InstatiateAmmoItem()
        {
            chest.InstantiateItem();

            chest.chestItem.Initialize(GameResources.Instance.bulletIcon, chest.ammoPercent.ToString() + "%", chest.itemSpawnPoint.position, chest.materializeColor);
        }

        public override void HandleUse()
        {
            base.HandleUse();
            CollectAmmoItem();
        }
 
        private void CollectAmmoItem()
        {
            if (chest.chestItem == null || !chest.chestItem.isItemMaterialized) return;

            Player player = GameManager.Instance.GetPlayer();

            player.ReloadWeaponEvent.CallReloadWeaponEvent(player.ActiveWeapon.GetCurrentWeapon(), chest.ammoPercent);

            SoundEffectManager.Instance.PlaySoundEffect(GameResources.Instance.ammoPickup);

            chest.ammoPercent = 0;

            chest.DestroyItem();

            chest.UpdateChestState();
        }
    }
}