using UnityEngine;

[System.Serializable]
public class FishStats {
    public int lives = 3;
    public float energy = 100f;
    public float maxEnergy = 100f;
    public float strength = 12f;

    public bool SpendEnergy(float amt) {
        if (energy < amt) return false;
        energy -= amt; return true;
    }
    public void AddEnergy(float amt) => energy = Mathf.Min(maxEnergy, energy + amt);
}
