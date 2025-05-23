using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class OptionsMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject videoMenuUI;
    [SerializeField] private GameObject audioMenuUI;

    private void OnEnable()
    {

    }
    private void OnDisable()
    {

    }
    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void OpenVideoMenuUI()
    {
        this.gameObject.SetActive(false);
        videoMenuUI.SetActive(true);
    }

    public void OpenAudioMenuUI()
    {
        this.gameObject.SetActive(false);
        audioMenuUI.SetActive(true);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
        Time.timeScale = 1f;
    }
}
