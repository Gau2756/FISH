using UnityEngine;

public class DamTunnelController : MonoBehaviour {
    [Header("Colliders")]
    public BoxCollider2D killZone;    // big trigger covering the dam face
    public BoxCollider2D tunnelZone;  // trigger for safe tunnel opening

    [Header("Tunnel width bounds")]
    public float minWidth = 0.5f;
    public float maxWidth = 6f;

    float epHits, lobbyHits;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Q)) epHits++;
        if (Input.GetKeyDown(KeyCode.E)) lobbyHits++;

        float diff = Mathf.Clamp(epHits - lobbyHits, -10, 10);
        float t = Mathf.InverseLerp(-10, 10, diff);
        float width = Mathf.Lerp(minWidth, maxWidth, t);

        var sz = tunnelZone.size;
        sz.x = width;
        tunnelZone.size = sz;
    }

    void OnTriggerEnter2D(Collider2D other) {
        // attach this script to an empty with no collider
    }

    void OnEnable() {
        // ensure both colliders are triggers
        if (killZone) killZone.isTrigger = true;
        if (tunnelZone) tunnelZone.isTrigger = true;
    }
}
