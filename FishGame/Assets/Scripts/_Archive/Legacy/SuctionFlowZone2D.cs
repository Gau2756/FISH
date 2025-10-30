using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class SuctionFlowZone2D : MonoBehaviour
{
    public Transform axisStart;
    public Transform axisEnd;
    public float suctionStrength = 15f;
    public float maxSuction = 35f;
    public float flowStrength = 3f;
    public Vector2 flowDirection = Vector2.right;
    public AnimationCurve distanceFalloff = AnimationCurve.EaseInOut(0, 1, 10, 0);

    private void Reset() => GetComponent<Collider2D>().isTrigger = true;

    private void OnTriggerStay2D(Collider2D other)
    {
        var rb = other.attachedRigidbody;
        if (!rb) return;

        Vector2 a = axisStart ? (Vector2)axisStart.position : (Vector2)transform.position + Vector2.left;
        Vector2 b = axisEnd   ? (Vector2)axisEnd.position   : (Vector2)transform.position + Vector2.right;
        Vector2 axis = b - a;
        float lenSq = axis.sqrMagnitude;
        if (lenSq < 1e-6f) return;

        Vector2 p = rb.worldCenterOfMass;
        float t = Mathf.Clamp01(Vector2.Dot(p - a, axis) / lenSq);
        Vector2 closest = a + t * axis;

        Vector2 toAxis = closest - p;
        float dist = toAxis.magnitude;
        Vector2 dirToAxis = dist > 1e-5f ? toAxis / dist : Vector2.zero;

        float suction = suctionStrength * distanceFalloff.Evaluate(dist);
        Vector2 suctionForce = dirToAxis * Mathf.Min(suction, maxSuction);
        Vector2 flowForce = (flowDirection.sqrMagnitude > 1e-6f ? flowDirection.normalized : Vector2.zero) * flowStrength;

        rb.AddForce(suctionForce + flowForce, ForceMode2D.Force);
    }
}
