using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPannel_Script : MonoBehaviour
{
    public TutorialSystem_Manager tutorialManager;
    public RectTransform thisRTrf;

    public void Init_Func(TutorialSystem_Manager _tutorialManager)
    {
        tutorialManager = _tutorialManager;
    }
    public void OnTutorial_Func(Vector2 _pos, Vector2 _size)
    {
        thisRTrf.anchoredPosition = _pos;
        thisRTrf.sizeDelta = _size;
    }
    public void OnButton_Func()
    {
        tutorialManager.OnButton_Func();
    }
}
