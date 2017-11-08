using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CharEffectData
{
    [SerializeField]
    private Transform parentTrf;
    [SerializeField]
    private bool isRotationSet;
    [SerializeField]
    private Transform effectPos;
    private Vector3 effectVec;
    [SerializeField]
    private GameObject effectObj;
    [SerializeField]
    private float activeDelayTime;

    public void SetEffectInfo(Vector3 _pos)
    {
        effectVec = _pos;
    }
    public void SetActiveDelayTime_Func(float _time)
    {
        activeDelayTime = _time;
    }

    public void SetParentTrf_Func(Transform _trf)
    {
        parentTrf = _trf;
    }
    public void ActiveEffect_Func()
    {
        Game_Manager.Instance.StartCoroutine(Active_Cor());
    }
    IEnumerator Active_Cor()
    {
        yield return new WaitForSeconds(activeDelayTime);

        GameObject _effectObj = null;
        if (effectObj != null)
        {
            _effectObj = ObjectPool_Manager.Instance.Get_Func(effectObj);

            if (effectPos != null)
                _effectObj.transform.position = effectPos.position;
            else
                _effectObj.transform.position = effectVec;

            if (isRotationSet == true)
                _effectObj.transform.rotation = effectPos.rotation;

            if (parentTrf != null)
            {
                _effectObj.transform.SetParent(parentTrf);
            }
        }
    }
}