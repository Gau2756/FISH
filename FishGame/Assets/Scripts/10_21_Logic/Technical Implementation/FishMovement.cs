public class FishMovement : MonoBehaviour 
{
    private Rigidbody2D rb;
    private FishStats stats;
    public WaterCurrent waterCurrent;
    
    // Game State
    private bool gameStarted = false;
    
    // Movement Config
    public float energyCostPerSecond = 5f;
    public float movementSmoothing = 0.1f;
    
    // Input
    private Vector2 inputDirection;
    
    void Start() 
    {
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<FishStats>();
        
        // Fish shouldn't be affected by physics until game starts
        rb.isKinematic = true;
    }
    
    void Update() 
    {
        HandleInput();
        
        // Future: Pre-game bobbing animation
        if (!gameStarted) 
        {
            // BobbingMotion();
        }
    }
    
    void HandleInput() 
    {
        // Get player input
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        inputDirection = new Vector2(moveX, moveY).normalized;
        
        // Start game on first input
        if (!gameStarted && inputDirection.magnitude > 0) 
        {
            StartGame();
        }
    }
    
    void StartGame() 
    {
        gameStarted = true;
        rb.isKinematic = false;
        GameManager.Instance.SetGameStarted(true);
        
        // Start music, UI timers, etc.
    }
    
    void FixedUpdate() 
    {
        if (!gameStarted) return;
        
        // Apply water current force
        Vector2 currentForce = waterCurrent.GetCurrentForce(transform.position);
        rb.AddForce(currentForce);
        
        // Player movement (if they have energy)
        if (inputDirection.magnitude > 0) 
        {
            float energyCost = energyCostPerSecond * Time.fixedDeltaTime;
            
            if (stats.UseEnergy(energyCost)) 
            {
                Vector2 movementForce = inputDirection * stats.strength;
                rb.AddForce(movementForce, ForceMode2D.Force);
                
                // Animation trigger
                SetSwimmingAnimation(true);
            } 
            else 
            {
                // No energy - show visual feedback
                ShowOutOfEnergyEffect();
                SetSwimmingAnimation(false);
            }
        } 
        else 
        {
            SetSwimmingAnimation(false);
        }
        
        // Apply drag for more realistic water movement
        rb.velocity *= (1f - movementSmoothing);
        
        // Rotate fish to face movement direction
        RotateFishToMovement();
    }
    
    void RotateFishToMovement() 
    {
        if (rb.velocity.magnitude > 0.1f) 
        {
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
    
    void SetSwimmingAnimation(bool isSwimming) 
    {
        // Trigger animator parameter
        GetComponent<Animator>()?.SetBool("IsSwimming", isSwimming);
    }
    
    void ShowOutOfEnergyEffect() 
    {
        // Particle effect, sound, screen flash, etc.
    }
}