using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour
{
    Vector3 randomPos;
    bool waiting;
    NavMeshPath navPath;
    bool correctPath;
    // Start is called before the first frame update
    void Start()
    {
        float redColour = Random.Range(0.0f, 1.0f);
        float blueColour = Random.Range(0.0f, 1.0f);
        float greenColour = Random.Range(0.0f, 1.0f);
        GetComponent<Renderer>().material.color = new Color(redColour, blueColour, greenColour);

        navPath = new NavMeshPath();
    }

    // Update is called once per frame
    void Update()
    {
        if (waiting == false)
        {
            StartCoroutine(WaitTime());
        }
    }

    IEnumerator CheckPath()
    {
        correctPath = GetComponent<NavMeshAgent>().CalculatePath(randomPos, navPath);

        while (correctPath == false)
        {
            yield return new WaitForSeconds(0.1f);
            randomPos = new Vector3(Random.Range(-200, 200), 0, Random.Range(-200, 200));
            correctPath = GetComponent<NavMeshAgent>().CalculatePath(randomPos, navPath);
        }
    }

    void MoveAway()
    {
        randomPos = new Vector3(Random.Range(-200, 200), 0, Random.Range(-200, 200));
        GetComponent<NavMeshAgent>().SetDestination(randomPos);
    }

    IEnumerator WaitTime()
    {
        waiting = true;
        StartCoroutine(CheckPath());
        yield return new WaitForSeconds(Random.Range(60, 180));
        MoveAway();
        waiting = false;
    }
}
