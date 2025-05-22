using System.Collections;
using System.Collections.Generic;
using TMPro;
using tuleeeeee.Managers;
using tuleeeeee.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject videoMenuUI;
    [SerializeField] private GameObject audioMenuUI;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
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

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(videoMenuUI), videoMenuUI);
        HelperUtilities.ValidateCheckNullValue(this, nameof(audioMenuUI), audioMenuUI);
    }
#endif
    #endregion
}
