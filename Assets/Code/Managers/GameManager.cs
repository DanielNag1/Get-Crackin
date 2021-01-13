﻿using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Variables
    [SerializeField] private LockToTarget LockToTarget;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject player;
    #endregion

    #region Methods
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        //Used to "generate" and save seed value for the Random class, needed for playback ability in logging tool
        #region Init Seed
        int SeedVal = (int)Random.Range(0, 9.99f);
        int seed = -1;
        switch (SeedVal)
        {
            case 0:
                seed = 0;
                Random.InitState(seed);
                break;
            case 1:
                seed = 1;
                Random.InitState(seed);
                break;
            case 2:
                seed = 2;
                Random.InitState(seed);
                break;
            case 3:
                seed = 3;
                Random.InitState(seed);
                break;
            case 4:
                seed = 4;
                Random.InitState(seed);
                break;
            case 5:
                seed = 5;
                Random.InitState(seed);
                break;
            case 6:
                seed = 6;
                Random.InitState(seed);
                break;
            case 7:
                seed = 7;
                Random.InitState(seed);
                break;
            case 8:
                seed = 8;
                Random.InitState(seed);
                break;
            case 9:
                seed = 9;
                Random.InitState(seed);
                break;
            default:
                seed = 10;
                Random.InitState(seed);
                break;
        }
        #endregion
    }

    void Start()
    {
        InputBuffer.Instance.lockToTarget = LockToTarget;
        InputBuffer.Instance.animator = animator;
        InputBuffer.Instance.player = player;
    }

    void Update()
    {
        InputManager.Instance.Update();
        SoundEngine.Instance.Update();
        EnemyManager.Instance.Update();
    }
    #endregion
}
