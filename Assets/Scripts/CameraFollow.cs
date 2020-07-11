using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform _target;
    [SerializeField]
    private float _delay;
    [SerializeField]
    private float _minZoomLevel;
    [SerializeField]
    private float _maxZoomLevel;

    void Start() {
        if (!_target) {
            _target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        
        _minZoomLevel = GetComponent<Camera>().orthographicSize;
        _maxZoomLevel = _minZoomLevel * 4;
    }

    void Update()
    {
        var pos = transform.position;
        pos.x = _target.position.x;
        pos.y = _target.position.y;
        transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime / _delay);

        if (Input.GetKey(KeyCode.LeftShift)) {
            GameManager.paused = true;
            GetComponent<Camera>().orthographicSize = Mathf.Lerp(GetComponent<Camera>().orthographicSize, _maxZoomLevel, Time.deltaTime * 4f);
        } else {
            GameManager.paused = false;
            GetComponent<Camera>().orthographicSize = Mathf.Lerp(GetComponent<Camera>().orthographicSize, _minZoomLevel, Time.deltaTime * 4f);
        }
    }
}
