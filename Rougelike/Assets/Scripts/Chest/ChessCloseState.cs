using System.Collections;
using tuleeeeee.StateMachine;
using UnityEngine;

namespace tuleeeeee.Chests
{
    public class ChessCloseState : ChestState
    {
        public ChessCloseState(Chest chest, StateManager stateManager, int animBoolID) : base(chest, stateManager, animBoolID)
        {
        }
        public override void HandleUse()
        {
            base.HandleUse();
            stateManager.ChangeState(chest.ChestOpenState);
        }
    }
}