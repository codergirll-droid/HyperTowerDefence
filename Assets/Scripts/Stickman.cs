using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Stickman : MonoBehaviour
{
    NavMeshAgent agent;
    Vector3 towerPos;
    Vector3 towerPosRandom;
    Collider[] childrenCollider;
    Rigidbody[] childrenRigidbody;
    Animator animator;
    Rigidbody rb;
    BoxCollider boxCollider;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        towerPos = GameObject.FindGameObjectWithTag("tower").transform.position;
        towerPosRandom = towerPos + new Vector3(Random.Range(-2, 2), 0, 0);
    }

    private void Start()
    {
        childrenCollider = GetComponentsInChildren<Collider>();
        childrenRigidbody = GetComponentsInChildren<Rigidbody>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        EnableRagdoll(false);


        agent.speed = Random.Range(3.5f, 6);
        agent.SetDestination(towerPosRandom);

    }

    private void Update()
    {
        if(Vector3.Distance(transform.position, towerPosRandom) < 2f)
        {
            GetComponentInChildren<Animator>().enabled = false;
        }

        if(transform.position.y < 0)
        {
            GetComponentInChildren<Outline>().enabled = false;
        }
    }



    public void EnableRagdoll(bool active)
    {

        foreach (var collider in childrenCollider)
        {
            collider.enabled = active;
        }

        foreach (var rigidbody in childrenRigidbody)
        {
            rigidbody.isKinematic = !active;

            rigidbody.detectCollisions = active;

            if (active)
            {
                rigidbody.AddForce(new Vector3(Random.Range(-70, 70), 20, Random.Range(-70, 70))
                        , ForceMode.VelocityChange);
            }

        }

        animator.enabled = !active;
        rb.detectCollisions = !active;
        rb.isKinematic = active;
        boxCollider.enabled = !active;
        agent.enabled = !active;

    }

}
