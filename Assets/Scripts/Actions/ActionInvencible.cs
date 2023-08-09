using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;

public class ActionInvencible : ActionBase
{
    public SOInt sOInt;
    public float invencibleDuration = 5f;

    private bool isInvulnerable = false;

    private void Start()
    {
        sOInt = ItemManager.Instance.GetItemByType(ItemType.INVENCIBLE).sOInt;
    }

    public override void PerformAction()
    {
        if (!isInvulnerable && sOInt.value > 0)
        {
            isInvulnerable = true;

            ItemManager.Instance.RemoveByType(ItemType.INVENCIBLE);
            PlayerController.Instance.SetInvencible(invencibleDuration);
        }
    }
}
