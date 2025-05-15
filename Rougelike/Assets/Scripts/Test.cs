using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI messageTextTMP;
    public bool CheckSymmetryNumber(int number)
    {
        int orignNumber = number;
        int a = number;
        bool isSymetryNumber = false;
        string c = "";
        while (a > 0)
        {
            int b = a % 10;
            c += b.ToString();
            a = a / 10;
        }
        if (orignNumber.ToString() == c)
        {
            isSymetryNumber = true;
        }
        else
        {
            isSymetryNumber = false;
        }
        return isSymetryNumber;
    }
    public void BinaryTransfer(int number)
    {
        int a = number;
        string c = "";
        while (a > 0)
        {
            int b = a % 2;
            c = b.ToString() + c;
            a = a / 2;
        }
        string text = $"Binary of {number} is {c}";
        StartCoroutine(DisplayMessageRoutine(text, Color.white, 3f));
    }
    public void Swap(int a, int b)
    {
        string text1 = $"Before swap: {a}, {b}.\n";
        int c = 0;
        c = a;
        a = b;
        b = c;
        string text2 = $"After swap: {a}, {b}.";
        string text = text1 + text2;
        StartCoroutine(DisplayMessageRoutine(text, Color.white, 3f));
    }
    public void Swap(int a, int b, bool noNeedThirdWheel)
    {
        string text1 = $"Before swap noNeedThirdWheel: {a}, {b}.\n";
        a = a + b;
        b = a - b;
        a = a - b;
        string text2 = $"After swap noNeedThirdWheel: {a}, {b}.";
        string text = text1 + text2;
        StartCoroutine(DisplayMessageRoutine(text, Color.white, 3f));
    }
    private IEnumerator DisplayMessageRoutine(string text, Color textColor, float displaySeconds)
    {
        messageTextTMP.SetText(text);
        messageTextTMP.color = textColor;

        if (displaySeconds > 0f)
        {
            float timer = displaySeconds;

            while (timer > 0f && !Input.GetKeyDown(KeyCode.Return))
            {
                timer -= Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            while (!Input.GetKeyDown(KeyCode.Return))
            {
                yield return null;
            }
        }

        messageTextTMP.SetText("");
    }
    private void Start()
    {
        StartCoroutine(Show());
    }
    private IEnumerator Show()
    {
        Swap(1, 2);
        yield return new WaitForSeconds(4f);

        Swap(3, 4, true);
        yield return new WaitForSeconds(4f);

        BinaryTransfer(25);
        yield return new WaitForSeconds(4f);

        int a = 1221;
        string resultText = CheckSymmetryNumber(a)
            ? $"{a} is a symmetry number"
            : $"{a} is not a symmetry number";

        yield return StartCoroutine(DisplayMessageRoutine(resultText, Color.white, 3f));
    }
}
