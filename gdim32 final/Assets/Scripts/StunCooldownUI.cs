using TMPro;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class StunCooldownUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MonsterBehavior monster;
    [SerializeField] private TMP_Text cooldownText;

    [Header("Formatting")]
    [SerializeField] private string prefix = "Stun ready in: ";
    [SerializeField] private string suffix = "s";
    [SerializeField, Min(0)] private int decimals = 1;

    private void Awake()
    {
        if (monster == null) monster = FindFirstObjectByType<MonsterBehavior>();
        if (cooldownText != null) cooldownText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (monster == null || cooldownText == null) return;

        float remaining = monster.StunCooldownRemainingSeconds;

        if (remaining <= 0f)
        {
            if (cooldownText.gameObject.activeSelf)
                cooldownText.gameObject.SetActive(false);
            return;
        }

        if (!cooldownText.gameObject.activeSelf)
            cooldownText.gameObject.SetActive(true);

        cooldownText.text = prefix + remaining.ToString($"F{decimals}") + suffix;
    }
}