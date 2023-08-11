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
        
        transform.position += (direction != null ? direction.Value : transform.forward) * speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject != this.gameObject)
        {
            foreach (var t in tagsToHit)
            { 
        
                if (collision.transform.CompareTag(t))
                {
                    var damageable = collision.transform.GetComponent<IDamageable>();
                    var playerController = collision.transform.GetComponent<PlayerController>();

                    if (damageable != null && playerController != null)
                    {
                        if (!playerController.isInvulnerable)
                        {
                            playerController.SetInvencible(playerController.invulnerabilityTimer);
                            damageable.Damage(damageAmount);

                        }
                    }
                    

                    break;
                }
            }

        }
        Destroy(gameObject);


    }
}
