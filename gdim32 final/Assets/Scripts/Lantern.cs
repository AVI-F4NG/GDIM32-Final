using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class Lantern : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MonsterBehavior monster;
    [SerializeField] private Light pointLight;

    [Header("Pulse")]
    [SerializeField, Min(0f)] private float intensityBoost = 2.0f;
    [SerializeField, Min(0.01f)] private float pulseDurationSeconds = 0.5f;


    [SerializeField] private FearMeter fearmeter;
    private float baseIntensity;
    private Coroutine pulseRoutine;

    private void Awake()
    {
        if (monster == null) monster = FindFirstObjectByType<MonsterBehavior>();
        if (pointLight == null) pointLight = GetComponentInChildren<Light>(true);

        if (pointLight != null) baseIntensity = pointLight.intensity;
    }

    private void OnEnable()
    {
        if (pointLight != null) baseIntensity = pointLight.intensity;
        if (monster != null) monster.Stunned += OnMonsterStunned;
    }

    private void OnDisable()
    {
        if (monster != null) monster.Stunned -= OnMonsterStunned;
    }

    private void OnMonsterStunned()
    {
        if (!isActiveAndEnabled) return;
        if (pointLight == null) return;

        if (pulseRoutine != null) StopCoroutine(pulseRoutine);
        pulseRoutine = StartCoroutine(Pulse());
        fearmeter.lanternIsActive = true;
    }

    private IEnumerator Pulse()
    {
        float start = baseIntensity;
        float peak = baseIntensity + intensityBoost;

        pointLight.intensity = peak;

        float endAt = Time.time + pulseDurationSeconds;
        while (Time.time < endAt)
            yield return null;

        pointLight.intensity = start;
        pulseRoutine = null;
    }
}