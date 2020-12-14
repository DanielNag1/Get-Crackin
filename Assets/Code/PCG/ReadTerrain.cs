using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.TerrainAPI;
using UnityEditor;

public class ReadTerrain : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;

    private Terrain terrain;
    private TerrainData terrainData;
    private Vector3 terrainPos;
    [Header("Texture Array Size")]
    public float[] textureValues;

    public CreateGrid grid;

    public GameObject[] objectsToSpawn;
    public string fileName;

    private List<GameObject> spawnedObjects;
    // Start is called before the first frame update
    void Start()
    {
        terrain = Terrain.activeTerrain;
        terrainData = terrain.terrainData;
        terrainPos = terrain.transform.position;
        spawnedObjects = new List<GameObject>();
    }

    // Update is called once per frame


    //Physics.Raycast(test.transform.position, Vector3.down, out hit, 500, 1);
    //Debug.DrawRay(test.transform.position, Vector3.down * 500, Color.red);

    private void GetTextureMix(Vector3 WorldPos)
    {
        // returns an array containing the relative mix of textures
        // on the main terrain at this world position.

        // The number of values in the array will equal the number
        // of textures added to the terrain.

        // calculate which splat map cell the worldPos falls within (ignoring y)
        int mapX = (int)(((WorldPos.x - terrainPos.x) / terrainData.size.x) * terrainData.alphamapWidth);
        int mapZ = (int)(((WorldPos.z - terrainPos.z) / terrainData.size.z) * terrainData.alphamapHeight);

        // get the splat data for this cell as a 1x1xN 3d array (where N = number of textures)
        float[,,] splatmapData = terrainData.GetAlphamaps(mapX, mapZ, 1, 1);

        textureValues[0] = splatmapData[0, 0, 0];
        textureValues[1] = splatmapData[0, 0, 1];
        //textureValues[2] = splatmapData[0, 0, 2];

    }

    private void CreateTree(Vector3 position, Quaternion rotation)
    {
        float scale = Random.Range(1, 2.5f);
        var objectToSpawn = objectsToSpawn[Random.Range(0, objectsToSpawn.Length)];

        GameObject spawnedObject = Instantiate(objectToSpawn, position, rotation, transform);

        //spawnedObject.transform.localScale = new Vector3(scale, scale, scale);
        spawnedObjects.Add(spawnedObject);
    }

    public void SpawnTrees()
    {
        grid.SpawnGrid(); //Spawnar en grid

        foreach (var g in spawnedObjects) //Tar bort spawnpoints
        {
            DestroyImmediate(g);
        }
        spawnedObjects.Clear(); //Tar bort spawnpoints listan

        foreach (var g in grid.positionList) //Gör en raycast från varje spawnpoint och antingen skapar ett träd på hit punkten eller tar bort spawnpointen
        {
            Physics.Raycast(g.transform.position, Vector3.down, out hit, 500, 1);
            //Debug.DrawRay(g.transform.position, Vector3.down * 500, Color.red);

            if (hit.point != null)
            {
                GetTextureMix(hit.point);

                if (textureValues[1] > 0) //Snow är större än 0
                {
                    DestroyImmediate(g);
                }
                else
                {
                    CreateTree(hit.point, hit.transform.rotation); //Placerar ut ett träd på hit punkten.
                }
            }
        }

        foreach (var g in grid.positionList)
        {
            DestroyImmediate(g);
        }

        grid.positionList.Clear();
    }
    public void SaveTrees()
    {
        string localPath = "Assets/Prefabs/Maps/ " + fileName + ".prefab";
        localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);
        PrefabUtility.SaveAsPrefabAssetAndConnect(gameObject, localPath, InteractionMode.UserAction);
    }

}
