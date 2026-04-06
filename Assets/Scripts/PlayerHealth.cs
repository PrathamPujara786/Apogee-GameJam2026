using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    private float _currentHealth;

    public float CurrentHealth => _currentHealth;
    public float MaxHealth => maxHealth;
    public bool IsDead => _currentHealth <= 0f;

    private void Awake()
    {
        _currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (IsDead) return;
        _currentHealth = Mathf.Max(0f, _currentHealth - amount);

        if (IsDead)
            HandleDeath();
    }

    private void HandleDeath()
    {
        Debug.Log("Player died.");
        // Hook up death screen later
    }
}