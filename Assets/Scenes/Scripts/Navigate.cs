using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigate : MonoBehaviour
{
    List<int> path = new List<int>();
    List<Vector3> nodeList = new List<Vector3>();
    List<Vector3> finalPath = new List<Vector3>();
    public float speed = 0.5f;
    Vector3 start, goal;
    MapGen Map;
    int index = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        Map = GameObject.Find("MapManager").GetComponent<MapGen>();
        start = Map.customStart;
        goal = Map.customGoal;
        path = GameObject.Find("MapManager").GetComponent<MapGen>().path;
        nodeList = GameObject.Find("MapManager").GetComponent<MapGen>().nodePos;
        finalPath = getPathNode(path, nodeList);
        index = 0;
        gameObject.transform.Translate(start);
    }

    // Update is called once per frame
    void Update()
    {   
        if(gameObject.transform.position == goal)
        {
            print("Goal Approached");
        }
       if(index == finalPath.Count && gameObject.transform.position != goal)
        {
            Vector3 dir = (goal - gameObject.transform.position).normalized;
            gameObject.transform.position += speed * Time.deltaTime * dir;
            gameObject.transform.forward = dir;
        }
       else if(gameObject.transform.position != finalPath[index])
        {
            Vector3 dir = (finalPath[index] - gameObject.transform.position).normalized;
            gameObject.transform.position += speed * Time.deltaTime * dir;
            gameObject.transform.forward = dir;
        }
        else
        {
            index++;
        }
    }

    List<Vector3> getPathNode(List<int> path, List<Vector3> nodeList)
    {
        int numStop = path.Count;
        List<Vector3> finalPath = new List<Vector3>();
        if (path[0] == -1) return finalPath;
        for(int i = 0; i < numStop; i++)
        {
            finalPath[i] = nodeList[path[i]];
        }
        return finalPath;
    }
}
