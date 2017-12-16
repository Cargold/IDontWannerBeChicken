using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrophyRoom_Script : LobbyUI_Parent
{
    public Animation anim;

    public Transform listGroupTrf;

    public GameObject trophyListObj;
    public TrophyList_Script[] trophyListClassArr;

    #region Override Group
    protected override void InitUI_Func()
    {
        this.gameObject.SetActive(false);

        Init_Func();
    }
    protected override void EnterUI_Func(int _referenceID = -1)
    {
        this.gameObject.SetActive(true);

        anim.Play();

        Active_Func();
    }
    public override void Exit_Func()
    {
        this.gameObject.SetActive(false);
    }
    #endregion

    void Init_Func()
    {
        int _trophyNum = DataBase_Manager.Instance.trophyDataArr.Length;
        trophyListClassArr = new TrophyList_Script[_trophyNum];

        for (int i = 0; i < _trophyNum; i++)
        {
            GameObject _trophyObj = Instantiate(trophyListObj);
            _trophyObj.transform.SetParent(listGroupTrf);

            trophyListClassArr[i] = _trophyObj.GetComponent<TrophyList_Script>();

            trophyListClassArr[i].Init_Func(this, i);
        }
    }

    void Active_Func()
    {
        for (int i = 0; i < trophyListClassArr.Length; i++)
        {
            trophyListClassArr[i].SetNum_Func(i);
        }
    }
}
