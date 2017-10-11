using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelect_Script : LobbyUI_Parent
{
    #region Override Group
    protected override void InitUI_Func()
    {

    }

    protected override void EnterUI_Func()
    {
        
    }

    public override void Exit_Func()
    {
        
    }
    #endregion

    public void BattleEnter_Func()
    {
        lobbyManager.BattleEnter_Func();
    }
}