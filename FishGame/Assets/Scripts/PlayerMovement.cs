using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    //public float jumpForce = 10f;
    private Rigidbody2D rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Movement Input
        float horizontalInput = Input.GetAxis("Horizontal"); // Uses the "Horizontal" axis from Input Manager
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocityY);

        // Jump Input
        // if (Input.GetButtonDown("Jump") && isGrounded) // Uses the "Jump" button from Input Manager
        // {
        //     rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        //     isGrounded = false;
        // }
    }

    // Example for checking if the player is grounded (you'll need more robust checks in a full game)
    // void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.gameObject.CompareTag("Ground"))
    //     {
    //         isGrounded = true;
    //     }
    // }
}
