using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockManager managerRef;
    float speed;
    bool turning = false;

    private void Start()
    {
        speed = UnityEngine.Random.Range(managerRef.minSpeed, managerRef.maxSpeed);
    }
    private void Update()
    {
        //bounding box of cube manager
        Bounds b = new Bounds(managerRef.transform.position, managerRef.spawnAreaLimit*1f);
        //if fish is out of bounds, turn it around
        if (!b.Contains(transform.position))
        {
            turning = true;
        }
        else
            turning = false;

        if (turning == true) //FISH IS OUT OF BOUNDARY
        {
            Vector3 direction = managerRef.transform.position - this.transform.position; //newDirection = to - from 
            if (direction != Vector3.zero) //rotate "this" smoothly to face the updated direction
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), managerRef.rotationSpeed * Time.deltaTime);
        }
        else
        {
            //FISH IS IN BOUNDARY
            if (Random.Range(1, 100) <= 10)
                ApplyRules();
        }



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
            if (g != this.gameObject) //no need to calculate distance between the same ("this") fish
            {
                neighborDistance = Vector3.Distance(g.transform.position, this.transform.position);
                if (neighborDistance <= managerRef.visionRadius) // within range to be a neighbor.
                {

                    //COHESION (average group center)
                    vGrpCenter += g.transform.position;
                    grpSize++;


                    //SEPERATION (average fish avoidance vector)
                    if (neighborDistance < managerRef.fishClipRange) //nbr fish too close to "this" fish
                    {
                        Vector3 vectorAway = this.transform.position - g.transform.position; //vector away from nbr fish (g)
                        vAvoidance = vAvoidance + vectorAway;
                        Debug.DrawRay(transform.position, vectorAway, Color.green);
                    }


                    //ALIGNMENT (average group speed)
                    Flock anotherFlock = g.GetComponent<Flock>();
                    avgGrpSpeed = avgGrpSpeed + anotherFlock.speed;

                }
            }
        }

        if (grpSize > 0) //check to see if we have a group
        {
            vGrpCenter = vGrpCenter / grpSize + (managerRef.goalPos - this.transform.position);
            speed = avgGrpSpeed / grpSize;
            Vector3 direction = (vGrpCenter + vAvoidance) - transform.position; //newDirection = desiredDirection - currentDirection  

            if (direction != Vector3.zero) //rotate "this" smoothly to face the updated direction
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), managerRef.rotationSpeed * Time.deltaTime);
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
