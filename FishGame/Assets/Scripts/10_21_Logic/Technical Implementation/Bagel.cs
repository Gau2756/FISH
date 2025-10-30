// Bagel.cs
public class Bagel : MonoBehaviour 
{
    public float energyRestore = 20f;
    
    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Fish")) 
        {
            other.GetComponent<FishStats>().EatBagel();
            PlayCollectionEffect();
            Destroy(gameObject);
        }
    }
    
    void PlayCollectionEffect() 
    {
        // Particle effect, sound
    }
}