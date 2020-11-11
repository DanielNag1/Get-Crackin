using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveFileManager : MonoBehaviour
{

    #region Singleton
    public static SaveFileManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion


    public void SaveGame()
    {
        /*
         Current scene
        ---------------
        Player
        ---------------
        Position, Take this value from the checkpoint
        Rotation, Take this value from the checkpoint
        Current health
        
        ---------------
        UI
        ---------------
        add text and animation for user to know we just saved
         */
    }
    public void LoadGame()
    {
        /*
        ---------------
        UI
        ---------------
        Load from black
         */
    }
}
