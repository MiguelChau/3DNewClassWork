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

        Items.ItemManager itemManager = GetComponent<Items.ItemManager>();

        SaveManager.Instance.Setup.coins = Items.ItemManager.Instance.GetItemByType(Items.ItemType.COIN).sOInt.value;
        SaveManager.Instance.Setup.health = Items.ItemManager.Instance.GetItemByType(Items.ItemType.HEALTH_POTION).sOInt.value;

        GameObject player = GameObject.FindWithTag("Player");

        if(player !=null)
        {
            Vector3 playerPosition = player.transform.position;
            SaveManager.Instance.Setup.playerPosition = playerPosition;
        }
       
        SaveManager.Instance.Save();
    }
}
