using System;
using System.Collections.Generic;
using UnityEngine;
using Core.Singleton;

public class HealthBase : Singleton<HealthBase>, IDamageable
{
    public float startLife = 10f;
    public bool destroyOnKill = false;
    [SerializeField] public float _currentLife;

    public Action<HealthBase> OnDamage;
    public Action<HealthBase> OnKill;

    public List<UIFillUpdater> uiFillUpdater;


    private bool isInvulnerable = false;
    private float invulnerableTime = .5f;
    private float invulnerableTimeStart;

    protected override void Awake()
    {
        Init();
    }
    public void Init()
    {
        ResetLife();
        
    }

    public void ResetLife()
    {
        _currentLife = startLife;
        UpdateUI();
    }

    protected virtual void Kill()
    {
        if (destroyOnKill)
            Destroy(gameObject, 3f);

        OnKill?.Invoke(this);
    }

    public void Damage(float f)
    {
        if (isInvulnerable)
            return;
       
        _currentLife -= f;

        if (_currentLife <= 0)
        {
            Kill();
        }
        UpdateUI();
        OnDamage?.Invoke(this);

        if(invulnerableTime > 0)
        {
            isInvulnerable = true;
            invulnerableTimeStart = Time.time;
        }
    }

    private void Update()
    {
        if(isInvulnerable)
        {
            if((Time.time - invulnerableTimeStart) > invulnerableTime)
            {
                isInvulnerable = false;
            }
        }
    }
    private void UpdateUI()
    {
        if (uiFillUpdater != null)
        {
            uiFillUpdater.ForEach(i => i.UpdateValue((float)_currentLife / startLife));
        }
    }
}
