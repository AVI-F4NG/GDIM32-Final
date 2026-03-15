using UnityEngine;

public class FearMeter : MonoBehaviour
{
    [Header("Fear Settings")]
    [Range(0f, 100f)] public float fear = 0f;
    public float fearIncreaseRate = 20f;
    // public float fearDecreaseRate = 15f;
    public float chaseThreshold = 75f;

    [Header("References")]
    public MonsterBehavior monster;
    public Renderer monsterRenderer;

    [Header("Lantern State")]
    public bool lanternIsActive = false;

    private bool playerInFOV = false;
    private bool forcedChase = false;

    private void Update()
    {
        bool blockFear = PlayerGameplayBlockState.Instance != null &&
                        PlayerGameplayBlockState.Instance.ShouldBlockFear;

        if (playerInFOV && !blockFear)
            fear += fearIncreaseRate * Time.deltaTime;

        if (lanternIsActive)
        {
            fear *= 0.5f;
            lanternIsActive = false;
        }

        fear = Mathf.Clamp(fear, 0f, 100f);
        Debug.Log($"Fear: {fear}");

        UpdateMonsterVisibility();

        if (!forcedChase && fear >= chaseThreshold)
        {
            forcedChase = true;
            //monster.ForceChaseMode();
        }
    }
    public void SetPlayerInFOV(bool inFOV)
    {
        playerInFOV = inFOV;
    }

    private void UpdateMonsterVisibility()
    {
        if (monsterRenderer == null) return;

        float alpha = 1f - (fear / 100f);
        Color c = monsterRenderer.material.color;
        c.a = alpha;
        monsterRenderer.material.color = c;
    }
}
