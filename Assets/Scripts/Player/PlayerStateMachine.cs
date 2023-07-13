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

public class PlayerStateMachine : StateMachine<PlayerStates>
{

    public override void Init()
    {
        base.Init();
        RegisterStates(PlayerStates.IDLE, new IdleState());
        RegisterStates(PlayerStates.RUN, new RunningState());
        RegisterStates(PlayerStates.JUMPING, new JumpingState());

        SwitchState(PlayerStates.IDLE);
    }    
        

     public void MoveForward()
     {
        SwitchState(PlayerStates.RUN);
     }

     public void Stop()
     {
        SwitchState(PlayerStates.IDLE);
     }

     public void Jump()
     {
        SwitchState(PlayerStates.JUMPING);
     }
}

