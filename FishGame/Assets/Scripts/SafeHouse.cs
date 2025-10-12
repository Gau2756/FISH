using UnityEngine;

public class SafeHouse : MonoBehaviour
{
    [SerializeField] private float timeScaleWhileInside = 0.3f;
    private bool fishInside = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Fish")) return;
        fishInside = true;
        // stop fish movement and slow time
        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
        Time.timeScale = timeScaleWhileInside;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Fish")) return;
        fishInside = false;
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }
}
