using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateLevelLanes : MonoBehaviour
{

    public Lane[] lanes;
    internal Lane[] lanesList;
    internal int lanesCreated = 0;
    public int precreateLanes = 20;
    public float nextLanePosition = 1;
    internal int index = 0;
    public Transform cameraObject;
    public Transform victoryLane;
    public int lanesToVictory = 0;

    // Start is called before the first frame update
    void Start()
    {
        int totalLanes = 0;
        int totalLanesIndex = 0;

        for (index = 0; index < lanes.Length; index++)
        {
            totalLanes += lanes[index].laneChance;
        }
        lanesList = new Lane[totalLanes];
        for (index = 0; index < lanes.Length; index++)
        {
            int laneChanceCount = 0;
            while (laneChanceCount < lanes[index].laneChance)
            {
                lanesList[totalLanesIndex] = lanes[index];
                laneChanceCount++;
                totalLanesIndex++;
            }
        }
        if (lanesList.Length > 0)
        {
            for (index = 0; index < precreateLanes; index++)
            {
                CreateLane();
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (lanesList.Length > 0 && nextLanePosition - cameraObject.position.x < precreateLanes)
        {
            CreateLane();
        }
    }


    void CreateLane()
    {

        int randomLane = Mathf.FloorToInt(Random.Range(0, lanesList.Length));
        Transform newLane = Instantiate(lanesList[randomLane].laneObject, new Vector3(nextLanePosition, 0, 0), Quaternion.identity) as Transform;
        nextLanePosition += lanesList[randomLane].laneWidth;
        lanesCreated++;
    }


    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(nextLanePosition, 0, -10), new Vector3(nextLanePosition, 0, 10));
    }
}
