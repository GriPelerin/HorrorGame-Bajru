using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPerlin : MonoBehaviour
{
    [SerializeField] private float moveRange = 0.2f;
    [SerializeField] private float speed = 1f;

    private Vector3 _startPos;
    private float _offsetX;
    private float _offsetY;
    void Start()
    {
        _startPos = transform.position;
        _offsetX = Random.Range(0f, 5f);
        _offsetY = Random.Range(0f, 5f);
    }
    void Update()
    {
        float x = (Mathf.PerlinNoise(Time.time * speed, _offsetX) - 0.5f) * 2f * moveRange;
        float y = (Mathf.PerlinNoise(Time.time * speed, _offsetY) - 0.5f) * 2f * moveRange;

        transform.position = _startPos + new Vector3(x, y, 0f);
    }
}
