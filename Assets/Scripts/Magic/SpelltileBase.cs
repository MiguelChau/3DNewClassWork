using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpelltileBase : MonoBehaviour
{
    public float timeToExpire = 2f;

    public int damageAmount = 1;
    public float speed = 50f;

    public List<string> tagsToHit;

    public Vector3? direction = null;

    private void Awake()
    {
        Destroy(gameObject, timeToExpire);
    }
    private void Update()
    {
        //transform.Translate(Vector3.forward * speed * Time.deltaTime);
        transform.position += (direction != null ? direction.Value : transform.forward) * speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("hit)");
        foreach (var t in tagsToHit)
        {
            if (collision.transform.CompareTag(t))
            {
                var damageable = collision.transform.GetComponent<IDamageable>();

                if (damageable != null)
                {

                    damageable.Damage(damageAmount);

                }

                break;
            }

        }
        Destroy(gameObject);


    }
}
