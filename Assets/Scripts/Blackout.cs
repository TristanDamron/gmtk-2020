using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blackout : MonoBehaviour
{
    [SerializeField]
    private Image _blackOutPanel;

    void Update()
    {
        if (GameManager.blackOut) {
            _blackOutPanel.color = Color.Lerp(_blackOutPanel.color, Color.black, Time.deltaTime * 2f);
        }
    }
}
