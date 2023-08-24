using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneHelper : MonoBehaviour
{
    public void LoadLevel(int level)
    {
        SceneManager.LoadScene(level);

        SaveManager.Instance.SaveItems();  
        SaveManager.Instance.Setup.lastLevel = SceneManager.GetActiveScene().buildIndex;  
        SaveManager.Instance.Save();
    }
}
