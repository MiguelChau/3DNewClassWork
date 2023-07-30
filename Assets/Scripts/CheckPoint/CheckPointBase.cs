using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointBase : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public int key = 01;

    private CheckPointManager checkPointManager;

    private bool checkPointActived = false;
    private string checkPointKey = "CheckPointKey";

    private void Start()
    {
        checkPointManager = CheckPointManager.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!checkPointActived && other.transform.tag == "Player")
        {
            CheckCheckPoint();
        }
    }

    private void CheckCheckPoint()
    {
        TurnItOn();
        SaveCheckPoint();
    }

    [NaughtyAttributes.Button]
    private void TurnItOn()
    {
        meshRenderer.material.SetColor("_EmissionColor", Color.yellow);
    }
    
    private void TurnItOff()
    {
        meshRenderer.material.SetColor("_EmissionColor", Color.blue);
    }

    private void SaveCheckPoint()
    {
        CheckPointManager.Instance.SaveCheckPoint(key);

        checkPointActived = true;

        checkPointManager.ShowCheckpointText();
    }
}
