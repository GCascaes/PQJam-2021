using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField]
    private float MaxHealth;

    private float currentHealth;

    public void TakeDamage(float damage)
    {
        Debug.Log("Taking damage");
        currentHealth -= damage;
    }
}
