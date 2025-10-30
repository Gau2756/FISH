public class DebugManager : MonoBehaviour
{
    public bool debugMode = false;

    void Update()
    {
        if (!debugMode) return;

        // Press keys for testing
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Add energy
            FindObjectOfType<FishStats>().RestoreEnergy(50);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // Add life
            FindObjectOfType<FishStats>().currentLives++;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            // Widen tunnel
            FindObjectOfType<TunnelController>().currentWidth += 0.5f;
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            // Reduce current
            FindObjectOfType<WaterCurrent>().currentStrength *= 0.5f;
        }
    }

    void OnGUI()
    {
        if (debugMode)
        {
            GUI.Label(new Rect(10, 10, 200, 20), $"Energy: {FindObjectOfType<FishStats>().currentEnergy}");
            GUI.Label(new Rect(10, 30, 200, 20), $"Current: {FindObjectOfType<WaterCurrent>().currentStrength}");
            GUI.Label(new Rect(10, 50, 200, 20), $"Tunnel: {FindObjectOfType<TunnelController>().currentWidth}");
        }
    }
}

    