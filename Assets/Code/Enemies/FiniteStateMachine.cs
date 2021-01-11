using System;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine
{
    #region Variables
    private IState _currentState;
    private Dictionary<IState, List<Transition>> _transitions = new Dictionary<IState, List<Transition>>(); //Type = classType(key). List<Transition = Value. => For every type, there is a list of possible transitions.
    private List<Transition> _currentTransitions = new List<Transition>();  //switching out which transition is current based on our state.
    private List<Transition> _aTransition = new List<Transition>();
    private static List<Transition> _emptyTransition = new List<Transition>(0);
    #endregion

    #region Methods
    public void TimeTick(int id)
    {
        var transiton = GetTransition(); //looks for a transition, if it gets a transition back...
        if (transiton != null)
        {
            SetState(transiton.NewState); //..set the state to the next state.
        }
        //If we dont find any transition, we have returned a null and then we just let it Tick. We only want to set a state which is not a null state.
        _currentState?.TimeTick(); //THIS IS WHERE THE MAGIC HAPPENS! Tell the current state to Tick.
        //Debug.Log(id + " has state: " + _currentState);
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
        _currentTransitions = _transitions[_currentState]; //Asks the dictionary(_transitions) to return the list<transition> for this Type(the key) and put it into _currentTransitions.
        if (_currentTransitions == null)
        {
            //If we dont find anything, give us the empty list so we can keep iterate without getting any null errors. Keep us from allocating any extra memory.
            _currentTransitions = _emptyTransition;
        }
        _currentState.OnEnter();
    }

    /// <summary>
    ///  <paramref name="previousState: "/> FROM. 
    ///  <paramref name="newState: "/> TO. 
    ///  <paramref name="predicate: "/> Need a function that we can give this which is going to tell us when we actually need to do the transition.
    /// </summary>
    public void AddTransition(IState previousState/*Key*/, IState newState, Func<bool> predicate)
    {
        //Try to get back the list of transitions from the previous state, if we dont...
        //Check dictionary for the list belonging to previousState, if it does not exist we create it.
        if (!_transitions.ContainsKey(previousState))
        {
            List<Transition> transitions = new List<Transition>(1);
            _transitions[previousState] = transitions;
        }
        _transitions[previousState].Add(new Transition(newState, predicate));
    }

    public void AddAnyTransition(IState state, Func<bool> predicate)
    {
        _aTransition.Add(new Transition(state, predicate));
    }

    private Transition GetTransition()
    {
        //loop through anyTransitions (The transitions comming from any state because they dont have a from state).
        foreach (var transition in _aTransition)
        {
            //Added && _currentState != transition.NewState, as it lead to problems with not beein able to leave it's own state.
            if (transition.Condition() && _currentState != transition.NewState)  //does the condition return true? 
            {
                return transition;
            }
        }
        //if we didnt have a anyTransition or didnt have any need for it right now then we go through our current transitions.
        foreach (var transition in _currentTransitions)
        {
            if (transition.Condition())
            {
                return transition;
            }
        }
        return null;
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
    #endregion
}
