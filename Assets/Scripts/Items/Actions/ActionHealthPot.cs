using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;

public class ActionHealthPot : ActionBase
{
    public SOInt sOInt;

    private void Start()
    {
        sOInt = ItemManager.Instance.GetItemByType(ItemType.HEALTH_POTION).sOInt;
    }

    public override void PerformAction()
    {
        if (sOInt.value > 0)
        {
            ItemManager.Instance.RemoveByType(ItemType.HEALTH_POTION);
            PlayerController.Instance.healthBase.ResetLife();
        }
    }
}

