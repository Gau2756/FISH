public class CollectibleSpawner : MonoBehaviour 
{
    public GameObject bagelPrefab;
    public GameObject starPrefab;
    
    public Transform[] spawnPoints;
    
    public float bagelSpawnInterval = 5f;
    public float starSpawnInterval = 8f;
    
    void Start() 
    {
        InvokeRepeating("SpawnBagel", 2f, bagelSpawnInterval);
        InvokeRepeating("SpawnStar", 5f, starSpawnInterval);
    }
    
    void SpawnBagel() 
    {
        if (GameManager.Instance.IsGameStarted()) 
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(bagelPrefab, spawnPoint.position, Quaternion.identity);
        }
    }
    
    void SpawnStar() 
    {
        if (GameManager.Instance.IsGameStarted()) 
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject star = Instantiate(starPrefab, spawnPoint.position, Quaternion.identity);
            
            // Assign random harmful policy
            star.GetComponent<Star>().harmfulPolicyText = GetRandomPolicy();
        }
    }
    
    string GetRandomPolicy() 
    {
        string[] policies = new string[] 
        {
            "Dam maintenance budget cut by 40%",
            "Fish ladder requirements reduced",
            "Environmental impact studies waived",
            "Water flow minimums eliminated",
            "Salmon protection zone shrunk by 60%",
            "Corporate fishing quotas increased 300%"
        };
        
        return policies[Random.Range(0, policies.Length)];
    }
}