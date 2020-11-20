using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGrid : MonoBehaviour
{
    public float[,] Grid;
    private float Vertical, Horizontal;
    [Range(1, 30)]
    public float Columns, Rows;
    [Range(1, 50)]
    public int horizontalFrequence, verticalFrequence;
    public TerrainData terrainData;
    public Sprite picture;
    public List<GameObject> losListos;
    void Start()
    {
        losListos = new List<GameObject>();
        Horizontal = terrainData.size.x;
        Vertical = terrainData.size.z;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            SpawnGrid();
        }
    }

    void SpawnGrid()
    {
        foreach (var g in losListos)
        {
            Destroy(g);
        }
        losListos.Clear();
        Grid = new float[(int)Columns, (int)Rows];

        for (int i = 0; i < Columns; i++)
        {
            for (int j = 0; j < Rows; j++)
            {
                SpawnTile(i * horizontalFrequence, j * verticalFrequence);
            }
        }
    }
    void SpawnTile(int x, int y)
    {
        if (x < Horizontal && y < Vertical)
        {
            GameObject g = new GameObject("X: " + x + "Y: " + y);
            var s = g.AddComponent<SpriteRenderer>();
            s.sprite = picture;
            g.transform.position = new Vector3(x, 100, y);
            g.transform.localScale = new Vector3(100, 100, 100);
            g.transform.Rotate(90, 0, 0);
            losListos.Add(g);
        }
    }
}

