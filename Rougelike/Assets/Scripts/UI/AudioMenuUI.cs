using System.Collections;
using System.Collections.Generic;
using TMPro;
using tuleeeeee.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AudioMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject optionsMenuUI;
    [SerializeField] private TextMeshProUGUI musicLevelText;
    [SerializeField] private TextMeshProUGUI soundsLevelText;

    private void OnEnable()
    {
        StartCoroutine(InitializeUI());
    }

    private void OnDisable()
    {
       
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private IEnumerator InitializeUI()
    {
        yield return null;

        soundsLevelText.SetText(SoundEffectManager.Instance.soundsVolume.ToString());
        musicLevelText.SetText(MusicManager.Instance.musicVolume.ToString());
    }

    public void IncreaseMusicVolume()
    {
        MusicManager.Instance.IncreaseMusicVolume();
        musicLevelText.SetText(MusicManager.Instance.musicVolume.ToString());
    }

    public void DecreaseMusicVolume()
    {
        MusicManager.Instance.DecreaseMusicVolume();
        musicLevelText.SetText(MusicManager.Instance.musicVolume.ToString());
    }

    public void IncreaseSoundvolume()
    {
        SoundEffectManager.Instance.IncreaseSoundVolume();
        soundsLevelText.SetText(SoundEffectManager.Instance.soundsVolume.ToString());
    }

    public void DecreaseSoundsVolume()
    {
        SoundEffectManager.Instance.DecreaseSoundVolume();
        soundsLevelText.SetText(SoundEffectManager.Instance.soundsVolume.ToString());
    }
    public void Return()
    {
        this.gameObject.SetActive(false);
        optionsMenuUI.SetActive(true);
    }

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(optionsMenuUI), optionsMenuUI);
        HelperUtilities.ValidateCheckNullValue(this, nameof(musicLevelText), musicLevelText);
        HelperUtilities.ValidateCheckNullValue(this, nameof(soundsLevelText), soundsLevelText);
    }
#endif
    #endregion
}
