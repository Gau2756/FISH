public class Star : MonoBehaviour 
{
    // Timer
    public float timeUntilLobbyistGets = 10f;
    private float timer = 0f;
    private bool collected = false;
    
    // Policy Data
    [TextArea(3, 5)]
    public string harmfulPolicyText;
    
    // Visual
    private SpriteRenderer spriteRenderer;
    public Color warningColor = Color.red;
    public float warningThreshold = 3f; // Start flashing at 3 seconds
    
    void Start() 
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    void Update() 
    {
        if (collected || !GameManager.Instance.IsGameStarted()) return;
        
        timer += Time.deltaTime;
        
        // Visual warning when time is running out
        if (timer >= timeUntilLobbyistGets - warningThreshold) 
        {
            FlashWarning();
        }
        
        // Lobbyist auto-collection
        if (timer >= timeUntilLobbyistGets) 
        {
            LobbyistCollectStar();
        }
    }
    
    void FlashWarning() 
    {
        // Pulse between normal and warning color
        float t = Mathf.PingPong(Time.time * 3f, 1f);
        spriteRenderer.color = Color.Lerp(Color.white, warningColor, t);
    }
    
    void OnTriggerEnter2D(Collider2D other) 
    {
        if (collected) return;
        
        if (other.CompareTag("Fish")) 
        {
            PlayerCollectStar(other.GetComponent<FishStats>());
        }
    }
    
    void PlayerCollectStar(FishStats fish) 
    {
        collected = true;
        
        // Give benefits
        fish.CollectStar();
        
        // Visual/audio feedback
        PlayCollectionEffect();
        
        // Add score
        GameManager.Instance.AddScore(100);
        
        Destroy(gameObject);
    }
    
    void LobbyistCollectStar() 
    {
        collected = true;
        
        // Show policy popup
        UIManager.Instance.ShowPolicyPopup(harmfulPolicyText);
        
        // Make game harder
        ApplyLobbyistPenalty();
        
        // Negative feedback
        PlayLobbyistCollectionEffect();
        
        Destroy(gameObject);
    }
    
    void ApplyLobbyistPenalty() 
    {
        // Increase current strength
        WaterCurrent current = FindObjectOfType<WaterCurrent>();
        current.baseCurrentStrength += 0.5f;
        
        // Could also narrow tunnel slightly
        // TunnelController tunnel = FindObjectOfType<TunnelController>();
        // tunnel.LobbyistButtonPress(); // Simulate button presses
    }
    
    void PlayCollectionEffect() 
    {
        // Positive sound, particle burst, UI animation
    }
    
    void PlayLobbyistCollectionEffect() 
    {
        // Negative sound, dark particles, screen shake
    }
}