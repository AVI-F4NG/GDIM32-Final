using UnityEngine;

public class Platform : MonoBehaviour
{
    [Header("Monster Spawn Settings")]
    public GameObject monsterPrefab;     
    public Transform spawnPoint;         
    private bool hasSpawned = false;     

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasSpawned) return;

        if (other.CompareTag("Player"))
        {
            hasSpawned = true;

            Vector3 position = spawnPoint != null ? spawnPoint.position : transform.position;

            Instantiate(monsterPrefab, position, Quaternion.identity);
        }
    }
}
