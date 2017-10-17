using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobby_Manager : MonoBehaviour
{
    public Transform menuGroupTrf;
    public LobbyUI_Parent[] lobbyUIParentClassArr;
    public MainLobby_Script mainLobbyClass;
    public StageSelect_Script stageSelectClass;
    public HeroManagement_Script heroManagementClass;
    public PartySetting_Script partySettingClass;
    public FeedingRoom_Script feedingRoomClass;
    public Store_Script storeClass;

    public enum LobbyState
    {
        None = -1,
        MainLobby,
        StageSelect,
        HeroManagement,
        PartySetting,
        FeedingRoom,
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

    public PlayerWealth_Script playerWealthClass;

    public IEnumerator Init_Cor()
    {
        int _menuNum = menuGroupTrf.childCount;
        lobbyUIParentClassArr = new LobbyUI_Parent[_menuNum];

        for (int i = 0; i < _menuNum; i++)
        {
            LobbyUI_Parent _lobbyUiParentClass = menuGroupTrf.GetChild(i).GetComponent<LobbyUI_Parent>();

            _lobbyUiParentClass.Init_Func(this, (LobbyState)i);

            lobbyUIParentClassArr[i] = _lobbyUiParentClass;
        }

        mainLobbyClass = (MainLobby_Script)lobbyUIParentClassArr[0];
        stageSelectClass = (StageSelect_Script)lobbyUIParentClassArr[1];
        heroManagementClass = (HeroManagement_Script)lobbyUIParentClassArr[2];
        partySettingClass = (PartySetting_Script)lobbyUIParentClassArr[3];
        feedingRoomClass = (FeedingRoom_Script)lobbyUIParentClassArr[4];
        storeClass = (Store_Script)lobbyUIParentClassArr[5];

        recentUIClass = lobbyUIParentClassArr[0];

        yield break;
    }
    #region Lobby Group
    public void LobbyEnter_Func()
    {
        Enter_Func(LobbyState.MainLobby);
    }
    public void Enter_Func(LobbyState _lobbyState)
    {
        Enter_Func((int)_lobbyState);
    }
    public void Enter_Func(string _loobyTypeText)
    {
        LobbyState _lobbyState = _loobyTypeText.ToEnum<LobbyState>();
        Enter_Func(_lobbyState);
    }
    public void Enter_Func(int _lobbyTypeID)
    {
        m_LobbyState = (LobbyState)_lobbyTypeID;

        recentUIClass.Exit_Func();
        recentUIClass = lobbyUIParentClassArr[_lobbyTypeID];
        recentUIClass.Enter_Func();
    }
    #endregion
    #region Stage Select Group
    public void BattleEnter_Func()
    {
        stageSelectClass.Exit_Func();
        mainLobbyClass.Exit_Func();

        Game_Manager.Instance.BattleEnter_Func();
    }
    #endregion
}