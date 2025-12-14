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
    public float slideDistance = 30f;
    public float slideDuration = 0.25f;
    public float scaleAmount = 1.15f;
    public AnimationCurve ease = AnimationCurve.EaseInOut(0, 0, 1, 1);
    Coroutine scoreAnimCoroutine;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();
        AdsManager.instance.HideBanner();
        score = 0;
        if (scoreTxt != null)
            scoreTxt.text = score.ToString();
    }

    // ---------------- GAME OVER ----------------
    public void GameOver()
    {
        // 40% chance to show ad
        int chance = Random.Range(0, 100);
        if (chance < 40 && AdsManager.instance != null)
        {
            AdsManager.instance.ShowInterstitialAd();
        }

        Debug.Log("GameOver");
        StartCoroutine(ShowGameOverPanel());
    }

    IEnumerator ShowGameOverPanel()
    {
        yield return new WaitForSecondsRealtime(0f);
        PauseGame();
    }

    // ---------------- SCORE ----------------
    public void ScoreUp()
    {
        score++;
        if (scoreTxt != null)
        {
            scoreTxt.text = score.ToString();

            if (scoreAnimCoroutine != null)
                StopCoroutine(scoreAnimCoroutine);

            scoreAnimCoroutine = StartCoroutine(ScoreSlideAndPop());
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

        while (t < half)
        {
            t += Time.unscaledDeltaTime;
            float p = Mathf.Clamp01(t / half);
            float eased = ease.Evaluate(p);

            rt.anchoredPosition = originalPos + Vector2.up * (slideDistance * eased);
            rt.localScale = Vector3.Lerp(originalScale, originalScale * scaleAmount, eased);
            yield return null;
        }

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

    // ---------------- PLAY / RESET ----------------
    public void PlayGame()
    {
        if (gameOverPannel != null)
            gameOverPannel.SetActive(false);

        score = 0;
        if (scoreTxt != null)
        {
            scoreTxt.text = score.ToString();
            scoreTxt.gameObject.SetActive(true);
        }

        Time.timeScale = 1f;

        Pipe[] pipes = FindObjectsOfType<Pipe>();
        foreach (var p in pipes)
            Destroy(p.gameObject);

        ReenableAllObstacles();

        if (player != null)
        {
            player.transform.position = Vector3.zero;
            player.transform.rotation = Quaternion.identity;

            var rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }
        }
    }

    void ReenableAllObstacles()
    {
        var obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (var o in obstacles)
        {
            foreach (var col in o.GetComponentsInChildren<Collider2D>())
                col.enabled = true;
        }
    }

    // ---------------- PAUSE ----------------
    public void PauseGame()
    {
        if (gameOverPannel != null)
            gameOverPannel.SetActive(true);

        if (scoreTxt != null)
            scoreTxt.gameObject.SetActive(false);

        if (gameOverScoretxt != null)
            gameOverScoretxt.text = score.ToString();

        Time.timeScale = 0f;
    }
}
