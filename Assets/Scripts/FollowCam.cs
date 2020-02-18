using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public GameObject target;
    Vector3 offset;
    private float mouseX;

    // Use this for initialization
    void Start()
    {
        offset = transform.position - target.transform.position;

    }

    private void Awake()
    {
        this.transform.RotateAround(target.transform.position, Vector3.up, 90.0f);
    }

    void LateUpdate()
    {

        mouseX = Input.GetAxis("Mouse X");
        float angleBetween = Vector3.Angle(Vector3.up, transform.forward);

        if (Input.GetMouseButton(1))
        {
            //create Quaternion rotation which is created from mouseX euler data driving y axis rotation
            offset = Quaternion.Euler(0, mouseX, 0) * offset;
            transform.position = target.transform.position + offset;
        }

        //check orientation of player in Y axis (direction faced atm)
        float desiredAngle = target.transform.eulerAngles.y;
        Quaternion rotation = Quaternion.Euler(0, desiredAngle, 0);

        //move offset representing the camera positive relative to the target
        transform.position = target.transform.position + (rotation * offset);
        transform.LookAt(target.transform);
    }
}