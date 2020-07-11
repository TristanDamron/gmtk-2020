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
    private bool _canReclaimBody;
    [SerializeField]
    private bool _isPossessed;
    [SerializeField]
    private Sprite _playerSprite;
    [SerializeField]
    private Sprite _ghostSprite;

    void Start()
    {
        _canPressButton = true;
        GetComponent<SpriteRenderer>().sprite = _playerSprite;
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

    public bool CheckPossessed() {
        return _isPossessed;
    }

    private void PickUp(GameObject g) {
        Destroy(g);
        GameManager.items++;
    }

    void OnTriggerStay(Collider collider) {
        if (collider.GetComponent<Button>() && _canPressButton && !_isPossessed) {
            PressButton(collider.GetComponent<Button>());            
        }

        if (collider.tag == "Ghost" && _canReclaimBody) {
            collider.GetComponent<GhostController>().RestartGhost();
            _isPossessed = false;
            GetComponent<SpriteRenderer>().sprite = _playerSprite;
            gameObject.layer = LayerMask.NameToLayer("Default");            
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
        _isPossessed = true;
        _canReclaimBody = false;    
        gameObject.layer = LayerMask.NameToLayer("Ghost");
        GetComponent<SpriteRenderer>().sprite = _ghostSprite;
        StartCoroutine(ReclaimBodyCooldown());
    }

    IEnumerator ReclaimBodyCooldown() {
        yield return new WaitForSeconds(1f);
        _canReclaimBody = true;
    }
}
