using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;


namespace Chau.StateMachine
{
    [Serializable]
    public class StateMachine<T> where T : System.Enum 
    {

    
    public Dictionary<T, StateBase> dictionaryState; 

    private StateBase _currentState;

    private T _currentStateType;

    public float timetoStartGame = 1f;

        public T CurrentStateType
        {
            get
            {
                return _currentStateType;
            }
        }
    
    public StateBase CurrentState
    {
        get { return _currentState; }
    }

    public virtual void Init()
    {
       dictionaryState = new Dictionary<T, StateBase>();
    }
    public void RegisterStates(T typeEnum,  StateBase state)
    {
       dictionaryState.Add(typeEnum, state); 
        
    }

    public void SwitchState(T state, params object[] objs)
    {
        if (_currentState != null) _currentState.OnStateExit();

        _currentState = dictionaryState[state];
        _currentStateType = state;

        _currentState.OnStateEnter(objs);
        
    }

    public void Update()
    {
        if (_currentState != null) _currentState.OnStateStay();
    }

    }
}
