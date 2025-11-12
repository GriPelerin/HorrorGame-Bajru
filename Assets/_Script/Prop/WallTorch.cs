using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTorch : MonoBehaviour
{
    [SerializeField]private float _maxInterval = 1f;

    private Light _myLight;
    private float _interval;
    private float _targetIntensity;
    private float _lastIntensity;
    private float _timer;


    private void Awake()
    {
        _myLight = GetComponentInChildren<Light>();
    }

    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer > _interval)
        {
            _lastIntensity = _myLight.intensity;
            _targetIntensity = Random.Range(30, 45);
            _timer = 0;
            _interval = Random.Range(0, _maxInterval);


        }

        _myLight.intensity = Mathf.Lerp(_lastIntensity, _targetIntensity, _timer / _interval);
    }
}
