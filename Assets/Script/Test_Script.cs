using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Test_Script : MonoBehaviour
{

    public Text text;
    public string _asd;

    private void Start()
    {
         text.text = _asd;

    }

    //public bool isTest = false;
    //void Update()
    //{
    //    if (isTest == false) return;
    //    isTest = false;

    //    int _cost = DataBase_Manager.Instance.GetUnitLevelUpCost_Func(level);
    //    Debug.Log("Test, Cost : " + _cost);
    //}
}
