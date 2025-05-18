using System.Collections;
using System.Collections.Generic;
using TMPro;
using tuleeeeee.Managers;
using tuleeeeee.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] GameObject audioMenu;
    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Time.timeScale = 0f;
    }
    private void OnDisable()
    {
        Time.timeScale = 1f;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
