using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Singleton;
using TMPro;

namespace Items
{
    public enum ItemType
    {
        COIN,
        HEALTH_POTION,
        INVENCIBLE
    }

    public class ItemManager : Singleton<ItemManager>
    {
        public List<ItemSetup> itemSetups;


        private void Start()
        {
            Reset();

        }
        private void Reset()
        {
            foreach (var i in itemSetups)
            {
                i.sOInt.value = 0;
            }


        }

        public ItemSetup GetItemByType(ItemType itemType)
        {
            return itemSetups.Find(i => i.itemType == itemType);
        }

        public void AddByType(ItemType itemType, int amount = 1)
        {

            if (amount < 0) return;
            itemSetups.Find(i => i.itemType == itemType).sOInt.value += amount;

        }

        public void RemoveByType(ItemType itemType, int amount = 1) 
        {
            var item = itemSetups.Find(i => i.itemType == itemType);
            item.sOInt.value -= amount;

            if (item.sOInt.value < 0) item.sOInt.value = 0; 
        }

        [NaughtyAttributes.Button]
        private void AddCoin()
        {
            AddByType(ItemType.COIN);
        }

        [NaughtyAttributes.Button]
        private void AddHealthPotion()
        {
            AddByType(ItemType.HEALTH_POTION);
        } 
        [NaughtyAttributes.Button]
        private void AddInvencible()
        {
            AddByType(ItemType.INVENCIBLE);
        }
    }

    [System.Serializable] 
    public class ItemSetup
    {
        public ItemType itemType;
        public SOInt sOInt;
        public Sprite icon;
    }
}
