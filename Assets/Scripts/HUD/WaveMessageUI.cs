using System;
using TMPro;
using UnityEngine;

public class WaveMessageUI : MonoBehaviour
{
    [SerializeField] private TMP_Text waveText;

    private void Awake()
    {
        if(waveText == null) waveText = GetComponentInChildren<TMP_Text>();
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void SetWave(int wave)
    {
        waveText.text = $"WAVE {wave}";
    }

    public void ShowWave(int wave)
    {
        waveText.text = $"WAVE {wave}";
        gameObject.SetActive(true);
        Invoke(nameof(Hide), 1f);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
