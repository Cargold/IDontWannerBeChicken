using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPannel_Script : MonoBehaviour
{
    public TutorialSystem_Manager tutorialManager;
    public RectTransform thisRTrf;

    public Image[] pannelImageArr;

    public void Init_Func(TutorialSystem_Manager _tutorialManager)
    {
        tutorialManager = _tutorialManager;

        for (int i = 0; i < pannelImageArr.Length; i++)
        {
            pannelImageArr[i].color.GetNaturalAlphaColor_Func(_tutorialManager.pannelAlphaValue);
        }
    }

    public void OnTutorial_Func(TutorialSystem_Manager.TutorialData _tutoridalData)
    {
        thisRTrf.anchoredPosition = _tutoridalData.pannelPos;
        thisRTrf.sizeDelta = _tutoridalData.pannelSize;
    }

    public void OnButton_Func()
    {
        tutorialManager.OnButton_Func();
    }
}
