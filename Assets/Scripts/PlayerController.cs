using System.Collections;
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
    [SerializeField]
    private bool _canPressButton;
    [SerializeField]
    private float _stunTime;
    // Affected by a cool down. Switched to false when the player gets possessed, then switches to true after a short delay.
    // During this delay, the player cannot move or collide with the ghost.
    [SerializeField]
    private bool _canReclaimBody;    
    // How slowly the player moves after being possessed.
    [SerializeField]
    private float _speedDamper;
    [SerializeField]
    private bool _isPossessed;
    [SerializeField]
    private Sprite _playerSprite;
    [SerializeField]
    private Sprite _ghostSprite;
    [SerializeField]
    private Animator _anim;
    private SpriteRenderer _renderer;

    void Start()
    {
        _canPressButton = true;
        _renderer = GetComponent<SpriteRenderer>();
        _renderer.sprite = _playerSprite;
        if (_stunTime <= 0) {
            _stunTime = 1;
        }

        _anim = GetComponent<Animator>();
        _anim.enabled = false;
    }

    void Update()
    {
        if (!GameManager.paused)
            Move();

    }

    private void PressButton(Button b) {
        b.ActivateButton();
        _canPressButton = false;
    }

    private void Move() {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        var moving = (horizontal != 0f || vertical != 0f);
        
        if (moving && !GameManager.playerPaused) {
            Accelerate();            
            var pos = transform.position;
            pos.x += horizontal;
            pos.y += vertical;

            if (horizontal < 0) 
                _renderer.flipX = false;
            else
                _renderer.flipX = true;

            transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * _playerSpeed);
            _anim.enabled = true;
        } else {
            Decellerate();
            _anim.enabled = false;
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

    public bool CheckPossessed() {
        return _isPossessed;
    }

    private void PickUp(GameObject g) {
        g.GetComponent<BoxCollider>().enabled = false;
        g.GetComponent<SpriteRenderer>().enabled = false;        
        GameManager.items++;
    }

    void OnTriggerStay(Collider collider) {
        if (collider.GetComponent<Button>() && _canPressButton && !_isPossessed) {
            PressButton(collider.GetComponent<Button>());            
        }

        if (collider.tag == "Ghost" && _canReclaimBody) {
            collider.GetComponent<GhostController>().RestartGhost();
            _isPossessed = false;
            _renderer.sprite = _playerSprite;
            gameObject.layer = LayerMask.NameToLayer("Default");   
            _maxSpeed *= 2;
        }

        if (collider.tag == "Item" && !_isPossessed) {
            PickUp(collider.gameObject);            
        } 
    }

    void OnTriggerExit(Collider collider) {
        if (collider.GetComponent<Button>()) {
            _canPressButton = true;
        }
    }

    public void Possession() {
        _maxSpeed /= 2;
        _playerSpeed = 0;
        _isPossessed = true;
        _canReclaimBody = false;    
        GameManager.playerPaused = true;
        gameObject.layer = LayerMask.NameToLayer("Ghost");
        _renderer.sprite = _ghostSprite;
        StartCoroutine(ReclaimBodyCooldown());
    }    

    IEnumerator ReclaimBodyCooldown() {
        yield return new WaitForSeconds(_stunTime);
        _canReclaimBody = true;        
        GameManager.playerPaused = false;
    }
}
