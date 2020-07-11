﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _playerSpeed;
    [SerializeField]
    private float _maxSpeed;
    [SerializeField]
    private float _accelerationRate;
    [SerializeField]
    private float _decellerationRate;

    void Start()
    {

    }

    void Update()
    {
        Move();
    }

    private void PressButton(Button b) {
        b.ActivateButton();
    }

    private void Move() {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        var moving = (horizontal != 0f || vertical != 0f);
        
        if (moving) {
            Accelerate();            
            var pos = transform.position;
            pos.x += horizontal;
            pos.y += vertical;
            transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * _playerSpeed);
        } else {
            Decellerate();
        }
    }

    private void Accelerate() {
        if (_playerSpeed <= _maxSpeed) {
            _playerSpeed += _accelerationRate;
        }
    }

    private void Decellerate() {
        if (_playerSpeed >= 0f) {
            _playerSpeed -= _decellerationRate;
        }

        if (_playerSpeed < 0f) {
            _playerSpeed = 0f;
        }
    }

    void OnTriggerStay(Collider collider) {
        if (collider.GetComponent<Button>() && Input.GetKey(KeyCode.Space)) {
            PressButton(collider.GetComponent<Button>());
        }
    }
}
