using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonBehaviorUI : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    private TextMeshProUGUI text;

    private const string white = "#FFFFFF";
    private const string grey = "#6F6F6F";
    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("OnPointerEnter");
        SetTextColor(white);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("OnPointerExit");
        SetTextColor(grey);
    }
    private void SetTextColor(string hex)
    {
        if (ColorUtility.TryParseHtmlString(hex, out Color color))
        {
            text.color = color;
        }
        else
        {
            Debug.LogWarning($"Invalid hex color code: {hex}");
        }
    }
}
