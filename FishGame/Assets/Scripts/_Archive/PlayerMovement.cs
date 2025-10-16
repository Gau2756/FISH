using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    //public float jumpForce = 10f;
    private Rigidbody2D rb;
    private enum ObstacleType {FOOD, LIFE, SLOW, COLLIDE, POINTS}
    public Slider energyBar;
    public Text pointDisplay;
    private int points = 0;
    public float energyDecayTime = 30f;
    private float currEnergy = 1.0f;
    public float slowMultiplier = 0.6f;
    private Vector2 vel;
    private bool isSlowed = false;
    float t = 0f;
    public int totalLives = 4;
    private int currLives;
    public float foodReplenishmentPercentage = 20f;
    public float maxEnergy = 1.0f;
    public Transform heartContainer;
    private Transform[] heart;
    public ObstacleSpawner spawner;
    public BackgroundScroller bgScroller;
    private bool movementEnabled = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        energyBar.value = 1.0f;
        currEnergy = maxEnergy;
        currLives = totalLives;
    }

    float EnergyAtTime(float t)
    {
        return 1 / Mathf.Sqrt(energyDecayTime) * Mathf.Sqrt(-t + energyDecayTime);
    }

    void setHeartDisplay()
    {
        int numLives = currLives;
        int counter = 0;
        for (int i = 0; i < totalLives; i++)
        {
            if (numLives >= 0)
            {
                heartContainer.GetChild(counter).GetComponent<Image>().enabled = true;
            }
            else
            {
                heartContainer.GetChild(counter).GetComponent<Image>().enabled = false;
            }
            numLives--;
        }
    }

    void Update()
    {
        if (!movementEnabled)
        {
            vel = Vector2.zero;
            return;
        }


        t += Time.deltaTime;
        if (EnergyAtTime(t) <= 0)
        {
            // game over, you ran out of energy
            GameOver();
        }
        energyBar.value = Mathf.Lerp(1, 0, t / energyDecayTime);
        currEnergy = EnergyAtTime(t);
        // Movement Input

        float verticalInput = Input.GetAxis("Vertical"); // Uses the "Horizontal" axis from Input Manager
        vel = new Vector2(rb.linearVelocityX, verticalInput * moveSpeed * currEnergy);
        if (isSlowed) vel *= slowMultiplier;



        // Jump Input
        // if (Input.GetButtonDown("Jump") && isGrounded) // Uses the "Jump" button from Input Manager
        // {
        //     rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        //     isGrounded = false;
        // }
    }

    public void GameOver()
    {
        movementEnabled = false;
        rb.linearVelocity = Vector2.zero;
        vel = Vector2.zero;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = vel;
    }

    // Example for checking if the player is grounded (you'll need more robust checks in a full game)
    // void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.gameObject.CompareTag("Ground"))
    //     {
    //         isGrounded = true;
    //     }
    // }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Obstacle o = other.gameObject.GetComponent<Obstacle>();
        ObstacleInfo.Type obstacleType = o.type;
        switch (obstacleType)
        {
            case ObstacleInfo.Type.FOOD:
                currEnergy += maxEnergy * foodReplenishmentPercentage / 100f;
                Destroy(other.gameObject);
                break;
            case ObstacleInfo.Type.LIFE:
                currLives++;
                currEnergy = maxEnergy;
                Destroy(other.gameObject);
                break;
            case ObstacleInfo.Type.SLOW:
                isSlowed = true;
                break;
            case ObstacleInfo.Type.COLLIDE:
                spawner.SetScrollSpeed(0);
                bgScroller.SetScrollSpeed(0);
                spawner.Stop();
                movementEnabled = false;
                // game over
                break;
            case ObstacleInfo.Type.POINTS:
                Destroy(other.gameObject);
                break;
            default:
                break;
        }
        points += o.points;
        pointDisplay.text = "Points: " + points;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Obstacle o = other.gameObject.GetComponent<Obstacle>();
        ObstacleType obstacleType = (ObstacleType)o.type;
        switch (obstacleType)
        {
            case ObstacleType.SLOW:
                isSlowed = false;
                break;
            default:
                break;

        }
    }
}
