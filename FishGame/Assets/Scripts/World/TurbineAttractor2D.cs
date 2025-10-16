using UnityEngine;

public class TurbineAttractor2D : MonoBehaviour {
    public float pullStrength = 30f;
    public float radius = 8f; // influence range
    public Vector2 ForceAt(Vector2 worldPos) {
        Vector2 dir = ((Vector2)transform.position - worldPos);
        float d = dir.magnitude;
        if (d > radius) return Vector2.zero;
        float falloff = 1f - (d / radius);
        return dir.normalized * (pullStrength * falloff);
    }
}
