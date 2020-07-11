using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool paused = false;
    public static int items = 0;            
    public static bool blackOut = false;
    public static bool ghostIntro = false;
    [SerializeField]
    private GameObject _ghost;
    [SerializeField]
    private Transform _ghostSpawnPoint;


    void Update() {
        if (ghostIntro) {            
            _ghost.transform.position = _ghostSpawnPoint.position; 
            _ghost.GetComponent<GhostController>().SetNextPosition(_ghostSpawnPoint.position);        
            paused = true; 
        }
    }    
}
