using UnityEngine;

public class BirdTilt : MonoBehaviour
{
    public float tiltUpAngle = 30f;        // angle when tapping
    public float tiltDownAngle = -60f;     // angle when falling
    public float tiltSpeed = 5f;           // how fast rotation changes

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float targetAngle;

        // If we tap screen / press mouse
        if (Input.GetMouseButtonDown(0))
        {
            targetAngle = tiltUpAngle;      // tilt upward
        }
        else
        {
            // The bird's falling velocity decides tilt
            targetAngle = Mathf.Lerp(tiltUpAngle, tiltDownAngle, -rb.velocity.y);
        }

        // Smooth rotation towards target
        float newZ = Mathf.LerpAngle(transform.eulerAngles.z, targetAngle, tiltSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, 0, newZ);
    }
}
