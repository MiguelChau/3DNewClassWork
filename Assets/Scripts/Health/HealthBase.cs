using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class HealthBase : MonoBehaviour, IDamageable
{
    public float startLife = 10f;
    public bool destroyOnKill = false;
    [SerializeField] public float _currentLife;

    public Action<HealthBase> OnDamage;
    public Action<HealthBase> OnKill;

    public float damageMultiply = 1f;

    public List<UIFillUpdater> uiFillUpdater;


    private bool isInvulnerable = false;
    private float invulnerableTime = 5f;
    private float invulnerableTimeStart;

    protected void Awake()
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

    public void SetInvulnerable()
    {
        isInvulnerable = true;
        invulnerableTimeStart = Time.time;
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
       
        _currentLife -= f * damageMultiply;

        if (_currentLife <= 0)
        {
            Kill();
        }
        UpdateUI();
        OnDamage?.Invoke(this);
    }

    public void ChangeDamageMultiply(float damage, float duration)
    {
        StartCoroutine(ChangeDamageMultiplyCoroutine(damage, duration));
    }

    IEnumerator ChangeDamageMultiplyCoroutine(float damageMultiply, float duration)
    {
        this.damageMultiply = damageMultiply;
        yield return new WaitForSeconds(duration);
        this.damageMultiply = 1;
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
