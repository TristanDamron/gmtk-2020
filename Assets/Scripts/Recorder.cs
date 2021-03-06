﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recorder : MonoBehaviour
{
    [SerializeField]
    private Transform _player;    
    [SerializeField]
    private int _accuracy;
    public static List<Vector3> points = new List<Vector3>();    
    public static bool recording = true;

    void Start()
    {        
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (recording) {
            if (points.Count == 0) {
                points.Add(_player.position);
            }

            if (Vector3.Distance(_player.position, points[points.Count - 1]) > 1f / _accuracy) {                
                points.Add(_player.position);
            }
        }        
    }
}
