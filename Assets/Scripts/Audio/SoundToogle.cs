using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundToogle : MonoBehaviour
{
    public AudioManager audioManager;
    public Button toogleButton;

    private bool soundOn = true;

    private void Start()
    {
        toogleButton.onClick.AddListener(ToogleSound);
    }

    private void ToogleSound()
    {
        soundOn = !soundOn;

        if(soundOn)
        {
            audioManager.TurnSoundOn();
        }
        else
        {
            audioManager.TurnSoundOff();
        }
    }
}
