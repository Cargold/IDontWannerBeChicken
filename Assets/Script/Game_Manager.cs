using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_Manager : MonoBehaviour
{
    public static Game_Manager Instance;

    public DataBase_Manager databaseClass;
    public Player_Data playerDataClass;
    public Battle_Manager battleClass;
    public Lobby_Manager lobbyClass;
    public ObjectPoolManager objectPoolManager;
    public SmoothFollow_Script mainCameraSmoothClass;
    public BattleStartDirection_Script directionClass;

    #region Reference Variable
    public Sprite[] populationSpriteArr;
    public Sprite[] manaCostSpriteArr;

    public Color textColor;

    public float[] foodGradePenaltyValue;
    #endregion
    
    public GameState gameState;
    public Image loadingImage;

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
        yield return directionClass.Init_Cor();     // 5. 메인로비의 Idle 애니메이션 연출 시작
        yield return battleClass.Init_Cor();        // 6. 스폰매니저 등 전투 관련 데이터 활성화

        yield return Loading_Cor();

        LobbyEnter_Func();
    }

    IEnumerator InitMain_Cor()
    {
        Instance = this;

        yield break;
    }
    
    IEnumerator Loading_Cor(bool _isOn)
    {
        yield break;
    }

    public void BattleEnter_Func(BattleType _battleType)
    {
        gameState = GameState.Battle;
        
        battleClass.BattleEnter_Func(_battleType);
        directionClass.EnterUI_Func(GameState.Battle);
    }

    public void LobbyEnter_Func()
    {
        gameState = GameState.Lobby;

        lobbyClass.Enter_Func(LobbyState.MainLobby);
        directionClass.EnterUI_Func(GameState.Lobby);
    }

    public void Loading_Func()
    {

    }
    public void LoadingOver_Func()
    {
        StartCoroutine(Loading_Cor());
    }
}
