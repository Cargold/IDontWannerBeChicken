using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell_Script : MonoBehaviour
{
    public Character_Script charClass;
    public GroupType groupType;
    public SphereCollider sphereCol;
    public SpriteRenderer thisRend;
    public bool isContactAttackTiming;
    public List<Character_Script> contactCharClassList;
    public CharEffectData effectData_ContactTarget;
    public CharEffectData effectData_ArriveTarget;

    public void Init_Func(Character_Script _charClass, int _sortingOrder)
    {
        charClass = _charClass;

        groupType = _charClass.groupType;

        sphereCol.enabled = false;

        if(thisRend != null)
            thisRend.sortingOrder = _sortingOrder;

        isContactAttackTiming = _charClass.isContactAttackTiming;

        if(isContactAttackTiming == true)
        {
            sphereCol.enabled = true;
            contactCharClassList = new List<Character_Script>();
            StartCoroutine(OnAttackByContact_Cor());
        }
    }

    public void OnAttack_Func()
    {
        if(this.gameObject.activeInHierarchy == true)
            StartCoroutine(OnAttack_Cor());
    }
    IEnumerator OnAttack_Cor()
    {
        sphereCol.enabled = true;

        if (effectData_ArriveTarget.isEffectOn == true)
        {
            GameObject _effectObj = ObjectPool_Manager.Instance.Get_Func(effectData_ArriveTarget.effectObj);
            _effectObj.transform.position = effectData_ArriveTarget.effectPos.position;

            if (effectData_ArriveTarget.isSetParentTrf == true)
            {
                _effectObj.transform.SetParent(effectData_ArriveTarget.effectPos);
                _effectObj.transform.rotation = effectData_ArriveTarget.effectPos.rotation;
            }
        }

        yield return new WaitForFixedUpdate();

        sphereCol.enabled = false;

        ObjectPool_Manager.Instance.Free_Func(this.gameObject);
    }
    IEnumerator OnAttackByContact_Cor()
    {
        while(true)
        {
            for (int i = 0; i < contactCharClassList.Count; i++)
            {
                Vector3 _thisPos = new Vector3(this.transform.position.x, 0f, 0f);

                float _distanceValue
                    = Vector3.Distance(_thisPos, contactCharClassList[i].transform.position);
                
                if(_distanceValue < 1f)
                {
                    if (effectData_ContactTarget.isEffectOn == true)
                    {
                        GameObject _effectObj = ObjectPool_Manager.Instance.Get_Func(effectData_ContactTarget.effectObj);
                        _effectObj.transform.position = effectData_ContactTarget.effectPos.position;

                        if(effectData_ContactTarget.isSetParentTrf == true)
                        {
                            _effectObj.transform.SetParent(effectData_ContactTarget.effectPos);
                            _effectObj.transform.rotation = effectData_ContactTarget.effectPos.rotation;
                        }
                    }

                    charClass.OnAttackPlural_Func(contactCharClassList[i], true);

                    sphereCol.enabled = false;

                    ObjectPool_Manager.Instance.Free_Func(this.gameObject);

                    yield break;
                }
            }

            yield return new WaitForFixedUpdate();
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Character")
        {
            Character_Script _targetCharClass = collision.gameObject.GetComponent<Character_Script>();

            if(isContactAttackTiming == false)
            {
                if (groupType != _targetCharClass.groupType)
                {
                    charClass.OnAttackPlural_Func(_targetCharClass);
                }
            }
            else if(isContactAttackTiming == true)
            {
                if (groupType != _targetCharClass.groupType)
                {
                    contactCharClassList.Add(_targetCharClass);
                }
            }
        }
    }

    public void Deactive_Func()
    {
        sphereCol.enabled = false;
        StopAllCoroutines();
        ObjectPool_Manager.Instance.Free_Func(this.gameObject);
    }
}