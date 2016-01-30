using System;
using System.Collections.Generic;
using UnityEngine;

public class Shaman: MonoBehaviour
{
    public tk2dSprite Graphics;
    public tk2dSpriteAnimator Anim;

    private bool _isPlayer = false;
    private bool _isPlaying = false;

    private List<String> stackMoves = new List<string>();

    void Start()
    {
        Anim.Play("ShamanStay");
    }

    void OnEnable()
    {
        Anim.AnimationCompleted += AnimationCompleted;
    }

    void OnDisable()
    {
        Anim.AnimationCompleted -= AnimationCompleted;
    }

    private void AnimationCompleted(tk2dSpriteAnimator anim, tk2dSpriteAnimationClip clip)
    {
        if (stackMoves.Count == 0)
        {
            if (clip.name != "ShamanStay")
            {
                _isPlaying = false;
                Anim.Play("ShamanStay");
            }
        }
        else
        {
            _isPlaying = true;
            Anim.Play(stackMoves[0]);
            stackMoves.RemoveAt(0);
        }
    }

    public void SetPlayerState(bool state)
    {
      Graphics.color = new Color(0.95f, 1f, 0.5f, 1f);
        RefreshGraphics();
    }

    private void RefreshGraphics()
    {
        if (_isPlayer)
        {
            Graphics.SetSprite("PlayerStay");
        }
        else
        {
            Graphics.SetSprite("ShamanStay");
        }
    }

    public void PlayAction(ActionType type)
    {
        var animName = "";
        switch (type)
        {
            case ActionType.Right:
                animName = "ShamanRight";
                break;
            case ActionType.Left:
                animName = "ShamanLeft";
                break;
            case ActionType.Jump:
                animName = "ShamanJump";
                break;
            case ActionType.Stump:
                animName = "ShamanStump";
                break;
            case ActionType.Music:
                animName = "ShamanMusic";
                break;
            case ActionType.Clap:
                animName = "ShamanClap";
                break;
            case ActionType.Magic:
                animName = "ShamanMagic";
                break;
            default:
                Debug.Log("[Error]: Wrong ActionType: " + type.ToString());
                break;
        }

        if (!_isPlaying)
        {
            Anim.Play(animName);
            _isPlaying = true;
        }
        else
        {
            stackMoves.Add(animName);
        }


    }
}
