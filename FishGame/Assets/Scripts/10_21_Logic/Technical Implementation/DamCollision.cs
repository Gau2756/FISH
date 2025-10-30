public class DamCollision : MonoBehaviour 
{
    public TunnelController tunnel;
    
    void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.CompareTag("Fish")) 
        {
            CheckCollision(collision);
        }
    }
    
    void CheckCollision(Collision2D collision) 
    {
        float fishY = collision.transform.position.y;
        
        // Check if fish is in the safe tunnel zone
        if (tunnel.IsInTunnel(fishY)) 
        {
            // Safe passage - fish is in the tunnel
            // Optional: give score bonus for successful passage
            return;
        } 
        else 
        {
            // Fish hit dam outside tunnel - death
            FishStats fish = collision.gameObject.GetComponent<FishStats>();
            
            // Death effect before killing
            PlayDeathEffect(collision.transform.position);
            
            fish.Die();
        }
    }
    
    void PlayDeathEffect(Vector3 position) 
    {
        // Particle effect, sound, camera shake
        // Instantiate death particles
        // Play death sound
    }
}