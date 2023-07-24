using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class EnemyShooter : EnemyBaseSM
    {
        public MagicBase enemyMagicBase;
        public float timeBetweenCast = 0.3f;

        protected override void Init()
        {
            base.Init();
            
        }

        public override void StartShoot(Action endCallback = null)
        {
            StartCoroutine(ShootCoroutine(endCallback));
        }

        private IEnumerator ShootCoroutine(Action endCallback)
        {
            enemyMagicBase.StartCast();

            yield return new WaitForSeconds(timeBetweenCast);

            
            endCallback?.Invoke();
        }
    }
}
