using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Singleton;
using UnityEngine.UI;
using TMPro;

public class CheckPointManager : Singleton<CheckPointManager>
{
    public int lastCheckPointKey = 0;

    public List<CheckPointBase> checkPoints;

    public TextMeshProUGUI checkpointText;
    public float checkpointTextDuration = 2f;
    private int _savedCheckPointKey = 0;

    private void Start()
    {
        checkpointText.gameObject.SetActive(false);
    }

    public bool HasCheckPoint()
    {
        return lastCheckPointKey > 0;
    }
    public void SaveCheckPoint(int i)
    {
        if (i > _savedCheckPointKey)
        {
            _savedCheckPointKey = i;
            Vector3 checkpointPosition = GetPositionFromLastCheckPoint();
            SaveManager.Instance.SaveLastCheckPoint(_savedCheckPointKey, checkpointPosition);
            ShowCheckpointText();
            SaveManager.Instance.ShowSavescreen();
        }
    }

    public Vector3 GetPositionFromLastCheckPoint()
    {
       var checkPoint = checkPoints.Find(i => i.key == lastCheckPointKey);
        return checkPoint.transform.position;
    }

    public void ShowCheckpointText()
    {
        StartCoroutine(ShowCheckpointTextCoroutine());
    }

    private IEnumerator ShowCheckpointTextCoroutine()
    {
        checkpointText.text = "Checkpoint Active";
        checkpointText.gameObject.SetActive(true);
        yield return new WaitForSeconds(checkpointTextDuration);
        checkpointText.gameObject.SetActive(false);
    }
}
