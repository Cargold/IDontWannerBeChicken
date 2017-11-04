using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Trophy_Data
{
    public int trophyID;
    public string trophyName;
    public string trophyDesc;
    public Sprite trophySprite;
    public TrophyType effectType;
    public int amountLimit;
}
