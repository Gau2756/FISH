public class WaterFlowVisualizer : MonoBehaviour 
{
    public ParticleSystem waterParticles;
    private WaterCurrent waterCurrent;
    
    void Start() 
    {
        waterCurrent = FindObjectOfType<WaterCurrent>();
    }
    
    void Update() 
    {
        // Update particle direction based on current
        var velocityModule = waterParticles.velocityOverLifetime;
        Vector2 currentDir = waterCurrent.GetFlowVisualizationDirection(transform.position);
        
        velocityModule.x = currentDir.x;
        velocityModule.y = currentDir.y;
    }
}