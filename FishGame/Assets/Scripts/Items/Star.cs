using UnityEngine;

public class Star : MonoBehaviour {
    public float energyValue = 20f;
    public float lifetime = 8f; // "lobbyist auto-collect" after lifetime ends

    float t;

    void Update() {
        t += Time.deltaTime;
        if (t >= lifetime) {
            // auto-despawn: considered "lobbyist collected"
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        var fish = other.GetComponent<FishController>();
        if (!fish) return;
        fish.stats.AddEnergy(energyValue);
        Destroy(gameObject);
    }
}
