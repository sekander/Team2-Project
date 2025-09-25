using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] public float lifespan = 5.0f;
    private float elapsedTime;

    [SerializeField] public int itemScoreValue = 0;
    // [SerializeField] private PlayerPrefs playerPrefs;

    
    [SerializeField] public AudioClip pickupSound;
    [SerializeField] private AudioSource audioSource;

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         if (pickupSound != null && audioSource != null)
    //         {
    //             audioSource.PlayOneShot(pickupSound);
    //         }
    //         // Add score logic here if needed
    //         Destroy(gameObject);
    //     }
    // }
    void Update()
    {
        elapsedTime += Time.deltaTime;
        // This method is called once per frame
        // You can add your update logic here

        if (elapsedTime >= lifespan)
        {
            // Destroy the pickup object after its lifespan
            Destroy(gameObject);
        }   
    }
    
}
