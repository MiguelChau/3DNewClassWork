using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chau.StateMachine;


namespace Boss
{
    public class BossStateMachine: StateBase
    {
        protected BossBase boss;

        public override void OnStateEnter(params object[] objs)
        {
            base.OnStateEnter(objs);
            boss = (BossBase)objs[0];
        }

    }

    public class BossStateInit : BossStateMachine
    {
        public override void OnStateEnter(params object[] objs)
        {
            base.OnStateEnter(objs);
            boss.StartInitAnimation();

        }


    }
    public class BossStateWalk : BossStateMachine
    {
        public override void OnStateEnter(params object[] objs)
        {
            base.OnStateEnter(objs);
            boss.GoToRandomPoint();

        }

        private void OnArrive()
        {
            boss.SwitchState(BossAction.PREPARE_ATTACK);
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            boss.StopAllCoroutines();
        }
    }
    public class BossStatePrepareAttack : BossStateMachine
    {
        public override void OnStateEnter(params object[] objs)
        {
            base.OnStateEnter(objs);
            boss.PrepareAttack();

        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            boss.StopAllCoroutines();
        }
    }  
    
    public class BossStateChargeAttack : BossStateMachine
    {
        public override void OnStateEnter(params object[] objs)
        {
            base.OnStateEnter(objs);
            boss.ChargeMelee();

        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            boss.StopAllCoroutines();
        }
    }

    public class BossStateDeath : BossStateMachine
    {
        public override void OnStateEnter(params object[] objs)
        {
            base.OnStateEnter(objs);
            boss.transform.localScale = Vector3.one * .2f;

        }
    }

    public class BossStateShoot : BossStateMachine
    {
        public override void OnStateEnter(params object[] objs)
        {
            base.OnStateEnter(objs);
            boss.StartShoot(EndShoot);
        }

        private void EndShoot()
        {
            boss.SwitchState(BossAction.PREPARE_ATTACK);
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
        }
    }

}
