using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject[] obstaclePrefabs;  // Fill this in the Inspector (array/list UI)

    [Header("Spawn Settings")]
    public float spawnInterval = 2f;              // seconds between spawns
    public float spawnX = 10f;                    // where obstacles appear (to the right)
    public Vector2 spawnYRange = new Vector2(-2f, 2f);

    [Header("Movement")]
    public float obstacleSpeed = 5f;              // how fast they move left

    [Header("Randomness")]
    public bool seedFromTime = true;              // seed once at Start()

    private float _timer;

    private void Start()
    {
        // Unity’s global RNG. Java analogy: seeding a singleton RNG.
        if (seedFromTime)
        {
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
        if (obstaclePrefabs == null || obstaclePrefabs.Length == 0) return;

        // pick a random prefab
        int idx = Random.Range(0, obstaclePrefabs.Length); // int: [min, max)
        GameObject prefab = obstaclePrefabs[idx];

        // instantiate (Java: new + add to scene)
        GameObject o = Instantiate(prefab);

        // random Y within range, X at spawnX to the right
        float y = Random.Range(spawnYRange.x, spawnYRange.y);
        o.transform.position = new Vector3(spawnX, y, 0f);

        // coin flip: hard or soft
        var kind = (Random.value < 0.5f) ? Obstacle.Hardness.Hard : Obstacle.Hardness.Soft;

        // initialize movement + hardness
        var obs = o.GetComponent<Obstacle>();
        if (obs == null) obs = o.AddComponent<Obstacle>(); // safety
        obs.Init(obstacleSpeed, kind);
    }
}
