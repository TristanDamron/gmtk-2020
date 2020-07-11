using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{    
    enum AIState {
        StalkingPlayer,
        BackToStart
    }

    [SerializeField]
    private Transform _target;    
    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    private AIState _state;

    void Start()
    {        
        _state = AIState.StalkingPlayer;
    }

    // Update is called once per frame
    void Update()
    {
        if (_state == AIState.StalkingPlayer) {
            _target = GameObject.FindGameObjectWithTag("Player").transform;            
            transform.position = Vector3.MoveTowards(transform.position, _target.position, Time.deltaTime * _moveSpeed);            
        }
    }
}
