using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAreaSpawner : MonoBehaviour
{
    public GameObject itemToSpread;
    public int numItemsToSpawn = 10;

    public float itemXspread = 10;
    public float itemYspread = 0;
    public float itemZspread = 10;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < numItemsToSpawn; i++)
        {
            SpreadItem();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpreadItem()
    {
        Vector3 randPosition = new Vector3(Random.Range(-itemXspread, itemXspread), Random.Range(-itemYspread, itemYspread), Random.Range(-itemZspread, itemZspread));
        GameObject clone = Instantiate(itemToSpread, randPosition, Quaternion.identity);
    }
}
