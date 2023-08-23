using Chau.StateMachine;
using UnityEngine;

public class GMStateIntro : StateBase
{
  
}

public class GMStateSave: StateBase
{
    public override void OnStateEnter(params object[] objs)
    {
        base.OnStateEnter();
        SaveManager.Instance.ShowSavescreen();
    }

    public override void OnStateExit()
    {
        base.OnStateExit();      
        SaveManager.Instance.HideSavescreen();
    }

    public override void OnStateStay()
    {
        base.OnStateStay();
        if (Input.GetKeyDown(KeyCode.S)) 
        {
            SaveManager.Instance.SaveItems();
            GameManager.Instance.TransitionToGameplayState();
        }
    }
}
