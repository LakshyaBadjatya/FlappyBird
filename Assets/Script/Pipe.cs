using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    public float movingSpeed = 3f;
    float leftCameraPoint;

    public void Start()
    {
        leftCameraPoint = Camera.main.ScreenToWorldPoint (Vector3.zero).x - 1f;
    }

    public void Update()
    {
        transform.position += Vector3.left * movingSpeed * Time.deltaTime;

        if (transform.position.x < leftCameraPoint)
            Destroy(gameObject);
    }
}
