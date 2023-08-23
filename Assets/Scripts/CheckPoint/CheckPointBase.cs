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
    private bool waitingForSaveConfirmation = false;

    private void Start()
    {
        checkPointManager = CheckPointManager.Instance;
    }

    private void Update()
    {
        if (waitingForSaveConfirmation)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                ConfirmSave();
            }
            else if (Input.GetKeyDown(KeyCode.N))
            {
                CancelSaveConfirmation();
            }
        }
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

        SaveManager.Instance.ShowSavescreen();
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

        StartCoroutine(WaitForSaveConfirmation());
    }

    private IEnumerator WaitForSaveConfirmation()
    {
        waitingForSaveConfirmation = true;
        yield return new WaitForSeconds(2f); 

        if (waitingForSaveConfirmation)
        {
            waitingForSaveConfirmation = false;
            SaveManager.Instance.SaveItems(); 
        }

        SaveManager.Instance.HideSavescreen(); 
    }

    public void ConfirmSave()
    {
        SaveManager.Instance.SaveItems();
        SaveManager.Instance.HideSavescreen();
    }

    public void CancelSaveConfirmation()
    {
        waitingForSaveConfirmation = false;
        SaveManager.Instance.HideSavescreen(); 
    }
}
