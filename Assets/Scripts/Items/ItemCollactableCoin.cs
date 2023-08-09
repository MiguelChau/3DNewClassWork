using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;


public class ItemCollactableCoin : ItemCollactableBase
{
    public Collider coinCollider;
    protected override void OnCollect()
    {
        base.OnCollect();
        ItemManager.Instance.AddByType(ItemType.COIN);
        ItemManager.Instance.AddByType(ItemType.HEALTH_POTION);
        coinCollider.enabled = false;
    }
}


