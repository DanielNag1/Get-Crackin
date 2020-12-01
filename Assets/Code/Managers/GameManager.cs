using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private LockToTarget LockToTarget;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject player;

    private void Awake()
    {
        int SeedVal = (int)Random.Range(0, 9.99f);
        int seed = -1;
        switch (SeedVal)
        {
            case 0:
                seed = 0;
                InputSave.Instance.SaveSeed(seed);
                Random.InitState(seed);
                break;
            case 1:
                seed = 1;
                InputSave.Instance.SaveSeed(seed);
                Random.InitState(seed);
                break;
            case 2:
                seed = 2;
                InputSave.Instance.SaveSeed(seed);
                Random.InitState(seed);
                break;
            case 3:
                seed = 3;
                InputSave.Instance.SaveSeed(seed);
                Random.InitState(seed);
                break;
            case 4:
                seed = 4;
                InputSave.Instance.SaveSeed(seed);
                Random.InitState(seed);
                break;
            case 5:
                seed = 5;
                InputSave.Instance.SaveSeed(seed);
                Random.InitState(seed);
                break;
            case 6:
                seed = 6;
                InputSave.Instance.SaveSeed(seed);
                Random.InitState(seed);
                break;
            case 7:
                seed = 7;
                InputSave.Instance.SaveSeed(seed);
                Random.InitState(seed);
                break;
            case 8:
                seed = 8;
                InputSave.Instance.SaveSeed(seed);
                Random.InitState(seed);
                break;
            case 9:
                seed = 9;
                InputSave.Instance.SaveSeed(seed);
                Random.InitState(seed);
                break;
            default:
                seed = 10;
                InputSave.Instance.SaveSeed(seed);
                Random.InitState(seed);
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        InputBuffer.Instance.LockToTarget = LockToTarget;
        InputBuffer.Instance.animator = animator;
        InputBuffer.Instance.player = player;
        InputSave.Instance.LockToTarget = LockToTarget;
    }

    // Update is called once per frame
    void Update()
    {
        InputManager.Instance.Update();
        SoundEngine.Instance.Update();
        EnemyManager.Instance.Update();
    }
}
