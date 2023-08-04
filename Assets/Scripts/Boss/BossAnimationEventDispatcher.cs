using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Boss;

public class BossAnimationEventDispatcher : MonoBehaviour
{
    [SerializeField] private BossBase _bossReference;

    public void AttackEvent()
    {
        _bossReference.AttackAnimationEvent();
    }
}
