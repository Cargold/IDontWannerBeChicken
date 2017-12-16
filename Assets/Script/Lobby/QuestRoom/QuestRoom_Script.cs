using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestRoom_Script : LobbyUI_Parent
{
    public Animation anim;

    #region Override Group
    protected override void InitUI_Func()
    {
        this.gameObject.SetActive(false);
    }

    protected override void EnterUI_Func(int _referenceID = -1)
    {
        this.gameObject.SetActive(true);

        anim.Play();
    }

    public override void Exit_Func()
    {
        this.gameObject.SetActive(false);
    }
    #endregion
}
