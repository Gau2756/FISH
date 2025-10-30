public class FishStats : MonoBehaviour 
{
    // Energy System
    public float maxEnergy = 100f;
    public float currentEnergy;
    
    // Strength System
    public float strength = 5f;
    
    // Lives System
    public int maxLives = 3;
    public int currentLives;
    
    // Collectibles
    public float bagelEnergyGain = 20f;
    public float starEnergyGain = 15f;
    
    void Start() 
    {
        currentEnergy = maxEnergy;
        currentLives = maxLives;
    }
    
    // Energy Management
    public bool UseEnergy(float amount) 
    {
        if (currentEnergy >= amount) 
        {
            currentEnergy -= amount;
            return true; // Movement allowed
        }
        return false; // Not enough energy
    }
    
    public void RestoreEnergy(float amount) 
    {
        currentEnergy = Mathf.Min(currentEnergy + amount, maxEnergy);
        // Trigger UI update
        UIManager.Instance.UpdateEnergyBar(currentEnergy / maxEnergy);
    }
    
    // Collectible Methods
    public void EatBagel() 
    {
        RestoreEnergy(bagelEnergyGain);
    }
    
    public void CollectStar() 
    {
        RestoreEnergy(starEnergyGain);
        // Future: boost strength temporarily
    }
    
    // Death and Respawn
    public void Die() 
    {
        currentLives--;
        
        if (currentLives <= 0) 
        {
            GameManager.Instance.GameOver();
        } 
        else 
        {
            Respawn();
        }
    }
    
    void Respawn() 
    {
        transform.position = GameManager.Instance.GetCheckpointPosition();
        currentEnergy = maxEnergy;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
}