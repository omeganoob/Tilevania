using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField] private float _speed = 40f;
    private PlayerMovement _player;
    private float _direction;
    private float _lifeTime = 3;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _player = FindObjectOfType<PlayerMovement>();
        _direction = Mathf.Sign(_player.transform.rotation.y);
    }

    void Update()
    {
        _lifeTime -= Time.deltaTime;
        if(_lifeTime <= 0)
        {
            Destroy(gameObject);
        }
        _rb.velocity = new Vector2(1f, 0f) * _direction * _speed * (400 * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag.Equals("Enemy"))
        {
            Destroy(collision.gameObject);
        }
        Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
