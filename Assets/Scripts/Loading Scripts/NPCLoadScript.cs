using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class NPCLoadScript : MonoBehaviour
{
    private GameObject player;
    private GameObject[] NPCs;
    private string NPCFolderLocation;
    public GameObject prefabToLoad;

    public struct NPCData
    {
        public string NPCName;
        public Vector3 NPCPosition;
        public Color NPCColour;
        public NPCBehaviour.NPCState NPCState;
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        NPCFolderLocation = Application.dataPath + "/JSONData/NPCFiles/";

        if (!Directory.Exists(NPCFolderLocation))
        {
            Directory.CreateDirectory(NPCFolderLocation);
        }
    }

    void Update()
    {
        Vector3 playerPosition = player.transform.position;
        UnloadingNPCs(playerPosition);
        LoadNPCs(playerPosition);
    }

    void UnloadingNPCs(Vector3 playerPos)
    {
        NPCs = GameObject.FindGameObjectsWithTag("NPC");

        foreach (GameObject currentNPC in NPCs)
        {
            Vector3 NPCPosition = currentNPC.transform.position;
            float NPCDistanceBetween = Vector3.Distance(playerPos, currentNPC.transform.position);

            if (currentNPC.name == "Thomas")
            {
                Debug.Log(NPCDistanceBetween);
            }


            if (NPCDistanceBetween > 200)
            {
                SaveData(currentNPC);
                GameObject.Destroy(currentNPC);
                currentNPC.gameObject.SetActive(false);
            }
        }
    }


    void SaveData(GameObject currentAlteredNPC)
    {
        NPCData currentNPCData = new NPCData();

        if (currentAlteredNPC.name.Contains("Clone"))
        {
            string removeClone = currentAlteredNPC.name.Replace("(Clone)", "");
            currentNPCData.NPCName = removeClone;
        }

        else
        {
            currentNPCData.NPCName = currentAlteredNPC.name;
        }

        currentNPCData.NPCPosition = currentAlteredNPC.transform.position;
        currentNPCData.NPCColour = currentAlteredNPC.GetComponent<Renderer>().material.color;
        currentNPCData.NPCState = currentAlteredNPC.GetComponent<NPCBehaviour>().NPCActing;
        string saving = JsonUtility.ToJson(currentNPCData);

        if (!File.Exists(NPCFolderLocation + currentNPCData.NPCName + ".json"))
        {
           File.WriteAllText(NPCFolderLocation + currentNPCData.NPCName + ".json", saving);
        }
    }

    void LoadNPCs(Vector3 playerPos)
    {
        List<NPCData> loadNPCDatas = new List<NPCData>();
        DirectoryInfo NPCDataDirectory = new DirectoryInfo(NPCFolderLocation);
        FileInfo[] NPCFileNames = NPCDataDirectory.GetFiles("*.json");
        
        foreach (FileInfo processingNPC in NPCFileNames)
        {
            string readingFrom = File.ReadAllText(processingNPC.FullName);
            NPCData currentLoadedChunk = JsonUtility.FromJson<NPCData>(readingFrom);
            loadNPCDatas.Add(currentLoadedChunk);
        }

        if (loadNPCDatas.Count != 0)
        {
            for (int j = 0; j < loadNPCDatas.Count; j++)
            {
                float distancefromNPC = Vector3.Distance(playerPos, loadNPCDatas[j].NPCPosition);

                if (distancefromNPC < 400)
                {
                     GameObject NPCLoaded = Instantiate(prefabToLoad, loadNPCDatas[j].NPCPosition, Quaternion.identity);
                     NPCLoaded.name = loadNPCDatas[j].NPCName;
                     NPCLoaded.GetComponent<Renderer>().material.color = loadNPCDatas[j].NPCColour;
                     NPCLoaded.GetComponent<NPCBehaviour>().NPCActing = loadNPCDatas[j].NPCState;
                     File.Delete(NPCFolderLocation + loadNPCDatas[j].NPCName + ".json");
                     loadNPCDatas.Remove(loadNPCDatas[j]);
                }
            }
        }
    }
}