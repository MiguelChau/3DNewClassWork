using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chau.StateMachine;


namespace Enemy
{
    public class EnemyStatesBase : StateBase
    {
        protected EnemyBaseSM enemy;

        public override void OnStateEnter(params object[] objs)
        {
            base.OnStateEnter(objs);
            enemy = (EnemyBaseSM)objs[0];
        }
       
    }

    public class EnemyStateInit : EnemyStatesBase
    {
        public override void OnStateEnter(params object[] objs)
        {
            base.OnStateEnter(objs);
            enemy.StartInitAnimation();

        }


    }
    public class EnemyStateWalk : EnemyStatesBase
    {
        public override void OnStateEnter(params object[] objs)
        {
            base.OnStateEnter(objs);
            enemy.GoToRandomPoint(OnArrive);

        }
        private void OnArrive()
        {
            enemy.SwitchState(EnemyAction.ATTACK);
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            enemy.StopAllCoroutines();
        }
    }
    public class EnemyStateAttack : EnemyStatesBase
    {
        public override void OnStateEnter(params object[] objs)
        {
            base.OnStateEnter(objs);
            enemy.StartAttack(EndAttacks);

        }

        private void EndAttacks()
        {
            enemy.SwitchState(EnemyAction.WALK);
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            enemy.StopAllCoroutines();
        }
    }

    public class EnemyStateDeath : EnemyStatesBase
    {
        public override void OnStateEnter(params object[] objs)
        {
            base.OnStateEnter(objs);
            enemy.transform.localScale = Vector3.one * .2f;

        }
    }

}
