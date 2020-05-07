using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class NPCManager : MonoBehaviour
{
    List<string> nameArray;
    public GameObject[] NPCs;
    bool initialisedNPCs = false;

    // Start is called before the first frame update
    void Start()
    {
        nameArray = new List<string>();
    }

    // Update is called once per frame
    void Update()
    {
        if (initialisedNPCs == false)
        {
            InitNPCs();
        }
    }

    void InitNPCs()
    {
        FilterNames();
        for (int i = 0; i < NPCs.Length; i++)
        {
            ApplyColour(NPCs[i]);
            ApplyName(NPCs[i]);
            ApplyBehaviour(NPCs[i]);
        }
        initialisedNPCs = true;
    }

    void FilterNames()
    {
        string namePath = Application.dataPath + "/JSONData/NPCNames.txt";
        StreamReader fileReader = new StreamReader(namePath);

        for (int i = 0; i < 20; i++)
        {
            string toRead = fileReader.ReadLine();
            nameArray.Add(toRead);
        }
        fileReader.Close();
    }

    void ApplyColour(GameObject currentNPC)
    {
        float redColour = Random.Range(0.0f, 1.0f);
        float blueColour = Random.Range(0.0f, 1.0f);
        float greenColour = Random.Range(0.0f, 1.0f);
        List<string> npcNames = new List<string>();
        currentNPC.GetComponent<Renderer>().material.color = new Color(redColour, blueColour, greenColour);
    }

    void ApplyName(GameObject currentNPC)
    {
        int nameGiven = Random.Range(0, 20);

        for (int i = 0; i < NPCs.Length; i++)
        {
            while (nameArray[nameGiven] == NPCs[i].name)
            {
                nameGiven = Random.Range(0, 20);
            }
        }

        currentNPC.name = nameArray[nameGiven];
    }

    void ApplyBehaviour(GameObject currentNPC)
    {
        int behaviourGen = Random.Range(0, 4);

        switch (behaviourGen)
        {
            case 0:
            {
              currentNPC.GetComponent<NPCBehaviour>().NPCActing = NPCBehaviour.NPCState.Idle;
              break;
            }

            case 1:
            {
              currentNPC.GetComponent<NPCBehaviour>().NPCActing = NPCBehaviour.NPCState.Walking;
              break;
            }

            case 2:
            {
              currentNPC.GetComponent<NPCBehaviour>().NPCActing = NPCBehaviour.NPCState.ApproachPlayer;
              break;
            }

            case 3:
            {
              currentNPC.GetComponent<NPCBehaviour>().NPCActing = NPCBehaviour.NPCState.ApproachHouse;
              break;
            }
        }

    }
}