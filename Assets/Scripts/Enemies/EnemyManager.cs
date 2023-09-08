using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;
using Core.Singleton;

public class EnemyManager : Singleton<EnemyManager>
{
    public List<EnemyBaseSM> enemies = new List<EnemyBaseSM>();

    public List<int> deadEnemyIDs = new List<int>();

    public void RegisterEnemy(EnemyBaseSM enemy)
    {
        if (!enemies.Contains(enemy))
        {
            enemies.Add(enemy);
        }
    }

    public void UnregisterEnemy(EnemyBaseSM enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
        }
    }

    public bool IsEnemyAlive(EnemyBaseSM enemy)
    {
        return enemies.Contains(enemy);
    }

    public void InformEnemyDied(EnemyBaseSM enemy)
    {
        deadEnemyIDs.Add(enemy.enemyID);
    }

    private void CheckEnemiesLoadData()
    {
        SaveSetup setup = SaveManager.Instance.Setup;
        if(setup != null)
        {
            if(setup.deadEnemyIDs != null && setup.deadEnemyIDs.Count > 0)
            {
                deadEnemyIDs = setup.deadEnemyIDs;
                for(int i = 0; i < enemies.Count; ++i)
                {
                    if(deadEnemyIDs.Contains(enemies[i].enemyID))
                    {
                        Destroy(enemies[i].gameObject);
                    }
                }
            }
        }
    }

    bool first = true;
    private void Update()
    {
        if(first)
        {
            CheckEnemiesLoadData();
            first = false;
        }
    }
}
