using System.Collections;
using System.Collections.Generic;
using tuleeeeee.StateMachine;
using UnityEngine;

public class EnemyState
{
    protected Core Core;
    protected Entity entity;
    protected StateManager stateManager;
    protected MovementDetailsSO enemyDetails;


    protected bool isAnimationFinished;
    protected bool isExitingState;
    public float startTime { get; protected set; }

    private string animBoolName;
    public EnemyState(Entity entity, StateManager stateManager, MovementDetailsSO EnemyDetails, string animBoolName)
    {
        this.entity = entity;
        this.stateManager = stateManager;
        this.enemyDetails = EnemyDetails;
        this.animBoolName = animBoolName;
        Core = entity.Core;
    }
    public virtual void Enter()
    {
        DoChecks();
        startTime = Time.time;
        entity.Animator.SetBool(animBoolName, true);
        isAnimationFinished = false;
        isExitingState = false;
    }
    public virtual void Exit()
    {
        entity.Animator.SetBool(animBoolName, false);
        isExitingState = true;
    }
    public virtual void LogicUpdate()
    {

    }
    public virtual void PhysicsUpdate()
    {
        DoChecks();
    }
    public virtual void DoChecks()
    {

    }
    public virtual void AnimationTrigger()
    {

    }
    public virtual void AnimationFinishedTrigger() => isAnimationFinished = true;
}
