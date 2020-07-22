using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller
{
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

    void Start()
    {
        canPressButton = true;
        renderer.sprite = _playerSprite;
        if (_stunTime <= 0) {
            _stunTime = 1;
        }
    }

    void Update()
    {            
        if (!GameManager.paused)
            Move();
    }

    private void Move() {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        
        moving = (horizontal != 0f || vertical != 0f);
        
        if (moving && !GameManager.playerPaused && !jumping) {
            Accelerate();            
            var pos = transform.position;
            pos.x += horizontal;
            pos.y += vertical;

            if (horizontal < 0) 
                renderer.flipX = false;
            else if (horizontal > 0)
                renderer.flipX = true;

            transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * speed);
	    if (!_isPossessed)
		anim.Play("Gwen Walk");
        } else {
            Decelerate();            
        }

        if (!moving && !jumping && !_isPossessed) {
            anim.Play("Gwen Idle");            
        }
    }

    public bool CheckPossessed() {
        return _isPossessed;
    }

    void OnTriggerEnter(Collider collider) {
        if (collider.GetComponent<Button>() && canPressButton && !_isPossessed) {
            PressButton(collider.GetComponent<Button>());            
        }

        if (collider.tag == "Ghost" && _canReclaimBody) {
            collider.GetComponent<GhostController>().ReleasePlayer();
            _isPossessed = false;
            renderer.sprite = _playerSprite;
            anim.enabled = true;
            gameObject.layer = LayerMask.NameToLayer("Default");   
            maxSpeed *= 2;
            GameManager.chase = false;
        }

        if (collider.tag == "Item" && !_isPossessed) {
            PickUp(collider.gameObject);            
        } 
    }

    void OnTriggerExit(Collider collider) {
        if (collider.GetComponent<Button>()) {
            canPressButton = true;
        }
    }

    public void Possession() {
        GameManager.chase = true;
	    CameraFollow.screenShake = true;
        maxSpeed /= 2;
        speed = 0;
        _isPossessed = true;
        _canReclaimBody = false;    
        GameManager.playerPaused = true;
        gameObject.layer = LayerMask.NameToLayer("Ghost");
	    anim.Play("Ghost Gwen Move");
        StartCoroutine(ReclaimBodyCooldown());
    }    

    IEnumerator ReclaimBodyCooldown() {
        yield return new WaitForSeconds(_stunTime);
        if (!moving) {
            StartCoroutine(ReclaimBodyCooldown());
        } else {
            _canReclaimBody = true;        
            GameManager.playerPaused = false;                    
        }
    }
}
