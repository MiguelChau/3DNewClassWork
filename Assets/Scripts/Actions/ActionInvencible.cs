using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;
using DG.Tweening;

public class ActionInvencible : ActionBase
{
    public SOInt sOInt;
    public float invencibleDuration = 5f;
    public float invencibleScale = 2f;
    public Ease easeInvencbile = Ease.OutBack;

    private bool isInvulnerable = false;

    private void Start()
    {
        sOInt = ItemManager.Instance.GetItemByType(ItemType.INVENCIBLE).sOInt;
    }

    public override void PerformAction()
    {
        Debug.Log("InvencibleUP!");
        if (!isInvulnerable && sOInt.value > 0)
        {
            isInvulnerable = true;
            ScalePlayer(invencibleScale);
            ItemManager.Instance.RemoveByType(ItemType.INVENCIBLE);
            HealthBase.Instance.Damage(invencibleDuration);

            StartCoroutine(ReturnToNormalSizeAfterDelay());
        }
    }

    private IEnumerator ReturnToNormalSizeAfterDelay()
    {
        yield return new WaitForSeconds(invencibleDuration); 

        
        ScalePlayer(1f);

        
        isInvulnerable = false;
    }
    private void ScalePlayer(float targetScale)
    {
        PlayerController.Instance.transform.localScale = new Vector3(targetScale, targetScale, targetScale);
        PlayerController.Instance.SetTargetScale(invencibleScale);
    }
   
}
