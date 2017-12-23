using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelect_Script : LobbyUI_Parent
{
    public Animation anim;
    public Text titleText;
    public Text[] stageTitleTextArr;
    public Image[] stageImageArr;
    public Text[] stageInfoTextArr;

    public Button autoBtn;

    public RectTransform drinkGroupRTrf;
    public GameObject drinkBtnObj;
    public DrinkBtn_Script[] drinkBtnClassArr;
    
    #region Override Group
    protected override void InitUI_Func()
    {
        InitStageSelect_Func();

        this.gameObject.SetActive(false);
    }
    protected override void EnterUI_Func(int _referenceID = -1)
    {
        EnterStageSelect_Func();
    }
    public override void Exit_Func()
    {
        this.gameObject.SetActive(false);
    }
    #endregion
    void InitStageSelect_Func()
    {
        InitDrinkBtn_Func();
    }
    void InitDrinkBtn_Func()
    {
        drinkBtnClassArr = new DrinkBtn_Script[4];
        for (int i = 0; i < 4; i++)
        {
            GameObject _drinkBtnObj = Instantiate(drinkBtnObj);
            _drinkBtnObj.transform.SetParent(drinkGroupRTrf);
            _drinkBtnObj.transform.localScale = Vector3.one;

            DrinkBtn_Script _drinkBtnClass = _drinkBtnObj.GetComponent<DrinkBtn_Script>();
            _drinkBtnClass.Init_Func(this, i);

            drinkBtnClassArr[i] = _drinkBtnClass;
        }
    }

    void EnterStageSelect_Func()
    {
        this.gameObject.SetActive(true);

        anim.Play();

        SetStageData_Func();
        SetDrinkData_Func();
    }
    void SetStageData_Func()
    {
        titleText.text = TranslationSystem_Manager.Instance.PreparingRaid;

        stageTitleTextArr[0].text = "Lv. " + (Player_Data.Instance.stageID_Normal + 1) + " " + TranslationSystem_Manager.Instance.ChickenRestaurant;

        stageTitleTextArr[1].text = "Lv. " + (Player_Data.Instance.stageID_Special + 1) + " " + TranslationSystem_Manager.Instance.ChickenRestaurantArr2;
    }
    void SetDrinkData_Func()
    {
        for (int i = 0; i < 4; i++)
        {
            bool _isDrinkUse = Player_Data.Instance.CheckDrinkUse_Func(i);
            drinkBtnClassArr[i].OnSelect_Func(_isDrinkUse);

            int _drinkNum = Player_Data.Instance.GetDrinkNum_Func(i);
            drinkBtnClassArr[i].SetNum_Func(_drinkNum);
        }
    }

    public void OnDrinkBtn_Func(int _btnID, bool _isOn)
    {
        bool _isDrinkUse = Player_Data.Instance.SetDrinkUse_Func(_btnID, _isOn);

        if(_isDrinkUse == true)
            drinkBtnClassArr[_btnID].OnSelect_Func(_isOn);
        else
            drinkBtnClassArr[_btnID].OnSelect_Func(!_isOn);
    }

    #region Enter Stage Group
    public void BattleEnterNormal_Func()
    {
        // Call : Btn Event

        lobbyManager.BattleEnter_Func(BattleType.Normal);
    }
    public void BattleEnterSpecial_Func()
    {
        // Call : Btn Event

        lobbyManager.BattleEnter_Func(BattleType.Special);
    }
    #endregion
}