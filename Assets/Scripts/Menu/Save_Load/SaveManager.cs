using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Core.Singleton;
using System;

public class SaveManager : Singleton<SaveManager>
{

    [NonSerialized] private SaveSetup _saveSetup = null;
    private string _path = Application.streamingAssetsPath + "/save.txt";

    public int lastLevel;

    public Action<SaveSetup> FileLoaded;

    public SaveSetup Setup
    {
        get { return _saveSetup; }
    }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    public void CreateNewSave()
    {
        _saveSetup = new SaveSetup();
        _saveSetup.lastLevel = 0;
        _saveSetup.playerName = "Diana";
        _saveSetup.playerPositionHasValue = false;
        _saveSetup.playerHealth = -1f;
        _saveSetup.deadEnemyIDs = new List<int>();
        
    }

    private void Start()
    {
        //Invoke(nameof(Load), .1f);
    }
    #region SAVE
    [NaughtyAttributes.Button]
    public void Save()
    {
        string setupToJson = JsonUtility.ToJson(_saveSetup, true);
        Debug.Log(setupToJson);
        SaveFile(setupToJson);
    }

    public void SaveItems()
    {
        _saveSetup.coins = Items.ItemManager.Instance.GetItemByType(Items.ItemType.COIN).sOInt.value;
        _saveSetup.health = Items.ItemManager.Instance.GetItemByType(Items.ItemType.HEALTH_POTION).sOInt.value;
        Save();
    }

    public void SaveEnemiesDead()
    {
        _saveSetup.deadEnemyIDs = EnemyManager.Instance.deadEnemyIDs;
        Save();
    }

    public void SavePlayerHealth()
    {
        _saveSetup.playerHealth = PlayerController.Instance.GetCurrentLife();
        Save();
    }
    private void SaveName(string text)
    {
        _saveSetup.playerName = text;
        Save();
    }
    public void SaveLastLevel(int level)
    {
        _saveSetup.lastLevel = level;
        SaveItems();
        Save();
    }
    #endregion

    private void SaveFile(string json)
    {
        Debug.Log(_path);
        File.WriteAllText(_path, json);
    }

    [NaughtyAttributes.Button]
    public void Load()
    {
        string fileLoaded = "";

        if (File.Exists(_path))
        {
            fileLoaded = File.ReadAllText(_path);
            _saveSetup = JsonUtility.FromJson<SaveSetup>(fileLoaded);

            lastLevel = _saveSetup.lastLevel;

            if (FileLoaded != null)
            {
                FileLoaded.Invoke(_saveSetup);
            }
        }
        else
        {
            CreateNewSave();
            Save();
        }       
    }

    [NaughtyAttributes.Button]
    private void SaveLevelOne()
    {
        SaveLastLevel(1);
    }
}

[Serializable]
public class SaveSetup
{
    public int lastLevel;
    public float coins;
    public float health;
    public bool playerPositionHasValue;
    public Vector3 playerPosition;
    public string playerName;
    public List<int> deadEnemyIDs;
    public float playerHealth;
}
