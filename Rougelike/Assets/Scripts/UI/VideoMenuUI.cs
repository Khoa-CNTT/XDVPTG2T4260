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
        SaveSettings();
    }
    public void ChangeFullScreen()
    {
        isFullScreen = toggle.isOn;

        int width = selectedResolutionList[selectedResolution].width;
        int height = selectedResolutionList[selectedResolution].height;

        Screen.SetResolution(width, height, isFullScreen);
        SaveSettings();
    }
    public void Return()
    {
        this.gameObject.SetActive(false);
        optionsMenuUI.SetActive(true);
    }
    private void LoadSettings()
    {
        if (PlayerPrefs.HasKey("ResolutionIndex"))
        {
            selectedResolution = PlayerPrefs.GetInt("ResolutionIndex");
            if (selectedResolution >= 0 && selectedResolution < selectedResolutionList.Count)
            {
                resDropdown.value = selectedResolution;
                resDropdown.RefreshShownValue();
            }
        }

        if (PlayerPrefs.HasKey("IsFullScreen"))
        {
            isFullScreen = PlayerPrefs.GetInt("IsFullScreen") == 1;
            toggle.isOn = isFullScreen;
        }

        // Apply loaded settings
        int width = selectedResolutionList[selectedResolution].width;
        int height = selectedResolutionList[selectedResolution].height;
        Screen.SetResolution(width, height, isFullScreen);
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("ResolutionIndex", selectedResolution);
        PlayerPrefs.SetInt("IsFullScreen", isFullScreen ? 1 : 0);
        PlayerPrefs.Save();
    }
}
