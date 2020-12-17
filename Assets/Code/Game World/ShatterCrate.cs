using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatterCrate : MonoBehaviour
{
    #region Singleton
    public static ShatterCrate Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public GameObject crate_shattered;
    private float transitionTime = 1f;
    public void DestroyCrate()
    {
        Instantiate(crate_shattered, transform.position, transform.rotation);
        Destroy(gameObject);
      //  StartCoroutine(FadeCrate());
    }

    //IEnumerator FadeCrate()
    //{
    //    for (var i = crate_shattered.transform.childCount - 1; i >= 0; i--)
    //    {
    //        var objA = crate_shattered.transform;
    //        var obj = crate_shattered.transform.GetChild(i);
    //        obj.transform.parent = null;
            
    //        Destroy(obj);
    //    }
    //    Destroy(crate_shattered.transform.root);
   
    //    yield return new WaitForSeconds(transitionTime);
    //}
}
