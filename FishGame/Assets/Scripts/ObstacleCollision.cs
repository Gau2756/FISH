using UnityEngine;
using UnityEngine.Events;

public enum ObstacleKind
{
    Hard,
    Soft,
    Heart,
    Bagel,
    Star
}

[DisallowMultipleComponent]
public class ObstacleCollision : MonoBehaviour
{
    [Header("Classification")]
    public ObstacleKind kind = ObstacleKind.Soft;

    [Header("Hit Settings")]
    [Tooltip("Layer mask used to recognize the fish.")]
    public LayerMask fishLayers;

    [Tooltip("If true, destroy this object after a successful hit.")]
    public bool destroyOnHit = true;

    [Header("Bagel Buff")]
    [Tooltip("Strength multiplier for bagel effect.")]
    public float bagelMultiplier = 1.25f;

    [Tooltip("Duration in seconds of bagel buff.")]
    public float bagelDuration = 8f;

    [Header("Star")]
    [Tooltip("Optional ID/key to map to an awareness text asset.")]
    public string starId = "";

    [Header("Events (optional)")]
    public UnityEvent onAnyHit;
    public UnityEvent onHardHit;
    public UnityEvent onSoftHit;
    public UnityEvent onHeartCollected;
    public UnityEvent onBagelCollected;
    public UnityEvent onStarCollected;

    private void OnTriggerEnter2D(Collider2D other)  => TryHandle(other.gameObject);
    private void OnCollisionEnter2D(Collision2D col) => TryHandle(col.collider.gameObject);

    private void TryHandle(GameObject other)
    {
        if (!IsFish(other)) return;

        var fish = other.GetComponentInParent<FishAgent>() ?? other.GetComponent<FishAgent>();
        if (!fish) return; // fish must have FishAgent for this system

        onAnyHit?.Invoke();

        switch (kind)
        {
            case ObstacleKind.Hard:
                fish.LoseLife(1);
                onHardHit?.Invoke();
                break;

            case ObstacleKind.Soft:
                // Placeholder: for now, soft does nothing but can be extended (e.g., energy drain)
                onSoftHit?.Invoke();
                break;

            case ObstacleKind.Heart:
                fish.AddExtraLife(1);
                onHeartCollected?.Invoke();
                break;

            case ObstacleKind.Bagel:
                fish.ApplyStrengthBuff(bagelMultiplier, bagelDuration);
                onBagelCollected?.Invoke();
                break;

            case ObstacleKind.Star:
                fish.NotifyStarCollected(starId);
                onStarCollected?.Invoke();
                break;
        }

        if (destroyOnHit)
        {
            Destroy(gameObject);
        }
    }

    private bool IsFish(GameObject obj)
    {
        // Layer check first (fast)
        if (((1 << obj.layer) & fishLayers.value) != 0) return true;

        // Fallback: tag check if you prefer Tag = "Fish" or "Player"
        // return obj.CompareTag("Fish") || obj.CompareTag("Player");

        return false;
    }
}
