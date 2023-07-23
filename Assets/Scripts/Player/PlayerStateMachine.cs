using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine;
using Chau.StateMachine;

public enum PlayerStates
{
    IDLE,
    RUN,
    JUMPING,
    DEATH
}

[Serializable]
public class PlayerStateMachine : StateMachine<PlayerStates>
{

    public override void Init()
    {
        base.Init();
        RegisterStates(PlayerStates.IDLE, new IdleState());
        RegisterStates(PlayerStates.RUN, new RunningState());
        RegisterStates(PlayerStates.JUMPING, new JumpingState());
        RegisterStates(PlayerStates.DEATH, new DeathState());

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

    public void Death(HealthBase h)
    {
        SwitchState(PlayerStates.DEATH);
    }
}

