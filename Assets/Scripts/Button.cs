using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField]
    private ButtonAction[] _actions;

    public void ActivateButton() {        
        foreach (ButtonAction action in _actions) {
            action.Do();
        }            
    }
}
