using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(CharacterController))]
public sealed class MonsterBehavior : MonoBehaviour
{
    private enum NpcState { Patrolling, Chasing }

    [Header("References")]
    [SerializeField] private Transform player;

    [Header("Speeds")]
    [SerializeField, Min(0f)] private float patrolSpeed = 2.5f;
    [SerializeField, Min(0f)] private float chaseSpeed = 5.0f;
    [SerializeField, Min(0f)] private float turnSpeedDegPerSec = 360f;

    [Header("Patrol")]
    [SerializeField, Min(0.1f)] private float patrolDirChangeInterval = 2.0f;
    [SerializeField, Range(0f, 1f)] private float patrolDirJitter = 0.35f;

    [Header("Obstacle Detection (Patrol)")]
    [SerializeField] private float obstacleRayHeight = 1.0f;
    [SerializeField, Min(0.1f)] private float obstacleDetectDistance = 1.5f;
    [SerializeField, Range(0.1f, 2f)] private float obstacleTurnBias = 1.0f;
    [SerializeField] private LayerMask obstacleMask = ~0;

    [Header("Chase Detection (SphereCast + LOS)")]
    [SerializeField, Min(0.1f)] private float chaseRange = 8.0f;          // how far the cast goes
    [SerializeField, Min(0.01f)] private float proximityRadius = 8.0f;     // sphere radius (usually same as range)
    [SerializeField] private float sensorHeight = 1.2f;
    [SerializeField] private LayerMask playerMask;                         // set to Player layer
    [SerializeField] private LayerMask losMask = ~0;                       // walls + player

    [Header("Gravity")]
    [SerializeField] private float gravity = -20f;

    private CharacterController controller;
    private NpcState state = NpcState.Patrolling;

    private Vector3 patrolDir;
    private float nextPatrolDirChangeTime;
    private float verticalVelocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        if (player == null)
        {
            var tagged = GameObject.FindGameObjectWithTag("Player");
            if (tagged != null) player = tagged.transform;
        }

        PickNewPatrolDirection(transform.forward);
        nextPatrolDirChangeTime = Time.time + patrolDirChangeInterval;
    }

    private void Update()
    {
        bool proximityHitPlayer = DetectPlayerProximitySphereCast(out Transform sensedPlayer);
        if (player == null && sensedPlayer != null) player = sensedPlayer;

        //bool canSeePlayer = player != null && CanSeePlayer(player);

        if (state == NpcState.Patrolling)
        {
            if (proximityHitPlayer)
                state = NpcState.Chasing;
        }
        else
        {
            // End chase if player not in proximity anymore OR LOS breaks
            if (!proximityHitPlayer)
            {
                state = NpcState.Patrolling;
                nextPatrolDirChangeTime = Time.time + patrolDirChangeInterval;
                PickNewPatrolDirection(patrolDir.sqrMagnitude > 0.001f ? patrolDir : transform.forward);
            }
        }

        if (state == NpcState.Patrolling) ExecutePatrol();
        else ExecuteChase();

        ApplyGravityOnly();
    }

    private bool DetectPlayerProximitySphereCast(out Transform hitPlayer)
    {
        hitPlayer = null;

        Vector3 origin = transform.position + Vector3.up * sensorHeight;

        // SphereCast needs a direction; we cast forward for range and also do a short "0-length" cast fallback.
        Vector3 forward = transform.forward;

        if (Physics.SphereCast(origin, proximityRadius, forward, out RaycastHit hit, chaseRange, playerMask, QueryTriggerInteraction.Ignore))
        {
            hitPlayer = hit.transform;
            return true;
        }

        // Fallback: treat as "nearby in any direction" using OverlapSphere when forward cast misses
        Collider[] cols = Physics.OverlapSphere(origin, proximityRadius, playerMask, QueryTriggerInteraction.Ignore);
        if (cols.Length > 0)
        {
            hitPlayer = cols[0].transform;
            return true;
        }

        return false;
    }

    private bool CanSeePlayer(Transform targetPlayer)
    {
        Vector3 origin = transform.position + Vector3.up * sensorHeight;
        Vector3 target = targetPlayer.position + Vector3.up * sensorHeight;
        Vector3 dir = target - origin;

        float maxDist = Mathf.Max(chaseRange, proximityRadius);

        if (dir.sqrMagnitude <= 0.0001f) return true;

        if (Physics.Raycast(origin, dir.normalized, out RaycastHit hit, maxDist, losMask, QueryTriggerInteraction.Ignore))
            return hit.transform == targetPlayer || hit.transform.IsChildOf(targetPlayer);

        return false;
    }

    private void ExecutePatrol()
    {
        if (Time.time >= nextPatrolDirChangeTime)
        {
            Vector3 baseDir = patrolDir.sqrMagnitude > 0.001f ? patrolDir : transform.forward;
            PickNewPatrolDirection(baseDir);
            nextPatrolDirChangeTime = Time.time + patrolDirChangeInterval;
        }

        Vector3 origin = transform.position + Vector3.up * obstacleRayHeight;
        Vector3 dir = patrolDir.sqrMagnitude > 0.001f ? patrolDir.normalized : transform.forward;

        if (Physics.Raycast(origin, dir, out RaycastHit hit, obstacleDetectDistance, obstacleMask, QueryTriggerInteraction.Ignore))
        {
            Vector3 away = Vector3.ProjectOnPlane(hit.normal, Vector3.up).normalized;
            Vector3 blended = Vector3.Slerp(dir, away, Mathf.Clamp01(obstacleTurnBias));
            PickNewPatrolDirection(blended);
            nextPatrolDirChangeTime = Time.time + patrolDirChangeInterval;
        }

        MoveInDirection(patrolDir, patrolSpeed);
    }

    private void ExecuteChase()
    {
        if (player == null)
        {
            ExecutePatrol();
            return;
        }

        Vector3 toPlayer = player.position - transform.position;
        toPlayer.y = 0f;

        if (toPlayer.sqrMagnitude < 0.0001f)
        {
            MoveInDirection(Vector3.zero, 0f);
            return;
        }

        MoveInDirection(toPlayer.normalized, chaseSpeed);
    }

    private void PickNewPatrolDirection(Vector3 preferred)
    {
        preferred.y = 0f;
        if (preferred.sqrMagnitude < 0.001f) preferred = Vector3.forward;

        Vector2 r = Random.insideUnitCircle.normalized;
        Vector3 randomDir = new Vector3(r.x, 0f, r.y);

        Vector3 mixed = Vector3.Slerp(preferred.normalized, randomDir, patrolDirJitter);
        patrolDir = mixed.sqrMagnitude > 0.001f ? mixed.normalized : preferred.normalized;
    }

    private void MoveInDirection(Vector3 dir, float speed)
    {
        Vector3 move = dir;
        move.y = 0f;

        if (move.sqrMagnitude > 1f) move.Normalize();

        Vector3 horizontal = move * speed;

        if (horizontal.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(horizontal.normalized, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, turnSpeedDegPerSec * Time.deltaTime);
        }

        Vector3 velocity = new Vector3(horizontal.x, verticalVelocity, horizontal.z);
        controller.Move(velocity * Time.deltaTime);
    }

    private void ApplyGravityOnly()
    {
        if (controller.isGrounded && verticalVelocity < 0f)
            verticalVelocity = -2f;

        verticalVelocity += gravity * Time.deltaTime;
    }
}