using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslationSystem_Manager : MonoBehaviour
{
    public static TranslationSystem_Manager Instance;
    
    public LanguageType languageType;
    public int languageTypeID;

    public string trophyRoomNumDesc
    {
        get
        {
            return trophyRoomNumDescArr[languageTypeID];
        }
    }
    [SerializeField]
    private string[] trophyRoomNumDescArr;

    public IEnumerator Init_Cor()
    {
        Instance = this;

        yield break;
    }
    public void SetLanguageType_Func(LanguageType _languageType)
    {
        languageType = _languageType;
        languageTypeID = (int)_languageType;
    }
}
