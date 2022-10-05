using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputControl : MonoBehaviour
{
    public float speed = 2.5f;
    public float stride = 2.5f;
    float ctrl_X, ctrl_Y;
    float motion = 0;
    Vector3 LookAt;
    Camera cam;
    public Vector3 myStart = new Vector3(0,0,0);
    public Vector3 myGoal = new Vector3(0,0,0);
    
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
        //camera rotate when press the right button
        if (Input.GetMouseButton(1))
        {
            //transform.Rotate(new Vector3(Input.GetAxis("Mouse Y")*speed, -Input.GetAxis("Mouse X") * speed, 0));
            transform.Rotate(0, Input.GetAxis("Mouse X") * speed, 0);
            ctrl_X = transform.rotation.eulerAngles.x;
            ctrl_Y = transform.rotation.eulerAngles.y;
            transform.rotation = Quaternion.Euler(ctrl_X, ctrl_Y, 0);
        }

        if (Input.GetKey("w"))
        {
            Vector3 forward = new Vector3(transform.forward.x, 0, transform.forward.z);
            motion = stride * Time.deltaTime;
            transform.position = transform.position + motion* forward;
        }
        if (Input.GetKey("s"))
        {
            Vector3 forward = new Vector3(transform.forward.x, 0, transform.forward.z);
            motion = -stride * Time.deltaTime;
            transform.position = transform.position + motion*forward;
        }
        if (Input.GetKey("a"))
        {
            Vector3 left = new Vector3(transform.forward.z, 0, transform.forward.x);
            motion = stride * Time.deltaTime;
            transform.position = transform.position + motion* left;
        }
        if (Input.GetKey("d"))
        {
            Vector3 left = new Vector3(transform.forward.z, 0, transform.forward.x);
            motion = -stride * Time.deltaTime;
            transform.position = transform.position + motion*left;
        }
        if (Input.GetKey("q"))
        {
            motion = stride * Time.deltaTime;
            transform.position = transform.position + motion*transform.up;
        }
        if (Input.GetKey("e"))
        {
            motion = -stride * Time.deltaTime;
            transform.position = transform.position + motion*transform.up;
        }
    }
}
