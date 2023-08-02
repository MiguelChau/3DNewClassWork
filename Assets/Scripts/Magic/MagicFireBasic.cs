using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicFireBasic : MagicBase
{
    public List<UIFillUpdater> uIUpdaters;

    public float maxCast = 10f;
    public float timeToRecharge = 1f;

    private float _currentCasts;
    private bool _recharging = false;
    public bool shouldUpdateUI = true;

    private void Awake()
    {
        //GetAllUIs();
    }

    protected override IEnumerator CastCoroutine()
    {

        if (_recharging) yield break; 

        while (true)
        {
            if (_currentCasts < maxCast)
            {
                Cast();
                _currentCasts++; 
                CheckRecharge();
                UpdateUI();
                yield return new WaitForSeconds(timeBetweenCast);
            }
        }
    }

    private void CheckRecharge()
    {
        if (_currentCasts >= maxCast)
        {
            StopCast();
            StartRecharge();
        }
    }

    private void StartRecharge()
    {
        _recharging = true;
        StartCoroutine(RechargeCoroutine());
    }

    IEnumerator RechargeCoroutine()
    {
        float time = 0;
        while (time < timeToRecharge)
        {
            time += Time.deltaTime;
            uIUpdaters.ForEach(i => i.UpdateValue(time / timeToRecharge));
            yield return new WaitForEndOfFrame();
        }
        _currentCasts = 0;
        _recharging = false;
    }

    private void UpdateUI()
    {
        if (shouldUpdateUI)
        {
            uIUpdaters.ForEach(i => i.UpdateValue(maxCast, _currentCasts));
        }
    }

    /*private void GetAllUIs()
    {
        uIUpdaters = GameObject.FindObjectsOfType<UIFillUpdater>().ToList();
    }*/

    public void SetShouldUpdateUI(bool value)
    {
        shouldUpdateUI = value;
    }
}
