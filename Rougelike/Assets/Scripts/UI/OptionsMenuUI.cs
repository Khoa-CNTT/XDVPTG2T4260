using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenuUI : MonoBehaviour
{
        [SerializeField] private GameObject videoMenuUI;
    [SerializeField] private GameObject audioMenuUI;
    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void OpenAudioMenuUI()
    {
        this.gameObject.SetActive(false);
        audioMenuUI.SetActive(true);
    }
    public void OpenVideoMenuUI()
    {
        this.gameObject.SetActive(false);
        videoMenuUI.SetActive(true);
    }
}
