using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDebug : MonoBehaviour
{
    public void Fuu(string a, int b, string c, bool d)
    {
        Debug.Log("Trigger call Success, Parameter value = " + a + b + c + d);
    }
}
