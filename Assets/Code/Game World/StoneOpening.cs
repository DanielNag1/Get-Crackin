using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneOpening : MonoBehaviour
{
    #region Singleton
    public static StoneOpening Instance;

    private void Awake()
    {
        Instance = this;

    }
    #endregion
    #region Variables
    public GameObject stoneOpening_shattered;
    private GameObject clone_shattered;

    private float waitToDestroy = 2f;
    #endregion

    /// <summary>
    /// Creates a clone of the shattered object, destroys the whole one and replace it with the shattered one.
    public void DestroyStone()
    {
        clone_shattered = Instantiate(stoneOpening_shattered, transform.position, transform.rotation);
        Destroy(this.gameObject);
  
    }

}
