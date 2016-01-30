﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour {

    public List<Shaman> ShamanList = new List<Shaman>();
    public float DanceAngle = 0;
    public float Radius;
    public float Ratio;

    public void AddShaman(Shaman shaman)
    {
        if (ShamanList.Count == 0)
        {
            shaman.SetPlayerState(true);
        }
        ShamanList.Add(shaman);
    }

    void Update()
    {
        UpdateShamansPosition();
//        DanceAngle++;
        if (DanceAngle >= 360)
        {
            DanceAngle = 0;
        }
    }

    private void UpdateShamansPosition()
    {
        if (ShamanList.Count > 0)
        {
            float gap = 360/ShamanList.Count;

            for (int i = 0; i < ShamanList.Count; i++)
            {
                ShamanList[i].transform.localPosition = GetPositionAround(i, gap);
            }
        }
    }

    private Vector3 GetPositionAround(int i, float gap)
    {
        float shamanAngle = DanceAngle + i*gap;
        shamanAngle %= 360;
        if (shamanAngle < 0) shamanAngle = 0;
        var z = (shamanAngle > 0 && shamanAngle < 180) ? 2 : -2;
        return new Vector3(Radius * (float)Math.Cos(Mathf.Deg2Rad * shamanAngle), Radius * (float)Math.Sin(Mathf.Deg2Rad * shamanAngle) / Ratio, z);
    }

    public void ShamanDance(ActionType type)
    {
        for (int i = 0; i < ShamanList.Count; i++)
        {
            if (i != 0)
            {
                ShamanList[i].PlayAction(type);
            }
        }
    }

    public void PlayerDance(ActionType type)
    {
        if (ShamanList.Count > 0)
        {
            ShamanList[0].PlayAction(type);
        }
    }
}
