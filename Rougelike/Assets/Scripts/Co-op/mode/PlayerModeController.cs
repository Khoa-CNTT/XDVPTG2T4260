using System.Collections;
using System.Collections.Generic;
using TrDuc.Managers;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerModeController : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput1;
    [SerializeField] private PlayerInput playerInput2;

    private PlayerModeManager playerModeManager;

    private void Awake()
    {
        playerModeManager = new PlayerModeManager(playerInput1, playerInput2);

        playerModeManager.SetPlayerMode(PlayerMode.Solo);
        playerModeManager.ApplyPlayerMode();
    }

    public void OnSelectSoloMode()
    {
        playerModeManager.SetPlayerMode(PlayerMode.Solo);
        playerModeManager.ApplyPlayerMode();
    }

    public void OnSelectTwoPlayerMode()
    {
        playerModeManager.SetPlayerMode(PlayerMode.TwoPlayer);
        playerModeManager.ApplyPlayerMode();
    }
}
