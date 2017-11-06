using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CharEffectData
{
    public bool isEffectOn;
    public Transform effectPos;
    public GameObject effectObj;
    public bool isSetParentTrf;
    public bool isRotationSet;
}