public class WaterCurrent : MonoBehaviour 
{
    // Main Current
    public Vector2 currentDirection = Vector2.right;
    public float baseCurrentStrength = 2f;
    public float currentStrength;
    
    // Turbine System
    public Transform turbinePosition;
    public float turbinePullStrength = 1.5f;
    public float turbineEffectRadius = 10f;
    
    // Time-based Difficulty
    public float lobbyistStrengthIncreaseRate = 0.1f; // Per second
    private float gameTime = 0f;
    
    void Start() 
    {
        currentStrength = baseCurrentStrength;
    }
    
    void Update() 
    {
        if (GameManager.Instance.IsGameStarted()) 
        {
            gameTime += Time.deltaTime;
            
            // Lobbyists make current stronger over time
            currentStrength = baseCurrentStrength + (gameTime * lobbyistStrengthIncreaseRate);
        }
    }
    
    public Vector2 GetCurrentForce(Vector2 fishPosition) 
    {
        // Main current force
        Vector2 mainCurrent = currentDirection.normalized * currentStrength;
        
        // Turbine pull calculation
        Vector2 turbinePull = CalculateTurbinePull(fishPosition);
        
        // Combined force
        return mainCurrent + turbinePull;
    }
    
    Vector2 CalculateTurbinePull(Vector2 fishPosition) 
    {
        Vector2 toTurbine = (Vector2)turbinePosition.position - fishPosition;
        float distance = toTurbine.magnitude;
        
        // No effect if too far from turbine
        if (distance > turbineEffectRadius) 
        {
            return Vector2.zero;
        }
        
        // Inverse square falloff for realistic feel
        float falloff = 1f - (distance / turbineEffectRadius);
        float effectiveStrength = turbinePullStrength * falloff * falloff;
        
        // Perpendicular pull (creates vortex effect)
        Vector2 pullDirection = toTurbine.normalized;
        
        return pullDirection * effectiveStrength;
    }
    
    // Visual feedback - particle system for water flow
    public Vector2 GetFlowVisualizationDirection(Vector2 position) 
    {
        return GetCurrentForce(position).normalized;
    }
}