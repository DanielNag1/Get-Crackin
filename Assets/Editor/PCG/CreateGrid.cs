using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGrid : MonoBehaviour
{
    public float[,] Grid;
    private float Vertical, Horizontal;
    [Header("Grid Settings")]
    [Range(1, 50)]
    public int horizontalFrequence;
    [Range(1, 50)]
    public int verticalFrequence;
    [Range(1, 500)]
    public float Columns;
    [Range(1, 500)]
    public float Rows;
    [Header("Noise Settings")]
    [Range(1, 10)]
    public float noiseScale;
    [Range(1, 100)]
    public float seed;

    [Header("Size of Terrain")]
    public TerrainData terrainData;

    [HideInInspector]
    public Sprite picture;
    public List<GameObject> positionList;

    void Start()
    {
        positionList = new List<GameObject>();
        Horizontal = terrainData.size.x;
        Vertical = terrainData.size.z;
    }

    public void SpawnGrid()
    {
        foreach (var g in positionList)
        {
            DestroyImmediate(g);
        }
        //positionList.Clear();

        Grid = new float[(int)Columns, (int)Rows];

        for (int i = 0; i < Columns; i++)
        {
            for (int j = 0; j < Rows; j++)
            {
                SpawnTile(i * horizontalFrequence, j * verticalFrequence);
            }
        }
    }

    Vector3 ApplyNoise(float x, float y)
    {
        float xCoord = seed * x / Columns;
        float yCoord = seed * y / Rows;
        float sample = Mathf.PerlinNoise(xCoord, yCoord);
        return new Vector3(x * sample * noiseScale, 100, y * sample * noiseScale);
    }

    void SpawnTile(int x, int y)
    {
        if (x < Horizontal && y < Vertical && x > 0 && y > 0) //Tar bort från grid om den är utanför banan
        {
            GameObject g = new GameObject("X: " + x + "Y: " + y);
            var s = g.AddComponent<SpriteRenderer>();
            s.sprite = picture;
            //g.transform.position = new Vector3(x, 100, y);

            g.transform.position = ApplyNoise(x, y);

            if (g.transform.position.x < Horizontal && g.transform.position.z < Vertical
                && g.transform.position.x > 0 && g.transform.position.z > 0) //Tar bort från noise om den är utanför banan
            {
                g.transform.Rotate(90, 0, 0);
                positionList.Add(g);
            }
            else
            {
                DestroyImmediate(g);
            }
        }
    }
}

