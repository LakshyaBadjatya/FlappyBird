using UnityEngine;

public class BirdJump : MonoBehaviour
{
    public float jumpForce = 5f;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))  // tap or click
        {
            rb.velocity = Vector2.up * jumpForce;
        }
    }
}
