using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonBehaviorUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [Header("Optional Visual Settings")]
    [SerializeField]
    private bool isAffectText = false;
    [SerializeField]
    private bool isAffectImage = false;
    [SerializeField]
    private bool isPrimary = false;

    private Button button;
    private TextMeshProUGUI text;
    private Image image;

    private const string white = "#FFFFFF";
    private const string grey = "#6F6F6F";
    private void Awake()
    {
        button = GetComponent<Button>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        image = GetComponentInChildren<Image>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(OnButtonPressed);
        if (isPrimary)
        {
            button.Select();
        }
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(OnButtonPressed);
        SetColors(grey, white);
    }
    private void OnButtonPressed()
    {
        if (SoundEffectManager.Instance == null) return;
        SoundEffectManager.Instance.PlaySoundEffect(GameResources.Instance.uiCursor);
    }

    public void OnSelect(BaseEventData eventData)
    {
        SetColors(white, grey);
        if (SoundEffectManager.Instance == null) return;
        SoundEffectManager.Instance.PlaySoundEffect(GameResources.Instance.uiSelect);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        SetColors(grey, white);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetColors(white, grey);
        if (SoundEffectManager.Instance == null) return;
        SoundEffectManager.Instance.PlaySoundEffect(GameResources.Instance.uiSelect);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetColors(grey, white);
    }

    private void SetColors(string primaryColor, string secondaryColor)
    {
        bool hasText = isAffectText && text != null;
        bool hasImage = isAffectImage && image != null;

        if (hasText && hasImage)
        {
            SetTextColor(primaryColor);
            SetImageColor(secondaryColor);
        }

        else if (hasText)
        {
            SetTextColor(primaryColor);
        }

        else if (hasImage)
        {
            SetImageColor(primaryColor);
        }
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

    private void SetImageColor(string hex)
    {
        if (!ColorUtility.TryParseHtmlString(hex, out Color color))
        {
            Debug.LogWarning($"Invalid hex color code: {hex}");
            return;
        }
        image.color = color;
    }
}
