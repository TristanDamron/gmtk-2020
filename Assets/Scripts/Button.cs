using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField]
    private State[] _actions;
    [SerializeField]
    private bool _foreverDoor;
    [SerializeField]
    private Sprite _onSprite;
    [SerializeField]    
    private Sprite _offSprite;
    private SpriteRenderer _renderer;    
    private bool _on;
    private BoxCollider _trigger;
    private Vector3 _defaultTriggerSize;
    private Vector3 _biggerTriggerSize;


    void Start() {
	    _renderer = GetComponent<SpriteRenderer>();
        _trigger = GetComponent<BoxCollider>();
        _defaultTriggerSize = _trigger.size;
        _biggerTriggerSize = _defaultTriggerSize * 6;
    }

    void Update() {
        if (GameManager.chase) {
            _trigger.size = _biggerTriggerSize;
        } else {
            _trigger.size = _defaultTriggerSize;
        }
    }
    
    public void ActivateButton() {        
        if (_on) {
            _on = false;
            _renderer.sprite = _offSprite;
        } else if (!_on) {
            _on = true;
            _renderer.sprite = _onSprite;
        }
        
        foreach (State action in _actions) {        
            if (_foreverDoor) {
                action.Disable();
		        _renderer.sprite = _offSprite;
            } else {
                action.Do();
            }
        }            
    }
}
