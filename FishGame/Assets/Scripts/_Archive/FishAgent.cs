using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
public class FishAgent : MonoBehaviour
{
    [Header("Vitals")]
    [Tooltip("Current lives (heart). When <= 0 → death.")]
    public int lives = 3;

    [Tooltip("Extra lives bank (earned by Heart pickups).")]
    public int extraLives = 0;

    [Header("Movement/Power")]
    [Tooltip("Base thrust strength used by your movement script.")]
    public float baseStrength = 12f;

    [Tooltip("Current strength applied by your movement script. Use GetCurrentStrength().")]
    [SerializeField] private float currentStrength;

    [Header("Optional Energy (if you use it)")]
    public bool useEnergy = false;
    public float maxEnergy = 100f;
    [SerializeField] private float energy;

    [Header("Events")]
    public UnityEvent onFishDied;
    public UnityEvent<int> onLivesChanged;
    public UnityEvent<int> onExtraLivesChanged;
    public UnityEvent<float> onStrengthChanged;
    public UnityEvent<string> onStarCollected; // payload can be an ID / key you map to texts

    void Awake()
    {
        currentStrength = baseStrength;
        if (useEnergy) energy = maxEnergy;
        onLivesChanged?.Invoke(lives);
        onExtraLivesChanged?.Invoke(extraLives);
        onStrengthChanged?.Invoke(currentStrength);
    }

    // ===== Lives / Extra lives =====
    public void LoseLife(int amount = 1)
    {
        lives -= Mathf.Max(1, amount);
        if (lives <= 0)
        {
            if (extraLives > 0)
            {
                extraLives--;
                lives = 1; // consume extra life to revive with 1 life
                onExtraLivesChanged?.Invoke(extraLives);
            }
            else
            {
                lives = 0;
                onLivesChanged?.Invoke(lives);
                onFishDied?.Invoke();
                return;
            }
        }
        onLivesChanged?.Invoke(lives);
    }

    public void AddExtraLife(int amount = 1)
    {
        extraLives += Mathf.Max(1, amount);
        onExtraLivesChanged?.Invoke(extraLives);
    }

    // ===== Strength (bagel buff) =====
    public void ApplyStrengthBuff(float multiplier = 1.25f, float durationSeconds = 8f)
    {
        StopCoroutine(nameof(StrengthBuffCo));
        StartCoroutine(StrengthBuffCo(multiplier, durationSeconds));
    }

    private IEnumerator StrengthBuffCo(float mult, float dur)
    {
        float original = currentStrength <= 0f ? baseStrength : currentStrength;
        currentStrength = original * Mathf.Max(0.01f, mult);
        onStrengthChanged?.Invoke(currentStrength);
        float t = 0f;
        while (t < dur)
        {
            t += Time.deltaTime;
            yield return null;
        }
        currentStrength = baseStrength;
        onStrengthChanged?.Invoke(currentStrength);
    }

    public float GetCurrentStrength() => currentStrength > 0f ? currentStrength : baseStrength;

    // ===== Energy (optional helpers) =====
    public void RefillEnergy(float amount)
    {
        if (!useEnergy) return;
        energy = Mathf.Clamp(energy + Mathf.Max(0f, amount), 0f, maxEnergy);
    }

    public bool TryConsumeEnergy(float amount)
    {
        if (!useEnergy) return true;
        if (energy < amount) return false;
        energy -= amount;
        return true;
    }

    // ===== Stars (awareness hooks) =====
    public void NotifyStarCollected(string starIdOrEmpty = "")
    {
        onStarCollected?.Invoke(starIdOrEmpty);
    }
}
