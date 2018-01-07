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

        pannelClass.Init_Func(this);
        boxClass.Init_Func(this);

        Deactive_Func();

        yield break;
    }

    public void OnButton_Func()
    {
        OnTutorial_Func(activeTutorialType, false);
    }
    public void OnTutorial_Func(TutorialType _tutorialType, bool _isFirst = true)
    {
        if (activeTutorialType != TutorialType.None && _isFirst == true) return;

        activeTutorialType = _tutorialType;

        if(_isFirst == true)
            tutorialID = 0;
        else
            tutorialID++;

        switch (_tutorialType)
        {
            case TutorialType.BattleClear:
                pannelClass.OnTutorial_Func(tutorialDataArr_BattleClear[tutorialID]);
                boxClass.OnTutorial_Func(tutorialDataArr_BattleClear[tutorialID]);
                OnBtn_BattleClear_Func();
                break;

            case TutorialType.PartySetting:
                pannelClass.OnTutorial_Func(tutorialDataArr_PartySetting[tutorialID]);
                boxClass.OnTutorial_Func(tutorialDataArr_PartySetting[tutorialID]);
                OnBtn_PartySetting_Func();
                break;

            case TutorialType.Feeding:
                pannelClass.OnTutorial_Func(tutorialDataArr_Feeding[tutorialID]);
                boxClass.OnTutorial_Func(tutorialDataArr_Feeding[tutorialID]);
                OnBtn_Feeding_Func();
                break;

            case TutorialType.SpecialBattle:
                pannelClass.OnTutorial_Func(tutorialDataArr_SpecialBattle[tutorialID]);
                boxClass.OnTutorial_Func(tutorialDataArr_SpecialBattle[tutorialID]);
                OnBtn_SpecialBattle_Func();
                break;

            case TutorialType.Museum:
                pannelClass.OnTutorial_Func(tutorialDataArr_Museum[tutorialID]);
                boxClass.OnTutorial_Func(tutorialDataArr_Museum[tutorialID]);
                OnBtn_Museum_Func();
                break;
        }
    }
    void OnBtn_BattleClear_Func()
    {
        switch (tutorialID)
        {
            case 2:
                Lobby_Manager.Instance.Enter_Func(LobbyState.StageSelect);
                break;
            case 3:
                Lobby_Manager.Instance.stageSelectClass.BattleEnterNormal_Func();

                Player_Data.Instance.isTutorial_BattleClear = true;
                SaveSystem_Manager.Instance.SaveData_Func(SaveType.Tutorial_BattleClear, true);
                Deactive_Func();
                break;
        }
    }
    void OnBtn_PartySetting_Func()
    {
        switch (tutorialID)
        {
            case 0:
                break;
            case 1:
                Player_Data.Instance.isTutorial_PartySetting = true;
                SaveSystem_Manager.Instance.SaveData_Func(SaveType.Tutorial_PartySetting, true);
                Deactive_Func();
                break;
        }
    }
    void OnBtn_Feeding_Func()
    {
        switch (tutorialID)
        {
            case 0:
                break;
            case 1:
                Player_Data.Instance.isTutorial_Feeding = true;
                SaveSystem_Manager.Instance.SaveData_Func(SaveType.Tutorial_Feeding, true);
                Deactive_Func();
                break;
        }
    }
    void OnBtn_SpecialBattle_Func()
    {
        switch (tutorialID)
        {
            case 0:
                break;
            case 1:
                Player_Data.Instance.isTutorial_SpecialBattle = true;
                SaveSystem_Manager.Instance.SaveData_Func(SaveType.Tutorial_SpecialBattle, true);
                Deactive_Func();
                break;
        }
    }
    void OnBtn_Museum_Func()
    {
        switch (tutorialID)
        {
            case 0:
                break;
            case 1:
                Player_Data.Instance.isTutorial_Museum = true;
                SaveSystem_Manager.Instance.SaveData_Func(SaveType.Tutorial_Museum, true);
                Deactive_Func();
                break;
        }
    }

    void Deactive_Func()
    {
        activeTutorialType = TutorialType.None;
        tutorialID = 0;

        pannelClass.Deactive_Func();
        boxClass.Deactive_Func();
    }
}
