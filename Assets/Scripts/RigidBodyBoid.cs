
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RigidBodyBoid : MonoBehaviour
{
    public float minSpeed = 20.0f;
    public float turnSpeed = 20.0f;
    public float randomFreq = 20.0f;
    public float randomForce = 20.0f;

    //alignment variables
    public float toOriginForce = 50.0f;
    public float toOriginRange = 100.0f;
    public float gravity = 2.0f;

    //seperation variables
    public float avoidanceRadius = 50.0f;
    public float avoidanceForce = 20.0f;

    //cohesion variables
    public float followVelocity = 4.0f;
    public float followRadius = 40.0f;

    //these variables control the movement of the boid
    private Transform origin;
    private Vector3 velocity;
    private Vector3 normalizedVelocity;
    private Vector3 randomPush;
    private Vector3 originPush;
    private Transform[] objects;
    private RigidBodyBoid[] otherFlocks;
    private Transform transformComponent;

    internal RigidBodyController controller;
    private new Rigidbody rigidbody;

    private void Start()
    {
        this.rigidbody = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (controller)
        {
            Vector3 relativePos = steer() * Time.deltaTime;
            if (relativePos != Vector3.zero)
                rigidbody.velocity = relativePos;

            // enforce minimum and maximum speeds for the boids
            float speed = rigidbody.velocity.magnitude;
            if (speed > controller.maxVelocity)
            {
                rigidbody.velocity = rigidbody.velocity.normalized *
                  controller.maxVelocity;
            }
            else if (speed < controller.minVelocity)
            {
                rigidbody.velocity = rigidbody.velocity.normalized *
                    controller.minVelocity;
            }
        }
    }

    private Vector3 steer()
    {
        Vector3 center = controller.flockCenter -
            transform.localPosition;  // cohesion
        Vector3 velocity = controller.flockVelocity -
            rigidbody.velocity;  // alignment
        Vector3 follow = controller.target.localPosition -
            transform.localPosition;  // follow leader
        Vector3 separation = Vector3.zero;
        foreach (RigidBodyBoid flock in controller.flockList)
        {
            if (flock != this)
            {
                Vector3 relativePos = transform.localPosition -
                    flock.transform.localPosition;
                separation += relativePos / (relativePos.sqrMagnitude);
            }
        }

        // randomize
        Vector3 randomize = new Vector3((Random.value * 2) - 1,
            (Random.value * 2) - 1, (Random.value * 2) - 1);
        randomize.Normalize();
        return (controller.centerWeight * center +
            controller.velocityWeight * velocity +
            controller.separationWeight * separation +
            controller.followWeight * follow +
            controller.randomizeWeight * randomize);
    }
}