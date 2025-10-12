using UnityEngine;

public class TunnelController : MonoBehaviour
{
    [SerializeField] private float minWidth = 0.5f;
    [SerializeField] private float maxWidth = 5f;
    [SerializeField] private float currentWidth = 1f;

    [Header("Hits")]
    [SerializeField] private int epHits = 0;
    [SerializeField] private int lobbyistHits = 0;

    public float CurrentWidth => currentWidth;

    private void Start()
    {
        UpdateTunnelWidth();
    }

    public void EPButtonPress()
    {
        epHits++;
        UpdateTunnelWidth();
    }

    public void LobbyistButtonPress()
    {
        lobbyistHits++;
        UpdateTunnelWidth();
    }

    public void SetHits(int ep, int lobbyist)
    {
        epHits = ep;
        lobbyistHits = lobbyist;
        UpdateTunnelWidth();
    }

    public void UpdateTunnelWidth()
    {
        float diff = epHits - lobbyistHits;
        // map diff to width; here each hit changes width by 0.5 units
        currentWidth = Mathf.Clamp(1f + diff * 0.5f, minWidth, maxWidth);
        // optional: update any visual (scale) if a child collider/visual exists
        Transform visual = transform.Find("Visual");
        if (visual != null)
        {
            Vector3 s = visual.localScale;
            s.x = currentWidth;
            visual.localScale = s;
        }
    }
}
