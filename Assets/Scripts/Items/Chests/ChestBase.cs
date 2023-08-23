using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ChestBase : MonoBehaviour
{
    public KeyCode keyCode = KeyCode.O;
    public Animator animator;
    public string triggerOpen = "Open";

    [Header("Notification")]
    public GameObject notification;
    public float tweenDuration = .2f;
    public Ease tweenEase = Ease.OutBack;

    [Space]
    public ChestItemBase chestItem;

    private float startScale;

    private bool _chestOpened = false;


    private void Start()
    {
        startScale = notification.transform.localScale.x;
        HideNotification();
    }

    

    [NaughtyAttributes.Button]
    private void OpenChest ()
    {
        if (_chestOpened) return; //isto evita bugs futuros como duplicar os items

        animator.SetTrigger(triggerOpen);
        _chestOpened = true;
        HideNotification();
        Invoke(nameof(ShowItem), 1f);
    }

    private void ShowItem()
    {
        chestItem.ShowItem();
        Invoke(nameof(CollectItem), 1f);
    }

    private void CollectItem()
    {
        chestItem.Collect();
    }
    public void OnTriggerEnter(Collider other)
    {
        PlayerController p = other.transform.GetComponent<PlayerController>();
        if(p != null)
        {
            ShowNotification();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerController p = other.transform.GetComponent<PlayerController>();
        if (p != null)
        {
            HideNotification();
        }
    }

    [NaughtyAttributes.Button]
    private void ShowNotification()
    {
        notification.SetActive(true);
        notification.transform.localScale = Vector3.zero;
        notification.transform.DOScale(startScale, tweenDuration);
    }
    [NaughtyAttributes.Button]
    private void HideNotification()
    {
        notification.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(keyCode) && notification.activeSelf) //se ete botao for apertado e a notificaçao tiver ativada, abrimos o bau
        {
            OpenChest();
        }
    }
}
