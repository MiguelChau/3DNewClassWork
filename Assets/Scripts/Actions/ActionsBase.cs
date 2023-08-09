using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionBase : MonoBehaviour
{
    public KeyCode keyCode;
    public abstract void PerformAction();

    protected void Update()
    {
        if (Input.GetKeyDown(keyCode))
        {
            PerformAction();
        }
    }
}


