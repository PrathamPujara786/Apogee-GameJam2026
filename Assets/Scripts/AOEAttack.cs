using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class AoEAttack : MonoBehaviour
{
    [Header("AoE Settings")]
    public float attackRadius = 2.5f;

    [Header("Cooldown")]
    public float cooldownDuration = 2f;
    private float _cooldownTimer = 0f;
    public bool IsOnCooldown => _cooldownTimer > 0f;
    public float CooldownProgress => _cooldownTimer / cooldownDuration;

    [Header("Sterilization")]
    public float sterilizationPerHit = 0.05f;

    [Header("References")]
    public LayerMask enemyLayer;
    public Tilemap tilemap;
    public TileSwapper tileSwapper;

    private void Update()
    {
        if (_cooldownTimer > 0f)
            _cooldownTimer -= Time.deltaTime;
    }

    // This is what you assign in the Unity Event slot
    public void OnAOE(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;   // ignore Started and Canceled phases

        if (IsOnCooldown)
        {
            Debug.Log($"AoE on cooldown! {_cooldownTimer:F1}s remaining");
            return;
        }

        TriggerAoE();
    }

    private void TriggerAoE()
    {
        Debug.Log("TriggerAoE called!");

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRadius, enemyLayer);
        foreach (var hit in hits)
            hit.GetComponent<EnemyHealth>()?.TakeDamage(1);

        tileSwapper.SwapTilesInRadius(transform.position, attackRadius);

        GameState.Instance.AddSterilization(sterilizationPerHit * Mathf.Max(1, hits.Length));

        _cooldownTimer = cooldownDuration;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = IsOnCooldown
            ? new Color(0.5f, 0.5f, 0.5f, 0.4f)
            : new Color(1f, 0.3f, 0f, 0.4f);
        Gizmos.DrawSphere(transform.position, attackRadius);
    }
}