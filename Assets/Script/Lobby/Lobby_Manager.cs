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

    public enum LobbyState
    {
        None = -1,
        MainLobby,
        StageSelect,
        HeroManagement,
        PartySetting,
        StoreRoom,
        QuestRoom,
        TrophyRoom,

        FeedingRoom,
    }
    //public LobbyState lobbyState
    //{
    //    get
    //    {
    //        return m_LobbyState;
    //    }
    //}
    //private LobbyState m_LobbyState;

    public IEnumerator Init_Cor()
    {
        Instance = this;

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
        storeRoomClass = (StoreRoom_Script)lobbyUIParentClassArr[4];
        questRoomClass = (QuestRoom_Script)lobbyUIParentClassArr[5];
        trophyRoomClass = (TrophyRoom_Script)lobbyUIParentClassArr[6];
        feedingRoomClass = (FeedingRoom_Script)lobbyUIParentClassArr[7];
        
        yield break;
    }
    #region Lobby Group
    public void LobbyEnter_Func()
    {
        Enter_Func(LobbyState.MainLobby);
    }
    public void Enter_Func(string _loobyTypeText)
    {
        LobbyState _lobbyState = _loobyTypeText.ToEnum<LobbyState>();
        Enter_Func(_lobbyState);
    }
    public void Enter_Func(LobbyState _lobbyState)
    {
        int _lobbyTypeID = (int)_lobbyState;
        
        lobbyUIParentClassArr[_lobbyTypeID].Enter_Func();
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