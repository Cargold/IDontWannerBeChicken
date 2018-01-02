using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSystem_Manager : MonoBehaviour
{
    public static TutorialSystem_Manager Instance;

    public struct TutorialData
    {
        public Vector2 pannelPos;
        public Vector2 pannelSize;
        public Vector2 chatBoxPos;
        public string chatContent;
    }

    public float pannelAlphaValue;

    public TutorialPannel_Script pannelClass;

    public TutorialData[] tutorialDataArr_BattleClear;
    public TutorialData[] tutorialDataArr_PartySetting;
    public TutorialData[] tutorialDataArr_Feeding;
    public TutorialData[] tutorialDataArr_SpecialBattle;
    public TutorialData[] tutorialDataArr_Museum;

    public TutorialType activeTutorialType;

    public IEnumerator Init_Cor()
    {
        Instance = this;

        yield break;
    }

    public void OnTutorial_Func(TutorialType _tutorialType)
    {
        activeTutorialType = _tutorialType;

        switch (_tutorialType)
        {
            case TutorialType.BattleClear:
                break;
            case TutorialType.PartySetting:
                break;
            case TutorialType.Feeding:
                break;
            case TutorialType.SpecialBattle:
                break;
            case TutorialType.Museum:
                break;
        }
    }

    public void OnButton_Func()
    {

    }
}
