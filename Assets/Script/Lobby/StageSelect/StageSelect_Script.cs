using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelect_Script : LobbyUI_Parent
{
    public Animation anim;

    #region Override Group
    protected override void InitUI_Func()
    {
        this.gameObject.SetActive(false);
    }

    protected override void EnterUI_Func()
    {
        this.gameObject.SetActive(true);

        anim.Play();
    }

    public override void Exit_Func()
    {
        this.gameObject.SetActive(false);
    }
    #endregion

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
}