using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cloth
{
    public class ClothItemImmolationAura : ClothItemBase
    {
        public float damagePerSecond = 2f; 

        public override void Collect()
        {
            base.Collect();

            PlayerController.Instance.ActivateImmolationAura(damagePerSecond, duration);
        }
    }
}
