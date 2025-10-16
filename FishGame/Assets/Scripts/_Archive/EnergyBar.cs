using UnityEngine;
using UnityEngine.UI;
public class EnergyBar : MonoBehaviour
{
    public Slider slider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void setEnergy(int energy)
    {
        slider.value = energy;
    }
    public void setMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
