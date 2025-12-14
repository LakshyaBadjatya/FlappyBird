using UnityEngine;

public class BirdJump : MonoBehaviour
{
    [Header("Movement")]
    public float jumpForce = 5f;
    public float gravity = -9f;
    public float maxFallSpeed = -10f;

    [Header("Rotation")]
    public float upRotation = 30f;
    public float downRotation = -90f;
    public float rotationSpeed = 5f;

    [Header("Sound")]
    public AudioClip jumpSound;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f; // we control gravity manually
    }

    void FixedUpdate()
    {
        // Smooth gravity
        rb.velocity = new Vector2(
            rb.velocity.x,
            Mathf.Max(rb.velocity.y + gravity * Time.fixedDeltaTime, maxFallSpeed)
        );

        // Smooth rotation based on vertical speed
        float t = Mathf.InverseLerp(maxFallSpeed, jumpForce, rb.velocity.y);
        float angle = Mathf.Lerp(downRotation, upRotation, t);
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.Euler(0, 0, angle),
            Time.fixedDeltaTime * rotationSpeed
        );
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            rb.velocity = Vector2.up * jumpForce;

            if (SoundManager.instance != null && jumpSound != null)
                SoundManager.instance.PlaySound(jumpSound);
        }
    }
}
