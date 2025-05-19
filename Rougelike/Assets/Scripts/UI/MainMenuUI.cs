using System.Collections;
using System.Collections.Generic;
using TMPro;
using tuleeeeee.Utilities;
using UnityEngine;
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
    [SerializeField] private TextMeshProUGUI versionText;

    private void Start()
    {
        MusicManager.Instance.PlayMusic(GameResources.Instance.mainMenuMusic, 0f, 2f);

        SceneManager.LoadScene("CharacterSelectorScene", LoadSceneMode.Additive);

        versionText.SetText(Application.version.ToString());
    }
    public void LoadCharacterSelector()
    {
        playButton.SetActive(true);
        optionsButton.SetActive(true);
        quitButton.SetActive(true);

        SceneManager.LoadScene("CharacterSelectorScene", LoadSceneMode.Additive);
    }
    public void LoadOptions()
    {
        playButton.SetActive(false);
        optionsButton.SetActive(false);
        quitButton.SetActive(false);

        SceneManager.UnloadSceneAsync("CharacterSelectorScene");
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("MainGameScene");
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
        HelperUtilities.ValidateCheckNullValue(this, nameof(quitButton), quitButton);
    }
#endif
    #endregion
}
