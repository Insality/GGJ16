using UnityEngine;

public class Shaman: MonoBehaviour
{
    public tk2dSprite Graphics;

    private bool _isPlayer = false;

    public void SetPlayerState(bool state)
    {
        _isPlayer = true;
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
}
