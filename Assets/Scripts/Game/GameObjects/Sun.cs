using System;
using UnityEngine;

public class Sun : MonoBehaviour
{

    public float StartHeight;
    public float ExtraHeight;
    public float LeftBorderX;

    private float testProgress = 0;

    void Update()
    {
        testProgress += Time.deltaTime / 5f;
        if (testProgress > 1) testProgress = 0;
        SetProgress(testProgress);
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
