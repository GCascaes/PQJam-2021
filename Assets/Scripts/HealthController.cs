using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField]
    private float MaxHealth;

    private float currentHealth;

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }
}
