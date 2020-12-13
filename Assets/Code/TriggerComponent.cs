using System.Collections.Generic;
using UnityEngine;

public class TriggerComponent : MonoBehaviour
{
    #region Variables
    private EnemyManager _enemyManager;
    [SerializeField] private string _soundPath; //The sound to be played when the trigger is activated
    [SerializeField] private float _volumeScale = 1; //volume scale
    [SerializeField] private GameObject _soundObjectPrefab; //Creates a temporary SoundObject on the location of the trigger that destroys itself when done.
    [SerializeField] private List<string> _classesToBeCalled;
    //Parameters that you want to send to the methods the trigger calls.
    [SerializeField] private int _setToUse;
    #endregion

    #region Methods
    public void Start()
    {
        _enemyManager = EnemyManager.Instance;
    }

    public void ActivateTrigger() //Activates the trigger when CharacterController collides with trigger hitbox.
    {
        for (int i = 0; _classesToBeCalled.Count > i; ++i)
        {
            if (_classesToBeCalled[i] == "EnemyManager")
            {
                _enemyManager.SpawnEnemyFromTrigger(_setToUse);
                continue;
            }
        }

        GameObject temp = Instantiate(_soundObjectPrefab, this.transform.position, Quaternion.identity); //Creates the temporary SoundObject
        SoundComponent tempComponent = temp.GetComponent<SoundComponent>(); //Gets the temporary SoundObjects SoundComponent.
        tempComponent.soundPath = _soundPath; //Assignes the correct sound to the SoundComponent.
        tempComponent.volumeScale = _volumeScale;//Assignes the correct soundVolume to the SoundComponent.
        Destroy(this.gameObject); //Destroyes the trigger object, freeing up resources and avoiding the trigger beeing activated twice.
    }
    #endregion
}
