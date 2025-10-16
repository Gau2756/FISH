using UnityEngine;
using UnityEngine.Events;

public class TunnelBalanceController : MonoBehaviour
{
    [Header("Click Weights (score = P*Pweight - L*Lweight)")]
    [Tooltip("How many score units 1 Protectionist click is worth.")]
    public float protectionistWeight = 1f;

    [Tooltip("How many score units 1 Lobbyist click is worth.")]
    public float lobbyistWeight = 1f;

    [Header("Width Mapping")]
    [Tooltip("Minimum physical width of the migration tunnel (world units).")]
    public float minTunnelWidth = 0.5f;

    [Tooltip("Maximum physical width of the migration tunnel (world units).")]
    public float maxTunnelWidth = 6f;

    [Tooltip("Score window used to normalize score → width. "
           + "If |score| >= scoreWindow, width clamps at min/max.")]
    public float scoreWindow = 10f;

    [Header("Smoothing")]
    [Tooltip("How quickly the tunnel interpolates to the new width.")]
    public float widthLerpSpeed = 6f;

    [Header("Events")]
    public UnityEvent<float> OnWidthChanged; // float = new width

    [Header("Debug (readonly)")]
    [SerializeField] private int protectionistClicks;
    [SerializeField] private int lobbyistClicks;
    [SerializeField] private float currentWidth;

    private float _targetWidth;

    void Awake()
    {
        currentWidth = Mathf.Clamp((minTunnelWidth + maxTunnelWidth) * 0.5f, minTunnelWidth, maxTunnelWidth);
        _targetWidth = currentWidth;
        OnWidthChanged?.Invoke(currentWidth);
    }

    void Update()
    {
        float score = protectionistClicks * protectionistWeight - lobbyistClicks * lobbyistWeight;

        // Normalize score to [0..1] across [-scoreWindow .. +scoreWindow]
        float t = 0.5f;
        if (scoreWindow > 0.0001f)
        {
            float clamped = Mathf.Clamp(score, -scoreWindow, scoreWindow);
            t = Mathf.InverseLerp(-scoreWindow, scoreWindow, clamped);
        }

        _targetWidth = Mathf.Lerp(minTunnelWidth, maxTunnelWidth, t);
        float newWidth = Mathf.Lerp(currentWidth, _targetWidth, Time.deltaTime * widthLerpSpeed);

        // Emit only if changed meaningfully (avoids noisy events).
        if (Mathf.Abs(newWidth - currentWidth) > 0.0001f)
        {
            currentWidth = newWidth;
            OnWidthChanged?.Invoke(currentWidth);
        }

        // Optional quick keyboard test (delete if not needed):
        if (Input.GetKeyDown(KeyCode.Alpha1)) RegisterProtectionistClick();
        if (Input.GetKeyDown(KeyCode.Alpha2)) RegisterLobbyistClick();
    }

    // Hook these to UI Buttons
    public void RegisterProtectionistClick() => protectionistClicks++;
    public void RegisterLobbyistClick() => lobbyistClicks++;

    // Optional: programmatic adds (e.g., from other systems)
    public void AddProtectionistClicks(int amount) => protectionistClicks += Mathf.Max(0, amount);
    public void AddLobbyistClicks(int amount) => lobbyistClicks += Mathf.Max(0, amount);

    public (int p, int l, float width) GetDebugState() => (protectionistClicks, lobbyistClicks, currentWidth);

    // Convenience presets (call from a dropdown or debug menu if you like)
    public void SetRatio_1P_Equals_1L() { protectionistWeight = 1f; lobbyistWeight = 1f; }
    public void SetRatio_1P_Equals_2L() { protectionistWeight = 2f; lobbyistWeight = 1f; } // 2 L = 1 P
    public void SetRatio_1P_Equals_3L() { protectionistWeight = 3f; lobbyistWeight = 1f; } // 3 L = 1 P
}
