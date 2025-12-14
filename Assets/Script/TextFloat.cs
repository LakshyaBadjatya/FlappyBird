using UnityEngine;
using TMPro;

public class TextFloat : MonoBehaviour
{
    public float amplitude = 0.5f;    // how far it moves left & right
    public float speed = 1.5f;        // how fast it moves

    RectTransform rt;
    Vector2 startPos;

    void Start()
    {
        rt = GetComponent<RectTransform>();
        startPos = rt.anchoredPosition;
    }

    void Update()
    {
        float x = Mathf.Sin(Time.time * speed) * amplitude;
        rt.anchoredPosition = startPos + new Vector2(x, 0f);
    }
}
