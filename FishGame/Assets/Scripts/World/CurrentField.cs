using UnityEngine;

public class CurrentField : MonoBehaviour {
    [Header("Base current to the right/ocean")]
    public float baseStrength = 8f;

    [Header("Lobbyist steepening over time")]
    public float steepenPerMinute = 4f; // grows the pull to the right over time

    [Header("Optional turbine pull")]
    public TurbineAttractor2D turbine;

    float elapsed;

    public Vector2 SampleAt(Vector2 worldPos) {
        float t = elapsed / 60f;
        Vector2 v = Vector2.right * (baseStrength + steepenPerMinute * t);
        if (turbine) v += turbine.ForceAt(worldPos);
        return v;
    }

    void FixedUpdate() { elapsed += Time.fixedDeltaTime; }
}
