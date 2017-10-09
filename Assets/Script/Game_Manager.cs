using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    public static Game_Manager Instance;

    public Battle_Manager battleClass;
    public Farm_Manager farmClass;

    public ObjectPoolManager objectPoolManager;
    public Player_Script playerClass;
    public SmoothFollow_Script mainCameraSmoothClass;

    public enum GameState
    {
        None = -1,
        MainState,
        FarmState,
        BattleState,
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
        yield return farmClass.Init_Cor();

        yield return playerClass.Init_Cor();
        yield return objectPoolManager.Init_Cor();
    }

    IEnumerator InitMain_Cor()
    {
        Instance = this;

        yield break;
    }

    #region Farm State
    public void FarmEnter_Func()
    {
        gameState = GameState.FarmState;

        farmClass.FarmEnter_Func();
    }
    #endregion
    #region Main State
    public void MainEnter_Func()
    {
        gameState = GameState.MainState;
    }
    #endregion
    #region Battle State
    public void BattleEnter_Func()
    {
        gameState = GameState.BattleState;

        battleClass.BattleEnter_Func();
    }
    #endregion
}
