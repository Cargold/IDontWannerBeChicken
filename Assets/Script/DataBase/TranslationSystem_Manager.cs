using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslationSystem_Manager : MonoBehaviour
{
    public static TranslationSystem_Manager Instance;

    public LanguageType languageType;

    public IEnumerator Init_Cor()
    {
        Instance = this;

        yield break;
    }
}
