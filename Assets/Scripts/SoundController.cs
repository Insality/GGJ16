using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundController: MonoBehaviour
{
    public SoundData[] Sounds;
    private static Dictionary<SoundType, AudioClip> _soundsDict = new Dictionary<SoundType, AudioClip>();

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        LoadSoundsToStatic();
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

}

[Serializable]
public class SoundData
{
    public SoundType Type;
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
