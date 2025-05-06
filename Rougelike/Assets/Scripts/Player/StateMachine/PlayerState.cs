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

        private string animBoolName;

        public PlayerState(Player player, StateManager stateManager, MovementDetailsSO playerData, string animBoolName)
        {
            this.player = player;
            this.stateManager = stateManager;
            this.playerData = playerData;
            this.animBoolName = animBoolName;
            Core = player.Core;
        }
        public virtual void Enter()
        {
            DoCheck();
            startTime = Time.time;
            player.Animator.SetBool(animBoolName, true);
            isAnimationFinished = false;
            isExitingState = false;
        }
        public virtual void Exit()
        {
            player.Animator.SetBool(animBoolName, false);
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
