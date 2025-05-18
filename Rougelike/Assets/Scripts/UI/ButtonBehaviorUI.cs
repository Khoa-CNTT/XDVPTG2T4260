using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonBehaviorUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
{
    private TextMeshProUGUI text;

    private const string white = "#FFFFFF";
    private const string grey = "#6F6F6F";
    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }
    private void Start()
    {
        SetTextColor(grey);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        SetTextColor(grey);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        SetTextColor(grey);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        SetTextColor(white);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
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
