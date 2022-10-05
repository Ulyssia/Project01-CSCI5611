using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CollisionTest : MonoBehaviour
{
    
    public bool nodeInCircle(Vector3 node, Vector3 center, float radii, float eps)
    {
        if((node-center).sqrMagnitude < radii * radii+eps)
        {
            return true;
        }
        return false;
    }

    public bool nodeInCircleList(Vector3[] centers, Vector3 node, float radii, float eps) 
    {
        foreach (Vector3 center in centers)
        {
            if(nodeInCircle(node, center, radii, eps))
            {
                return true;
            }
        }
        return false;
    }

    public Hit rayIntersectCircle(Vector3 center, float radii, Vector3 init, Vector3 dir, float max_t)
    {
        Hit check = new Hit();
        check.hit = false;
        check.t = max_t;
        
        Vector3 toCircle = center - init;
        float a = 1;  
        float b = -2 * Vector3.Dot(dir, toCircle); 
        float c = toCircle.sqrMagnitude - radii*radii; 

        float d = b * b - 4 * a * c; 

        if (d >= 0)
        {
            float t1 = (-b - Mathf.Sqrt(d)) / (2 * a); 
            float t2 = (-b + Mathf.Sqrt(d)) / (2 * a); 
                                                 
            if (t1 > 0 && t1 < max_t)
            {
                check.hit = true;
                check.t = t1;
            }
            else if (t1 < 0 && t2 > 0)
            {
                check.hit = true;
                check.t = -1;
            }

        }
        return check;
    }

    public bool rayIntersectCircleList(Vector3[] center, float radii, int obNum, Vector3 init, Vector3 node, float max_t)
    {
        Hit check = new Hit();
        check.hit = false;
        check.t = max_t;
        Vector3 dir = (node - init).normalized;
        for (int i = 0; i < obNum; i++)
        {
            Vector3 obc = center[i];
            float r = radii;

            Hit subCheck = rayIntersectCircle(obc, r, init, dir, check.t);
            if (subCheck.t > 0 && subCheck.t < check.t)
            {
                check.hit = true;
                check.t = subCheck.t;
            }
            else if (subCheck.hit && subCheck.t < 0)
            {
                check.hit = true;
                check.t = -1;
            }
        }
        return check.hit;
    }
}
