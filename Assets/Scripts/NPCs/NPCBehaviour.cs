using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBehaviour : MonoBehaviour
{
    enum NPCState
    { 
        Idle, 
        Walking,
        ApproachPlayer,
        ApproachHouse
    }

    NPCState NPCActing;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (NPCActing)
        {
            case NPCState.Idle:
            {
               //Idle code, might be worked into NPCMovement
               break;
            }

            case NPCState.Walking:
            {
               //NPCMovement code
               break;
            }

            case NPCState.ApproachPlayer:
            {
               //ApproachScript code actuive for approaching player
               break;
            }

            case NPCState.ApproachHouse:
            {
               //ApproachScript code active for approaching house
               break;
            }

        }
        //switch behaviour through iEnumerator
    }
}
