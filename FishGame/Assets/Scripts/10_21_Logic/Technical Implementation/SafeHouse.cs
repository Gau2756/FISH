public class SafeHouse : MonoBehaviour 
{
    // Time manipulation
    public float timeScaleInside = 0.3f; // 30% speed
    
    // Current negation
    public bool negateCurrentInside = true;
    
    // Visual
    public GameObject shelterEffect; // Bubble shield or glow
    
    // Optional: Time limit
    public bool hasTimeLimit = false;
    public float maxStayDuration = 5f;
    private float stayTimer = 0f;
    
    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Fish")) 
        {
            EnterShelter(other.gameObject);
        }
    }
    
    void OnTriggerStay2D(Collider2D other) 
    {
        if (other.CompareTag("Fish") && hasTimeLimit) 
        {
            stayTimer += Time.unscaledDeltaTime; // Use unscaled time
            
            if (stayTimer >= maxStayDuration) 
            {
                // Force exit
                ExitShelter(other.gameObject);
            }
        }
    }
    
    void OnTriggerExit2D(Collider2D other) 
    {
        if (other.CompareTag("Fish")) 
        {
            ExitShelter(other.gameObject);
        }
    }
    
    void EnterShelter(GameObject fish) 
    {
        // Slow down time
        Time.timeScale = timeScaleInside;
        
        // Stop fish velocity
        Rigidbody2D rb = fish.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        
        // Optionally disable current force
        if (negateCurrentInside) 
        {
            fish.GetComponent<FishMovement>().enabled = false;
            // Or set a flag to ignore current in FixedUpdate
        }
        
        // Visual effect
        shelterEffect?.SetActive(true);
        
        // Audio feedback
        PlayShelterEnterSound();
        
        stayTimer = 0f;
    }
    
    void ExitShelter(GameObject fish) 
    {
        // Resume normal time
        Time.timeScale = 1f;
        
        // Re-enable current effects
        if (negateCurrentInside) 
        {
            fish.GetComponent<FishMovement>().enabled = true;
        }
        
        // Brief invincibility (optional)
        // fish.GetComponent<FishStats>().SetInvincible(2f);
        
        // Visual effect
        shelterEffect?.SetActive(false);
        
        // Audio feedback
        PlayShelterExitSound();
        
        stayTimer = 0f;
    }
    
    void PlayShelterEnterSound() { }
    void PlayShelterExitSound() { }
}