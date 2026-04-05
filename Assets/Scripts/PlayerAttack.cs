using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("Gun Settings")]
    public float fireRate = 0.2f;
    public int damage = 15;
    public float bulletSpeed = 20f;
    public float bulletLifetime = 2f;

    [Header("References")]
    public GameObject bulletPrefab;
    public Transform gunPivot;       // Drag GunPivot here
    public Transform firePoint;      // Drag FirePoint here
   // public Animator animator;

    private PlayerInputActions inputActions;
    private float nextFireTime;

    void Awake() => inputActions = new PlayerInputActions();

    void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.PrimaryAttack.performed += OnFire;
    }

    void OnDisable()
    {
        inputActions.Player.PrimaryAttack.performed -= OnFire;
        inputActions.Player.Disable();
    }
    void OnFire(InputAction.CallbackContext ctx) => Shoot();
    void Update()
    {
        AimGunAtMouse();
            
    }
 
    void AimGunAtMouse()
    {
        Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(
            Mouse.current.position.ReadValue());

        Vector2 dir = (mouseWorld - (Vector2)transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        gunPivot.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    void Shoot()
    {
        if (Time.time < nextFireTime) return;
        nextFireTime = Time.time + fireRate;

        GameObject bullet = Instantiate(
            bulletPrefab, firePoint.position, firePoint.rotation);

        if (bullet.TryGetComponent<Bullet>(out var b))
            b.Init(damage, bulletSpeed, bulletLifetime);

        //animator?.SetTrigger("Shoot");
    }
}