using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatterCrate : MonoBehaviour
{
    #region Singleton
    public static ShatterCrate Instance;
    //private List<GameObject> _crateList = new List<GameObject>();
    //public List<GameObject> spawnPoints = new List<GameObject>();
  
    private void Awake()
    {
        Instance = this;
       
    }
    private void Start()
    {
        //clone_whole = gameObject.GetComponent<GameObject>();
        // clone_whole = gameObject.GetComponent<Transform>().gameObject;

    }
    #endregion

    #region Variables
   // public GameObject crate_whole;
    public GameObject crate_shattered;
    private GameObject clone_shattered;
    private GameObject clone_whole;
    private float waitToDestroy = 5f;
    #endregion
  
    /// <summary>
    /// Creates a clone of the shattered object, destroys the whole one and destroys the shattered object after 5 seconds.
    /// </summary>
    public void DestroyCrate()
    {
        clone_shattered = Instantiate(crate_shattered, transform.position, transform.rotation);
        Destroy(this.gameObject);
        Destroy(clone_shattered, waitToDestroy);
    }

}
