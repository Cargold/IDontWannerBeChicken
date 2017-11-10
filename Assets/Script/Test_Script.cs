using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Test_Script : MonoBehaviour
{
    public int level;
    public bool isTest = false;
    void Update()
    {
        if (isTest == false) return;
        isTest = false;

        int _cost = DataBase_Manager.Instance.GetUnitLevelUpCost_Func(level);
        Debug.Log("Test, Cost : " + _cost);
    }
}
