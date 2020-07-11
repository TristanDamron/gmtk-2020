using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField]
    private State[] _actions;

    public void ActivateButton() {        
        foreach (State action in _actions) {
            action.Do();
        }            
    }
}
