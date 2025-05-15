using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

using tuleeeeee.Managers;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button soloButton;
    [SerializeField] private Button twoPlayerButton;
    [SerializeField] private Button playButton;

    private PlayerMode selectedMode = PlayerMode.Solo;

    private void Start()
    {
        if (soloButton == null || twoPlayerButton == null || playButton == null)
        {
            Debug.LogError("Các button chưa được gán trong Inspector!");
            return;
        }

        if (PlayerPrefs.HasKey("SelectedMode"))
        {
            selectedMode = (PlayerMode)PlayerPrefs.GetInt("SelectedMode");
        }

        soloButton.onClick.AddListener(() => SetMode(PlayerMode.Solo));
        twoPlayerButton.onClick.AddListener(() => SetMode(PlayerMode.TwoPlayer));
        playButton.onClick.AddListener(StartGame);
    }

    private void SetMode(PlayerMode mode)
    {
        selectedMode = mode;
        Debug.Log("Đã chọn chế độ: " + mode);

        PlayerPrefs.SetInt("SelectedMode", (int)mode);
        PlayerPrefs.Save();

        UpdateButtonVisuals();
    }

    private void UpdateButtonVisuals()
    {
        soloButton.interactable = (selectedMode != PlayerMode.Solo);
        twoPlayerButton.interactable = (selectedMode != PlayerMode.TwoPlayer);
    }

    private void StartGame()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError(" GameManager null");
            return;
        }

            Debug.Log("Bắt đầu game với chế độ: " + selectedMode);
            GameManager.Instance.SetPlayerMode(selectedMode);
            SceneManager.LoadScene("MainGameScene");
    }

}