using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicFireAOE : MagicFireBasic
{
    public int amountPerCast = 4;
    public float angle = 15f;

    public override void Cast()
    {
        int mult = 0; 

        for (int i = 0; i < amountPerCast; i++)
        {
            if (i % 2 == 0)
            {
                mult++;  
            }


            var projectile = Instantiate(prefabSpelltile, positionToCast);

            projectile.transform.localPosition = Vector3.zero; 
            projectile.transform.localEulerAngles = Vector3.zero + Vector3.up * (i % 2 == 0 ? angle : -angle) * mult; 
            projectile.speed = speed; 
            projectile.transform.parent = null;
        }


    }
}
