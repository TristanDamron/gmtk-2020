using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Controller : MonoBehaviour
{
    public float speed;
    public float maxSpeed;
    public float accelerationRate;
    public float decelerationRate;
    public bool moving;
    public bool jumping;
    public bool canPressButton;
    public Animator anim;
    public SpriteRenderer renderer;
    public UnityEngine.AI.NavMeshAgent agent;
    public enum AI {
        StalkingPlayer,
        BackToStart
    }

    void Start()
    {        
        anim = GetComponentInChildren<Animator>();        
        if (!anim) {
            Debug.LogWarning("Warning: Controlled object does not have a child with an Animator component.");
        }

        renderer = GetComponentInChildren<SpriteRenderer>();

        if (!renderer) {
            Debug.LogWarning("Warning: Controlled object does not have a child with an SpriteRenderer component.");
        }

        if (GetComponent<NavMeshAgent>())
            agent = GetComponent<NavMeshAgent>();
    }
    
    public void PickUp(GameObject g) {
        g.GetComponent<BoxCollider>().enabled = false;
        g.GetComponent<SpriteRenderer>().enabled = false;        
        GameManager.items++;
    }

    public void PressButton(Button b) {
        anim.Play("Gwen Jump", -1, 0f);
        jumping = true;        
        canPressButton = true;
        StartCoroutine(StopJumpAndActivateButton(b));
    }

    private IEnumerator StopJumpAndActivateButton(Button b) {
        yield return new WaitForSeconds(0.2f);
        b.ActivateButton();        
        jumping = false;
        anim.Play("Gwen Walk");        
    }   

    public void Accelerate() {
        if (speed <= maxSpeed) {
            speed += accelerationRate;
        }
    } 

    public void Decelerate() {
        if (speed >= 0f) {
            speed -= decelerationRate;
        }

        if (speed < 0f) {
            speed = 0f;
        }        
    }
}
