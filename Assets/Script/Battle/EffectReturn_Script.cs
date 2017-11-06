using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectReturn_Script : MonoBehaviour
{
    public float returnTime;

    private void OnEnable()
    {
        StartCoroutine(ReturnEffect_Cor());
    }
    IEnumerator ReturnEffect_Cor()
    {
        yield return new WaitForSeconds(returnTime);

        ObjectPool_Manager.Instance.Free_Func(this.gameObject);
    }
}
