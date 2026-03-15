using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public sealed class Settings : MonoBehaviour
{
    private const string PrefKey = "MouseSensitivity";

    [Header("References")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private GameObject panelRoot;

    [Header("UI")]
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private TMP_InputField sensitivityInput;
    [SerializeField] private TMP_Text sensitivityValueLabel;

    [Header("Behavior")]
    [SerializeField] private KeyCode toggleKey = KeyCode.Escape;
    [SerializeField, Min(0f)] private float minSensitivity = 0.1f;
    [SerializeField, Min(0f)] private float maxSensitivity = 10f;
    [SerializeField, Min(0f)] private float defaultSensitivity = 2f;
    [SerializeField, Range(0, 4)] private int displayDecimals = 2;

    private bool isOpen;
    private bool suppressUiEvents;

    private void Awake()
    {
        if (panelRoot == null) panelRoot = gameObject;

        float saved = PlayerPrefs.GetFloat(PrefKey, defaultSensitivity);
        float initial = Mathf.Clamp(saved, minSensitivity, maxSensitivity);

        ConfigureUiRanges();
        ApplyToPlayer(initial);
        SetUiValue(initial);

        SetOpen(false);
    }

    private void OnEnable()
    {
        if (sensitivitySlider != null) sensitivitySlider.onValueChanged.AddListener(OnSliderChanged);
        if (sensitivityInput != null) sensitivityInput.onEndEdit.AddListener(OnInputEndEdit);
    }

    private void OnDisable()
    {
        if (sensitivitySlider != null) sensitivitySlider.onValueChanged.RemoveListener(OnSliderChanged);
        if (sensitivityInput != null) sensitivityInput.onEndEdit.RemoveListener(OnInputEndEdit);
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
            SetOpen(!isOpen);
    }

    private void ConfigureUiRanges()
    {
        if (sensitivitySlider != null)
        {
            sensitivitySlider.minValue = minSensitivity;
            sensitivitySlider.maxValue = maxSensitivity;
            sensitivitySlider.wholeNumbers = false;
        }

        if (sensitivityInput != null)
            sensitivityInput.contentType = TMP_InputField.ContentType.DecimalNumber;
    }

    private void SetOpen(bool open)
    {
        isOpen = open;

        if (panelRoot != null)
            panelRoot.SetActive(open);

        if (playerMovement != null)
            playerMovement.SetCursorLocked(!open);

        if (open && sensitivityInput != null)
            sensitivityInput.ActivateInputField();

        var blockState = PlayerGameplayBlockState.GetOrFind();
        if (blockState != null) blockState.SetSettingsOpen(open);
    }

    private void OnSliderChanged(float value)
    {
        if (suppressUiEvents) return;

        float v = Mathf.Clamp(value, minSensitivity, maxSensitivity);
        ApplyToPlayer(v);
        SetUiValue(v);
        Save(v);
    }

    private void OnInputEndEdit(string text)
    {
        if (suppressUiEvents) return;

        if (!TryParseFloatInvariant(text, out float v))
            v = GetPlayerSensitivityOrDefault();

        v = Mathf.Clamp(v, minSensitivity, maxSensitivity);

        ApplyToPlayer(v);
        SetUiValue(v);
        Save(v);
    }

    private void ApplyToPlayer(float value)
    {
        if (playerMovement != null)
            playerMovement.MouseSensitivity = value;
    }

    private float GetPlayerSensitivityOrDefault()
    {
        if (playerMovement != null)
            return playerMovement.MouseSensitivity;

        return defaultSensitivity;
    }

    private void SetUiValue(float value)
    {
        suppressUiEvents = true;

        if (sensitivitySlider != null)
            sensitivitySlider.value = value;

        if (sensitivityInput != null)
            sensitivityInput.text = value.ToString($"F{displayDecimals}");

        if (sensitivityValueLabel != null)
            sensitivityValueLabel.text = value.ToString($"F{displayDecimals}");

        suppressUiEvents = false;
    }

    private void Save(float value)
    {
        PlayerPrefs.SetFloat(PrefKey, value);
        PlayerPrefs.Save();
    }

    private static bool TryParseFloatInvariant(string s, out float value)
    {
        return float.TryParse(
            s,
            System.Globalization.NumberStyles.Float,
            System.Globalization.CultureInfo.InvariantCulture,
            out value
        );
    }
}