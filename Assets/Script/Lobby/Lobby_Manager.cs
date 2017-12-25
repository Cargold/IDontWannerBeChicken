using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobby_Manager : MonoBehaviour
{
    public static Lobby_Manager Instance;

    public Transform menuGroupTrf;
    [System.NonSerialized]
    public LobbyUI_Parent[] lobbyUIParentClassArr;
    [System.NonSerialized]
    public MainLobby_Script mainLobbyClass;
    [System.NonSerialized]
    public StageSelect_Script stageSelectClass;
    [System.NonSerialized]
    public HeroManagement_Script heroManagementClass;
    [System.NonSerialized]
    public PartySetting_Script partySettingClass;
    [System.NonSerialized]
    public FeedingRoom_Script feedingRoomClass;
    [System.NonSerialized]
    public StoreRoom_Script storeRoomClass;
    [System.NonSerialized]
    public TrophyRoom_Script trophyRoomClass;
    [System.NonSerialized]
    public QuestRoom_Script questRoomClass;

    public IEnumerator Init_Cor()
    {
        Instance = this;

        int _menuNum = 8;
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
        storeRoomClass = (StoreRoom_Script)lobbyUIParentClassArr[4];
        questRoomClass = (QuestRoom_Script)lobbyUIParentClassArr[5];
        trophyRoomClass = (TrophyRoom_Script)lobbyUIParentClassArr[6];
        feedingRoomClass = (FeedingRoom_Script)lobbyUIParentClassArr[7];

        yield break;
    }
    #region Lobby Group
    public void Enter_Func(string _loobyTypeText)
    {
        // Call : Btn Event

        LobbyState _lobbyState = _loobyTypeText.ToEnum<LobbyState>();
        Enter_Func(_lobbyState);
    }
    public void Enter_Func(LobbyState _lobbyState, int _referenceID = -1)
    {
        int _lobbyTypeID = (int)_lobbyState;
        
        lobbyUIParentClassArr[_lobbyTypeID].Enter_Func(_referenceID);

        Player_Data.Instance.OnLobbyWealthUI_Func();
        Player_Data.Instance.ActiveWealthUI_Func();
    }
    public void Exit_Func(LobbyState _lobbyState)
    {
        Exit_Func((int)_lobbyState);
    }
    public void Exit_Func(int _lobbyTypeID)
    {
        lobbyUIParentClassArr[_lobbyTypeID].Exit_Func();
    }
    #endregion
    #region FeedingRoom Group
    public void OnFeedingRoom_Func(int _selectUnitID)
    {
        // 999 = Hero, Other = Unit

        feedingRoomClass.Enter_Func(_selectUnitID);
    }
    public void OffFeedingRoom_Func(int _selectUnitID)
    {
        if(_selectUnitID == 999)
        {
            heroManagementClass.ReturnUI_Func();
        }
        else
        {
            partySettingClass.ReturnUI_Func();
        }
    }
    #endregion
    #region Stage Select Group
    public void BattleEnter_Func(BattleType _battleType)
    {
        stageSelectClass.Exit_Func();
        mainLobbyClass.Exit_Func();

        Game_Manager.Instance.BattleEnter_Func(_battleType);
    }
    #endregion
}