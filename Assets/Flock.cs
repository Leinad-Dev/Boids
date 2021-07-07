using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockManager managerRef;
    float speed;

    private void Start()
    {
        speed = UnityEngine.Random.Range(managerRef.minSpeed, managerRef.maxSpeed);
    }
    private void Update()
    {
        ApplyRules();
        transform.Translate(0, 0, speed * Time.deltaTime);
    }



    private void ApplyRules()
    {
        GameObject[] fishHolder;
        fishHolder = managerRef.allFish;
        Vector3 vGrpCenter = Vector3.zero;  //Point to center of group
        Vector3 vAvoidance = Vector3.zero; //Point away from particular neighbor
        float avgGrpSpeed = 0.01f;
        float neighborDistance;
        int grpSize = 0;

        foreach (GameObject g in fishHolder)
        {
            if (g != this.gameObject) //no need to calculate distance between the same fish
            {
                neighborDistance = Vector3.Distance(g.transform.position, this.transform.position);

                

                if (neighborDistance <= managerRef.visionRadius) // within range to be a neighbor.
                {
                    vGrpCenter += g.transform.position;
                    grpSize++;

                    if (neighborDistance < managerRef.fishClipRange) //nbr fish too close to "this" fish
                    {
                        Vector3 vectorAway = this.transform.position - g.transform.position; //vector away from nbr fish (g)
                        vAvoidance = vAvoidance + vectorAway;
                        Debug.DrawRay(transform.position, vectorAway, Color.blue);
                    }
                    Flock anotherFlock = g.GetComponent<Flock>();
                    avgGrpSpeed = avgGrpSpeed + anotherFlock.speed;
                }
            }
        }

        if (grpSize > 0) //check to see if we have a group
        {
            vGrpCenter = vGrpCenter / grpSize;
            speed = avgGrpSpeed / grpSize;

            //[vector towards the center] + [add the vector away from anyone we are might going to hit] - [our current position]
            Vector3 direction = (vGrpCenter + vAvoidance) - transform.position;
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(direction), managerRef.rotationSpeed * Time.deltaTime);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, managerRef.visionRadius);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, managerRef.fishClipRange);
    }



}
