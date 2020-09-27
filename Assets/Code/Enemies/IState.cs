using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    void TimeTick();

    void OnEnter();

    void OnExit();
}
