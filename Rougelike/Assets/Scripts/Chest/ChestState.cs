using System.Collections;
using System.Collections.Generic;
using tuleeeeee.StateMachine;
using UnityEngine;

public class ChestState
{
    protected Chest chest;
    protected StateManager stateManager;

    private int animBoolID;
    public ChestState(Chest chest, StateManager stateManager, int animBoolID)
    {
        this.chest = chest;
        this.stateManager = stateManager;
        this.animBoolID = animBoolID;
    }

    public virtual void Enter()
    {
        chest.Animator.SetBool(animBoolID, true);
        Debug.Log("Enter: " + this.ToString());
    }
    public virtual void Exit()
    {
        chest.Animator.SetBool(animBoolID, true);
        Debug.Log("Exit: " + this.ToString());
    }
    public virtual void Initialize() { }
    public virtual void HandleUse() { }
}
