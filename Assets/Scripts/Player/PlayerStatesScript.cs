using Chau.StateMachine;
using UnityEngine;

public class IdleState : StateBase
{
    public override void OnStateEnter(object o = null)
    {
        Debug.Log("Entering Idle State");

    }

    public override void OnStateStay()
    {
        Debug.Log("Idle State");

    }

    public override void OnStateExit()
    {
        Debug.Log("Exiting Idle State");

    }
}

public class RunningState : StateBase
{
    public override void OnStateEnter(object o = null)
    {
        Debug.Log("Entering Running State");

    }

    public override void OnStateStay()
    {
        Debug.Log("Running  State");

    }

    public override void OnStateExit()
    {
        Debug.Log("Exiting Running  State");

    }
}

public class JumpingState : StateBase
{
    public override void OnStateEnter(object o = null)
    {
        Debug.Log("Entering Jumping State");

    }

    public override void OnStateStay()
    {
        Debug.Log("Jumping State");

    }

    public override void OnStateExit()
    {
        Debug.Log("Exiting Jumping State");

    }
}
