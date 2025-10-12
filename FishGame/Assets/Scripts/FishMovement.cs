using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FishMovement : MonoBehaviour
{
    [SerializeField] private FishStats stats;
    [SerializeField] private WaterCurrent waterCurrent;

    [Header("Movement")]
    [SerializeField] private float energyCostPerMove = 5f; // cost per input
    [SerializeField] private bool gameStarted = false;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (stats == null) stats = GetComponent<FishStats>();
    }

    private void Update()
    {
        // start game when player presses any movement key (space or arrow)
        if (!gameStarted && (Input.anyKey))
        {
            gameStarted = true;
        }
    }

    private void FixedUpdate()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (input.sqrMagnitude > 0f && stats != null)
        {
            // consume energy per move (scaled by delta time for smoother feel)
            float cost = energyCostPerMove * Time.fixedDeltaTime;
            if (stats.UseEnergy(cost))
            {
                Vector2 force = input.normalized * stats.Strength * Time.fixedDeltaTime;
                rb.AddForce(force, ForceMode2D.Force);
            }
        }

        if (gameStarted && waterCurrent != null)
        {
            Vector2 currentForce = waterCurrent.GetCurrentForce(rb.position);
            rb.AddForce(currentForce * Time.fixedDeltaTime, ForceMode2D.Force);
        }
    }
}
