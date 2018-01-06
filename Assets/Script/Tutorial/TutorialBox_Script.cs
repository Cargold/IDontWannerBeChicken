using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialBox_Script : MonoBehaviour
{
    public TutorialSystem_Manager tutorialManager;
    
    public Transform[] tutorialTrfArr;

    public RectTransform thisRTrf;
    public Text tutorialText;

    public void Init_Func(TutorialSystem_Manager _tutorialManager)
    {
        tutorialManager = _tutorialManager;
    }
    public void OnTutorial_Func(TutorialSystem_Manager.TutorialData _tutoridalData)
    {
        int _chatBoxPosID = (int)_tutoridalData.chatBoxPos;
    
        thisRTrf.anchoredPosition = tutorialTrfArr[_chatBoxPosID].position;
        tutorialText.text = _tutoridalData.chatContent[TranslationSystem_Manager.Instance.languageTypeID];
    }
}
