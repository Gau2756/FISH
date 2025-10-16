using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FishController : MonoBehaviour {
    public FishStats stats = new FishStats();
    public float inputEnergyCostPerSecond = 5f;
    public bool started = false;

    Rigidbody2D rb;
    CurrentField current;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        current = FindAnyObjectByType<CurrentField>();
        rb.simulated = false; // idle until first input
    }

    void Update() {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (!started && input != Vector2.zero) { started = true; rb.simulated = true; }

        if (!started) return;

        if (input != Vector2.zero) {
            float cost = inputEnergyCostPerSecond * Time.deltaTime;
            if (stats.SpendEnergy(cost)) {
                rb.AddForce(input.normalized * stats.strength, ForceMode2D.Force);
            }
        }
    }

    void FixedUpdate() {
        if (!started || current == null) return;
        rb.AddForce(current.SampleAt(transform.position), ForceMode2D.Force);
    }

    public void Kill() {
        stats.lives--;
        // simple respawn: reset pos & energy
        transform.position = GameManager.Instance.spawnPoint.position;
        rb.linearVelocity = Vector2.zero;
        stats.energy = stats.maxEnergy * 0.6f;
        if (stats.lives < 0) GameManager.Instance.GameOver();
    }
}
