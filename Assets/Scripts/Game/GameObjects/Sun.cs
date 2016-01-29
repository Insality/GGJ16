using System;
using UnityEngine;

public class Sun : MonoBehaviour
{

    public float StartHeight;
    public float ExtraHeight;
    public float LeftBorderX;

    private float testProgress = 0;

    private float curProgress;
    public float TargetProgress;

    void Update()
    {
        if (Math.Abs(curProgress - TargetProgress) > 0.01f)
        {
            if (curProgress > TargetProgress)
            {
                curProgress -= 0.005f;
            }
            else
            {
                curProgress += 0.005f;
            }
        }
        SetProgress(curProgress);
    }

    // perc: 0..1f
    public void SetProgress(float perc)
    {
        // -0.5 .. 0.5
        var centerPerc = perc - 0.5f;

        var newY = (float)(StartHeight + (ExtraHeight * 2 * (0.5 - Math.Pow(Math.Abs(centerPerc), 2))));
        var newX = LeftBorderX + Math.Abs(LeftBorderX)*2 * perc;
        
        transform.localPosition = new Vector3(newX, newY, -2);
    }
}
