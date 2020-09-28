using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine
{
    private IState _currentState;

    private Dictionary<Type, List<Transition>> _transitions = new Dictionary<Type, List<Transition>>(); //Type = classType(key). List<Transition = Value. => For every type, there is a list of possible transitions.
    private List<Transition> _currentTransitions = new List<Transition>();  //switching out which transition is current based on our state.
    private List<Transition> _aTransition = new List<Transition>();
    private static List<Transition> EmptyTransition = new List<Transition>(0);

    public void TimeTick()
    {
        var transiton = GetTransition(); //looks for a transition, if it gets a transition back...
        if (transiton != null)
        {
            SetState(transiton.NewState); //..set the state to the next state.
        }
        //If we dont find any transition, we have returned a null and then we just let it Tick. We only want to set a state which is not a null state.
        _currentState?.TimeTick(); //THIS IS WHERE THE MAGIC HAPPENS! Tell the current state to Tick.
    }

    public void SetState(IState state)
    {
        // Check to see if the state that we passed in, not is our current state. return if it is.
        if (state == _currentState)
        {
            return;
        }

        _currentState?.OnExit();  //Did we already have a previous State? If we did OnExit is called for that state.
        _currentState = state;  //Set current State to the state that was passed in. (so when we tick, we will be ticking against that current state).

        _transitions.TryGetValue(_currentState.GetType(), out _currentTransitions); //Asks the dictionary(_transitions) to return the list<transition> for this Type(the key) and put it into _currentTransitions.
        if (_currentTransitions == null)
        {
            //If we dont find anything, give us the empty list so we can keep iterate without getting any null errors. Keep us from allocating any extra memory.
            _currentTransitions = EmptyTransition;
        }

        _currentState.OnEnter();
    }

    /// <summary>
    ///  <paramref name="previousState: "/> FROM. 
    ///  <paramref name="newState: "/> TO. 
    ///  <paramref name="predicate: "/> Need a function that we can give this which is going to tell us when we actually need to do the transition.
    /// </summary>
    public void AddTransition(IState previousState, IState newState, Func<bool> predicate)
    {
        //Try to get back the list of transitions from the previous state, if we dont...
        if (_transitions.TryGetValue(previousState.GetType(), out var transitions) == false)
        {
            //...if we dont have a list<transitions< for that state(previous state), we create a new list.
            transitions = new List<Transition>();
            _transitions[previousState.GetType()] = transitions; //and add it into our dictionary.
        }

        transitions.Add(new Transition(newState, predicate));
    }

    private class Transition
    {
        public Func<bool> Condition { get; }  //Returns a bool

        public IState NewState { get; }  //The state we're going to transition to.

        public Transition(IState newState, Func<bool> condition)
        {
            NewState = newState;
            Condition = condition;
        }
    }

    public void AddAnyTransition(IState state, Func<bool> predicate)
    {
        _aTransition.Add(new Transition(state, predicate));
    }

    private Transition GetTransition()
    {
        //loop through anyTrnsitions (The transitions comming from any state because they dont have a from state).
        foreach (var transition in _aTransition)
        {
            if(transition.Condition())  //does the condition return true?
            {
                return transition;
            }
        }

        //if we didnt have a anyTransition or didnt have any need for it right now then we go through our current transitions.
        foreach (var transition in _currentTransitions)
        {
            if(transition.Condition())
            {
                return transition;
            }
        }

        return null;
    }

}
