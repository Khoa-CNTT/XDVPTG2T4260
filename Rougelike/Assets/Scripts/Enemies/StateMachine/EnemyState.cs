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
    public float StartTime { get; protected set; }

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
        StartTime = Time.time;
        entity.Animator.SetBool(animBoolName, true);
        Debug.Log($"{entity.ToString()} Enter: {animBoolName}");
        isAnimationFinished = false;
        isExitingState = false;
    }
    public virtual void Exit()
    {
        entity.Animator.SetBool(animBoolName, false);
        Debug.Log($"{entity.ToString()} Exit: {animBoolName}");
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
