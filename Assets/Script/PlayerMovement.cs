using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 _playerMovement;
    private Rigidbody2D _rb;
    private Vector2 _mVelocity = Vector2.zero;
    [SerializeField] private LayerMask _platformLayerMask;
    [SerializeField] private LayerMask _ememies;
    [SerializeField] private LayerMask _hazards;
    [SerializeField] private uint speed;
    [SerializeField] private uint jumpVel;
    [SerializeField] private int maxJumpVel;
    [SerializeField] private bool facingRight = true;
    private Animator _animator;
    private CapsuleCollider2D _collider2D;
    [SerializeField] private float _fallMultiplier = 5f;
    private float _gravity;
    private bool isAlive = true;
    #region cached
    private static readonly int IsRunning = Animator.StringToHash("isRunning");
    private static readonly int IsClimbing = Animator.StringToHash("isClimbing");
    private static readonly int IsDead = Animator.StringToHash("Dead");
    #endregion

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _collider2D = GetComponent<CapsuleCollider2D>();

        _gravity = _rb.gravityScale;
    }

    void Update()
    {
        if(!isAlive) return;
        Move();
        Climb();
        //For draw ray gizmo
        IsGrounded();
        Die();
    }

    private void FixedUpdate()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, Math.Min(_rb.velocity.y, maxJumpVel));
        JumpTweak();
    }

    void Move()
    {
        var velocity = _rb.velocity;
        var targetVelocity = new Vector2(_playerMovement.x * speed * Time.fixedDeltaTime, velocity.y);
        
        _rb.velocity = Vector2.SmoothDamp(velocity, targetVelocity, ref _mVelocity, .1f);

        Flip(_playerMovement.x);
        
        var isMoving = Mathf.Abs(_playerMovement.x) > Mathf.Epsilon;
        _animator.SetBool(IsRunning, isMoving);
    }

    void Climb()
    {
        if (!_collider2D.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            _rb.gravityScale = _gravity;
            _animator.SetBool(IsClimbing, false);
            return;
        }
        
        _rb.gravityScale = 0f;
        var velocity = _rb.velocity;
        var targetVelocity = new Vector2(velocity.x, _playerMovement.y * speed * Time.fixedDeltaTime);
        // _rb.velocity = Vector2.SmoothDamp(velocity, targetVelocity, ref _mVelocity, .1f);
        _rb.velocity = targetVelocity;
        var isClimbing = Mathf.Abs(_playerMovement.y) > Mathf.Epsilon;
        _animator.SetBool(IsClimbing, isClimbing);
    }

    void OnMove(InputValue value)
    {
        if (!isAlive) return;
        _playerMovement = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!isAlive) return;
        if (!value.isPressed || !IsGrounded()) return;
        _rb.velocity += Vector2.up * jumpVel;
    }

    private bool IsGrounded() {
        RaycastHit2D raycastHit = Physics2D.BoxCast(_collider2D.bounds.center, _collider2D.bounds.size * 0.8f /*smaller box to prevent wall jump*/, 0f, Vector2.down, .35f, _platformLayerMask);
        Color rayColor = raycastHit.collider != null ? Color.cyan : Color.yellow;
        DrawRayBox(rayColor, _collider2D);
        return raycastHit.collider != null;
    }

    private void DrawRayBox(Color rayColor, Collider2D _boxCollider) {
        //vertical rays
        Debug.DrawRay(_boxCollider.bounds.center + new Vector3(_boxCollider.bounds.extents.x * 0.8f, 0), Vector2.down * (_boxCollider.bounds.extents.y + 0.35f), rayColor);

        Debug.DrawRay(_boxCollider.bounds.center - new Vector3(_boxCollider.bounds.extents.x * 0.8f, 0), Vector2.down * (_boxCollider.bounds.extents.y + 0.35f), rayColor);
        //horizontal ray
        Debug.DrawRay(_boxCollider.bounds.center - new Vector3(_boxCollider.bounds.extents.x * 0.8f, _boxCollider.bounds.extents.y + 0.35f), Vector2.right * (_boxCollider.bounds.extents.x*0.8f * 2), rayColor);
    }

    void Flip(float hMove)
    {
        switch (hMove)
        {
            case > 0 when !facingRight:
            case < 0 when facingRight:
                facingRight = !facingRight;
                transform.Rotate(0f, 180f, 0f);
                break;
        }
    }

    void JumpTweak()
    {
        if (_rb.velocity.y < 0)
        {
            _rb.velocity += Vector2.up * Physics2D.gravity.y * (_fallMultiplier - 1) * Time.deltaTime;
        }
    }

    private void Die()
    {
        if (_collider2D.IsTouchingLayers(_ememies) || _collider2D.IsTouchingLayers(_hazards))
        {
            isAlive = false;
            _rb.AddForce(new Vector2(0f, 350f));
            _animator.SetTrigger(IsDead);
            GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
        }
    }
}
