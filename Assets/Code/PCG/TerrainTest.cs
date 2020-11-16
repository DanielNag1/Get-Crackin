using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.TerrainAPI;


[ExecuteInEditMode]
public class TerrainTest : MonoBehaviour
{
    public TerrainLayer assda;
    Ray ray;
    RaycastHit hit;
    public GameObject test;

    public int surfaceIndex = 0;

    private Terrain terrain;
    private TerrainData terrainData;
    private Vector3 terrainPos;

    public float[] textureValues;

    // Start is called before the first frame update
    void Start()
    {
        terrain = Terrain.activeTerrain;
        terrainData = terrain.terrainData;
        terrainPos = terrain.transform.position;


    }

    // Update is called once per frame
    void Update()
    {
        Physics.Raycast(test.transform.position, Vector3.down, out hit, 500, 1);
        Debug.DrawRay(test.transform.position, Vector3.down * 500, Color.red);

        GetTextureMix(hit.point);


        if (textureValues[0] > 0)
        {
            Debug.Log("dirt");
        }
        if (textureValues[1] > 0)
        {
            Debug.Log("snow");
        }





    }

    void OnGUI()
    {

    }

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



}
