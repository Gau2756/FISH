using UnityEngine;

/// <summary>
/// Spawns obstacles at the LEFT, moves them to the RIGHT, and despawns at a right boundary.
/// Randomly assigns "Hard" or "Soft" tag on spawn. No fish logic here.
/// </summary>
public class ObstacleRightManager : MonoBehaviour
{
    [Header("Prefabs (drag your obstacle prefabs here)")]
    public GameObject[] obstaclePrefabs;     // Fill via Inspector (array list UI)

    [Header("Spawn Settings")]
    public float spawnInterval = 2f;         // seconds between spawns
    public float spawnXLeft = -10f;          // where obstacles appear (to the LEFT)
    public Vector2 spawnYRange = new Vector2(-2f, 2f);

    [Header("Movement")]
    public float baseSpeed = 5f;             // rightward speed (units/sec toward +X)
    public float speedJitter = 0f;           // optional ± per obstacle speed variation

    [Header("Despawn")]
    public float rightLimitX = 20f;          // when obstacle x > this, destroy it

    [Header("Randomness")]
    public bool seedFromTime = true;         // seed global RNG once

    private float _timer;

    private void Start()
    {
        if (seedFromTime)
        {
            // Seeding Unity's global RNG (like seeding a singleton RNG in Java).
            Random.InitState((int)System.DateTime.Now.Ticks);
        }
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= spawnInterval)
        {
            SpawnOnce();
            _timer = 0f;
        }
    }

    private void SpawnOnce()
    {
        if (obstaclePrefabs == null || obstaclePrefabs.Length == 0)
        {
            Debug.LogWarning("[ObstacleRightManager] No obstaclePrefabs assigned.");
            return;
        }

        // Pick a random prefab
        int idx = Random.Range(0, obstaclePrefabs.Length); // int range [min,max)
        GameObject prefab = obstaclePrefabs[idx];

        // Instantiate (Java: new + add to scene)
        GameObject o = Instantiate(prefab);

        // Position at left, random Y
        float y = Random.Range(spawnYRange.x, spawnYRange.y);
        o.transform.position = new Vector3(spawnXLeft, y, 0f);

        // Compute speed with optional jitter
        float speed = baseSpeed;
        if (speedJitter > 0f)
            speed += (Random.value * 2f - 1f) * speedJitter;
        speed = Mathf.Max(0f, speed);

        // Ensure obstacle has a BoxCollider2D (project requirement)
        if (!o.TryGetComponent<BoxCollider2D>(out _))
        {
            Debug.LogWarning($"[ObstacleRightManager] Spawned obstacle '{o.name}' is missing BoxCollider2D.");
        }

        // Ensure data tag exists and set Hard/Soft (no behavior here)
        var data = o.GetComponent<ObstacleData>();
        if (data == null) data = o.AddComponent<ObstacleData>();
        data.hardness = (Random.value < 0.5f) ? ObstacleData.Hardness.Hard : ObstacleData.Hardness.Soft;

        // Movement: if Rigidbody2D exists, use physics velocity; else add a simple mover
        var rb = o.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Move toward +X with no gravity influence on Y
            rb.gravityScale = 0f;
#if UNITY_6000_0_OR_NEWER || UNITY_2023_1_OR_NEWER
            // If your Unity exposes linearVelocity, use it; otherwise fallback to velocity.
            // rb.linearVelocity = new Vector2(Mathf.Abs(speed), rb.linearVelocityY);
            rb.linearVelocity = new Vector2(Mathf.Abs(speed), rb.linearVelocity.y);
#else
            rb.velocity = new Vector2(Mathf.Abs(speed), rb.velocity.y);
#endif
            // Add despawn watcher
            var despawn = o.GetComponent<DespawnWhenRightOfX>();
            if (despawn == null) despawn = o.AddComponent<DespawnWhenRightOfX>();
            despawn.xLimit = rightLimitX;
        }
        else
        {
            // Transform-based movement if no Rigidbody2D present
            var mover = o.GetComponent<MoveRight>();
            if (mover == null) mover = o.AddComponent<MoveRight>();
            mover.speed = Mathf.Abs(speed);

            var despawn = o.GetComponent<DespawnWhenRightOfX>();
            if (despawn == null) despawn = o.AddComponent<DespawnWhenRightOfX>();
            despawn.xLimit = rightLimitX;
        }
    }
}

/// <summary>
/// Data holder for obstacle hardness (no behavior differences here).
/// </summary>
public class ObstacleData : MonoBehaviour
{
    public enum Hardness { Hard, Soft }
    [Tooltip("Assigned on spawn. Another teammate handles behavior differences.")]
    public Hardness hardness = Hardness.Hard;
}

/// <summary>
/// Non-physics movement to the RIGHT (+X). Used only if no Rigidbody2D is present.
/// </summary>
public class MoveRight : MonoBehaviour
{
    [Tooltip("Units per second to the right.")]
    public float speed = 5f;

    private void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime, Space.World);
    }
}

/// <summary>
/// Destroys the GameObject once x exceeds the right boundary.
/// </summary>
public class DespawnWhenRightOfX : MonoBehaviour
{
    public float xLimit = 20f;

    private void Update()
    {
        if (transform.position.x > xLimit)
        {
            Destroy(gameObject);
        }
    }
}
