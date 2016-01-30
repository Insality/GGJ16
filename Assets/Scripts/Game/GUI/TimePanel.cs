using UnityEngine;
using System.Collections;

public class TimePanel : MonoBehaviour {

    public tk2dClippedSprite FillBar;
    
    public void SetPercentage(float perc)
    {
        FillBar.ClipRect = new Rect(0, 0, 1-perc, 1);
    }
}
