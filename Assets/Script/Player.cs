using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    //Animation
    private SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    int spriteIndex;
    public float AnimationSpeed = 0.1f;

    //Jump
    public float jumpForce = 3f;
    public float fallSpeed = -15f;
    private Vector3 direction;

    public void Awake () {
        spriteRenderer = GetComponent<SpriteRenderer> (); }

    public void Start() {
        InvokeRepeating(nameof(AnimateBird), AnimationSpeed, AnimationSpeed);
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            direction = Vector3.up * jumpForce;
        }
        direction.y += fallSpeed * Time.deltaTime;
        transform.position += direction * Time.deltaTime;
    }

    public void AnimateBird() {
        spriteIndex++;
        if (spriteIndex >= sprites.Length) {
            spriteIndex = 0;
        }
        spriteRenderer.sprite = sprites[spriteIndex];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Obstacle") {
            GameManager.instance.GameOver();
        }
        if (other.gameObject.tag == "Score") {
            GameManager.instance.ScoreUp();
        }

    }
        
            
}
