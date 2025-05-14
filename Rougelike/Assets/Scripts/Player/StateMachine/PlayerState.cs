using System.Collections;
using System.Collections.Generic;
using tuleeeeee.Data;
using UnityEngine;

namespace tuleeeeee.StateMachine
{
    public class PlayerState
    {
        protected Core Core;
        protected Player player;
        protected StateManager stateManager;
        protected MovementDetailsSO playerData;

        protected bool isAnimationFinished;
        protected bool isExitingState;

        protected float startTime;

        private int animBoolID;

        public PlayerState(Player player, StateManager stateManager, MovementDetailsSO playerData, int animBoolID)
        {
            this.player = player;
            this.stateManager = stateManager;
            this.playerData = playerData;
            this.animBoolID = animBoolID;
            Core = player.Core;
        }
        public virtual void Enter()
        {
            DoCheck();
            startTime = Time.time;
            player.Animator.SetBool(animBoolID, true);
            isAnimationFinished = false;
            isExitingState = false;
        }
        public virtual void Exit()
        {
            player.Animator.SetBool(animBoolID, false);
            isExitingState = true;
        }
        public virtual void LogicUpdate() { }
        public virtual void PhysicUpdate()
        {
            DoCheck();
        }
        public virtual void DoCheck() { }
        public virtual void AnimationTrigger() { }
        public virtual void AnimationFinishedTrigger() => isAnimationFinished = true;

    }
}
