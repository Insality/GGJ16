using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class Action : MonoBehaviour
{
    public ActionType Type;
    public tk2dSprite Sprite;

    public Animator Anim;

    private float _destroyDelay = -1;
    private BoxCollider _boxCollider;
    private Transform _myTransform;

    private bool _isParticle = false;

    void OnEnable()
    {
        _myTransform = transform;
        _boxCollider = GetComponent<BoxCollider>();
    }

    public bool IsCollideWith(BoxCollider collider)
    {
        return _boxCollider.bounds.Intersects(collider.bounds);
    }

    public void RefreshGraphics()
    {
        var spriteName = GetSpriteNameByType(Type);
        if (_isParticle) spriteName += "Aura";
        Sprite.SetSprite(spriteName);
    }

    void Update()
    {
        if (_destroyDelay != -1)
        {
            _destroyDelay -= Time.deltaTime;
            if (_destroyDelay < 0)
            {
                Destroy(gameObject);
            }
        }

        Sprite.Build();
    }


    public void Hide()
    {
        Sprite.gameObject.SetActive(false);
        
        _boxCollider.enabled = false;
    }

    public void Randomize(int MaxVariableActions)
    {
        SetType((ActionType)Random.Range(0, MaxVariableActions));
        RefreshGraphics();
    }

    public void SetType(ActionType type)
    {
        Type = type;
        RefreshGraphics();
    }

    public void SetParticle()
    {
        _isParticle = true;
        Anim.enabled = true;
        Anim.Play("ActionParticle");
        SetLifeTime(1.05f);
        transform.localScale =  new Vector3(1.5f, 1.5f, 1);
            RefreshGraphics();
    }

    private void SetLifeTime(float time)
    {
        _destroyDelay = time;
    }


    public void PlaySound()
    {
        PlaySoundByType(Type);   
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
            case ActionType.Magic:
                return "Magic";
            case ActionType.Sad:
                return "SadSmile";
            default:
                Debug.Log("[Error]: Wrong ActionType: " + type);
                break;
        }
        return "";
    }

    public void PlaySoundByType(ActionType type)
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
            case ActionType.Magic:
                soundType = SoundType.Dance;
                break;
            case ActionType.Sad:
                soundType = SoundType.WrongHit;
                break;
            default:
                Debug.Log("[Error]: Wrong ActionType");
                return;
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
    Magic = 6,
    Sad = 7,
}
