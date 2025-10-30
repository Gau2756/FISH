public class GameManager : MonoBehaviour 
{
    public static GameManager Instance;
    
    // Game State
    private bool gameStarted = false;
    private bool gameOver = false;
    
    // Scoring
    public int score = 0;
    
    // Checkpoints
    public Transform[] checkpoints;
    private int currentCheckpoint = 0;
    
    void Awake() 
    {
        // Singleton pattern
        if (Instance == null) 
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } 
        else 
        {
            Destroy(gameObject);
        }
    }
    
    public void SetGameStarted(bool started) 
    {
        gameStarted = started;
    }
    
    public bool IsGameStarted() 
    {
        return gameStarted;
    }
    
    public void GameOver() 
    {
        gameOver = true;
        Time.timeScale = 0f;
        UIManager.Instance.ShowGameOverScreen(score);
    }
    
    public void AddScore(int points) 
    {
        score += points;
        UIManager.Instance.UpdateScore(score);
    }
    
    public Vector3 GetCheckpointPosition() 
    {
        return checkpoints[currentCheckpoint].position;
    }
    
    public void SetCheckpoint(int checkpointIndex) 
    {
        currentCheckpoint = checkpointIndex;
    }
    
    public void RestartGame() 
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
        );
    }
}