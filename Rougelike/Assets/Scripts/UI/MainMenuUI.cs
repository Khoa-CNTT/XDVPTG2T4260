using System.Collections;
using System.Collections.Generic;
using TMPro;
using tuleeeeee.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{

    #region Header OBJECT REFERENCES
    [Space(10)]
    [Header("OBJECT REFERENCES")]
    #endregion
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject optionsButton;
    [SerializeField] private GameObject quitButton;

    [SerializeField] private GameObject coopButton;

    [SerializeField] private TextMeshProUGUI versionText;

    private bool isChosenCharacter;

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(playButton);

        isChosenCharacter = false;

        MusicManager.Instance.PlayMusic(GameResources.Instance.mainMenuMusic, 0f, 2f);

        versionText.SetText(Application.version.ToString());
    }
    public void LoadCharacterSelector()
    {
        playButton.SetActive(true);
        optionsButton.SetActive(true);
        quitButton.SetActive(true);

        coopButton.SetActive(true);

        isChosenCharacter = true;

        SceneManager.LoadScene("CharacterSelectorScene", LoadSceneMode.Additive);
    }
    public void UnLoadOptions()
    {
        playButton.SetActive(true);
        optionsButton.SetActive(true);
        quitButton.SetActive(true);

        coopButton.SetActive(true);


        if (isChosenCharacter)
        {
            SceneManager.LoadScene("CharacterSelectorScene", LoadSceneMode.Additive);
        }
    }
    public void LoadOptions()
    {
        playButton.SetActive(false);
        optionsButton.SetActive(false);
        quitButton.SetActive(false);

        coopButton.SetActive(false);

        if (isChosenCharacter)
        {
            SceneManager.UnloadSceneAsync("CharacterSelectorScene");
        }
    }
    public void PlayGame()
    {
        if (isChosenCharacter)
        {
            SceneManager.LoadScene("MainGameScene");
        }
        else
        {
            LoadCharacterSelector();
        }
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(playButton), playButton);
        HelperUtilities.ValidateCheckNullValue(this, nameof(coopButton), coopButton);
        HelperUtilities.ValidateCheckNullValue(this, nameof(optionsButton), optionsButton);
        HelperUtilities.ValidateCheckNullValue(this, nameof(quitButton), quitButton);
        HelperUtilities.ValidateCheckNullValue(this, nameof(versionText), versionText);
    }
#endif
    #endregion
}
