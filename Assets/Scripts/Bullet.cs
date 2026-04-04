using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int damage;
    private Rigidbody2D rb;

    void Awake() => rb = GetComponent<Rigidbody2D>();

    public void Init(int damage, float speed, float lifetime)
    {
        this.damage = damage;
        rb.linearVelocity = transform.right * speed;
        StartCoroutine(DestroyAfter(lifetime));
    }

    IEnumerator DestroyAfter(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            if (col.gameObject.TryGetComponent<IDamageable>(out var d))
                d.TakeDamage(damage);
        }

        // Destroy bullet on anything it physically hits (walls, enemies, etc.)
        // but the wall itself is untouched
        Destroy(gameObject);
    }
}