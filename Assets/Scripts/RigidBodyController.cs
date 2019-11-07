using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RigidBodyController : MonoBehaviour
{
    public float minVelocity = 1;  //Min Velocity
    public float maxVelocity = 8;  //Max Flock speed
    public int flockSize = 20;  //Number of flocks in the group
                                //How far the boids should stick to the center (the more
                                //weight stick closer to the center)

    public float centerWeight = 1;
    public float velocityWeight = 1;  //Alignment behavior
                                      //How far each boid should be separated within the flock
    public float separationWeight = 1;

    //How close each boid should follow to the leader (the more
    //weight make the closer follow)
    public float followWeight = 1;

    //Additional Random Noise
    public float randomizeWeight = 1;
    public RigidBodyBoid prefab;
    public Transform target;

    //Center position of the flock in the group
    internal Vector3 flockCenter;
    internal Vector3 flockVelocity;  //Average Velocity
    public ArrayList flockList = new ArrayList();

    void Start()
    {
        for (int i = 0; i < flockSize; i++)
        {
            RigidBodyBoid boid = Instantiate(prefab, transform.position,
                transform.rotation) as RigidBodyBoid;
            boid.transform.parent = transform;
            boid.controller = this;
            flockList.Add(boid);
        }
    }

    void Update()
    {
        //Calculate the Center and Velocity of the whole flock group
        Vector3 center = Vector3.zero;
        Vector3 velocity = Vector3.zero;
        foreach (RigidBodyBoid boid in flockList)
        {
            center += boid.transform.localPosition;
            velocity += boid.GetComponent<Rigidbody>().velocity;
        }
        flockCenter = center / flockSize;
        flockVelocity = velocity / flockSize;
    }
}
