using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class EnemyShooter : EnemyBaseSM
    {
        public MagicBase enemyMagicBase;

        protected override void Init()
        {
            base.Init();
            enemyMagicBase.StartCast();
        }
    }
}
