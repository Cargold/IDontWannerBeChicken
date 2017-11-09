using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushEffect_Script : MonoBehaviour
{
    public Rush_Script rushClass;
    public GameObject effectObj;

    public void Init_Func(Rush_Script _rushClass)
    {
        rushClass = _rushClass;

        GameObject _effectObj = Instantiate(effectObj);
        effectObj = _effectObj;
        effectObj.transform.SetParent(this.transform);
        effectObj.transform.localPosition = new Vector3(-1.39f, 2.76f, 0f);

        this.gameObject.SetActive(false);
    }

    public void Active_Func(float _time, Transform _playerTrf)
    {
        this.gameObject.SetActive(true);
        this.transform.SetParent(_playerTrf);
        this.transform.localPosition = Vector3.zero;
        
        StartCoroutine(Active_Cor(_time));
    }
    IEnumerator Active_Cor(float _time)
    {
        yield return new WaitForSeconds(_time);
        this.transform.SetParent(rushClass.transform);
        this.gameObject.SetActive(false);
    }
}
