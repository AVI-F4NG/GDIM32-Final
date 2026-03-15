using TMPro;
using UnityEngine;

[DisallowMultipleComponent]
public class FearMeterUI : MonoBehaviour
{
    [SerializeField] private FearMeter fearMeter;
    [SerializeField] private TMP_Text fearText;

    private void Awake()
    {
        if (fearMeter == null) fearMeter = FindFirstObjectByType<FearMeter>();
        if (fearText == null) fearText = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (fearMeter == null || fearText == null) return;

        int fearInt = Mathf.RoundToInt(Mathf.Clamp(fearMeter.fear, 0f, 100f));
        fearText.text = $"Fear: {fearInt}/100";
    }
}