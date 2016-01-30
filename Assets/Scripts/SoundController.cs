using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SoundController: MonoBehaviour
{
    public SoundData[] Sounds;
    public MusicData[] Musics;
    public AudioSource Audio;
    private static Dictionary<SoundType, AudioClip> _soundsDict = new Dictionary<SoundType, AudioClip>();
    private static Dictionary<MusicType, AudioClip> _musicDict = new Dictionary<MusicType, AudioClip>();
    private static AudioSource _audio;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        LoadSoundsToStatic();
        LoadMusicsToStatic();
        _audio = Audio;
    }

    private void LoadSoundsToStatic()
    {
        if (Sounds != null)
        {
            for (int i = 0; i < Sounds.Length; i++)
            {
                _soundsDict.Add(Sounds[i].Type, Sounds[i].Clip);
            }
        }
        else
        {
            Debug.Log("[Error]: No Sounds Initialized");
        }
    }
    
    private void LoadMusicsToStatic()
    {
        if (Musics != null)
        {
            for (int i = 0; i < Musics.Length; i++)
            {
                _musicDict.Add(Musics[i].Type, Musics[i].Clip);
            }
        }
        else
        {
            Debug.Log("[Error]: No Musics Initialized");
        }
    }

    public static void PlaySound(SoundType type)
    {
        if (_soundsDict[type] != null)
        {
            AudioSource.PlayClipAtPoint(_soundsDict[type], Vector3.zero);
        }
        else
        {
            Debug.Log("The SoundType " + type + " is null");
        }
    }

    public static void PlayMusic(MusicType type) {
        if (_musicDict[type] != null) {
            _audio.Stop();
            _audio.clip = _musicDict[type];
            _audio.loop = true;
            _audio.volume = 0.2f;
            _audio.Play();
        } else {
            Debug.Log("The MusicType " + type + " is null");
        }
    }

}

[Serializable]
public class SoundData
{
    public SoundType Type;
    public AudioClip Clip;
}

[Serializable]
public class MusicData {
    public MusicType Type;
    public AudioClip Clip;
}



public enum SoundType
{
    WrongHit = 0,
    OkHit = 1,

    //Actions Sample
    Right = 2,
    Left = 3,
    Jump = 4,
    Stump = 5,
    Music = 6,
    Clap = 7,
    Dance = 8,

    // ====== 
    StartLevel = 10,
    EndLevel = 11,
}


public enum MusicType {
    GameDrums = 0,
}
