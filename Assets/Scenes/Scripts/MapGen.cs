using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapGen : MonoBehaviour
{
    Vector3 mapCenter = Vector3.zero;
    float mapSize = 64f;
    public int ObstacleNum = 10;
    public int numNodes = 20;
    static int maxNumNode = 100;
    
    public Vector3[] obstaclePos = new Vector3[10];
    public List<Vector3> nodePos = new List<Vector3>();
    public List<int> path = new List<int>();
    int[][] neighbors;
    bool[] visited;
    int[] parent;

    public Vector3 customStart;
    public Vector3 customGoal;
    public GameObject agent;
    CollisionTest collisionTest;

    float boxRadii, agentRadii;

    // Start is called before the first frame update
    void Start()
    {
        agentRadii = agent.GetComponent<CapsuleCollider>().radius;
        collisionTest = GameObject.Find("MapManager").GetComponent<CollisionTest>();
        boxRadii = GameObject.Find("obstacle").GetComponent<CapsuleCollider>().radius;
        customStart = GameObject.Find("startPoint").GetComponent<Transform>().position;
        customGoal = GameObject.Find("goalPoint").GetComponent<Transform>().position;
        visited = new bool[numNodes];
        parent = new int[numNodes];
        neighbors = new int[numNodes][];
        //List<int> current = new List<int>();
        for (int k = 0; k < numNodes; k++)
        {
            neighbors[k] = new int[numNodes];
        }
        for (int a = 0; a < numNodes; a++)
        {
            for (int b = 0; b < numNodes; b++)
            {
                neighbors[a][b] = -1;
            }
        }
        avoidList(GameObject.Find("Obstacles"));
        nodeGen(nodePos, numNodes, mapCenter, mapSize, obstaclePos, boxRadii, agentRadii);
        connectNeighbors(nodePos, numNodes, obstaclePos, ObstacleNum, boxRadii);
        pathGen(nodePos, customStart, customGoal, numNodes, obstaclePos, boxRadii, ObstacleNum);    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void avoidList(GameObject obstacle)
    {
        int index_o = 0;
        foreach (Transform child in obstacle.transform)
        {
            obstaclePos[index_o++] = child.position;
        }
    }

    void nodeGen(List<Vector3> nodeList, int numNode, Vector3 mapCenter, float mapSize, Vector3[] obstacles, float obRadii, float agentRadii)
    {
        Vector3 genPos = Vector3.zero;
        for(int i = 0; i < numNode; i++)
        {
            //print("Generating begin!");
            genPos.x = Random.Range(mapCenter.x - mapSize * 0.5f+agentRadii, mapCenter.x + mapSize * 0.5f-agentRadii);
            genPos.y = 0f;
            genPos.z = Random.Range(mapCenter.z - mapSize * 0.5f+agentRadii, mapCenter.z + mapSize * 0.5f-agentRadii);
            
            while(collisionTest.nodeInCircleList(obstacles, genPos, obRadii, 0.01f)){
                genPos.x = Random.Range(mapCenter.x - mapSize * 0.5f + agentRadii, mapCenter.x + mapSize * 0.5f - agentRadii);
                genPos.z = Random.Range(mapCenter.z - mapSize * 0.5f + agentRadii, mapCenter.z + mapSize * 0.5f - agentRadii);
            }
            //print(genPos);
            nodeList.Add(genPos);
        }
    }

    void connectNeighbors(List<Vector3> nodeList, int numNode, Vector3[] obList, int obNum, float obRadii)
    {
        for (int i = 0; i < numNode; i++)
        {
            int iter = 0;
            for (int j = 0; j < numNode; j++)
            {
                if (i == j) continue; 
                Vector3 dir = new Vector3(0,0,0);
                dir = (nodeList[j]-nodeList[i]).normalized;
                float distBetween = (nodeList[i]-nodeList[j]).magnitude;
                bool circleListCheck = collisionTest.rayIntersectCircleList(obList, obRadii,obNum, nodeList[i], dir, distBetween);
                if (!circleListCheck)
                {
                    neighbors[i][iter++] = j;
                }
            }
        }
    }

    int closestNode(Vector3 point, List<Vector3> nodeList, int numNode, Vector3[] obList, float obRadii,int obNum)
    {
        float minDist = 9999;
        int closeID = -1;
        for(int i = 0; i < numNode; i++)
        {
            float dist = (point - nodeList[i]).magnitude;
            Vector3 dir = (nodeList[i] - point).normalized;
            bool intersectCheck = collisionTest.rayIntersectCircleList(obList, obRadii, obNum, point, nodeList[i], 99f);
            if (!intersectCheck && dist < minDist)
            {
                closeID = i;
                minDist = dist;
            }
        }
        return closeID;
    }

    void pathGen(List<Vector3> nodeList, Vector3 start, Vector3 goal, int numNode, Vector3[] obList, float obRadii, int obNum)
    {
        int startID = closestNode(start, nodeList, numNode, obList, obRadii, obNum);
        int endID = closestNode(goal, nodeList, numNode, obList, obRadii, obNum);

        print(startID);
        print(endID);
        //path = runAstar(startID, endID, nodeList, numNode);
    }

    List<int> runAstar(int startID, int goalID, List<Vector3> nodeList, int nodeNum)
    {
        path = new List<int>();
        List<int> fringe = new List<int>();
        for (int i = 0; i < nodeNum; i++)
        {
            visited[i] = false;
            parent[i] = -1;
        }
        float[] f = new float[nodeNum];
        float[] g = new float[nodeNum];
        float[] h = new float[nodeNum];
        print("check one");
        for (int i = 0; i < nodeNum; i++)
        {
            g[i] = 9999;
            h[i] = (nodeList[i] - nodeList[goalID]).magnitude;
        }
        print("check two");
        visited[startID] = true;
        fringe.Add(startID);
        g[startID] = 0;
        f[startID] = g[startID] + h[startID];
        while (fringe.Count > 0)
        {
            int currentNode = fringe[0];
            fringe.Remove(0);
            print("check three");
            if (currentNode == goalID)
            {
                print("goal find!");
                break;
            }
            print("check 4");
            print(currentNode);
            int iter = 0;
            int neighborSize = 0;
            while (neighbors[currentNode][iter++] != -1)
            {
                neighborSize++;
            }           
            print("check 5");
            for (int j = 0; j < neighborSize; j++)
            {
                print("check 6");
                int currNeighbor = neighbors[currentNode][j];
                
                if (!visited[currNeighbor])
                {
                    float temp_g = g[currentNode] + (nodeList[currNeighbor]-nodeList[currentNode]).magnitude;
                    visited[currNeighbor] = true;
                    if (temp_g < g[currNeighbor])
                    {
                        fringe.Add(currNeighbor);
                        parent[currNeighbor] = currentNode;
                        g[currNeighbor] = temp_g;
                        f[currNeighbor] = g[currNeighbor] + h[currNeighbor];
                    }
                }
            }
        }
        if (fringe.Count == 0)
        {
            path.Insert(0,-1);
        }
        print("check five");
        int prevNode = parent[goalID];
        path.Insert(0, goalID);
        
        while (prevNode >= 0)
        {
            path.Insert(0, prevNode);
            prevNode = parent[prevNode];
        }
        return path;
    }
    
}
