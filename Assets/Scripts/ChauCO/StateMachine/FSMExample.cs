using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chau.StateMachine;
public class FSMExample : MonoBehaviour //FinishStateMachine
{
    public enum ExampleEnum
    {
        STATE_ONE,
        STATE_TWO,
        STATE_THERE
    }

    public StateMachine<ExampleEnum> stateMachine;

    private void Start()
    {
        stateMachine = new StateMachine<ExampleEnum>();
        stateMachine.Init();
        stateMachine.RegisterStates(ExampleEnum.STATE_ONE, new StateBase());
        stateMachine.RegisterStates(ExampleEnum.STATE_TWO, new StateBase());
        
    }
}
