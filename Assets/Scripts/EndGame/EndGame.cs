using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    public GameObject uiEndGame;
    public float delayEndGame = 3f;
    
    public void CallEndGame()
    {
       
        uiEndGame.SetActive(true);

    }
}
