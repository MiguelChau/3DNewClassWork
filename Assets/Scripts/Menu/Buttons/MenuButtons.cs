using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.SceneManagement;


public class MenuButtons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{    
    public ParticleSystem buttonsparticleSystem;
   

    [Header("Animation Buttons")]
    public float finalScale = 1f;
    public float scaleDuration = 0.5f;
    public Ease ease = Ease.OutBack;

    private Vector3 _defaultScale;
    private Tween _currentTween;

    private void Awake()
    {
        _defaultScale = transform.localScale; 
    }
    public void OnClick()
    {
        buttonsparticleSystem.Play();
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Enter");
        _currentTween = transform.DOScale(_defaultScale * finalScale, scaleDuration);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Exit");
        _currentTween.Kill();
        transform.localScale = _defaultScale; 
    }  
}
