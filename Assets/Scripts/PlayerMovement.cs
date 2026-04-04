using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 200f;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 600f;
    [SerializeField] private float dashDuration = 0.18f;
    [SerializeField] private float dashCooldown = 0.8f;

    private Rigidbody2D _rb;

    private Vector2 _moveInput;
    private Vector2 _lastMoveDirection = Vector2.right;

    private bool _isDashing;
    private float _dashTimer;
    private float _cooldownTimer;
    private Vector2 _dashDirection;

    public bool IsDashing => _isDashing;
    public Vector2 FacingDirection => _lastMoveDirection;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0f;
        _rb.freezeRotation = true;
    }

    private void Update()
    {
        if (_moveInput != Vector2.zero)
            _lastMoveDirection = _moveInput.normalized;

        if (_dashTimer > 0f)
        {
            _dashTimer -= Time.deltaTime;
            if (_dashTimer <= 0f)
                _isDashing = false;
        }

        if (_cooldownTimer > 0f)
            _cooldownTimer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (_isDashing)
        {
            float progress = 1f - (_dashTimer / dashDuration);
            float eased = 1f - (1f - progress) * (1f - progress);
            _rb.linearVelocity = _dashDirection * dashSpeed * (1f - eased * 0.6f);
        }
        else
        {
            _rb.linearVelocity = _moveInput.normalized * moveSpeed * Time.fixedDeltaTime;
        }
    }


    public void OnMove(InputAction.CallbackContext ctx)
    {
        _moveInput = ctx.ReadValue<Vector2>();
    }

    public void OnDash(InputAction.CallbackContext ctx)
    {
        // Only fire on button press, not release
        if (!ctx.performed) return;
        if (_isDashing || _cooldownTimer > 0f) return;

        _isDashing = true;
        _dashTimer = dashDuration;
        _cooldownTimer = dashCooldown;

        _dashDirection = _moveInput != Vector2.zero
            ? _moveInput.normalized
            : _lastMoveDirection;
    }
}