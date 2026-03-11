using UnityEngine;

public class FearMeter : MonoBehaviour
{
    [Header("Fear Settings")]
    [Range(0f, 100f)] public float fear = 0f;
    public float fearIncreaseRate = 20f;
    public float fearDecreaseRate = 15f;
    public float chaseThreshold = 75f;

    [Header("References")]
    public MonsterBehavior monster;          // Drag your MonsterBehavior here
    public Renderer monsterRenderer;         // Drag the monster's mesh renderer here

    [Header("Lantern State")]
    public bool lanternIsActive = false;     // You toggle this from your match system

    private bool playerInFOV = false;
    private bool forcedChase = false;

    private void Update()
    {
        // Increase fear when monster sees player
        if (playerInFOV)
            fear += fearIncreaseRate * Time.deltaTime;

        // Decrease fear when lantern is active
        if (lanternIsActive)
            fear -= fearDecreaseRate * Time.deltaTime;

        fear = Mathf.Clamp(fear, 0f, 100f);
        Debug.Log($"Fear: {fear}");

        UpdateMonsterVisibility();

        // Trigger chase at 75%
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
