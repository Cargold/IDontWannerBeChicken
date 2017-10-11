using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobby_Manager : MonoBehaviour
{
    public LobbyUI_Parent[] lobbyUIParentClassArr;
    LobbyUiControl lobbyUiControlClass;
    public enum LobbyState
    {
        None = -1,
        StageSelect,
        HeroManagement,
        PoultryFarm,
        Store,
    }
    public LobbyState lobbyState
    {
        get
        {
            return m_LobbyState;
        }
    }
    private LobbyState m_LobbyState;
    private LobbyUI_Parent recentUIClass;

    public IEnumerator Init_Cor()
    {
        for (int i = 0; i < 4; i++)
        {
            lobbyUIParentClassArr[i].Init_Func(this, (LobbyState)i);
        }

        recentUIClass = lobbyUIParentClassArr[0];

        lobbyUiControlClass = new LobbyUiControl();
        lobbyUiControlClass.Init_Func();

        yield break;
    }
    #region Lobby Group
    public void LobbyEnter_Func()
    {
        lobbyUiControlClass.Active_Func(true);

        Enter_Func(0);

        Game_Manager.Instance.LobbyEnter_Func();
    }
    public void Enter_Func(int _lobbyTypeID)
    {
        m_LobbyState = (LobbyState)_lobbyTypeID;

        lobbyUiControlClass.MoveView_Func(_lobbyTypeID);

        recentUIClass.Exit_Func();
        recentUIClass = lobbyUIParentClassArr[_lobbyTypeID];
        recentUIClass.Enter_Func();
    }
    #endregion
    #region Stage Select Group
    public void BattleEnter_Func()
    {
        lobbyUiControlClass.Active_Func(false);

        Game_Manager.Instance.BattleEnter_Func();
    }
    #endregion
}

class LobbyUiControl
{
    private RectTransform menuGroupTrf;
    private RectTransform lobbyViewTrf;

    public void Init_Func()
    {
        menuGroupTrf = Game_Manager.Instance.menuGroupTrf;
        lobbyViewTrf = Game_Manager.Instance.lobbyViewTrf;
    }
    public void Active_Func(bool _isActive)
    {
        if(_isActive == true)
        {
            lobbyViewTrf.gameObject.SetActive(true);
            menuGroupTrf.anchoredPosition = Vector2.zero;
        }
        else if(_isActive == false)
        {
            lobbyViewTrf.gameObject.SetActive(false);
            menuGroupTrf.anchoredPosition = Vector2.left * 300f;
        }
    }
    public void MoveView_Func(int _lobbyTypeID)
    {
        Vector2 _arrivePos = Vector2.zero;

        _arrivePos.x = lobbyViewTrf.anchoredPosition.x;
        _arrivePos.y = _lobbyTypeID * 1080f;

        lobbyViewTrf.anchoredPosition = _arrivePos;
    }
}