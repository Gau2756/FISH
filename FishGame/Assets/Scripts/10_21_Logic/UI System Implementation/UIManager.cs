public class UIManager : MonoBehaviour 
{
    public static UIManager Instance;
    
    public EnergyBar energyBar;
    public LivesDisplay livesDisplay;
    public TunnelWidthIndicator tunnelIndicator;
    public PolicyPopup policyPopup;
    public Text scoreText;
    public GameObject gameOverPanel;
    
    void Awake() 
    {
        if (Instance == null) 
        {
            Instance = this;
        } 
        else 
        {
            Destroy(gameObject);
        }
    }
    
    public void UpdateEnergyBar(float fillAmount) 
    {
        energyBar.UpdateEnergy(fillAmount * 100f, 100f);
    }
    
    public void UpdateLives(int lives) 
    {
        livesDisplay.UpdateLives(lives);
    }
    
    public void UpdateTunnelWidth(float width, float min, float max) 
    {
        tunnelIndicator.UpdateTunnelWidth(width, min, max);
    }
    
    public void ShowPolicyPopup(string message) 
    {
        policyPopup.ShowPolicy(message);
    }
    
    public void UpdateScore(int score) 
    {
        scoreText.text = $"Score: {score}";
    }
    
    public void ShowGameOverScreen(int finalScore) 
    {
        gameOverPanel.SetActive(true);
        // Display final score, restart button, etc.
    }
}