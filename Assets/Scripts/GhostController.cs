using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhostController : MonoBehaviour
{    
    enum AI {
        StalkingPlayer,
        BackToStart
    }

    // The object the AI is moving towards.
    [SerializeField]
    private Vector3 _target;    
    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    private float _currentSpeedMultiplier;
    // The maximum speed that the ghost moves after it possesses the player.
    [SerializeField]
    private float _maxSpeedMultiplier;
    [SerializeField]
    private AI _state;
    [SerializeField]
    private NavMeshAgent _agent;
    [SerializeField]
    private Transform _exit;
    [SerializeField]
    private Transform _hubSpawn;
    // The ghost's spawn point during the opening cutscene.
    [SerializeField]
    private Transform _spawn;

    void Start()
    {        
        _state = AI.StalkingPlayer;

        if (!_exit) {
            _exit = GameObject.Find("Exit").transform;
        }

        if (!_hubSpawn) {
            _hubSpawn = GameObject.Find("Ghost Hub Spawn").transform;
        }

        if (!_spawn) {
            _spawn = GameObject.Find("Ghost Spawn Point").transform;
        }
    }
    
    void Update()
    {    
        if (GameManager.ghostIntro) {
            SetNextPosition(_spawn.position);
            GameManager.playerPaused = true;
        }

        _agent.destination = transform.position;        
        if (!GameManager.paused) {   
            _agent.isStopped = false;                
            if (_state == AI.StalkingPlayer) {
                Recorder.recording = true;
                _target = GameObject.FindGameObjectWithTag("Player").transform.position;   
                _currentSpeedMultiplier = 1f;                     
            } else if (_state == AI.BackToStart && Recorder.points.Count != 0) {
                _target = Recorder.points[Recorder.points.Count - 1];
                if (Vector3.Distance(transform.position, _target) < 0.25f) {
                    Recorder.points.Remove(_target);
                }
                _currentSpeedMultiplier = _maxSpeedMultiplier;
            }

            if (Recorder.points.Count == 0 && _state == AI.BackToStart) {
                _target = _exit.position;
            }

            _agent.SetDestination(_target);        
            _agent.speed = _moveSpeed * _currentSpeedMultiplier;    
        }


        if (Vector3.Distance(transform.position, _exit.position) < 1f && _state == AI.BackToStart) {
            GameManager.blackOut = true;
            SetNextPosition(_hubSpawn.position);
            _state = AI.StalkingPlayer;
        }
    }

    private void PressButton(Button b) {
        b.ActivateButton();
    }

    void OnTriggerEnter(Collider c) {
        if (c.tag == "Player" && _state == AI.StalkingPlayer) {
            c.GetComponent<PlayerController>().Possession();            
            _state = AI.BackToStart;            
            Recorder.recording = false;
        }

        if (c.GetComponent<Button>() != null && _state == AI.BackToStart) {            
            PressButton(c.GetComponent<Button>());
        }
    }

    public void SetNextPosition(Vector3 pos) {
        Debug.Log("Warping to " + pos);
        _agent.Warp(pos);    
    }

    public void RestartGhost() {
        if (Recorder.points.Count == 0) {
            transform.position = _exit.position;
        } else {
            transform.position = Recorder.points[0];
        }
        _state = AI.StalkingPlayer;        
    }
}
