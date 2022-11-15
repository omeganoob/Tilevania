using Unity.VisualScripting;
using UnityEngine;

public class EmenyMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    private Rigidbody2D _rigidbody;
    private CapsuleCollider2D _capsuleCollider;
    private BoxCollider2D _boxCollider;
    [SerializeField] private bool _facingRight;
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        _rigidbody.velocity = new Vector2(_speed, _rigidbody.velocity.y);
    }
    void OnTriggerExit2D(Collider2D other)
    {
        _speed *= -1;
        Flip(_speed);
    }
    private void Flip(float hMove)
    {
        switch (hMove)
        {
            case > 0 when !_facingRight:
            case < 0 when _facingRight:
                _facingRight = !_facingRight;
                transform.Rotate(0f, 180f, 0f);
                break;
        }
    }
}
