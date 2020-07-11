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
    private Vector3 _target;    
    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    private float _currentSpeedMultiplier;
    [SerializeField]
    private float _maxSpeedMultiplier;
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

        transform.position = Vector3.MoveTowards(transform.position, _target, Time.deltaTime * _moveSpeed * _currentSpeedMultiplier);                        
    }

    void OnTriggerEnter(Collider c) {
        if (c.tag == "Player" && _state == AIState.StalkingPlayer) {
            c.GetComponent<PlayerController>().Possession();
            transform.position = c.transform.position;                        
            _state = AIState.BackToStart;            
            Recorder.recording = false;
        }
    }

    public void RestartGhost() {
        transform.position = Recorder.points[0];
        _state = AIState.StalkingPlayer;        
    }
}
