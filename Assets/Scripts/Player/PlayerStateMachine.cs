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
    DEATH,
    ATTACK
}

[Serializable]
public class PlayerStateMachine : StateMachine<PlayerStates>
{
    private Animator _animator;
    public override void Init()
    {
        
        base.Init();
        RegisterStates(PlayerStates.IDLE, new IdleState());
        RegisterStates(PlayerStates.RUN, new RunningState());
        RegisterStates(PlayerStates.JUMPING, new JumpingState());
        RegisterStates(PlayerStates.DEATH, new DeathState());
        RegisterStates(PlayerStates.ATTACK, new AttackState());

        SwitchState(PlayerStates.IDLE);
    }    

    public PlayerStateMachine(Animator animator)
    {
        _animator = animator;
        Init();
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
    public void AttackMagic()
     {
        SwitchState(PlayerStates.ATTACK);
     }

    public void Death(HealthBase h)
    {
        SwitchState(PlayerStates.DEATH);
        if(_animator != null)
        {
            _animator.SetTrigger("Death");
        }
    }

    public void Revive()
    {
        SwitchState(PlayerStates.IDLE);
        if (_animator != null)
        {
            _animator.SetTrigger("Revive");
        }
    }
}

