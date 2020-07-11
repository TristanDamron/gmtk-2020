using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform _target;
    [SerializeField]
    private float _delay;

    void Start() {
        if (!_target) {
            _target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Update()
    {
        var pos = transform.position;
        pos.x = _target.position.x;
        pos.y = _target.position.y;
        transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime / _delay);
    }
}
