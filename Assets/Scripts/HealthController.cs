using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField]
    private float MaxHealth;
    [SerializeField]
    private bool shouldDie;

    private float currentHealth;

    private void Awake()
    {
        currentHealth = MaxHealth;
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("Taking damage");
        currentHealth -= damage;
        if (shouldDie && currentHealth <= 0)
            Destroy(gameObject);
    }
}
