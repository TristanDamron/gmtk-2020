using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
    enum StateType {
        Open,
        Close
    }

    [SerializeField]
    private StateType _type;
    [SerializeField]
    private bool _disabled;
    [SerializeField]
    private ParticleSystem _openingParticles;

    void Start() {
        _openingParticles = GetComponentInChildren<ParticleSystem>();	

        if (!_disabled) {
            switch(_type) {
                case StateType.Open:
                    GetComponent<BoxCollider>().isTrigger = true;    
                    // GetComponent<SpriteRenderer>().color = Color.green;            
                    GetComponent<SpriteRenderer>().enabled = false;                
                    break;
                case StateType.Close:
                    GetComponent<BoxCollider>().isTrigger = false; 
                    // GetComponent<SpriteRenderer>().color = Color.red;
                    GetComponent<SpriteRenderer>().enabled = true;               
                    break;
            }                    
        }
    }

    public void Disable() {
        GetComponent<BoxCollider>().isTrigger = true;    
        // GetComponent<SpriteRenderer>().color = Color.green;            
        GetComponent<SpriteRenderer>().enabled = false;   
        if (_type == StateType.Close)
            _openingParticles.Play();
        _disabled = true;             
    }

    public void Do() {
        if (!_disabled) {
            switch(_type) {
                // OPEN = OFF = GREEN
                case StateType.Open:
                    _type = StateType.Close;
                    GetComponent<BoxCollider>().isTrigger = false;                
                    // GetComponent<SpriteRenderer>().color = Color.red;
                    GetComponent<SpriteRenderer>().enabled = true;                
                    break;
                // CLOSED = ON = RED
                case StateType.Close:
                    _type = StateType.Open;
                    GetComponent<BoxCollider>().isTrigger = true; 
                    // GetComponent<SpriteRenderer>().color = Color.green;
                    GetComponent<SpriteRenderer>().enabled = false;                                               
                    _openingParticles.Play();
                    break;
            }
        }
    }
}
