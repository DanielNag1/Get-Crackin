using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class TriggerComponent : MonoBehaviour
{
    private LevelManager levelManager;
    private EnemyManager enemyManager;
    private SaveFileManager saveFileManager;

    [SerializeField] private string SoundPath; //The sound to be played when the trigger is activated
    [SerializeField] private float volumeScale = 1; //volume scale
    [SerializeField] private GameObject SoundObjectPrefab; //Creates a temporary SoundObject on the location of the trigger that destroys itself when done.
    [SerializeField] private List<string> ClassesToBeCalled;

    //Parameters that you want to send to the methods the trigger calls.
    [SerializeField] private int setToUse;

    public void Start()
    {
        levelManager = LevelManager.Instance;
        enemyManager = EnemyManager.Instance;
        saveFileManager = SaveFileManager.Instance;
    }

    public void ActivateTrigger() //Activates the trigger when CharacterController collides with trigger hitbox.
    {
        for (int i = 0; ClassesToBeCalled.Count > i; ++i)
        {
            if (ClassesToBeCalled[i] == "LevelManager")
            {
                levelManager.LoadNextLevel();
                continue;
            }
            else if (ClassesToBeCalled[i] == "EnemyManager")
            {
                enemyManager.SpawnEnemyFromTrigger(setToUse);
                continue;
            }
        }

        GameObject temp = Instantiate(SoundObjectPrefab, this.transform.position, Quaternion.identity); //Creates the temporary SoundObject
        SoundComponent tempComponent = temp.GetComponent<SoundComponent>(); //Gets the temporary SoundObjects SoundComponent.
        tempComponent.soundPath = SoundPath; //Assignes the correct sound to the SoundComponent.
        tempComponent.volumeScale = volumeScale;//Assignes the correct soundVolume to the SoundComponent.
        Destroy(this.gameObject); //Destroyes the trigger object, freeing up resources and avoiding the trigger beeing activated twice.
    }
}
