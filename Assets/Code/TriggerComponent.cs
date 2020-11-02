using UnityEngine;
using System.Collections;
using System;

public class TriggerComponent : MonoBehaviour
{
    [SerializeField] private TriggerDebug triggerDebug; //The script that you want to run, Can be multiple
    [SerializeField] private string SoundPath; //The sound to be played when the trigger is activated
    [SerializeField] private float volumeScale = 1; //volume scale
    [SerializeField] private GameObject SoundObjectPrefab; //Creates a temporary SoundObject on the location of the trigger that destroys itself when done.

    //Parameters that you want to send to the methods the trigger calls.
    [SerializeField] private string parameterItem1;
    [SerializeField] private int parameterItem2;
    [SerializeField] private string parameterItem3;
    [SerializeField] private bool parameterItem4;

    public void ActivateTrigger() //Activates the trigger when CharacterController collides with trigger hitbox.
    {
        triggerDebug.Fuu(parameterItem1, parameterItem2, parameterItem3, parameterItem4); //Method in script to be called.
        Debug.Log("TriggerComponent:SoundPath = " + SoundPath);
        GameObject temp = Instantiate(SoundObjectPrefab, this.transform.position, Quaternion.identity); //Creates the temporary SoundObject
        SoundComponent tempComponent = temp.GetComponent<SoundComponent>(); //Gets the temporary SoundObjects SoundComponent.
        tempComponent.soundPath = SoundPath; //Assignes the correct sound to the SoundComponent.
        tempComponent.volumeScale = volumeScale;//Assignes the correct soundVolume to the SoundComponent.
        Destroy(this.gameObject); //Destroyes the trigger object, freeing up resources and avoiding the trigger beeing activated twice.
    }
}
