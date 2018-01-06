using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSystem_Manager : MonoBehaviour
{
    public static TutorialSystem_Manager Instance;

    [System.Serializable]
    public struct TutorialData
    {
        public Vector2 pannelPos;
        public Vector2 pannelSize;

        public enum ChatBoxPos
        {
            None = -1,
            Top,
            Center,
            Bottom,
        }

        public ChatBoxPos chatBoxPos;
        [TextArea]
        public string[] chatContent;

        public bool isWholeBtn;
    }

    public float pannelAlphaValue;

    public TutorialPannel_Script pannelClass;
    public TutorialBox_Script boxClass;

    public TutorialData[] tutorialDataArr_BattleClear;
    public TutorialData[] tutorialDataArr_PartySetting;
    public TutorialData[] tutorialDataArr_Feeding;
    public TutorialData[] tutorialDataArr_SpecialBattle;
    public TutorialData[] tutorialDataArr_Museum;

    public TutorialType activeTutorialType;
    public int tutorialID;

    public IEnumerator Init_Cor()
    {
        Instance = this;

        yield break;
    }

    public void OnButton_Func()
    {
        OnTutorial_Func(activeTutorialType);
    }
    public void OnTutorial_Func(TutorialType _tutorialType, bool _isFirst = false)
    {
        activeTutorialType = _tutorialType;

        if(_isFirst == true)
            tutorialID = 0;
        else
            tutorialID++;

        switch (_tutorialType)
        {
            case TutorialType.BattleClear:
                if (tutorialID < tutorialDataArr_BattleClear.Length)
                {
                    pannelClass.OnTutorial_Func(tutorialDataArr_BattleClear[tutorialID]);
                    boxClass.OnTutorial_Func(tutorialDataArr_BattleClear[tutorialID]);
                }
                else
                {
                    Player_Data.Instance.isTutorial_BattleClear = true;
                    SaveSystem_Manager.Instance.SaveData_Func(SaveType.Tutorial_BattleClear, true);
                }
                OnBtn_BattleClear_Func();
                break;

            case TutorialType.PartySetting:
                if (tutorialID < tutorialDataArr_PartySetting.Length)
                {
                    tutorialID++;

                    pannelClass.OnTutorial_Func(tutorialDataArr_PartySetting[tutorialID]);
                    boxClass.OnTutorial_Func(tutorialDataArr_PartySetting[tutorialID]);
                }
                else
                {
                    Player_Data.Instance.isTutorial_PartySetting = true;
                    SaveSystem_Manager.Instance.SaveData_Func(SaveType.Tutorial_PartySetting, true);
                }
                OnBtn_PartySetting_Func();
                break;

            case TutorialType.Feeding:
                if (tutorialID < tutorialDataArr_Feeding.Length)
                {
                    tutorialID++;

                    pannelClass.OnTutorial_Func(tutorialDataArr_Feeding[tutorialID]);
                    boxClass.OnTutorial_Func(tutorialDataArr_Feeding[tutorialID]);
                }
                else
                {
                    Player_Data.Instance.isTutorial_Feeding = true;
                    SaveSystem_Manager.Instance.SaveData_Func(SaveType.Tutorial_Feeding, true);
                }
                OnBtn_Feeding_Func();
                break;

            case TutorialType.SpecialBattle:
                if (tutorialID < tutorialDataArr_SpecialBattle.Length)
                {
                    tutorialID++;

                    pannelClass.OnTutorial_Func(tutorialDataArr_SpecialBattle[tutorialID]);
                    boxClass.OnTutorial_Func(tutorialDataArr_SpecialBattle[tutorialID]);
                }
                else
                {
                    Player_Data.Instance.isTutorial_SpecialBattle = true;
                    SaveSystem_Manager.Instance.SaveData_Func(SaveType.Tutorial_SpecialBattle, true);
                }
                OnBtn_SpecialBattle_Func();
                break;

            case TutorialType.Museum:
                if (tutorialID < tutorialDataArr_Museum.Length)
                {
                    tutorialID++;

                    pannelClass.OnTutorial_Func(tutorialDataArr_Museum[tutorialID]);
                    boxClass.OnTutorial_Func(tutorialDataArr_Museum[tutorialID]);
                }
                else
                {
                    Player_Data.Instance.isTutorial_Museum = true;
                    SaveSystem_Manager.Instance.SaveData_Func(SaveType.Tutorial_Museum, true);
                }
                OnBtn_Museum_Func();
                break;
        }
    }
    void OnBtn_BattleClear_Func()
    {
        switch (tutorialID)
        {
            case 1:
                Lobby_Manager.Instance.Enter_Func(LobbyState.StageSelect);
                break;
            case 2:
                Lobby_Manager.Instance.stageSelectClass.BattleEnterNormal_Func();
                break;
        }
    }
    void OnBtn_PartySetting_Func()
    {

    }
    void OnBtn_Feeding_Func()
    {

    }
    void OnBtn_SpecialBattle_Func()
    {

    }
    void OnBtn_Museum_Func()
    {

    }

    public void OnButtonWhole_Func()
    {

    }
}
