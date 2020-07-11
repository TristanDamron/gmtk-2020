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

    void Start() {
        switch(_type) {
            case StateType.Open:
                GetComponent<BoxCollider>().isTrigger = true;    
                GetComponent<SpriteRenderer>().color = Color.green;            
                // GetComponent<SpriteRenderer>().enabled = false;                
                break;
            case StateType.Close:
                GetComponent<BoxCollider>().isTrigger = false; 
                GetComponent<SpriteRenderer>().color = Color.red;
                // GetComponent<SpriteRenderer>().enabled = true;               
                break;
        }                    
    }

    public void Do() {
        switch(_type) {
            // OPEN = OFF = GREEN
            case StateType.Open:
                _type = StateType.Close;
                GetComponent<BoxCollider>().isTrigger = false;                
                GetComponent<SpriteRenderer>().color = Color.red;
                // GetComponent<SpriteRenderer>().enabled = true;                
                break;
            // CLOSED = ON = RED
            case StateType.Close:
                _type = StateType.Open;
                GetComponent<BoxCollider>().isTrigger = true; 
                GetComponent<SpriteRenderer>().color = Color.green;
                // GetComponent<SpriteRenderer>().enabled = false;                                               
                break;
        }
    }
}
