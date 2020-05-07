using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class BasicLoading : MonoBehaviour
{
    private GameObject player;
    private GameObject[] terrains;
    private string JSONFolderLocation;
    private int saveInterval;
    public GameObject[] prefabTerrain;

    public struct ChunkData
    {
        public string chunkName;
        public Vector3 chunkPosition;
        public Quaternion chunkRotation;
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        JSONFolderLocation = Application.dataPath + "/JSONData/TerrainFiles/";

        if (!Directory.Exists(JSONFolderLocation))
        {
            Directory.CreateDirectory(JSONFolderLocation);
        }
    }

    void Update()
    {
        Vector3 playerPosition = player.transform.position;
        UnloadingChunks(playerPosition);
        LoadData(playerPosition);
    }

    void UnloadingChunks(Vector3 playerPos)
    {
        terrains = GameObject.FindGameObjectsWithTag("TerrainSegment");

        foreach (GameObject currentTerrain in terrains)
        {
            Vector3 terrainPosition = currentTerrain.transform.position;
            float distanceBetweenTerrain = Vector3.Distance(playerPos, currentTerrain.transform.position);

            if (distanceBetweenTerrain > 400)
            {
                SaveData(currentTerrain);
                GameObject.Destroy(currentTerrain);
                currentTerrain.gameObject.SetActive(false);
            }
        }
    }


    void SaveData(GameObject currentTerrainPiece)
    {
        ChunkData currentChunkData = new ChunkData();

        if (currentTerrainPiece.name.Contains("Clone"))
        {
           string removeClone = currentTerrainPiece.name.Replace("(Clone)", "");
           currentChunkData.chunkName = removeClone;
        }

        else
        {
          currentChunkData.chunkName = currentTerrainPiece.name;
        }

        currentChunkData.chunkPosition = currentTerrainPiece.transform.position;
        currentChunkData.chunkRotation = currentTerrainPiece.transform.rotation;
        saveInterval = 1;
        string jsontest = JsonUtility.ToJson(currentChunkData);

        if (!File.Exists(JSONFolderLocation + currentChunkData.chunkName + ".json"))
        {
           //saveInterval++;
           File.WriteAllText(JSONFolderLocation + currentChunkData.chunkName + ".json", jsontest);
        }
    }

    void LoadData(Vector3 playerPos)
    {
        List<ChunkData> loadChunkDatas = new List<ChunkData>();
        DirectoryInfo chunkDirectory = new DirectoryInfo(JSONFolderLocation);
        FileInfo[] chunkFileNames = chunkDirectory.GetFiles("*.json");
        //int i = 0;

        foreach (FileInfo currentProcessedChunk in chunkFileNames)
        {
            string readFrom = File.ReadAllText(currentProcessedChunk.FullName);
            ChunkData currentLoadedChunk = JsonUtility.FromJson<ChunkData>(readFrom);
            //add currentLoadedChunk to loadChunkDatas
            //if (i < 16)
            //{
            //    bool doNotAdd = false;

            //    for (int k = 0; k < loadChunkDatas.Count; k++)
            //    {
            //        if (currentLoadedChunk.chunkName == loadChunkDatas[k].chunkName)
            //        {
            //            doNotAdd = true;
            //        }
            //    }

            //    if (doNotAdd == false)
            //    {
            //        loadChunkDatas[i] = currentLoadedChunk;
            //        //Debug.Log(loadChunkDatas[i].chunkName + ", " + i);
            //        i++;
            //    }
            //}
            loadChunkDatas.Add(currentLoadedChunk);
        }

        if (loadChunkDatas.Count != 0)
        {
            for (int j = 0; j < loadChunkDatas.Count; j++)
            {
                float distanceBetweenChunk = Vector3.Distance(playerPos, loadChunkDatas[j].chunkPosition);

                if (distanceBetweenChunk < 400)
                {
                    terrains = GameObject.FindGameObjectsWithTag("TerrainSegment");


                    for (int l = 0; l < prefabTerrain.Length; l++)
                    {
                        if (loadChunkDatas[j].chunkName == prefabTerrain[l].gameObject.name)
                        {
                            Instantiate(prefabTerrain[l], loadChunkDatas[j].chunkPosition, loadChunkDatas[j].chunkRotation);
                            File.Delete(JSONFolderLocation + loadChunkDatas[j].chunkName + ".json");
                            loadChunkDatas.Remove(loadChunkDatas[j]);
                        }
                    }

                    //    int existsCounter = 0;

                    //    foreach (GameObject currentTerrain in terrains)
                    //    {
                    //        Debug.Log(loadChunkDatas[j].chunkName + ", " + currentTerrain.gameObject.name);

                    //        if ((loadChunkDatas[j].chunkName == currentTerrain.gameObject.name)
                    //            || (loadChunkDatas[j].chunkName + "(Clone)") == currentTerrain.gameObject.name)
                    //        {
                    //            existsCounter++;
                    //        }
                    //    }

                    //    if (existsCounter == 0)
                    //    {
                    //        for (int l = 0; l < prefabTerrain.Length; l++)
                    //        {
                    //            if (loadChunkDatas[l].chunkName == prefabTerrain[l].gameObject.name)
                    //            {
                    //                Instantiate(prefabTerrain[l], loadChunkDatas[l].chunkPosition, loadChunkDatas[l].chunkRotation);
                    //            }
                    //        }
                    //    }
                    //}
                }
            }
        }
    }
}