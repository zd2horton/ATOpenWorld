using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private bool jump;

    void Start()
    {
        
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
        }

        Vector3 playerMovement = new Vector3(horizontal, 0, vertical) * Time.deltaTime * 40;
        GetComponent<Rigidbody>().MovePosition(transform.position + playerMovement);

        if (jump == true)
        {
            jump = false;
            GetComponent<Rigidbody>().AddForce(0, 10, 0, ForceMode.Impulse);
        }
    }
}
