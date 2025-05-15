using System.Collections;
using tuleeeeee.Managers;
using tuleeeeee.StateMachine;


namespace tuleeeeee.Chests
{
    public class ChestWeaponItemState : ChestState
    {
        public ChestWeaponItemState(Chest chest, StateManager stateManager, int animBoolID) : base(chest, stateManager, animBoolID)
        {
        }
        public override void HandleUse()
        {
            base.HandleUse();
            CollectWeaponItem();
        }

        public void InstatiateWeaponItem()
        {
            chest.InstantiateItem();

            chest.chestItemGameObject.GetComponent<ChestItem>().Initialize(chest.weaponDetails.weaponSprite, chest.weaponDetails.weaponName, chest.itemSpawnPoint.position, chest.materializeColor);
        }
        public void CollectWeaponItem()
        {
            if (chest.chestItem == null || !chest.chestItem.isItemMaterialized) return;

            if (!GameManager.Instance.GetPlayer().IsWeaponHeldByPlayer(chest.weaponDetails))
            {

                GameManager.Instance.GetPlayer().AddWeaponToPlayer(chest.weaponDetails);

                SoundEffectManager.Instance.PlaySoundEffect(GameResources.Instance.weaponPickup);
            }
            chest.weaponDetails = null;

            chest.DestroyItem();

            chest.UpdateChestState();
        }
    }
}