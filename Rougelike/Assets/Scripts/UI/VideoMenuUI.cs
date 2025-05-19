using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VideoMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject optionsMenuUI;
    [SerializeField] private TMP_Dropdown resDropdown;
    [SerializeField] private Toggle toggle;

    private Resolution[] resolutionArray;

    private int selectedResolution;

    private List<Resolution> selectedResolutionList;

    private bool isFullScreen = false;

    private void OnEnable()
    {
        Time.timeScale = 0f;
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
    }

    private void Awake()
    {

    }

    private void Start()
    {
        this.gameObject.SetActive(false);

        selectedResolutionList = new List<Resolution>();

        resolutionArray = Screen.resolutions;

        HashSet<string> seenRes = new HashSet<string>();

        List<string> resolutionStringList = new List<string>();

        foreach (Resolution res in resolutionArray)
        {
            string resString = $"{res.width} x {res.height}";
            if (!seenRes.Contains(resString))
            {
                seenRes.Add(resString);
                resolutionStringList.Add(resString);
                selectedResolutionList.Add(res);
            }
        }

        resDropdown.AddOptions(resolutionStringList);

        LoadSettings();
    }

    public void ChangeResolution()
    {
        selectedResolution = resDropdown.value;

        int width = selectedResolutionList[selectedResolution].width;
        int height = selectedResolutionList[selectedResolution].height;

        Screen.SetResolution(width, height, isFullScreen);

        PlayerPrefs.SetInt("SavedResolutionWidth", width);
        PlayerPrefs.SetInt("SavedResolutionHight", height);

        PlayerPrefs.SetInt("ResolutionIndex", selectedResolution);

        PlayerPrefs.Save();
    }

    public void ChangeFullScreen()
    {
        isFullScreen = toggle.isOn;

        int width = selectedResolutionList[selectedResolution].width;
        int height = selectedResolutionList[selectedResolution].height;

        Screen.SetResolution(width, height, isFullScreen);
        PlayerPrefs.SetInt("IsFullScreen", isFullScreen ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void Return()
    {
        this.gameObject.SetActive(false);
        optionsMenuUI.SetActive(true);
    }

    private void LoadSettings()
    {
        int width, height;
        if (PlayerPrefs.HasKey("IsFullScreen"))
        {
            isFullScreen = PlayerPrefs.GetInt("IsFullScreen", 1) == 1;
            toggle.isOn = isFullScreen;
        }
        if (PlayerPrefs.HasKey("SavedResolutionWidth") && PlayerPrefs.HasKey("SavedResolutionHight") && PlayerPrefs.HasKey("ResolutionIndex"))
        {
            selectedResolution = PlayerPrefs.GetInt("ResolutionIndex", 0);

            width = PlayerPrefs.GetInt("SavedResolutionWidth", 1920);
            height = PlayerPrefs.GetInt("SavedResolutionHight", 1080);

            if (selectedResolution >= 0 && selectedResolution < selectedResolutionList.Count)
            {
                resDropdown.value = selectedResolution;
            }
            Screen.SetResolution(width, height, isFullScreen);
        }
    }
}
