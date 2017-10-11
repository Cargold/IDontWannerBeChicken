using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LobbyUI_Parent : MonoBehaviour
{
    public Lobby_Manager.LobbyState lobbyType;
    public Lobby_Manager lobbyManager;

    public virtual void Init_Func(Lobby_Manager _lobbyManagerClass, Lobby_Manager.LobbyState _lobbyType)
    {
        lobbyManager = _lobbyManagerClass;

        lobbyType = _lobbyType;

        InitUI_Func();
    }
    protected abstract void InitUI_Func();

    public void Enter_Func()
    {
        EnterUI_Func();
    }
    protected abstract void EnterUI_Func();

    public abstract void Exit_Func();
}