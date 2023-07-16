using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DogBehaviour : MonoBehaviour
{
    //Variables
    [SerializeField] float moveSpeed, walkSpeed, runSpeed;

    //References
    NavMeshAgent _dogAgent;
    Animator _dogAnimator;
    public Transform owner;

    // Start is called before the first frame update
    void Start()
    {
        _dogAgent = GetComponent<NavMeshAgent>();
        _dogAnimator= GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        DogMovementControls();
    }

    private void DogMovementControls()
    {
        _dogAgent.speed = moveSpeed;
        _dogAgent.SetDestination(owner.position);
        if (_dogAgent.remainingDistance<3.5)
        {
            Idle();
        }
        else if (_dogAgent.remainingDistance >= 8)
        {
            Run();
        }
        else if (_dogAgent.remainingDistance < 8 && _dogAgent.remainingDistance > 3)
        {
            Walk();
        }
    }

    private void Idle()
    {
        moveSpeed = 0;
        _dogAnimator.SetFloat("Speed", 0f, 0.1f, Time.deltaTime);
    }

    private void Walk()
    {
        moveSpeed = walkSpeed;
        _dogAnimator.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
    }

    private void Run()
    {
        moveSpeed = runSpeed;
        _dogAnimator.SetFloat("Speed", 1f, 0.1f, Time.deltaTime);
    }
}
