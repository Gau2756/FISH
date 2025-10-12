using UnityEngine;

public class WaterCurrent : MonoBehaviour
{
    [Header("Current Settings")]
    [SerializeField] private Vector2 baseDirection = Vector2.down; // default pulls down
    [SerializeField] private float baseStrength = 0f; // no effect until game starts

    [Header("Turbine")]
    [SerializeField] private Transform turbineTransform;
    [SerializeField] private float turbineInfluenceRadius = 5f;
    [SerializeField] private float turbineMaxForce = 50f;

    [Header("Lobbyist influence")]
    [SerializeField] private float lobbyistTimeMultiplier = 0.1f; // how fast lobbyist increases strength per second
    [SerializeField] private float lobbyistAccumulatedTime = 0f;

    private float currentStrength;

    private void Start()
    {
        currentStrength = baseStrength;
    }

    private void FixedUpdate()
    {
        // accumulate lobbyist effect over time
        lobbyistAccumulatedTime += Time.fixedDeltaTime;
        currentStrength = baseStrength + lobbyistAccumulatedTime * lobbyistTimeMultiplier;
    }

    public Vector2 GetCurrentForce(Vector2 fishPosition)
    {
        // If no turbine, return a uniform current
        if (turbineTransform == null)
        {
            return baseDirection.normalized * currentStrength;
        }

        Vector2 toFish = fishPosition - (Vector2)turbineTransform.position;
        float dist = toFish.magnitude;
        if (dist > turbineInfluenceRadius) return Vector2.zero;

        // compute perpendicular direction to turbine axis (assume turbine axis = transform.up)
        Vector2 axis = turbineTransform.up; // axis direction
        // perpendicular: rotate axis by 90 degrees
        Vector2 perp = new Vector2(-axis.y, axis.x).normalized;

        // influence falls off with distance
        float falloff = 1f - Mathf.Clamp01(dist / turbineInfluenceRadius);

        Vector2 force = perp * turbineMaxForce * falloff * (currentStrength / Mathf.Max(1f, baseStrength + 0.0001f));
        return force;
    }

    public void IncreaseLobbyistEffect(float amount)
    {
        lobbyistAccumulatedTime += amount;
    }
}
