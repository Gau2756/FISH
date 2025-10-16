using UnityEngine;

public class DamKillZone : MonoBehaviour {
    public BoxCollider2D tunnelZone;

    void OnTriggerEnter2D(Collider2D other) {
        var fish = other.GetComponent<FishController>();
        if (!fish) return;

        // if fish is ALSO inside tunnelZone, it's safe; otherwise die
        if (!IsInside(tunnelZone, fish.transform.position)) {
            fish.Kill();
        }
    }
    bool IsInside(BoxCollider2D box, Vector3 worldPos) {
        if (!box) return false;
        Vector3 local = box.transform.InverseTransformPoint(worldPos) - (Vector3)box.offset;
        Vector2 half = box.size * 0.5f;
        return Mathf.Abs(local.x) <= half.x && Mathf.Abs(local.y) <= half.y;
    }
}
