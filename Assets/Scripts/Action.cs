using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class Action : MonoBehaviour
{
    public ActionType Type;
    public tk2dSprite Sprite;

    private Transform _myTransform;

    void OnEnable()
    {
        _myTransform = transform;
    }

    public void RefreshGraphics()
    {
        Sprite.SetSprite(GetSpriteNameByType(Type));
    }
    public void Randomize()
    {
        Type = (ActionType)Random.Range(0, Constants.ACTIONS_COUNT);
        RefreshGraphics();
    }

    public void MoveTo(Vector3 target, float speed = 4)
    {
        _myTransform.position = Vector3.MoveTowards(_myTransform.position, target, speed*Time.deltaTime);
    }

#region StaticActionsMethod
    public static string GetSpriteNameByType(ActionType type)
    {
        switch (type)
        {
            case ActionType.Right:
                return "Right";
            case ActionType.Left:
                return "Left";
            case ActionType.Jump:
                return "Jump";
            case ActionType.Stump:
                return "Stump";
            case ActionType.Music:
                return "Music";
            case ActionType.Clap:
                return "Clap";
            default:
                Debug.Log("[Error]: Wrong ActionType");
                break;
        }
        return "";
    }

    public void PlayMusicByType(ActionType type)
    {
        SoundType soundType;
        switch (type) {
            case ActionType.Right:
                soundType =  SoundType.Right;
                break;
            case ActionType.Left:
                soundType = SoundType.Left;
                break;
            case ActionType.Jump:
                soundType = SoundType.Jump;
                break;
            case ActionType.Stump:
                soundType = SoundType.Stump;
                break;
            case ActionType.Music:
                soundType = SoundType.Music;
                break;
            case ActionType.Clap:
                soundType = SoundType.Clap;
                break;
            default:
                soundType = SoundType.Clap;
                Debug.Log("[Error]: Wrong ActionType");
                break;
        }
        SoundController.PlaySound(soundType);
    } 


#endregion

}

public enum ActionType
{
    Right = 0,
    Left = 1,
    Jump = 2,
    Stump = 3,
    Music = 4,
    Clap = 5,
}
