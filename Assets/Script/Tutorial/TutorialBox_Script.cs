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
        this.gameObject.SetActive(true);

        if (_tutoridalData.chatBoxPos != TutorialSystem_Manager.TutorialData.ChatBoxPos.None)
        {
            int _chatBoxPosID = (int)_tutoridalData.chatBoxPos;
            thisRTrf.gameObject.SetActive(true);
            thisRTrf.position = tutorialTrfArr[_chatBoxPosID].position;
        }
        else
        {
            thisRTrf.gameObject.SetActive(false);
        }
        tutorialText.text = _tutoridalData.chatContent[TranslationSystem_Manager.Instance.languageTypeID];
    }

    public void Deactive_Func()
    {
        this.gameObject.SetActive(false);
    }
}
