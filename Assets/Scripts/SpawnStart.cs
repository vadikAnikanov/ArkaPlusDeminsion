using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventAggregation;

public class SpawnStart : MonoBehaviour
{

    List<Transform> spawnList = new List<Transform>();



    void Start()
    {
       
        foreach (Transform point in GetComponentInChildren<Transform>())
        {
            spawnList.Add(point);
        }

        SpawnFpsPlayer spawnStartPos = new SpawnFpsPlayer();
        spawnStartPos.startPos = spawnList[PlayerPrefs.GetInt("level")].position;
        EventAggregator.Publish(spawnStartPos);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
