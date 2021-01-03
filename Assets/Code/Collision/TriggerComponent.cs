using System.Collections.Generic;
using UnityEngine;

public class TriggerComponent : MonoBehaviour
{
    #region Variables
    private EnemyManager _enemyManager;
    [SerializeField] private bool _destroyOnTriggerActive;
    [SerializeField] private string _soundPath; //The sound to be played when the trigger is activated
    [SerializeField] private float _volumeScale = 1; //volume scale
    [SerializeField] private GameObject _soundObjectPrefab; //Creates a temporary SoundObject on the location of the trigger that destroys itself when done.
    [SerializeField] private List<string> _classesAndMethodsToBeCalled;
    //Parameters that you want to send to the methods the trigger calls.
    [SerializeField] private List<int> _setToUse;
    [SerializeField] private List<string> _arenaFinishedClassesAndMethodsToBeCalled; //Can add a specific break character to know when one arena phase is done, so as to allow more then two waves of enemies in one arena.
    [SerializeField] private List<GameObject> _objectToMove;
    [SerializeField] private LoadLevel LoadLevel;
    #endregion

    #region Methods
    public void Start()
    {
        _enemyManager = EnemyManager.Instance;
    }

    #region Get/SET
    public List<string> ClassesAndMethodsToBeCalled
    {
        get { return _classesAndMethodsToBeCalled; }
        set { _classesAndMethodsToBeCalled = value; }
    }
    public List<string> ArenaFinishedClassesAndMethodsToBeCalled
    {
        get { return _arenaFinishedClassesAndMethodsToBeCalled; }
        set { _arenaFinishedClassesAndMethodsToBeCalled = value; }
    }
    public List<GameObject> ObjectToMove
    {
        get { return _objectToMove; }
        set { _objectToMove = value; }
    }
    public List<int> SetToUse
    {
        get { return _setToUse; }
        set { _setToUse = value; }
    }
    #endregion

    //Use the unity list to specify when something happens, We assume that you want spawns and objects to be done in the order of input.
    public void ActivateTrigger() //Activates the trigger when CharacterController collides with trigger hitbox.
    {
        for (int i = 0; _classesAndMethodsToBeCalled.Count > i; ++i)
        {
            if (_classesAndMethodsToBeCalled[i] == "EnemyManager.SpawnEnemyFromTrigger")
            {
                _enemyManager.SpawnEnemyFromTrigger(_setToUse[0]);
                _setToUse.RemoveAt(0);
                continue;
            }
            if (_classesAndMethodsToBeCalled[i] == "EnemyManager.StartArenaFight")
            {
                _enemyManager.StartArenaFight(_arenaFinishedClassesAndMethodsToBeCalled, _setToUse, _objectToMove);
                continue;
            }
            //Assuming we move enviroment objects using animations.
            if (_classesAndMethodsToBeCalled[i] == "MoveGameObject")
            {
                _objectToMove[0].GetComponent<Animator>().SetBool("Move", true/*!_objectToMove[0].GetComponent<Animator>().GetBool("Move")*/);
                _objectToMove.RemoveAt(0);
                continue;
            }
            if (_classesAndMethodsToBeCalled[i] == "LoadLevel.LoadNextLevel")
            {
                LoadLevel.LoadNextLevel();
                continue;
            }
            
            if (_classesAndMethodsToBeCalled[i] == "Break") //can be done in EnemyManager.StartArenaFight, IF we make sure to place everything in the correct order with EnemyManager.StartArenaFight beeing last!
            {
                break;
            }
        }
        if (_soundPath != null || _soundPath != "")
        {
            GameObject temp = Instantiate(_soundObjectPrefab, this.transform.position, Quaternion.identity); //Creates the temporary SoundObject
            SoundComponent tempComponent = temp.GetComponent<SoundComponent>(); //Gets the temporary SoundObjects SoundComponent.
            tempComponent.soundPath = _soundPath; //Assignes the correct sound to the SoundComponent.
            tempComponent.volumeScale = _volumeScale;//Assignes the correct soundVolume to the SoundComponent.        
        }
        if (_destroyOnTriggerActive)
        {
            Destroy(this.gameObject); //Destroyes the trigger object, freeing up resources and avoiding the trigger beeing activated twice.
        }
    }
    #endregion
}
