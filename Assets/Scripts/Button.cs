using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField]
    private bool _activated;

    void Update() {
        if (_activated) {
            // do something
        }        
    }    

    public void ActivateButton() {
        _activated = !_activated;        
    }
}
