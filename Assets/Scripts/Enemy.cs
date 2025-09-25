using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;

    // Patrol between two points
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float speed = 2f;

    private Transform targetPoint;
    // private Transform targetPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        // spriteRenderer = GetComponent<SpriteRenderer>();
        movement = new Vector2(1f, 0f); // Initial movement direction
        targetPoint = pointB;
    }

    // Update is called once per frame
    void Update()
    {
        bool isWalking = movement.x != 0f;
        animator.SetBool("isWalking", isWalking);
    }


    private void FixedUpdate()
    {
        if (pointA == null || pointB == null) return;

             // Move towards the current target point
        Vector2 targetPos = new Vector2(targetPoint.position.x, rb.position.y);
        Vector2 direction = (targetPos - rb.position).normalized;

        rb.linearVelocity = new Vector2(direction.x * speed, rb.linearVelocity.y);

        // Check if close enough to switch direction
        if (Mathf.Abs(rb.position.x - targetPoint.position.x) < 0.05f)
        {
            targetPoint = (targetPoint == pointA) ? pointB : pointA;
        }

        // Flip sprite based on direction
        if (rb.linearVelocity.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * Mathf.Sign(rb.linearVelocity.x);
            transform.localScale = scale;
        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Alien collided with: " + collision.gameObject.name);
        if (collision.gameObject != null)
        {
            Destroy(gameObject); // Destroys the bullet on any collision
        }
    }

    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     if (collision.CompareTag("Player"))
    //     {
    //         // Handle collision with player
    //         Debug.Log("Enemy collided with Player");
    //     }
    //     else if (collision.CompareTag("Bullet"))
    //     {
    //         // Handle collision with bullet
    //         Debug.Log("Enemy collided with Bullet");
    //         Destroy(collision.gameObject); // Destroy the bullet
    //         Destroy(gameObject); // Destroy the enemy
    //     }
    // }

}
