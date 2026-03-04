using UnityEngine;

public class MonsterFOV : MonoBehaviour
{
    public FearMeter fearMeter;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            fearMeter.SetPlayerInFOV(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            fearMeter.SetPlayerInFOV(false);
        }
    }
}
