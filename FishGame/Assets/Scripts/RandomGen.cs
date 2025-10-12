using UnityEngine;

public class RandomGen : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject[] obstaclePrefabs;        // Assign in Inspector
    public float spawnInterval = 2f;            // Seconds between spawns
    public Vector2 spawnYRange = new Vector2(-2f, 2f); // Random Y range
    public float spawnX = 10f;                  // Where obstacles appear (right side)

    [Header("Movement Settings")]
    public float obstacleSpeed = 5f;            // Units/sec to the left

    [Header("Randomness")]
    public bool seedFromTime = true;            // Use system time once in Start()

    private float _timer;

    void Start()
    {
        // C# vs Java:
        // C#: Unity has a global RNG in UnityEngine.Random (static).
        // Seeding it globally is like setting a static seed on a singleton RNG.
        if (seedFromTime)
        {
            // DateTime.Now.Ticks = 100-ns ticks -> huge long; cast to int is fine here
            Random.InitState((int)System.DateTime.Now.Ticks);
        }
    }

    void Update()
    {
        _timer += Time.deltaTime;   // Delta time since last frame (Unity gives this)
        if (_timer >= spawnInterval)
        {
            SpawnObstacle();
            _timer = 0f;
        }
    }

    void SpawnObstacle()
    {
        if (obstaclePrefabs == null || obstaclePrefabs.Length == 0) return;

        int index = Random.Range(0, obstaclePrefabs.Length); // [min, max)
        GameObject prefab = obstaclePrefabs[index];

        // Instantiate = Java new + add to scene
        GameObject o = Instantiate(prefab);

        // Place it off to the right at a random Y
        float y = Random.Range(spawnYRange.x, spawnYRange.y);
        o.transform.position = new Vector3(spawnX, y, 0f);

        // Assign "hard" or "soft" (50/50). Teammate will decide behavior later.
        var hardness = (Random.value < 0.5f) ? Obstacle.Hardness.Hard : Obstacle.Hardness.Soft;

        // Initialize obstacle (speed + hardness)
        Obstacle obs = o.GetComponent<Obstacle>();
        if (obs == null) obs = o.AddComponent<Obstacle>(); // Ensure it exists
        obs.Init(obstacleSpeed, hardness);
    }
}
