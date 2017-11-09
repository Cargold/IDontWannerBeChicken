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
    private Transform effectTrf;
    private Vector3 effectVec;
    private Vector3 effectRot;
    [SerializeField]
    private GameObject effectObj;
    [SerializeField]
    private float activeDelayTime;

    public void SetEffectTrf(Transform _trf)
    {
        effectTrf = _trf;
    }
    public void SetEffectPos(Vector3 _pos)
    {
        effectVec = _pos;
    }
    public void SetActiveDelayTime_Func(float _time)
    {
        activeDelayTime = _time;
    }
    public void SetRotation_Func(Vector3 _rot)
    {
        effectRot = _rot;
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

            if (effectTrf != null)
                _effectObj.transform.position = effectTrf.position;
            else
                _effectObj.transform.position = effectVec;

            if (isRotationSet == true)
            {
                if(effectTrf != null)
                    _effectObj.transform.rotation = effectTrf.rotation;
                else
                    _effectObj.transform.eulerAngles = effectRot;
            }

            if (parentTrf != null)
            {
                _effectObj.transform.SetParent(parentTrf);
            }
        }
    }
}