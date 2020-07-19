using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordingResetTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player" && !other.GetComponent<PlayerController>().CheckPossessed()) {            
            Recorder.points.Clear();
            Destroy(gameObject);         
        }
    }
}
