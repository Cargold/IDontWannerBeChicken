using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroManagement_Script : LobbyUI_Parent
{
    #region Override Group
    protected override void InitUI_Func()
    {
        this.gameObject.SetActive(false);
    }

    protected override void EnterUI_Func()
    {
        this.gameObject.SetActive(true);
    }

    public override void Exit_Func()
    {
        // Call : Btn Event

        this.gameObject.SetActive(false);
    }
    #endregion

    public void OnFeedingRoom_Func()
    {
        lobbyManager.OnFeedingRoom_Func(999);
    }
}
