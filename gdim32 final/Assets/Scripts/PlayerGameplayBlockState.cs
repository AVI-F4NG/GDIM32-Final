using UnityEngine;

[DisallowMultipleComponent]
public sealed class PlayerGameplayBlockState : MonoBehaviour
{
    public static PlayerGameplayBlockState Instance { get; private set; }

    public bool IsTalking { get; private set; }
    public bool IsSettingsOpen { get; private set; }

    public bool ShouldBlockFear => IsTalking || IsSettingsOpen;
    public bool IsPausedGameplay => IsTalking || IsSettingsOpen;

    public static PlayerGameplayBlockState GetOrFind()
    {
        if (Instance != null) return Instance;
        Instance = FindFirstObjectByType<PlayerGameplayBlockState>();
        return Instance;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnEnable()
    {
        if (Instance == null) Instance = this;
    }

    public void SetTalking(bool value) => IsTalking = value;
    public void SetSettingsOpen(bool value) => IsSettingsOpen = value;
}