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
                GetComponent<BoxCollider>().isTrigger = false;                
                GetComponent<SpriteRenderer>().enabled = true;                
                break;
            case StateType.Close:
                GetComponent<BoxCollider>().isTrigger = false; 
                GetComponent<SpriteRenderer>().enabled = true;               
                break;
        }                    
    }

    public void Do() {
        switch(_type) {
            case StateType.Open:
                GetComponent<BoxCollider>().isTrigger = true;                
                GetComponent<SpriteRenderer>().enabled = false;
                _type = StateType.Close;
                break;
            case StateType.Close:
                GetComponent<BoxCollider>().isTrigger = false; 
                GetComponent<SpriteRenderer>().enabled = true;               
                _type = StateType.Open;
                break;
        }
    }
}
