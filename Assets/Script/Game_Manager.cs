using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    public static Game_Manager Instance;

    public Battle_Manager battleClass;
    public Lobby_Manager lobbyClass;
    public ObjectPoolManager objectPoolManager;
    public SmoothFollow_Script mainCameraSmoothClass;

    #region Reference Variable
    // Lobby_Manager . LobbyUiControl
    public RectTransform menuGroupTrf;
    public RectTransform lobbyViewTrf;


    #endregion

    public enum GameState
    {
        None = -1,
        Battle,
        Lobby,
    }
    public GameState gameState;

    void Awake()
    {
        StartCoroutine(Init_Cor());
    }

    IEnumerator Init_Cor()
    {
        yield return InitMain_Cor();
        yield return battleClass.Init_Cor();
        yield return lobbyClass.Init_Cor();
        yield return objectPoolManager.Init_Cor();

        yield return LoadingOver_Cor();
    }

    IEnumerator InitMain_Cor()
    {
        Instance = this;

        yield break;
    }
    
    IEnumerator LoadingOver_Cor()
    {
        yield break;
    }

    public void BattleEnter_Func()
    {
        gameState = GameState.Battle;

        battleClass.BattleEnter_Func();
    }

    public void LobbyEnter_Func()
    {
        gameState = GameState.Lobby;
    }
}
