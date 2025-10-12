using UnityEngine;
using System;
using System.Collections.Generic;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject[] obstaclePrefabs;  // Fill this in the Inspector (array/list UI)
    private List<GameObject> obstacles;

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
    private bool isSpawning = true;

    private float _timer;
    float randomInterval;

    private void Start()
    {
        // Unity’s global RNG. Java analogy: seeding a singleton RNG.
        if (seedFromTime)
        {
            UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);
        }
        randomInterval = UnityEngine.Random.value * 3;
        randomObstacleTotalWeight = 0;
        foreach (GameObject obj in obstaclePrefabs)
        {
            // print("Here");
            randomObstacleTotalWeight += obj.GetComponent<Obstacle>().weight;
        }
        // print("Total weight: " + randomObstacleTotalWeight);
        obstacles = new List<GameObject>();
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (!isSpawning) return;
        if (_timer >= spawnInterval + randomInterval)
        {
            SpawnOnce();
            _timer = 0f;
            randomInterval = UnityEngine.Random.value * extraRandomIntervalFactor;
        }
    }

    private void SpawnOnce()
    {
        if (obstaclePrefabs == null || obstaclePrefabs.Length == 0) return;

        // pick a random prefab
        int weight = UnityEngine.Random.Range(0, randomObstacleTotalWeight);
        // print("Initial weight: " + weight);
        GameObject prefab = null;
        foreach (GameObject obj in obstaclePrefabs)
        {
            weight -= obj.GetComponent<Obstacle>().weight;
            // print("Weight after subtraction: " + weight);
            if (weight < 0)
            {
                prefab = obj;
                break;
            }
        }

        // instantiate (Java: new + add to scene)
        GameObject o = Instantiate(prefab);
        obstacles.Add(o);

        // random Y within range, X at spawnX to the right
        float y = UnityEngine.Random.Range(spawnYRange.x, spawnYRange.y);
        o.transform.position = new Vector3(spawnX, y, 0f);

        // coin flip: hard or soft
        var kind = (UnityEngine.Random.value < 0.5f) ? Obstacle.Hardness.Hard : Obstacle.Hardness.Soft;

        // initialize movement + hardness
        var obs = o.GetComponent<Obstacle>();
        if (obs == null) obs = o.AddComponent<Obstacle>(); // safety
        obs.Init(obstacleSpeed, kind);
    }

    public void SetScrollSpeed(int value)
    {
        Predicate<GameObject> predicate = isNull;
        obstacles.RemoveAll(predicate);
        foreach (GameObject o in obstacles)
        {
            print("setting speed to " + value);
            o.GetComponent<Obstacle>().SetSpeed(value);
        }
    }

    private static bool isNull(GameObject obj)
    {
        return obj == null;
    }

    public void Stop()
    {
        isSpawning = false;
    }
}
