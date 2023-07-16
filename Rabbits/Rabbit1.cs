using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rabbit1 : MonoBehaviour
{
    //Variables
    [SerializeField] int hp;
    [SerializeField] float moveSpeed, multiplier;

    //References
    NavMeshAgent _rabbit1Agent;
    Animator _rabbitAnimator;
    public Transform Destination1, Destination2, Destination3, owner;

    // Start is called before the first frame update
    void Start()
    {
        _rabbit1Agent = GetComponent<NavMeshAgent>();
        _rabbitAnimator = GetComponentInChildren<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Rabbit1MovementControls();
        if (hp <= 0)
        {
            Dead();
        }
    }

    private void Rabbit1MovementControls()
    {
        _rabbit1Agent.speed = moveSpeed;
        //Random Destinations
        if (Vector3.Distance(transform.position, owner.position) < 4)
        {
            Vector3 runTo = transform.position + ((transform.position - owner.position) * multiplier);
            _rabbit1Agent.SetDestination(runTo);
        }
        else if (_rabbit1Agent.isStopped)
        {
            Idle();
        }
        else
        {
            Run();
        }
    }

    private void Idle()
    {
        _rabbitAnimator.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
    }

    private void Run()
    {
        _rabbitAnimator.SetFloat("Speed", 1, 0.1f, Time.deltaTime);

        List<Transform> destinations = new List<Transform>(3);
        destinations.Add(Destination1);
        destinations.Add(Destination2);
        destinations.Add(Destination3);
        int destinationchoices = Random.Range(0, destinations.Count);
        _rabbit1Agent.SetDestination(destinations[destinationchoices].position);
    }

    private void Dead()
    {
        _rabbitAnimator.SetBool("Dead", true);
        _rabbit1Agent.enabled = false;
    }
}
