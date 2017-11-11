﻿using System;
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

    public Text drinkNumText;
    public Image drinkUsableImage;
    
    #region Override Group
    protected override void InitUI_Func()
    {
        this.gameObject.SetActive(false);
    }

    protected override void EnterUI_Func()
    {
        this.gameObject.SetActive(true);

        EnterStageSelect_Func();
    }

    public override void Exit_Func()
    {
        this.gameObject.SetActive(false);
    }
    #endregion
    void EnterStageSelect_Func()
    {
        anim.Play();

        SetStageData_Func();
    }
    void SetStageData_Func()
    {
        titleText.text = "치킨을 위하여!";

        stageTitleTextArr[0].text = "Lv. " + (Player_Data.Instance.stageID_Normal + 1) + " 일반 치킨집";

        stageTitleTextArr[1].text = "Lv. " + (Player_Data.Instance.stageID_Special + 1) + " 무한 치킨집";
    }
    void SetDrinkData_Func()
    {

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