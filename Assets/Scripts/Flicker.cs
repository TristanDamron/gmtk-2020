using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flicker : MonoBehaviour
{
    [SerializeField]
    private float _minimumLightIntensity; 
    [SerializeField]
    private float _maximumLightIntensity;
    [SerializeField]
    private float _maxDelay;
    private float _delay;
    private Light _light;
    private float _timer;

    void Start() {
        if (_minimumLightIntensity == 0f) {
            _minimumLightIntensity = 25f;
        }

        if (_maximumLightIntensity == 0f) {
            _maximumLightIntensity = 30f;
        } 

        if (_maxDelay == 0f) {
            _maxDelay = 2f;
        } 

        _light = GetComponent<Light>();        
        _timer = 0f;
        _delay = Random.Range(_maxDelay / 2, _maxDelay);
    }

    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _delay) {
            _delay = Random.Range(_maxDelay / 2, _maxDelay);
            _timer = 0f;
            _light.intensity = Random.Range(_minimumLightIntensity, _maximumLightIntensity);
        }
    }
}
