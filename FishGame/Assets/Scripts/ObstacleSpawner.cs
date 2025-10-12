using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject[] obstaclePrefabs;  // Fill this in the Inspector (array/list UI)

    [Header("Spawn Settings")]
    public float spawnInterval = 1f;              // seconds between spawns
    public float extraRandomIntervalFactor = 1f;
    public float spawnX = 10f;                    // where obstacles appear (to the right)
    public Vector2 spawnYRange = new Vector2(-2f, 2f);

    [Header("Movement")]
    public float obstacleSpeed = 5f;              // how fast they move left
    private int randomObstacleTotalWeight;

    [Header("Randomness")]
    public bool seedFromTime = true;              // seed once at Start()

    private float _timer;
    float randomInterval;

    private void Start()
    {
        // Unity’s global RNG. Java analogy: seeding a singleton RNG.
        if (seedFromTime)
        {
            Random.InitState((int)System.DateTime.Now.Ticks);
        }
        randomInterval = Random.value * 3;
        randomObstacleTotalWeight = 0;
        foreach (GameObject obj in obstaclePrefabs)
        {
            print("Here");
            randomObstacleTotalWeight += obj.GetComponent<Obstacle>().weight;
        }
        print("Total weight: " + randomObstacleTotalWeight);
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= spawnInterval + randomInterval)
        {
            SpawnOnce();
            _timer = 0f;
            randomInterval = Random.value * extraRandomIntervalFactor;            
        }
    }

    private void SpawnOnce()
    {
        if (obstaclePrefabs == null || obstaclePrefabs.Length == 0) return;

        // pick a random prefab
        int weight = Random.Range(0, randomObstacleTotalWeight);
        print("Initial weight: " + weight);
        GameObject prefab = null;
        foreach (GameObject obj in obstaclePrefabs)
        {
            weight -= obj.GetComponent<Obstacle>().weight;
            print("Weight after subtraction: " + weight);
            if (weight < 0)
            {
                prefab = obj;
                break;
            }
        }

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
