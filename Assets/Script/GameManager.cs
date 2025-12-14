using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public TMP_Text scoreTxt;

    // game over panel
    public GameObject gameOverPannel;
    public TMP_Text gameOverScoretxt;

    int score;
    Player player;

    // ---- score animation settings ----
    [Header("Score Animation")]
    public float slideDistance = 30f;      // how many pixels up
    public float slideDuration = 0.25f;    // total time up+down
    public float scaleAmount = 1.15f;      // scale multiplier at peak
    public AnimationCurve ease = AnimationCurve.EaseInOut(0, 0, 1, 1); // smoothing
    Coroutine scoreAnimCoroutine;

    private void Awake()
    {
        // singleton: ensure only one instance in scene
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();
        // initialize score display
        score = 0;
        if (scoreTxt != null) scoreTxt.text = score.ToString();
    }

    public void GameOver()
    {
        Debug.Log("GameOver");
        // keep a short realtime delay to allow physics effects to show (if called from a coroutine)
        StartCoroutine(ShowGameOverPanel());
    }

    IEnumerator ShowGameOverPanel()
    {
        // small realtime wait so knockback/rotation/camera shake can be seen
        yield return new WaitForSecondsRealtime(0f);
        PauseGame();
    }

    public void ScoreUp()
    {
        score++;
        if (scoreTxt != null)
        {
            scoreTxt.text = score.ToString();

            // restart animation if already running
            if (scoreAnimCoroutine != null)
                StopCoroutine(scoreAnimCoroutine);
            scoreAnimCoroutine = StartCoroutine(ScoreSlideAndPop());
        }
        else
        {
            Debug.LogWarning("scoreTxt is not assigned on GameManager.");
        }
    }

    IEnumerator ScoreSlideAndPop()
    {
        if (scoreTxt == null) yield break;

        RectTransform rt = scoreTxt.rectTransform;
        Vector2 originalPos = rt.anchoredPosition;
        Vector3 originalScale = rt.localScale;

        float half = slideDuration / 2f;
        float t = 0f;

        // phase 1: move up & scale to peak
        while (t < half)
        {
            t += Time.unscaledDeltaTime;
            float p = Mathf.Clamp01(t / half);
            float eased = ease.Evaluate(p);

            rt.anchoredPosition = originalPos + Vector2.up * (slideDistance * eased);
            rt.localScale = Vector3.Lerp(originalScale, originalScale * scaleAmount, eased);

            yield return null;
        }

        // phase 2: move back down & scale back to original
        t = 0f;
        while (t < half)
        {
            t += Time.unscaledDeltaTime;
            float p = Mathf.Clamp01(t / half);
            float eased = 1f - ease.Evaluate(p);

            rt.anchoredPosition = originalPos + Vector2.up * (slideDistance * eased);
            rt.localScale = Vector3.Lerp(originalScale, originalScale * scaleAmount, eased);

            yield return null;
        }

        rt.anchoredPosition = originalPos;
        rt.localScale = originalScale;
        scoreAnimCoroutine = null;
    }

    // Call this to (re)start the play session
    public void PlayGame()
    {
        Debug.Log("PlayGame called - resetting scene");
        if (gameOverPannel != null) gameOverPannel.SetActive(false);

        score = 0;
        if (scoreTxt != null)
        {
            scoreTxt.text = score.ToString();
            scoreTxt.gameObject.SetActive(true);
        }

        Time.timeScale = 1f;

        // If your project uses pooling, prefer resetting colliders rather than destroying.
        // Attempt to destroy currently active Pipe objects (works if you are not pooling).
        Pipe[] pipes = FindObjectsOfType<Pipe>();
        for (int i = 0; i < pipes.Length; i++)
        {
            Destroy(pipes[i].gameObject);
        }

        // Re-enable any obstacle colliders that may have been disabled on death
        ReenableAllObstacles();

        // position our player to start and reset its state
        if (player != null)
        {
            player.transform.position = Vector3.zero;
            player.transform.rotation = Quaternion.identity;

            // Reset Rigidbody if present
            var rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }

            // If your bird script has ResetBird(), call it to re-enable collider and isDead flag
            var birdDeath = player.GetComponent<MonoBehaviour>(); // fallback to search
            if (birdDeath != null)
            {
                // Try to call ResetBird if it exists (BirdDeath from earlier examples)
                var method = birdDeath.GetType().GetMethod("ResetBird");
                if (method != null)
                {
                    method.Invoke(birdDeath, null);
                    Debug.Log("Called ResetBird() on player.");
                }
            }
        }
        else
        {
            Debug.LogWarning("Player reference is null in GameManager.PlayGame()");
        }
    }

    void ReenableAllObstacles()
    {
        // find objects tagged Obstacle and make sure their colliders are enabled and not triggers
        var obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        if (obstacles == null || obstacles.Length == 0)
        {
            Debug.Log("No objects found with tag 'Obstacle' during reset.");
            return;
        }

        foreach (var o in obstacles)
        {
            // enable colliders on the object and children
            var col = o.GetComponent<Collider2D>();
            if (col != null)
            {
                col.enabled = true;
                // Do NOT force isTrigger here; assume collision design is correct.
            }

            var childCols = o.GetComponentsInChildren<Collider2D>();
            foreach (var cc in childCols)
            {
                cc.enabled = true;
            }

            // If score zones exist as child triggers, they should remain triggers; only re-enable them.
            // If your pool or scoring code disabled a collider on reuse, it will be re-enabled here.
            Debug.Log($"Re-enabled colliders on obstacle: {o.name}");
        }
    }

    public void PauseGame()
    {
        if (gameOverPannel != null) gameOverPannel.SetActive(true);
        if (scoreTxt != null) scoreTxt.gameObject.SetActive(false);
        if (gameOverScoretxt != null) gameOverScoretxt.text = score.ToString();
        Time.timeScale = 0f;
    }
}
