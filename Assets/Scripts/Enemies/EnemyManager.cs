using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    public List<EnemyBaseSM> enemies = new List<EnemyBaseSM>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

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
}
