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

    void Start() {
	    _renderer = GetComponent<SpriteRenderer>();
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
