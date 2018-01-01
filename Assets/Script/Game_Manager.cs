using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_Manager : MonoBehaviour
{
    public static Game_Manager Instance;
    
    public TranslationSystem_Manager translationSystem;
    public SaveSystem_Manager saveSystemManager;
    public DataBase_Manager databaseClass;
    public ObjectPool_Manager objectPoolManager;
    public Player_Data playerDataClass;
    public Lobby_Manager lobbyClass;
    public BattleStartDirection_Script directionClass;
    public Battle_Manager battleClass;
    public SkillSystem_Manager skillSystemManager;
    public Enviroment_Manager enviromentClass;
    public SmoothFollow_Script mainCameraSmoothClass;
    public SoundSystem_Manager soundManager;

    public GameState gameState;
    public Button loadingBtn;
    public Text loadingText;

    void Awake()
    {
        StartCoroutine(Init_Cor());
    }   

    IEnumerator Init_Cor()
    {
        InitMain_Func();
        translationSystem.Init_Func();  // 언어 설정

        loadingText.text = translationSystem.LoadingArr;

        yield return saveSystemManager.Init_Cor();  // 1. 세이브 기능 세팅
        yield return databaseClass.Init_Cor();      // 2. DB에서 고정 데이터 불러오기
        yield return objectPoolManager.Init_Cor();  // 3. DB 정보를 바탕으로 풀링 생성
        yield return playerDataClass.Init_Cor();    // 4. 생성된 풀링들 중 샘플에 플레이어 데이터 적용
        yield return lobbyClass.Init_Cor();         // 5. 플레이어 데이터를 바탕으로 로비 구성
        yield return directionClass.Init_Cor();     // 6. 메인로비의 Idle 애니메이션 연출 시작
        yield return battleClass.Init_Cor();        // 7. 스폰매니저 등 전투 관련 데이터 활성화
        yield return skillSystemManager.Init_Cor(); // 8. 스킬매니저 초기화
        yield return enviromentClass.Init_Cor();    // 9. 환경 생성
        yield return soundManager.Init_Cor();

        yield return Loading_Cor();

        LobbyEnter_Func();
    }

    void InitMain_Func()
    {
        Instance = this;

        Screen.SetResolution(1920, 1080, true);

        //int _height = Screen.currentResolution.height;
        //int _width = Screen.currentResolution.width;

        //Debug.Log("Test : " + _width + ", " + _height);
    }
    
    IEnumerator Loading_Cor()
    {
        loadingText.text = translationSystem.PressToStartArr;

        loadingBtn.interactable = true;

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
        
        directionClass.EnterUI_Func(GameState.Lobby);
        
        lobbyClass.Enter_Func(LobbyState.MainLobby);
    }
}
