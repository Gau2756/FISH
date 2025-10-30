public class EnergyBar : MonoBehaviour 
{
    public Image fillImage;
    public Text energyText;
    
    public void UpdateEnergy(float currentEnergy, float maxEnergy) 
    {
        float fillAmount = currentEnergy / maxEnergy;
        fillImage.fillAmount = fillAmount;
        
        energyText.text = $"{currentEnergy:F0} / {maxEnergy:F0}";
        
        // Color code: green → yellow → red
        fillImage.color = Color.Lerp(Color.red, Color.green, fillAmount);
    }
}