using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhostController : MonoBehaviour
{    
    enum AIState {
        StalkingPlayer,
        BackToStart
    }

    [SerializeField]
    private Vector3 _target;    
    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    private float _currentSpeedMultiplier;
    [SerializeField]
    private float _maxSpeedMultiplier;
    [SerializeField]
    private AIState _state;
    [SerializeField]
    private NavMeshAgent _agent;

    void Start()
    {        
        _state = AIState.StalkingPlayer;
    }

    // Update is called once per frame
    void Update()
    {
        _agent.destination = transform.position;
        if (!GameManager.paused) {            
            if (_state == AIState.StalkingPlayer) {
                Recorder.recording = true;
                _target = GameObject.FindGameObjectWithTag("Player").transform.position;   
                _currentSpeedMultiplier = 1f;                     
            } else if (_state == AIState.BackToStart && Recorder.points.Count != 0) {
                _target = Recorder.points[Recorder.points.Count - 1];
                if (Vector3.Distance(transform.position, _target) < 0.25f) {
                    Recorder.points.Remove(_target);
                }
                _currentSpeedMultiplier = _maxSpeedMultiplier;
            }

            _agent.SetDestination(_target);        
            _agent.speed = _moveSpeed * _currentSpeedMultiplier;    
        }

        if (Recorder.points.Count == 0 && _state == AIState.BackToStart) {
            GameManager.blackOut = true;
        }
    }

    void OnTriggerEnter(Collider c) {
        if (c.tag == "Player" && _state == AIState.StalkingPlayer) {
            c.GetComponent<PlayerController>().Possession();            
            _state = AIState.BackToStart;            
            Recorder.recording = false;
        }
    }

    public void RestartGhost() {
        transform.position = Recorder.points[0];
        _state = AIState.StalkingPlayer;        
    }
}
