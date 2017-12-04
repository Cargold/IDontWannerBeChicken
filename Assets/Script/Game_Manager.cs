using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_Manager : MonoBehaviour
{
    public static Game_Manager Instance;
    
    public TranslationSystem_Manager translationSystem;
    public DataBase_Manager databaseClass;
    public Player_Data playerDataClass;
    public Battle_Manager battleClass;
    public Lobby_Manager lobbyClass;
    public ObjectPool_Manager objectPoolManager;
    public SmoothFollow_Script mainCameraSmoothClass;
    public BattleStartDirection_Script directionClass;
    public Enviroment_Manager enviromentClass;

    public GameState gameState;
    public Image loadingImage;

    void Awake()
    {
        StartCoroutine(Init_Cor());
    }   

    IEnumerator Init_Cor()
    {
        yield return InitMain_Cor();
        yield return translationSystem.Init_Cor();  // 1. 언어 설정
        yield return databaseClass.Init_Cor();      // 2. DB에서 고정 데이터 불러오기
        yield return objectPoolManager.Init_Cor();  // 3. DB 정보를 바탕으로 풀링 생성
        yield return playerDataClass.Init_Cor();    // 4. 생성된 풀링들 중 샘플에 플레이어 데이터 적용
        yield return lobbyClass.Init_Cor();         // 5. 플레이어 데이터를 바탕으로 로비 구성
        yield return directionClass.Init_Cor();     // 6. 메인로비의 Idle 애니메이션 연출 시작
        yield return battleClass.Init_Cor();        // 7. 스폰매니저 등 전투 관련 데이터 활성화
        yield return enviromentClass.Init_Cor();    // 8. 환경 생성

        yield return Loading_Cor(true);

        LobbyEnter_Func();
    }

    IEnumerator InitMain_Cor()
    {
        Instance = this;

        yield break;
    }
    
    IEnumerator Loading_Cor(bool _isLoadingClear)
    {
        if (_isLoadingClear == false)
        {
            loadingImage.SetNaturalAlphaColor_Func(1f);
            loadingImage.raycastTarget = true;
        }
        else if(_isLoadingClear == true)
        {
            loadingImage.SetNaturalAlphaColor_Func(0f);
            loadingImage.raycastTarget = false;
        }

        yield break;
    }

    public void BattleEnter_Func(BattleType _battleType, int _stageID_Next = -1)
    {
        gameState = GameState.Battle;

        if (_stageID_Next == -1)
        {
            if (_battleType == BattleType.Normal)
            {
                _stageID_Next = Player_Data.Instance.stageID_Normal;
            }
            else if (_battleType == BattleType.Special)
            {
                _stageID_Next = Player_Data.Instance.stageID_Special;
            }
        }

        battleClass.BattleEnter_Func(_battleType, _stageID_Next + 1);
        directionClass.EnterUI_Func(GameState.Battle);
        enviromentClass.OnSky_Func(_battleType);
    }

    public void LobbyEnter_Func()
    {
        gameState = GameState.Lobby;
        
        lobbyClass.Enter_Func(LobbyState.MainLobby);
        directionClass.EnterUI_Func(GameState.Lobby);
    }

    public void Loading_Func()
    {
        StartCoroutine(Loading_Cor(false));
    }
    public void LoadingClear_Func()
    {
        StartCoroutine(Loading_Cor(true));
    }
}
