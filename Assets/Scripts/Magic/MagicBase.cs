using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animation;

public class MagicBase : MonoBehaviour
{
    public SpelltileBase prefabSpelltile;
    public AudioSource audioSource;
    public AnimationBase animationBase;

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

    public virtual void Cast(Transform target = null)
    {
        var projectile = Instantiate(prefabSpelltile);
        projectile.transform.position = positionToCast.position;
        projectile.transform.rotation = positionToCast.rotation;
        projectile.direction = target != null ? (target.position - projectile.transform.position).normalized : null;
        projectile.speed = speed;

        if (audioSource != null) audioSource.Play();
    }

    public void StartCast()
    {
        StopCast();
        _currentCoroutine = StartCoroutine(CastCoroutine());
        animationBase.PlayAnimationByTrigger(AnimationType.ATTACK);
    }

    public void StopCast()
    {
        if (_currentCoroutine != null)
            StopCoroutine(_currentCoroutine);
    }
}

