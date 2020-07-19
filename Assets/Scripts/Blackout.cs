using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Blackout : MonoBehaviour
{
    [SerializeField]
    private Image _blackOutPanel;

    void Update()
    {
        if (GameManager.blackOut) {
            GameManager.paused = true;
            _blackOutPanel.color = Color.Lerp(_blackOutPanel.color, Color.black, Time.deltaTime * 2f);
            Invoke("Restart", 2f);
        }
    }

    void Restart() {
        GameManager.blackOut = false; 
        GameManager.paused = false;      
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
