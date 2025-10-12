using UnityEngine;

public class DamCollision : MonoBehaviour
{
    [SerializeField] private TunnelController tunnel;
    [SerializeField] private FishStats fishStats;
    [SerializeField] private Collider2D damCollider; // defines the dam area (including tunnel)

    private void Start()
    {
        if (fishStats == null) fishStats = GetComponent<FishStats>();
        if (damCollider == null)
        {
            Debug.LogWarning("DamCollision: damCollider not set. Please assign a collider representing the dam bounds.");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Dam")) return;

        // Determine if fish is within tunnel safe area
        if (tunnel == null)
        {
            Debug.LogWarning("DamCollision: tunnel reference missing.");
            return;
        }

        // assume tunnel is centered at tunnel.transform.position and extends horizontally by currentWidth/2
        Vector2 tunnelCenter = tunnel.transform.position;
        float halfWidth = tunnel.CurrentWidth / 2f;

        Vector2 fishPos = transform.position;
        // check horizontal distance from tunnel center -> if outside, die
        if (Mathf.Abs(fishPos.x - tunnelCenter.x) > halfWidth)
        {
            fishStats?.Die();
        }
    }
}
