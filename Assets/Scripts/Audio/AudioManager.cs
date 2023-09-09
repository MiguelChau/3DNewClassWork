using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Singleton;

public class AudioManager : Singleton<AudioManager>
{
    public List<MusicSetup> musicSetups;
    public List<SFXSetup> sfxSetups;

    public AudioSource musicSource;
    public AudioSource SFXSource;

    public void PlayMusicByType(MusicType musicType)
    {
        var music = GetMusicByType(musicType);
        musicSource.clip = music.audioClip;
        musicSource.Play();
    }

    public MusicSetup GetMusicByType(MusicType musicType)
    {
        return musicSetups.Find(i => i.musicType == musicType);
    }
    public SFXSetup GetSFXByType(SFXType sfxType)
    {
        return sfxSetups.Find(i => i.sfxType == sfxType);
    }

    private bool isSoundOn = true; 

    
    public void TurnSoundOn()
    {
        isSoundOn = true;
        
        musicSource.volume = 1f; 
        SFXSource.volume = 1f; 
    }

    
    public void TurnSoundOff()
    {
        isSoundOn = false;
        
        musicSource.volume = 0.0f; 
        SFXSource.volume = 0.0f; 
    }

   
    public bool IsSoundOn()
    {
        return isSoundOn;
    }
}

public enum MusicType
{
    MUSIC_01,
    MUSIC_02,
    MUSIC_03
}
[System.Serializable]
public class MusicSetup
{
    public MusicType musicType;
    public AudioClip audioClip;
}

public enum SFXType
{
    NONE,
    SFX_01,
    SFX_02,
    SFX_03,
    SFX_04,
    SFX_05,
    SFX_06
}
[System.Serializable]
public class SFXSetup
{
    public SFXType sfxType;
    public AudioClip audioClip;
}