using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainLobby_Script : LobbyUI_Parent
{
    public BattleStartDirection_Script directionClass;

    #region Override Group
    protected override void InitUI_Func()
    {

    }
    protected override void EnterUI_Func()
    {
        this.gameObject.SetActive(true);
    }
    public override void Exit_Func()
    {
        this.gameObject.SetActive(false);
    }
    #endregion
}
