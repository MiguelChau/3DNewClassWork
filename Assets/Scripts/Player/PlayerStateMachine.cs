using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chau.StateMachine;

public enum PlayerStates
{
    IDLE,
    RUN,
    JUMPING
}

public class PlayerStateMachine : MonoBehaviour
{
    
    public StateMachine<PlayerStates> stateMachine;

    private void Awake()
    {       
        stateMachine = new StateMachine<PlayerStates>();
    }

    private void Start()
    {
        stateMachine.Init();
        stateMachine.RegisterStates(PlayerStates.IDLE, new IdleState());
        stateMachine.RegisterStates(PlayerStates.RUN, new RunningState());
        stateMachine.RegisterStates(PlayerStates.JUMPING, new JumpingState());

        stateMachine.SwitchState(PlayerStates.IDLE);
    }

    private void Update()
    {
        stateMachine.Update();
    }

    public void MoveForward()
    {
        stateMachine.SwitchState(PlayerStates.RUN);
    }

    public void Stop()
    {
        stateMachine.SwitchState(PlayerStates.IDLE);
    }

    public void Jump()
    {
        stateMachine.SwitchState(PlayerStates.JUMPING);
    }
}
