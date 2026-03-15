using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;

public class FearMeter : MonoBehaviour
{
    [Header("Fear Settings")]
    [Range(0f, 100f)] public float fear = 0f;
    public float fearIncreaseRate = 20f;
    public float chaseThreshold = 75f;

    [Header("References")]
    public MonsterBehavior monster;
    public Renderer monsterRenderer;

    [Header("Lantern State")]
    public bool lanternIsActive = false;

    [Header("Lose")]
    [SerializeField] private string loseSceneName = "LoseScene";

    [Header("Post Processing - Vignette (PPv2)")]
    [SerializeField] private PostProcessVolume postProcessVolume;
    [SerializeField, Range(0f, 100f)] private float vignetteStartFear = 50f;
    [SerializeField, Range(0f, 1f)] private float vignetteIntensityAtStart = 0.25f;
    [SerializeField, Range(0f, 1f)] private float vignetteIntensityAtMax = 0.6f;

    private bool playerInFOV = false;
    private bool forcedChase = false;
    private bool loseTriggered = false;

    private Vignette vignette;
    private float baseVignetteIntensity;
    private bool vignetteReady;

    private void Awake()
    {
        if (postProcessVolume != null && postProcessVolume.profile != null)
        {
            vignetteReady = postProcessVolume.profile.TryGetSettings(out vignette);
            if (vignetteReady && vignette != null)
                baseVignetteIntensity = vignette.intensity.value;
        }
    }

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

        UpdateVignette();
        UpdateMonsterVisibility();

        if (!loseTriggered && fear >= 100f)
        {
            loseTriggered = true;
            SceneManager.LoadScene(loseSceneName);
            return;
        }

        if (!forcedChase && fear >= chaseThreshold)
        {
            forcedChase = true;
            // monster.ForceChaseMode();
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

    private void UpdateVignette()
    {
        if (!vignetteReady || vignette == null) return;

        if (fear < vignetteStartFear)
        {
            vignette.intensity.value = baseVignetteIntensity;
            return;
        }

        float t = Mathf.InverseLerp(vignetteStartFear, 100f, fear);
        float target = Mathf.Lerp(vignetteIntensityAtStart, vignetteIntensityAtMax, t);
        vignette.intensity.value = target;
    }
}