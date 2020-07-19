using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField]
    private State[] _actions;
    [SerializeField]
    private bool _foreverDoor;

    public void ActivateButton() {        
        foreach (State action in _actions) {        
            if (_foreverDoor) {
                action.Disable();
            } else {
                action.Do();
            }
        }            
    }
}
