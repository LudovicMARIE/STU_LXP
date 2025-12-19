using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [Header("Réglages")]
    [SerializeField] private int healAmount = 10;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();

            if (player != null)
            {
                player.Heal(healAmount);
                
                Destroy(gameObject);
            }
        }
    }
}