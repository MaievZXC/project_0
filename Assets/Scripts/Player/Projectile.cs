using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    private float direction;
    private bool hit;
    private float lifetime;

    private Animator anim;
    private BoxCollider2D boxCollider;
    private Rigidbody2D rigidbody;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        rigidbody = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        //transform.Translate(movementSpeed, 0, 0);

        lifetime += Time.deltaTime;
        if (lifetime > 5) Deactivate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "Enemy")
        {
            hit = true;
            collision.GetComponent<PlayerHealth>()?.TakeDamage(1);
        }
        if (collision.tag == "Wall")
        {
            hit = true;
            Deactivate();
        }
    }
    public void SetDirection(float _direction)
    {
        lifetime = 0;
        direction = _direction;
        gameObject.SetActive(true);
        hit = false;
        boxCollider.enabled = true;

        float localScaleX = transform.localScale.x;
        rigidbody.velocity = new Vector2(speed * Mathf.Sign(_direction), 0);
        if (Mathf.Sign(localScaleX) != _direction)
            localScaleX = -localScaleX;
 

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }
    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}