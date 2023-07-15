using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBase : MonoBehaviour
{
    public SpelltileBase prefabSpelltile;

    public Transform positionToCast;
    public float timeBetweenCast = .3f;
    public float speed = 50f;

    private Coroutine _currentCoroutine;

    protected virtual IEnumerator CastCoroutine() 
    {
        while (true)
        {
            Cast();
            yield return new WaitForSeconds(timeBetweenCast);
        }
    }

    public virtual void Cast()
    {
        var projectile = Instantiate(prefabSpelltile);
        projectile.transform.position = positionToCast.position;
        projectile.transform.rotation = positionToCast.rotation;
        projectile.speed = speed;
    }

    public void StartCast()
    {
        StopCast();
        _currentCoroutine = StartCoroutine(CastCoroutine());
    }

    public void StopCast()
    {
        if (_currentCoroutine != null)
            StopCoroutine(_currentCoroutine);
    }
}

