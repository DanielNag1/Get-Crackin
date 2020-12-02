using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    public Text TextUI;
    private string textToShow;
  
    private void Awake()
    {
        textToShow = "The rage inside of me.. it's unbearable. One year ago today, a peasant killed my family. I managed to escape but at what cost? .... " +
            "Im all alone, surrounded by the same trees day after day. " +
            "The foxes here are vicious .... I must escape this forest!";
    }

    private void Start()
    {
        TextWriter.AddWriterStatic(TextUI, textToShow, 0.05f, true);
    }
}
