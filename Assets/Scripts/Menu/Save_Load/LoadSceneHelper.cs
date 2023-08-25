using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneHelper : MonoBehaviour
{
    public void LoadLevel(int level)
    {
        if( level == 1)
        {
            LoadSavedData();
        }
        else
        {
            SaveManager.Instance.SaveItems();
            SaveManager.Instance.Setup.lastLevel = SceneManager.GetActiveScene().buildIndex;
            SaveManager.Instance.Save();
        }
       
        SceneManager.LoadScene(level);
    }

    private void LoadSavedData()
    {
        SaveManager.Instance.Load(); 

        
        Vector3 playerPosition = SaveManager.Instance.Setup.playerPosition;
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            player.transform.position = playerPosition;
        }

    }
}
