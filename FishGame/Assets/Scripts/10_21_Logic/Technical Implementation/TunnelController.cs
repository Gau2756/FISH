public class TunnelController : MonoBehaviour 
{
    // Tunnel Dimensions
    public float currentWidth = 2f;
    public float minWidth = 1f;
    public float maxWidth = 5f;
    public float baseWidth = 2f;
    
    // Button Press Counters
    private int epHits = 0;
    private int lobbyistHits = 0;
    
    // Sensitivity
    public float widthChangeRate = 0.1f;
    
    // Visual Components
    public Transform topBoundary;
    public Transform bottomBoundary;
    public Transform tunnelHighlight; // Visual indicator of safe zone
    
    void Update() 
    {
        HandleInput();
    }
    
    void HandleInput() 
    {
        // Environmental Protectionist Input
        if (Input.GetKeyDown(KeyCode.W)) 
        {
            EPButtonPress();
        }
        
        // Lobbyist Input
        if (Input.GetKeyDown(KeyCode.UpArrow)) 
        {
            LobbyistButtonPress();
        }
    }
    
    public void EPButtonPress() 
    {
        epHits++;
        UpdateTunnelWidth();
        // Particle effect for feedback
        PlayEPEffect();
    }
    
    public void LobbyistButtonPress() 
    {
        lobbyistHits++;
        UpdateTunnelWidth();
        // Particle effect for feedback
        PlayLobbyistEffect();
    }
    
    void UpdateTunnelWidth() 
    {
        int hitDifference = epHits - lobbyistHits;
        float targetWidth = baseWidth + (hitDifference * widthChangeRate);
        currentWidth = Mathf.Clamp(targetWidth, minWidth, maxWidth);
        
        // Update visual representation
        UpdateTunnelVisuals();
        
        // Update UI display
        UIManager.Instance.UpdateTunnelWidth(currentWidth, minWidth, maxWidth);
    }
    
    void UpdateTunnelVisuals() 
    {
        float halfWidth = currentWidth / 2f;
        
        // Position boundaries
        topBoundary.localPosition = new Vector3(0, halfWidth, 0);
        bottomBoundary.localPosition = new Vector3(0, -halfWidth, 0);
        
        // Scale highlight zone
        tunnelHighlight.localScale = new Vector3(
            tunnelHighlight.localScale.x, 
            currentWidth, 
            1f
        );
        
        // Color code: green when wide, red when narrow
        float widthPercent = (currentWidth - minWidth) / (maxWidth - minWidth);
        Color tunnelColor = Color.Lerp(Color.red, Color.green, widthPercent);
        tunnelHighlight.GetComponent<SpriteRenderer>().color = tunnelColor;
    }
    
    // Check if a Y position is inside the safe tunnel
    public bool IsInTunnel(float yPosition) 
    {
        float tunnelCenterY = transform.position.y;
        float halfWidth = currentWidth / 2f;
        return Mathf.Abs(yPosition - tunnelCenterY) <= halfWidth;
    }
}