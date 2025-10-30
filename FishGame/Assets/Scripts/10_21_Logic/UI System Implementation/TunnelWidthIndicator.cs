public class TunnelWidthIndicator : MonoBehaviour 
{
    public Slider widthSlider;
    public Text widthText;
    public Text epCountText;
    public Text lobbyistCountText;
    
    public void UpdateTunnelWidth(float width, float minWidth, float maxWidth) 
    {
        widthSlider.minValue = minWidth;
        widthSlider.maxValue = maxWidth;
        widthSlider.value = width;
        
        widthText.text = $"Tunnel: {width:F1}m";
    }
    
    public void UpdateButtonCounts(int epHits, int lobbyistHits) 
    {
        epCountText.text = $"EP: {epHits}";
        lobbyistCountText.text = $"Lobby: {lobbyistHits}";
    }
}