public class CameraFollow : MonoBehaviour 
{
    public Transform target; // Fish
    public float smoothSpeed = 0.125f;
    public Vector3 offset = new Vector3(0, 0, -10);
    
    // Boundaries
    public float minX, maxX, minY, maxY;
    
    void LateUpdate() 
    {
        Vector3 desiredPosition = target.position + offset;
        
        // Clamp to boundaries
        desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);
        
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}