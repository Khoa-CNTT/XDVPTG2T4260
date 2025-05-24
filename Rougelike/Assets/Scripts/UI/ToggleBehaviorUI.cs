using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToggleBehaviorUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, ISelectHandler, IDeselectHandler
{
    [Header("Optional Visual Settings")]
    [SerializeField]
    private bool isPrimary = false;

    [SerializeField]
    SoundEffectSO uiSelect; 
    [SerializeField]
    SoundEffectSO uiCursor;

    private Toggle toggle;
    private TextMeshProUGUI text;
    [SerializeField]
    private Image backgroundImage;
    [SerializeField]
    private Image checkmarkImage;

    private const string white = "#FFFFFF";
    private const string grey = "#6F6F6F";
    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        if (isPrimary)
        {
            toggle.Select();
        }
    }

    private void OnDisable()
    {
        SetColors(grey, white);
    }

    public void OnSelect(BaseEventData eventData) 
    { 
        SetColors(white, grey);
    }
    public void OnDeselect(BaseEventData eventData) { SetColors(grey, white); }
    public void OnPointerDown(PointerEventData eventData) 
    { 
        SetColors(grey, white);
    }
    public void OnPointerEnter(PointerEventData eventData) 
    { 
        SetColors(white, grey);
    }
    public void OnPointerExit(PointerEventData eventData) { SetColors(grey, white); }

    private void SetColors(string primaryColor, string secondaryColor)
    {
        SetTextColor(primaryColor);
        SetImageColor(primaryColor);
        SetCheckmarkColor(secondaryColor);
    }
    private void SetTextColor(string hex)
    {
        if (!ColorUtility.TryParseHtmlString(hex, out Color color))
        {
            Debug.LogWarning($"Invalid hex color code: {hex}");
            return;
        }
        text.color = color;

    }
    private void SetCheckmarkColor(string hex)
    {
        if (!ColorUtility.TryParseHtmlString(hex, out Color color))
        {
            Debug.LogWarning($"Invalid hex color code: {hex}");
            return;
        }
        checkmarkImage.color = color;
    }
    private void SetImageColor(string hex)
    {
        if (!ColorUtility.TryParseHtmlString(hex, out Color color))
        {
            Debug.LogWarning($"Invalid hex color code: {hex}");
            return;
        }
        backgroundImage.color = color;
    }
    private void TryPlaySound(SoundEffectSO soundEffect)
    {
        if (soundEffect == null) return;
        if (SoundEffectManager.Instance == null) return;

        SoundEffectManager.Instance.PlaySoundEffect(soundEffect);
    }
}
