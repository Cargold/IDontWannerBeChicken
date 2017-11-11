using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateSystem_Manager : MonoBehaviour
{
    public static TranslateSystem_Manager Instance;

    public LanguageType languageType;

    public IEnumerator Init_Cor()
    {
        Instance = this;

        yield break;
    }
}
