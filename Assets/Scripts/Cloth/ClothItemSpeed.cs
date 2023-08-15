using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cloth
{
    public class ClothItemSpeed : ClothItemBase
    {
        public float targetSpeed = 2f;
        public override void Collect()
        {
            base.Collect();
            PlayerController.Instance.ChangeSpeed(targetSpeed, duration);
           
        }
    }
}

