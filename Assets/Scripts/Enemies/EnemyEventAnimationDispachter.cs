using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;

public class EnemyEventAnimationDispachter : MonoBehaviour
{
    [SerializeField] private EnemyBaseSM _enemyReference;

    public void AttackEvent()
    {
        _enemyReference.AttackAnimationEvent();
    }
}
