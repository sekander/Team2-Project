using UnityEngine;

public class BulletController : MonoBehaviour
{

    [SerializeField] private float lifespan = 5.0f;
    private float elapsedTime;
    private float detectionRadius = 0.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        if(elapsedTime > lifespan)
            Destroy(gameObject); // Destroys the bullet on any collision 

        // Check for collisions using a raycast
        // RaycastHit hit;
        // if (Physics.Raycast(transform.position, transform.forward, out hit, speed * Time.deltaTime))
        // if (Physics.Raycast(transform.position, transform.forward, out hit, speed * Time.deltaTime))
        // {
        //     Debug.Log("Bullet hit: " + hit.collider.gameObject.name);
        //     Destroy(gameObject); // Destroy the bullet when a collision is detected
        // }
             // Check for objects in the detection radius
        // Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
        // foreach (var hitCollider in hitColliders)
        // {
        //     if (hitCollider.gameObject != gameObject)
        //     {
        //         Debug.Log("Bullet hit: " + hitCollider.gameObject.name);
        //         Destroy(gameObject); // Destroy the bullet when it hits an object
        //         break;
        //     }
        // }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Bullet collided with: " + collision.gameObject.name);
        if (collision.gameObject != null)
        {
            Destroy(gameObject); // Destroys the bullet on any collision
        }
    }

    // void OnCollisionEnter(Collision collision)
    // {
    //     Debug.Log("Bullet collided with: " + collision.gameObject.name);
    //     // Destroy(gameObject); // Destroy the bullet upon collision
    // }

    // void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.gameObject.CompareTag("Wall"))
    //     {
    //         Debug.Log("Wall hit");
    //         // isGrounded = true;
    //     }
    
    // void OnTriggerEnter(Collider other)
    // {
    //     Debug.Log("Bullet triggered with: " + other.gameObject.name);
    //     // Destroy(gameObject);
    // }
}
