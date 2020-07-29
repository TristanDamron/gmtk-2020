using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Optimizer : MonoBehaviour
{
    private GameObject[] _allGameObjects;
    private Camera _camera;
    private Transform _player;
   	[SerializeField]	
	private float _viewDistance;
	[SerializeField]
	private Transform _vingette;
	private Vector3 _defaultVingetteScale;

    void Start()
    {
    	_camera = GameObject.Find("Main Camera").GetComponent<Camera>();
    	_player = GameObject.FindGameObjectWithTag("Player").transform;
		
		if (_viewDistance < 1f) {
			_viewDistance = 1f;
			Debug.LogWarning("Warning: Fog density must be >= 1");
		}
		
		_defaultVingetteScale = _vingette.localScale;
	}

    void Update()
    {
	/*
	 * For all objects outside of the camera bounds, turn off that item's renderer
	 */

       _allGameObjects = FindObjectsOfType<GameObject>();		 
       _vingette.localScale = _defaultVingetteScale * ((_camera.orthographicSize * _viewDistance) - (int)(_viewDistance * 1.3f));

	   foreach (GameObject obj in _allGameObjects) {
       	   if (obj.layer == 0 && Vector3.Distance(obj.transform.position, _player.position) >= _camera.orthographicSize * _viewDistance) {
				if (obj.GetComponent<MeshRenderer>()) {
		    		obj.GetComponent<MeshRenderer>().enabled = false;
				}
	   	
				if (obj.GetComponent<Light>()) {
					obj.GetComponent<Light>().enabled = false;	
				}

				if (obj.GetComponent<ParticleSystem>()) {
					obj.GetComponent<ParticleSystem>().Stop();
				}

				if (obj.GetComponent<Animator>()) {
					obj.GetComponent<Animator>().enabled = false;
				}

				if (obj.GetComponent<SpriteRenderer>()) {
		    		obj.GetComponent<SpriteRenderer>().enabled = true;
				}
	   		} else if (obj.layer == 0) {
				if (obj.GetComponent<MeshRenderer>()) {
		    		obj.GetComponent<MeshRenderer>().enabled = true;
				}		

				if (obj.GetComponent<Light>()) {
					obj.GetComponent<Light>().enabled = true;	
				}
			
				if (obj.GetComponent<ParticleSystem>()) {
					obj.GetComponent<ParticleSystem>().Play();
				}

				if (obj.GetComponent<Animator>()) {
					obj.GetComponent<Animator>().enabled = true;
				}

				if (obj.GetComponent<SpriteRenderer>()) {
		    		obj.GetComponent<SpriteRenderer>().enabled = true;
				}
	   		}
       } 
    }
}
