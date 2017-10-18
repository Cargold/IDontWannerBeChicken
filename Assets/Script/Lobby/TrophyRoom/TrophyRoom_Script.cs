using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrophyRoom_Script : LobbyUI_Parent
{
    #region Override Group
    protected override void InitUI_Func()
    {
        this.gameObject.SetActive(false);
    }

    protected override void EnterUI_Func()
    {

    }

    public override void Exit_Func()
    {

    }
    #endregion
}
