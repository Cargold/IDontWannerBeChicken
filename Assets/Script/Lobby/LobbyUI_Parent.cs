using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LobbyUI_Parent : MonoBehaviour
{
    [System.NonSerialized]
    public LobbyState lobbyType;
    [System.NonSerialized]
    public Lobby_Manager lobbyManager;

    public virtual void Init_Func(Lobby_Manager _lobbyManagerClass, LobbyState _lobbyType)
    {
        lobbyManager = _lobbyManagerClass;

        lobbyType = _lobbyType;

        RectTransform _thisRTrf = this.gameObject.GetComponent<RectTransform>();
        _thisRTrf.localPosition = Vector3.zero;
        _thisRTrf.anchoredPosition = Vector2.zero;

        InitUI_Func();
    }
    protected abstract void InitUI_Func();

    public void Enter_Func(int _referenceID = -1)
    {
        EnterUI_Func(_referenceID);
    }
    protected abstract void EnterUI_Func(int _referenceID = -1);

    public abstract void Exit_Func();
}