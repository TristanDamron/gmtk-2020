using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : Controller
{    
    // The object the AI is moving towards.
    [SerializeField]
    private Vector3 _target;    
    [SerializeField]
    private float _currentSpeedMultiplier;
    // The maximum speed that the ghost moves after it possesses the player.
    [SerializeField]
    private float _maxSpeedMultiplier;
    [SerializeField]
    private AI _state;
    [SerializeField]
    private Transform _exit;
    [SerializeField]
    private Transform _hubSpawn;
    // The ghost's spawn point during the opening cutscene.
    [SerializeField]
    private Transform _spawn;    
    [SerializeField]
    private Sprite _ghostSprite;
    [SerializeField]
    private Sprite _humanSprite;    
    private GameObject _player;

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

        if (!_ghostSprite) {
            _ghostSprite = renderer.sprite;
        }        
    }
    
    void Update()
    {   
        // Player is not a persistent object, so we need to update it's reference every frame 
        _player = GameObject.FindGameObjectWithTag("Player");        

        if (GameManager.ghostIntro) {
            SetNextPosition(_spawn.position);
            GameManager.playerPaused = true;
        }

        Move();        
    }

    void OnTriggerEnter(Collider c) {
        if (c.tag == "Player" && _state == AI.StalkingPlayer) {            
            PossessPlayer(_player.GetComponent<PlayerController>());        
        }

        if (c.GetComponent<Button>() != null && _state == AI.BackToStart) {            
            PressButton(c.GetComponent<Button>());
        }
    }

    private void Move() {
        FaceSpriteTowardTarget();
        agent.destination = transform.position;        
        if (!GameManager.paused && !jumping) {   
            agent.isStopped = false; 

            if (_state == AI.BackToStart && Recorder.points.Count != 0) {    
                _target = Recorder.points[Recorder.points.Count - 1];
                if (Vector3.Distance(transform.position, _target) < 0.25f) {
                    Recorder.points.Remove(_target);
                }            
            } else if (_state == AI.StalkingPlayer) {
                _target = _player.transform.position;   
            }

            if (Recorder.points.Count == 0 && _state == AI.BackToStart) {
                _target = _exit.position;
            }

            agent.SetDestination(_target);        
            agent.speed = speed * _currentSpeedMultiplier;    
        }


        if (Vector3.Distance(transform.position, _exit.position) < 1f && _state == AI.BackToStart) {
            GameManager.blackOut = true;
            SetNextPosition(_hubSpawn.position);
            if (renderer.sprite != _ghostSprite) {
                renderer.sprite = _ghostSprite;
                anim.Play("Ghost Idle");            
            }          
            Recorder.recording = true;        
            _currentSpeedMultiplier = 1f;                             
            _state = AI.StalkingPlayer;
        }
    }

    private void FaceSpriteTowardTarget() {
        if (_target.x < transform.position.x) {
            renderer.flipX = true;
        } else {
            renderer.flipX = false;
        }
    }

    public void SetNextPosition(Vector3 pos) {
        Debug.Log("Warping to " + pos);
        agent.Warp(pos);    
    }

    public void PossessPlayer(PlayerController p) {
        p.Possession();
        anim.Play("Gwen Walk");
        _currentSpeedMultiplier = _maxSpeedMultiplier;
        _state = AI.BackToStart;            
        Recorder.recording = false;
    }

    public void ReleasePlayer() {
        SetNextPosition(_exit.position);
        // if (Recorder.points.Count == 0) {
        //     _exit.position;
        // } else {
        //     transform.position = Recorder.points[0];
        // }

        _state = AI.StalkingPlayer;        

        if (renderer.sprite != _ghostSprite) {
            renderer.sprite = _ghostSprite;
            anim.Play("Ghost Idle");            
        }           

        Recorder.recording = true;        
        _currentSpeedMultiplier = 1f;                     
    }
}
