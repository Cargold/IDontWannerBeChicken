﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MainLobby_Script : LobbyUI_Parent
{
    public RectTransform thisRTrf;
    public Transform[] partyMemberTrfArr;
    public List<GameObject> partyMemberObjList;

    #region Override Group
    protected override void InitUI_Func()
    {
        thisRTrf = this.GetComponent<RectTransform>();

        partyMemberObjList = new List<GameObject>();

        PrintPartyMember_Func();
    }
    protected override void EnterUI_Func()
    {
        // Call : Btn Event

        thisRTrf.DOSizeDelta(new Vector3(0f, 0f), 0.5f);
    }
    public override void Exit_Func()
    {
        thisRTrf.DOSizeDelta(new Vector3(0f, 600f), 1f);
    }
    #endregion
    public void PrintPartyMember_Func()
    {
        for (int i = 0; i < 5; i++)
        {
            int _partyMemberID = Player_Data.Instance.partyUnitIdArr[i];

            if (0 <= _partyMemberID)
            {
                string _unitName = DataBase_Manager.Instance.GetUnitName_Func(_partyMemberID);
                GameObject _unitObj = ObjectPool_Manager.Instance.Get_Func(_unitName);
                _unitObj.transform.SetParent(partyMemberTrfArr[i]);
                _unitObj.transform.localPosition = Vector3.zero;
                _unitObj.transform.localEulerAngles = Vector3.zero;
                _unitObj.transform.localScale = Vector3.one;

                partyMemberObjList.Add(_unitObj);
            }
        }
    }
    public void HidePartyMember_Func()
    {
        for (int i = 0; i < partyMemberObjList.Count; i++)
        {
            ObjectPool_Manager.Instance.Free_Func(partyMemberObjList[i]);
        }

        partyMemberObjList.Clear();
    }
}
