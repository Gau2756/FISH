using UnityEngine;

[ExecuteAlways]
public class Tunnel2DColliderAdapter : MonoBehaviour
{
    [Header("Dam Dimensions")]
    public float damWidth = 12f;
    public float damHeight = 2f;
    public float damY = 0f;

    [Header("Tunnel Placement")]
    public float tunnelCenterX = 0f;

    [Header("Colliders (assign in Inspector)")]
    public BoxCollider2D leftWall;    // tag: DamWall
    public BoxCollider2D rightWall;   // tag: DamWall
    public BoxCollider2D tunnelTrigger; // isTrigger = true, tag: Tunnel

    [Header("Visuals (optional)")]
    public Transform damVisual;   // stretched sprite for dam bar
    public Transform tunnelMask;  // sprite/quad to visualize the gap

    [SerializeField] private float _tunnelWidth = 3f;

    void Reset()
    {
        if (!leftWall) leftWall = transform.Find("LeftWall")?.GetComponent<BoxCollider2D>();
        if (!rightWall) rightWall = transform.Find("RightWall")?.GetComponent<BoxCollider2D>();
        if (!tunnelTrigger) tunnelTrigger = transform.Find("TunnelTrigger")?.GetComponent<BoxCollider2D>();
        if (!damVisual) damVisual = transform.Find("DamVisual");
        if (!tunnelMask) tunnelMask = transform.Find("TunnelMask");
    }

    void OnValidate()
    {
        damWidth = Mathf.Max(0.1f, damWidth);
        damHeight = Mathf.Max(0.1f, damHeight);
        _tunnelWidth = Mathf.Clamp(_tunnelWidth, 0.05f, damWidth - 0.05f);
        ApplyGeometry();
    }

    /// <summary>
    /// Called by TunnelBalanceController.OnWidthChanged event.
    /// </summary>
    public void SetTunnelWidth(float width)
    {
        _tunnelWidth = Mathf.Clamp(width, 0.05f, damWidth - 0.05f);
        ApplyGeometry();
    }

    public void SetTunnelCenterX(float x)
    {
        tunnelCenterX = x;
        ApplyGeometry();
    }

    private void ApplyGeometry()
    {
        float halfDam = damWidth * 0.5f;
        float halfTunnel = _tunnelWidth * 0.5f;

        // Keep gap inside dam bounds
        tunnelCenterX = Mathf.Clamp(tunnelCenterX, -halfDam + halfTunnel, halfDam - halfTunnel);

        float leftWidth = Mathf.Max(0f, (tunnelCenterX - halfTunnel) - (-halfDam));
        float rightWidth = Mathf.Max(0f, (halfDam) - (tunnelCenterX + halfTunnel));

        if (leftWall)
        {
            leftWall.size = new Vector2(leftWidth, damHeight);
            leftWall.offset = new Vector2(-halfDam + leftWidth * 0.5f, damY);
        }

        if (rightWall)
        {
            rightWall.size = new Vector2(rightWidth, damHeight);
            rightWall.offset = new Vector2(tunnelCenterX + halfTunnel + rightWidth * 0.5f, damY);
        }

        if (tunnelTrigger)
        {
            tunnelTrigger.isTrigger = true;
            tunnelTrigger.size = new Vector2(_tunnelWidth, damHeight);
            tunnelTrigger.offset = new Vector2(tunnelCenterX, damY);
        }

        if (damVisual)
        {
            damVisual.localPosition = new Vector3(0f, damY, 0f);
            damVisual.localScale = new Vector3(damWidth, damHeight, 1f);
        }

        if (tunnelMask)
        {
            tunnelMask.localPosition = new Vector3(tunnelCenterX, damY, 0f);
            tunnelMask.localScale = new Vector3(_tunnelWidth, damHeight, 1f);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireCube(new Vector3(0f, damY, 0f), new Vector3(damWidth, damHeight, 0.01f));
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(new Vector3(tunnelCenterX, damY, 0f), new Vector3(_tunnelWidth, damHeight, 0.01f));
    }
}
