using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    public GameObject pipePrebab;
    public float minHeight = -1f;
    public float maxHeight = 1.5f;
    public float repeatRate = 1.5f;

    public void Start()
    {
        InvokeRepeating(nameof(Spawn), repeatRate, repeatRate);
    }

    public void Spawn()
    {
        GameObject pipe = Instantiate(pipePrebab, transform.position, Quaternion.identity);
        pipe.transform.position += Vector3.up * Random.Range(minHeight, maxHeight);
    }
}
