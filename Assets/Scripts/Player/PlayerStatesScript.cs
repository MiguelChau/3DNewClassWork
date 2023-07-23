using Chau.StateMachine;
using UnityEngine;

public class IdleState : StateBase
{
    public override void OnStateEnter(params object[] objs)
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
    public override void OnStateEnter(params object[] objs)
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
    public override void OnStateEnter(params object[] objs)
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
public class DeathState : StateBase
{
    public override void OnStateEnter(params object[] objs)
    {
        Debug.Log("Entering Death State");

    }

    public override void OnStateStay()
    {
        Debug.Log("Death State");

    }

    public override void OnStateExit()
    {
        Debug.Log("Exiting Death State");

    }
}
