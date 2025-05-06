using tuleeeeee.Enums;
using tuleeeeee.Misc;
using tuleeeeee.StateMachine;
using tuleeeeee.Utilities;
using UnityEngine;


public class PlayerRollState : PlayerAbilityState
{
    public bool CanRoll { get; private set; }
    public bool IsRolling { get; private set; }

    private bool isHolding;
    private bool rollInputStop;

    private float lastRollTime;

    private Vector2 rollDirection;
    private Vector2 rollDirectionInput;

    public PlayerRollState(Player player, StateManager stateManager, MovementDetailsSO playerData, string animBoolName) : base(player, stateManager, playerData, animBoolName)
    {

    }
    public override void Enter()
    {
        base.Enter();
        IsRolling = true;

        CanRoll = false;
        player.InputHandler.UseRollInput();

        isHolding = true;
      

        Time.timeScale = playerData.holdTimeScale;
        startTime = Time.unscaledTime;

    }
    public override void Exit()
    {
        base.Exit();
        IsRolling = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        Movement.CheckIfShouldFlip(xInput);

        if (!isExitingState)
        {
            if (isHolding)
            {
                rollDirectionInput = player.InputHandler.RawMovementInput;
                rollInputStop = player.InputHandler.RollInputStop;

                if (rollDirectionInput != Vector2.zero)
                {
                    rollDirection = rollDirectionInput;
                    rollDirection.Normalize();
                }

                if (rollInputStop || Time.unscaledTime >= startTime + playerData.maxHoldTime)
                {
                    isHolding = false;
                    Time.timeScale = 1f;
                    startTime = Time.time;
                    Movement.RB.drag = playerData.drag;

                    Movement.SetVelocity(playerData.rollVelocity, rollDirection);
                    RollAnimation(rollDirection);
                }
            }
            else
            {
                Movement.SetVelocity(playerData.rollVelocity, rollDirection);
                RollAnimation(rollDirection);

                if (Time.time >= startTime + playerData.rollTime)
                {
                    Movement.RB.drag = 0f;
                    isAbilityDone = true;
                    lastRollTime = Time.time;
                }
            }
        }
    }

    public bool CheckIfCanDash()
    {
        return CanRoll && Time.time >= lastRollTime + playerData.rollCooldown;
    }

    public void ResetCanRoll() => CanRoll = true;

    #region ANIMATION
    private void RollAnimation(Vector2 direction)
    {
        float angle = HelperUtilities.GetAngleFromVector(direction);
        Direction moveDirection = HelperUtilities.GetDirection(angle);

        InitializeRollAnimationParameters();
        SetRollAnimationParamters(moveDirection);
    }
    private void InitializeRollAnimationParameters()
    {
        player.Animator.SetBool(Settings.rollDown, false);
        player.Animator.SetBool(Settings.rollUp, false);
        player.Animator.SetBool(Settings.rollUpRight, false);
        player.Animator.SetBool(Settings.rollRight, false);
    }
    private void SetRollAnimationParamters(Direction moveDirection)
    {
        switch (moveDirection)
        {
            case Direction.down:
                player.Animator.SetBool(Settings.rollDown, true);
                break;
            case Direction.up:
                player.Animator.SetBool(Settings.rollUp, true);
                break;
            case Direction.upleft:
            case Direction.upright:
                player.Animator.SetBool(Settings.rollUpRight, true);
                break;
            case Direction.left:
            case Direction.right:
                player.Animator.SetBool(Settings.rollRight, true);
                break;
        }
    }
    #endregion
}
