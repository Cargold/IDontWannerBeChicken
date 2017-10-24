using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    public static Game_Manager Instance;

    public DataBase_Manager databaseClass;
    public Player_Data playerDataClass;
    public Battle_Manager battleClass;
    public Lobby_Manager lobbyClass;
    public ObjectPoolManager objectPoolManager;
    public SmoothFollow_Script mainCameraSmoothClass;

    #region Reference Variable
    public Sprite[] populationSpriteArr;
    public Sprite[] manaCostSpriteArr;

    public Color textColor;

    public float[] foodGradePenaltyValue;
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
        yield return databaseClass.Init_Cor();      // 1. DB에서 고정 데이터 불러오기
        yield return objectPoolManager.Init_Cor();  // 2. DB 정보를 바탕으로 풀링 생성
        yield return playerDataClass.Init_Cor();    // 3. 생성된 풀링들 중 샘플에 플레이어 데이터 적용
        yield return lobbyClass.Init_Cor();         // 4. 플레이어 데이터를 바탕으로 로비 구성
        yield return battleClass.Init_Cor();

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

        lobbyClass.LobbyEnter_Func();
    }
}
