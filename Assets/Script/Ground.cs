using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    public float MovingSpeed = 1;
    public void Awake ()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void Update()
    {
        meshRenderer.material.mainTextureOffset += new Vector2(MovingSpeed * Time.deltaTime, 0f);
    }
}
