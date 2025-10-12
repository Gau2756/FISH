using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))] // C# attribute: auto-ensures there’s a BoxCollider2D
public class Obstacle : MonoBehaviour
{
    // C# enum (same idea as Java enum)
    public enum Hardness { Hard, Soft }

    [SerializeField] private float speed = 5f;        // leftward speed
    [SerializeField] private Hardness hardness = Hardness.Hard;

    private Rigidbody2D _rb;

    // C# expression-bodied property: Java-ish getter, just shorter
    public Hardness Kind => hardness;

    // Called by the spawner immediately after Instantiate()
    public void Init(float leftwardSpeed, Hardness kind)
    {
        speed = leftwardSpeed;
        hardness = kind;

        if (_rb == null) _rb = GetComponent<Rigidbody2D>();

        // If you added a Rigidbody2D, let physics handle it
        if (_rb != null)
        {
            _rb.linearVelocity = new Vector2(-Mathf.Abs(speed), 0f);
        }
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        // If you want obstacles to be triggers instead of the fish, uncomment:
        // GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private void Update()
    {
        // If no Rigidbody2D, move manually each frame
        if (_rb == null)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }

        // Cleanup when far off-screen
        if (transform.position.x < -20f)
        {
            Destroy(gameObject);
        }
    }
}
