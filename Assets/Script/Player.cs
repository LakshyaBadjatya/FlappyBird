using UnityEngine;

public class Player : MonoBehaviour
{
    // Animation
    private SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    private int spriteIndex;
    public float animationSpeed = 0.1f;

    // Sounds
    public AudioClip gameOverSound;
    public AudioClip scoreSound;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (sprites != null && sprites.Length > 0)
        {
            InvokeRepeating(nameof(AnimateBird), animationSpeed, animationSpeed);
        }
    }

    void AnimateBird()
    {
        if (sprites == null || sprites.Length == 0) return;

        spriteIndex++;
        if (spriteIndex >= sprites.Length)
            spriteIndex = 0;

        spriteRenderer.sprite = sprites[spriteIndex];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle"))
        {
            if (SoundManager.instance != null && gameOverSound != null)
                SoundManager.instance.PlaySound(gameOverSound);

            if (GameManager.instance != null)
                GameManager.instance.GameOver();
        }

        if (other.CompareTag("Score"))
        {
            if (GameManager.instance != null)
                GameManager.instance.ScoreUp();

            if (SoundManager.instance != null && scoreSound != null)
                SoundManager.instance.PlaySound(scoreSound);
        }
    }
}
