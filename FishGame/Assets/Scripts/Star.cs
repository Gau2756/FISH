using UnityEngine;

public class Star : MonoBehaviour
{
    [SerializeField] private float timeUntilLobbyistGets = 10f;
    private float timer = 0f;
    [SerializeField] private float energyGain = 20f;
    [SerializeField] private float strengthGain = 10f;

    [SerializeField] private WaterCurrent waterCurrent;

    private bool claimed = false;

    private void Start()
    {
        timer = 0f;
    }

    private void Update()
    {
        if (claimed) return;
        timer += Time.deltaTime;
        if (timer >= timeUntilLobbyistGets)
        {
            LobbyistGetsStar();
        }
    }

    private void LobbyistGetsStar()
    {
        claimed = true;
        // show evil policy text - placeholder
        Debug.Log("Lobbyist claimed the star and activated an evil policy!");

        // increase difficulty: increase lobbyist effect on water current
        if (waterCurrent != null)
        {
            waterCurrent.IncreaseLobbyistEffect(5f);
        }

        // destroy star
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (claimed) return;
        if (collision.CompareTag("Fish"))
        {
            claimed = true;
            FishStats stats = collision.GetComponent<FishStats>();
            if (stats != null)
            {
                stats.EatBagel(energyGain);
                stats.IncreaseStrength(strengthGain);
            }
            Destroy(gameObject);
        }
    }
}
