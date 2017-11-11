using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrinkBtn_Script : MonoBehaviour
{
    public StageSelect_Script stageSelectClass;
    public int btnID;
    public Image dirnkImage;
    public GameObject selectImageObj;
    public Text numText;

    public void Init_Func(StageSelect_Script _stageSelectClass, int _btnID)
    {
        stageSelectClass = _stageSelectClass;

        btnID = _btnID;

        dirnkImage.sprite = DataBase_Manager.Instance.drinkDataArr[_btnID].drinkBtnSprite;
        dirnkImage.SetNativeSize();
    }
    public void SetNum_Func(int _num)
    {
        if (99 < _num)
            _num = 99;

        numText.text = _num.ToString();
    }
    public void OnSelect_Func(bool _isOn)
    {
        Debug.Log("Test, IsOn : " + _isOn);
        selectImageObj.SetActive(_isOn);
    }
    public void OnButton_Func()
    {
        // Call : Button Event

        stageSelectClass.OnDrinkBtn_Func(btnID, !selectImageObj.activeSelf);
    }
}
