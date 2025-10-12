using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FishStats : MonoBehaviour
{
    [Header("Energy")]
    [SerializeField] private float maxEnergy = 100f;
    [SerializeField] private float currentEnergy = 100f;

    [Header("Attributes")]
    [SerializeField] private float strength = 200f; // how strong movement force is
    [SerializeField] private int lives = 3;

    public float MaxEnergy => maxEnergy;
    public float CurrentEnergy => currentEnergy;
    public float Strength => strength;
    public int Lives => lives;

    public event System.Action OnDied;
    public event System.Action<int> OnLivesChanged;

    private void Start()
    {
        currentEnergy = Mathf.Clamp(currentEnergy, 0f, maxEnergy);
    }

    public bool UseEnergy(float amount)
    {
        if (amount <= 0f) return true;
        if (currentEnergy <= 0f) return false;

        currentEnergy -= amount;
        currentEnergy = Mathf.Clamp(currentEnergy, 0f, maxEnergy);
        return currentEnergy > 0f;
    }

    public void EatBagel(float energyGain)
    {
        currentEnergy += energyGain;
        currentEnergy = Mathf.Clamp(currentEnergy, 0f, maxEnergy);
    }

    public void IncreaseStrength(float delta)
    {
        strength = Mathf.Max(0f, strength + delta);
    }

    public void Die()
    {
        lives = Mathf.Max(0, lives - 1);
        OnLivesChanged?.Invoke(lives);

        Debug.Log($"Fish died. Lives remaining: {lives}");

        OnDied?.Invoke();

        if (lives <= 0)
        {
            GameOver();
        }
        else
        {
            // simple death behaviour: disable fish for now
            gameObject.SetActive(false);
        }
    }

    private void GameOver()
    {
        Debug.Log("Game Over: no lives remaining.");
        // TODO: hook into a GameManager or scene reload
        gameObject.SetActive(false);
    }
}
