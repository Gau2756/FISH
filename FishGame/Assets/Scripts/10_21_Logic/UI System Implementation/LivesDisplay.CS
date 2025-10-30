public class LivesDisplay : MonoBehaviour 
{
    public Image[] lifeIcons;
    
    public void UpdateLives(int currentLives) 
    {
        for (int i = 0; i < lifeIcons.Length; i++) 
        {
            lifeIcons[i].enabled = (i < currentLives);
        }
    }
}