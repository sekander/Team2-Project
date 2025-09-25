using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    // Movement
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private TMPro.TextMeshProUGUI moveSpeedText;
    [SerializeField] private UnityEngine.UI.Slider moveSpeedSlider;

    // Jumping
    [Header("Jumping Settings")]
    [SerializeField] private float currentJumpCharge = 0f;
    [SerializeField] private TMPro.TextMeshProUGUI currentJumpChargeSlider;

    [Range(2.0f, 10.0f)]
    [SerializeField] private float MAXJUMP_CHARGE = 2.5f;
    [SerializeField] private TMPro.TextMeshProUGUI maxJumpChargeText;
    [SerializeField] private UnityEngine.UI.Slider maxJumpChargeSlider;

    [Range(1.0f, 10.0f)]
    [SerializeField] private float jumpChargeRate = 1f;
    [SerializeField] private TMPro.TextMeshProUGUI jumpChargeRateText;
    [SerializeField] private UnityEngine.UI.Slider jumpChargeRateSlider;

    [Range(2.0f, 10.0f)]
    [SerializeField] private float jumpSpeed = 0f;
    [SerializeField] private TMPro.TextMeshProUGUI jumpSpeedText;
    [SerializeField] private UnityEngine.UI.Slider jumpSpeedSlider;

    [Range(0.3f, 5.0f)]
    [SerializeField] private float jumpPeak = 1.0f;
    [SerializeField] private TMPro.TextMeshProUGUI jumpPeakText;
    [SerializeField] private UnityEngine.UI.Slider jumpPeakSlider;

    [Range(0.0f, 0.9f)]
    [SerializeField] private float jumpPeakGravityScale = 0.5f;
    [SerializeField] private TMPro.TextMeshProUGUI jumpPeakGravityScaleText;
    [SerializeField] private UnityEngine.UI.Slider jumpPeakGravityScaleSlider;

    private bool jumpCharging = false;
    private bool jumpRequested = false;
    private bool isGrounded = false;
    float maxHeight = 3.0f;

    // Shooting
    [Header("Shooting Settings")]
    [SerializeField] private GameObject squarePrefab;
    [SerializeField] private GameObject redAmmoPrefab;
    [SerializeField] private GameObject blueAmmoPrefab;
    [SerializeField] private GameObject yellowAmmoPrefab;

    [Range(0.0f, 2.0f)]
    [SerializeField] private float shootDelay = 0.5f;
    [SerializeField] private TMPro.TextMeshProUGUI shootDelayText;
    [SerializeField] private UnityEngine.UI.Slider shootDelaySlider;

    [Range(1.0f, 20.0f)]
    [SerializeField] private float launchForce = 5f;
    [SerializeField] private TMPro.TextMeshProUGUI launchForceText;
    [SerializeField] private UnityEngine.UI.Slider launchForceSlider;

    [SerializeField] private GameObject rightBulletSpawnPosition;
    [SerializeField] private GameObject leftBulletSpawnPosition;

    // private const int MAXAMMO = 30;
    // private Queue<int> ammoQueue = new Queue<int>();

    private float capturedBulletTime = 0.0f;

    public AmmoManager ammoManager;
    [SerializeField] private TMPro.TextMeshProUGUI redAmmoValueText;
    [SerializeField] private TMPro.TextMeshProUGUI blueAmmoValueText;
    [SerializeField] private TMPro.TextMeshProUGUI yellowAmmoValueText;

    [Range(1.0f, 60.0f)]
    private float ammoLifespan = 15.0f;
    [SerializeField] private UnityEngine.UI.Slider ammoLifespanSlider;
    [SerializeField] private TMPro.TextMeshProUGUI AmmoLifeSpanValueText;

    // Score Logic
    [SerializeField] private TMPro.TextMeshProUGUI ScoreValueText;

    // UI Button Test
    [Header("UI Button Test")]
    [SerializeField] private UnityEngine.UI.Button testButton;
    [SerializeField] private UnityEngine.UI.Button redButton;
    [SerializeField] private UnityEngine.UI.Button blueButton;
    [SerializeField] private UnityEngine.UI.Button yellowButton;
    // Action listeners for ammo buttons


    public GameObject AmmoUIContainer; // Reference to the AmmoUI container
    private void OnRedButtonClicked()
    {
        SpawnAmmoBox(redAmmoPrefab);
    }

    private void OnBlueButtonClicked()
    {
        SpawnAmmoBox(blueAmmoPrefab);
    }

    private void OnYellowButtonClicked()
    {
        SpawnAmmoBox(yellowAmmoPrefab);
    }

    // private void OnEnable()
    // {
    //     if (redButton != null)
    //         redButton.onClick.AddListener(OnRedButtonClicked);
    //     if (blueButton != null)
    //         blueButton.onClick.AddListener(OnBlueButtonClicked);
    //     if (yellowButton != null)
    //         yellowButton.onClick.AddListener(OnYellowButtonClicked);
    // }

    // private void OnDisable()
    // {
    //     if (redButton != null)
    //         redButton.onClick.RemoveListener(OnRedButtonClicked);
    //     if (blueButton != null)
    //         blueButton.onClick.RemoveListener(OnBlueButtonClicked);
    //     if (yellowButton != null)
    //         yellowButton.onClick.RemoveListener(OnYellowButtonClicked);
    // }
    private void Awake()
    {
        if (testButton != null)
        {
            testButton.onClick.AddListener(OnTestButtonClicked);
        }
        if (redButton != null)
        {
            redButton.onClick.AddListener(OnRedButtonClicked);
        }
        if (blueButton != null)
        {
            blueButton.onClick.AddListener(OnBlueButtonClicked);
        }
        if (yellowButton != null)
        {
            yellowButton.onClick.AddListener(OnYellowButtonClicked);
        }
    }

    private void SpawnAmmoBox(GameObject ammoPrefab)
    {
        Vector3 spawnPos = transform.position + Vector3.right * 1.5f;
        GameObject ammoBox = Instantiate(ammoPrefab, spawnPos, Quaternion.identity);
        // Access the Pickup component on the spawned prefab
        Pickup pickup = ammoBox.GetComponent<Pickup>();
        pickup.lifespan = ammoLifespan; // Set the lifespan of the pickup
    }  

    private void OnTestButtonClicked()
    {
        Debug.Log("Test Button Clicked!");
        score += 10000; // Increment score by 100 when the button is clicked
        // UpdateUI();
        // You can add more logic here for testing
    }

    private int score = 0;

    private AmmoColor? TryFire()
    {
        var method = typeof(AmmoManager).GetMethod("TryConsumeAmmo", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        // If you have an AmmoUI instance, call RefreshAmmoUI() on it, e.g.:
        AmmoUIContainer.GetComponent<AmmoUI>().RefreshAmmoUI(); // Uncomment and use your actual AmmoUI instance

        // Or, if RefreshAmmoUI should be static, make sure it's declared as static in AmmoUI:
        // public static void RefreshAmmoUI() { ... }
        return (AmmoColor?)method.Invoke(ammoManager, null);
    }


    private void UpdateUI()
    {
        currentJumpChargeSlider.text = currentJumpCharge.ToString("F2");
        moveSpeedText.text = moveSpeed.ToString("F2");
        maxJumpChargeText.text = MAXJUMP_CHARGE.ToString("F2");
        jumpChargeRateText.text = jumpChargeRate.ToString("F2");
        jumpSpeedText.text = jumpSpeed.ToString("F2");
        jumpPeakText.text = jumpPeak.ToString("F2");
        jumpPeakGravityScaleText.text = jumpPeakGravityScale.ToString("F2");
        shootDelayText.text = shootDelay.ToString("F2");
        launchForceText.text = launchForce.ToString("F2");

        redAmmoValueText.text = ammoManager.GetAmmoCountByColor(AmmoColor.Red).ToString();
        blueAmmoValueText.text = ammoManager.GetAmmoCountByColor(AmmoColor.Blue).ToString();
        yellowAmmoValueText.text = ammoManager.GetAmmoCountByColor(AmmoColor.Yellow).ToString();

        AmmoLifeSpanValueText.text = ammoLifespan.ToString("F2");

        ScoreValueText.text = score.ToString();






        if (moveSpeedSlider != null)
            // moveSpeedSlider.value = moveSpeed;
            moveSpeed = moveSpeedSlider.value;
        if (currentJumpChargeSlider != null) currentJumpChargeSlider.text = currentJumpCharge.ToString("F2");


        if (maxJumpChargeSlider != null)
            // maxJumpChargeSlider.value = MAXJUMP_CHARGE;
            MAXJUMP_CHARGE = maxJumpChargeSlider.value;
        if (jumpChargeRateSlider != null)
            // jumpChargeRateSlider.value = jumpChargeRate;
            jumpChargeRate = jumpChargeRateSlider.value;
        if (jumpSpeedSlider != null)
            // jumpSpeedSlider.value = jumpSpeed;
            jumpSpeed = jumpSpeedSlider.value;
        if (jumpPeakSlider != null)
            // jumpPeakSlider.value = jumpPeak;
            jumpPeak = jumpPeakSlider.value;
        if (jumpPeakGravityScaleSlider != null)
            // jumpPeakGravityScaleSlider.value = jumpPeakGravityScale;
            jumpPeakGravityScale = jumpPeakGravityScaleSlider.value;
        if (shootDelaySlider != null)
            // shootDelaySlider.value = shootDelay;
            shootDelay = shootDelaySlider.value;
        if (launchForceSlider != null)
            // launchForceSlider.value = launchForce;
            launchForce = launchForceSlider.value;

        if (ammoLifespanSlider != null)
            // launchForceSlider.value = launchForce;
            ammoLifespan = ammoLifespanSlider.value;
    }

    // private void Start()
    // {
    //     UpdateUI();
    // }

    // private void Update()
    // {
    //     UpdateUI();
    // }




    private float elapsedTime = 0.0f;   // Accumulated time since script start



    //Player Components
    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        rb.gravityScale = 1f; // No gravity
        UpdateUI();
    }


    void Update()
    {
        elapsedTime += Time.deltaTime;
        UpdateUI();

        // Get movement input
        movement.x = Input.GetAxisRaw("Horizontal");
        // movement.y = Input.GetAxisRaw("Vertical");
        // Set animator bool
        bool isWalking = movement != Vector2.zero;
        animator.SetBool("isWalking", isWalking);

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // Debug.Log("Mouse World Position: " + mouseWorldPos);

        // Flip sprite if moving left/right
        if (movement.x != 0)
        {
            spriteRenderer.flipX = movement.x < 0;
        }
        // else if(rb.position.x < mouseWorldPos.x)
        else if (rb.position.x < mouseWorldPos.x && Input.GetMouseButton(0))
        {
            spriteRenderer.flipX = false;
        }
        else if (rb.position.x > mouseWorldPos.x && Input.GetMouseButton(0))
        {
            spriteRenderer.flipX = true;
        }
        // else
        // {
        //     spriteRenderer.flipX = true;
        // }


        if (Input.GetKeyDown(KeyCode.H))
        {
            score += 2000; // Increment score by 10 when H is presse`d
        }

        // Jumping logic

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                // jumpForce = 0f;
                currentJumpCharge = 0f;
                jumpCharging = true;
            }

        if (Input.GetKey(KeyCode.Space) && jumpCharging)
        {
            // Increase jump force while holding the jump button
            // jumpForce += jumpChargeRate * Time.deltaTime;
            currentJumpCharge += jumpChargeRate * Time.deltaTime;
            // Debug.Log("Jumping Charge: " + jumpForce);

            // if(jumpForce > MAXJUMP)
            if (currentJumpCharge > MAXJUMP_CHARGE)
            {
                // jumpForce = MAXJUMP;
                currentJumpCharge = MAXJUMP_CHARGE;
                jumpCharging = false;
                jumpRequested = true;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space) && jumpCharging)
        {
            // Jump when the button is released
            jumpCharging = false;
            jumpRequested = true;
            // Debug.Log("Jumping Released: " + jumpForce);
            Debug.Log("Jumping Released: " + currentJumpCharge);
        }


        if (movement.x == 0 && isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetMouseButtonDown(0))
            {

                if (elapsedTime - capturedBulletTime < shootDelay)
                {
                    // Debug.Log("Shoot Delay: " + (elapsedTime - capturedBulletTime));
                    return; // Exit if the delay hasn't passed
                }
                // float xoffset = 1.25f;
                if (spriteRenderer.flipX)
                {
                    AmmoColor? ammoUsed = TryFire();
                    if (ammoUsed != null)
                    {
                        Debug.Log("Fired a " + ammoUsed.Value + " bullet!");
                        Debug.Log("Ammo Left: " + ammoManager.CurrentAmmo);
                        // Optional: use the ammo color to change the bullet appearance
                        GameObject square = Instantiate(squarePrefab, new Vector2(leftBulletSpawnPosition.transform.position.x + 1.125f, leftBulletSpawnPosition.transform.position.y + 0.225f), Quaternion.identity);
                        // squarePrefab.GetComponent<SpriteRenderer>().color = Color.blue;    

                        // Apply color to the instantiated object (not the prefab!)
                        SpriteRenderer sr = square.GetComponentInChildren<SpriteRenderer>();

                        if (sr != null)
                        {
                            if (ammoUsed.Value == AmmoColor.Blue)
                                sr.color = Color.blue;
                            else if (ammoUsed.Value == AmmoColor.Red)
                                sr.color = Color.red;
                            else if (ammoUsed.Value == AmmoColor.Yellow)
                                sr.color = Color.yellow;
                        }


                        Rigidbody2D bullet = square.GetComponent<Rigidbody2D>();
                        Vector2 direction = new Vector2(mouseWorldPos.x, mouseWorldPos.y) - new Vector2(leftBulletSpawnPosition.transform.position.x, leftBulletSpawnPosition.transform.position.y);
                        // bullet.linearVelocity = (new Vector2(mouseWorldPos.x, mouseWorldPos.y) - new Vector2(rightBulletSpawnPosition.transform.position.x, rightBulletSpawnPosition.transform.position.y)).normalized * launchForce;
                        bullet.linearVelocity = direction.normalized * launchForce;
                        capturedBulletTime = elapsedTime;
                        // Debug.Log("Current Ammo: " + currentAmmo);
                    }
                    else
                    {
                        Debug.Log("No ammo left!");
                    }
                }
                // xoffset = -1.0f;
                else
                {
                    AmmoColor? ammoUsed = TryFire();
                    if (ammoUsed != null)
                    {
                        Debug.Log("Fired a " + ammoUsed.Value + " bullet!");
                        Debug.Log("Ammo Left: " + ammoManager.CurrentAmmo);
                        // Optional: use the ammo color to change the bullet appearance
                        GameObject square = Instantiate(squarePrefab, new Vector2(rightBulletSpawnPosition.transform.position.x + 1.225f, rightBulletSpawnPosition.transform.position.y + 0.225f), Quaternion.identity);

                        // Apply color to the instantiated object (not the prefab!)
                        SpriteRenderer sr = square.GetComponentInChildren<SpriteRenderer>();

                        if (sr != null)
                        {
                            if (ammoUsed.Value == AmmoColor.Blue)
                                sr.color = Color.blue;
                            else if (ammoUsed.Value == AmmoColor.Red)
                                sr.color = Color.red;
                            else if (ammoUsed.Value == AmmoColor.Yellow)
                                sr.color = Color.yellow;
                        }

                        Rigidbody2D bullet = square.GetComponent<Rigidbody2D>();
                        Vector2 direction = new Vector2(mouseWorldPos.x, mouseWorldPos.y) - new Vector2(rightBulletSpawnPosition.transform.position.x, rightBulletSpawnPosition.transform.position.y);
                        // bullet.linearVelocity = (new Vector2(mouseWorldPos.x, mouseWorldPos.y) - new Vector2(rightBulletSpawnPosition.transform.position.x, rightBulletSpawnPosition.transform.position.y)).normalized * launchForce;
                        bullet.linearVelocity = direction.normalized * launchForce;
                        capturedBulletTime = elapsedTime;
                        // Debug.Log("Current Ammo: " + currentAmmo);
                    }
                    else
                    {
                        Debug.Log("No ammo left!");
                    }
                }
            }
        }

        // Spawn AmmoBox prefabs with number keys 1, 2, 3
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Spawn Red AmmoBox at player's position + offset
            Vector3 spawnPos = transform.position + Vector3.right * 1.5f;
            GameObject ammoBox = Instantiate(redAmmoPrefab, spawnPos, Quaternion.identity);
            // Access the Pickup component on the spawned prefab
            Pickup pickup = ammoBox.GetComponent<Pickup>();
            pickup.lifespan = ammoLifespan; // Set the lifespan of the pickup

        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // Spawn Blue AmmoBox at player's position + offset
            Vector3 spawnPos = transform.position + Vector3.right * 1.5f;
            GameObject ammoBox = Instantiate(blueAmmoPrefab, spawnPos, Quaternion.identity);
            // Access the Pickup component on the spawned prefab
            Pickup pickup = ammoBox.GetComponent<Pickup>();
            pickup.lifespan = ammoLifespan; // Set the lifespan of the pickup
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            // Spawn Yellow AmmoBox at player's position + offset
            Vector3 spawnPos = transform.position + Vector3.right * 1.5f;
            GameObject ammoBox = Instantiate(yellowAmmoPrefab, spawnPos, Quaternion.identity);
            // Access the Pickup component on the spawned prefab
            Pickup pickup = ammoBox.GetComponent<Pickup>();
            pickup.lifespan = ammoLifespan; // Set the lifespan of the pickup
        }


    }

    void FixedUpdate()
    {
        // Debug.Log("Elapsed Time: " + elapsedTime);
        // rb.AddForce(movement * moveSpeed * Time.deltaTime);
        // if(!Input.GetMouseButton(0))
        rb.linearVelocity = new Vector2(movement.x * moveSpeed, rb.linearVelocity.y);
        // transform.position += new Vector3(movement.x * moveSpeed * Time.deltaTime, 0, 0);
        if (jumpRequested)
        {
            Debug.Log("Fixed Update Jumping");
            // rb.AddForce(Vector2.up * jumpForce  , ForceMode2D.Impulse);
            // rb.AddForce(Vector2.up * Mathf.Clamp(jumpForce, 0, MAXJUMP), ForceMode2D.Impulse);
            // float finalJumpForce = Mathf.Clamp(jumpForce * jumpSpeed, 0, MAXJUMP * jumpSpeed);
            float finalJumpForce = Mathf.Clamp(currentJumpCharge * jumpSpeed, 0, MAXJUMP_CHARGE * jumpSpeed);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0); // Cancel any downward velocity
            rb.AddForce(Vector2.up * finalJumpForce, ForceMode2D.Impulse);

            // isHanging = true;
            float initialVelocityY = finalJumpForce / rb.mass; // or just finalJumpForce if mass = 1
            maxHeight = rb.position.y + (initialVelocityY * initialVelocityY) / (2 * Mathf.Abs(Physics2D.gravity.y));
            Debug.Log("Estimated Max Jump Height: " + maxHeight);

            jumpRequested = false;
            currentJumpCharge = 0f;
            // reachedMaxHeight = false;
            // captureHangTime = elapsedTime;

        }
        if (!isGrounded)
            Debug.Log("Jump Height: " + rb.position.y);
        else
        {
            // moveSpeed = 3.0f;
            rb.gravityScale = 1.0f; // Reset gravity
        }

        if (Math.Abs(rb.linearVelocity.y) <= jumpPeak && !isGrounded)
        {
            Debug.Log("SLOW DOWN GRAVITY");
            rb.gravityScale = jumpPeakGravityScale; // Slow down gravity
        }
        else
        {
            rb.gravityScale = 1.0f; // Reset gravity
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Grounded");
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Grounded FAlse");
            isGrounded = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("AmmoBox_Red"))
        {
            ammoManager.AddAmmoBox(AmmoColor.Red);
            AmmoUIContainer.GetComponent<AmmoUI>().RefreshAmmoUI(); // Uncomment and use your actual AmmoUI instance
            Debug.Log("Picked up Red Ammo!");
            var pickup = other.GetComponent<Pickup>();
            score += pickup.itemScoreValue;
            // Play pickup audio if available
            if (pickup != null && pickup.pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickup.pickupSound, transform.position);
            }
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("AmmoBox_Blue"))
        {
            ammoManager.AddAmmoBox(AmmoColor.Blue);
            AmmoUIContainer.GetComponent<AmmoUI>().RefreshAmmoUI(); // Uncomment and use your actual AmmoUI instance
            Debug.Log("Picked up Blue Ammo!");
            var pickup = other.GetComponent<Pickup>();
            score += pickup.itemScoreValue;
            // Play pickup audio if available
            if (pickup != null && pickup.pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickup.pickupSound, transform.position);
            }
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("AmmoBox_Yellow"))
        {
            ammoManager.AddAmmoBox(AmmoColor.Yellow);
            AmmoUIContainer.GetComponent<AmmoUI>().RefreshAmmoUI(); // Uncomment and use your actual AmmoUI instance
            Debug.Log("Picked up Yellow Ammo!");
            var pickup = other.GetComponent<Pickup>();
            score += pickup.itemScoreValue;
            // Play pickup audio if available
            if (pickup != null && pickup.pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickup.pickupSound, transform.position);
            }
            Destroy(other.gameObject);
        }
    }

    
}
