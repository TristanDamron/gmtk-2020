using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAction : MonoBehaviour
{
    enum ActionType {
        Open,
        Close
    }

    [SerializeField]
    private ActionType _type;

    public void Do() {
        switch(_type) {
            case ActionType.Open:
                GetComponent<BoxCollider>().isTrigger = true;                
                GetComponent<SpriteRenderer>().enabled = false;
                _type = ActionType.Close;
                break;
            case ActionType.Close:
                GetComponent<BoxCollider>().isTrigger = false; 
                GetComponent<SpriteRenderer>().enabled = true;               
                _type = ActionType.Open;
                break;
        }
    }
}
