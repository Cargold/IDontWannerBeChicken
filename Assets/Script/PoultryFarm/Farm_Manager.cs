using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm_Manager : MonoBehaviour
{
    public static Farm_Manager Instance;

    public GameObject farmObj;

    public IEnumerator Init_Cor()
    {
        Instance = this;

        farmObj.SetActive(false);

        yield break;
    }

    public void FarmEnter_Func()
    {
        StartCoroutine(InitFarm_Cor());
    }

    IEnumerator InitFarm_Cor()
    {
        farmObj.SetActive(true);

        yield break;
    }

    public void FarmExit_Func()
    {
        farmObj.SetActive(false);
    }
}
