using System.Collections;
using UnityEngine;

public class HealingItem : MonoBehaviour
{
    public float healingAmount = 15f;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                if (playerHealth.currentHealth < PersistentValues.instance.GetMaxHealth())
                {
                    playerHealth.Heal(healingAmount);
                    Destroy(gameObject);
                }
            }
            else
            {
                Debug.LogError("No PlayerHealth component found on the player object.");
            }
        }
    }

}
