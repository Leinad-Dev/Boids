using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    [SerializeField] GameObject agentPrefab;
    [Range(10, 500)] public int numFish;
    public GameObject[] allFish;
    [SerializeField] Vector3 spawnAreaLimit = new Vector3(5, 5, 5);

    [Header("Fish Settings")]
    [Range(0.1f, 5.0f)]
    public float minSpeed;
    [Range(0.1f, 5.0f)]
    public float maxSpeed;
    [Range(1.0f, 200.0f)]
    public float visionRadius;
    [Range(1.0f, 5.0f)]
    public float rotationSpeed;
    [Range(1.0f, 20.0f)]
    public float fishClipRange;

    private void Start()
    {
        allFish = new GameObject[numFish]; //initialize array
        for(int i=0; i<numFish; i++) //initilize all agents positions in array
        {
            Vector3 pos = this.transform.position + new Vector3(Random.Range(-spawnAreaLimit.x, spawnAreaLimit.x),
                                                                Random.Range(-spawnAreaLimit.y, spawnAreaLimit.y),
                                                                Random.Range(-spawnAreaLimit.z, spawnAreaLimit.z));
            allFish[i] = (GameObject)Instantiate(agentPrefab, pos, Quaternion.identity);
            allFish[i].GetComponent<Flock>().managerRef = this;
        }
    }
}
