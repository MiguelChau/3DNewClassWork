using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Items;

public class ChestItemCoin : ChestItemBase
{
    public SFXType sfxType;
    public ItemType itemType; 
    public int itemCount = 5;
    public GameObject itemPrefab;

    private List<GameObject> _items = new List<GameObject>();

    public Vector2 randomRange = new Vector2(-2f, 2f);
    public float tweenEndTime = .5f;

    public override void ShowItem()
    {
        base.ShowItem();
        CreateItems();
    }

    [NaughtyAttributes.Button]
    private void CreateItems()
    {
        {
            for (int i = 0; i < itemCount; i++)
            {
                var item = Instantiate(itemPrefab);
                item.transform.position = transform.position + Vector3.forward * Random.Range(randomRange.x, randomRange.y) + Vector3.right * Random.Range(randomRange.x, randomRange.y);
                item.transform.DOScale(0, 1f).SetEase(Ease.OutBack).From();
                _items.Add(item);

            }
        }
    }

    private void PlaySFX()
    {
        SFXPool.Instance.Play(sfxType);
    }

    [NaughtyAttributes.Button]
    public override void Collect()
    {
        base.Collect();
        PlaySFX();
        foreach (var i in _items)
        {
            var itemComponent = i.GetComponent<ItemManager>();
            if (itemComponent != null)
            {
                i.transform.DOMoveY(2f, tweenEndTime).SetRelative(); 
                i.transform.DOScale(0, tweenEndTime / 2).SetDelay(tweenEndTime / 2);
                ItemManager.Instance.AddByType(itemType);
                
            }
            
        }
    }
}
