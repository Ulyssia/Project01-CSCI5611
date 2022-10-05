using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathCustomInit : MonoBehaviour
{
    public Vector3 customStart = Vector3.zero;
    public Vector3 customGoal = Vector3.zero;
    public GameObject startPoint, goalPoint;
    Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {
        //customized start point with left click
        if (Input.GetMouseButton(0))
        {
            Vector3 selectPoint = new Vector3();
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit)
            {
                selectPoint = hitInfo.point;
            }
            selectPoint.y = 0.5f;
            customStart = selectPoint;
            startPoint.transform.position = customStart;
        }
        //customized goal point with middle click
        if (Input.GetMouseButton(2))
        {
            Vector3 selectPoint = new Vector3();
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit)
            {
                selectPoint = hitInfo.point;
            }
            selectPoint.y = 0.5f;
            customGoal = selectPoint;
            goalPoint.transform.position = customGoal;
        }
    }
}
