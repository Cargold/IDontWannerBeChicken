using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPackCol_Script : MonoBehaviour
{
    public HealPack_Script healpackClass;
    public SphereCollider sphereCol;
    public List<Character_Script> charClassList;
    public bool isActive;
    public CharEffectData effectData_Heal;

    public void Init_Func(HealPack_Script _healpackClass)
    {
        healpackClass = _healpackClass;
        sphereCol.enabled = false;
        charClassList = new List<Character_Script>();
    }

    public void Active_Func()
    {
        isActive = true;
        this.gameObject.SetActive(true);
    }

    public void Heal_Func()
    {
        StartCoroutine(Heal_Cor());
    }
    IEnumerator Heal_Cor()
    {
        sphereCol.enabled = true;

        yield return new WaitForFixedUpdate();

        sphereCol.enabled = false;

        healpackClass.SetTarget_Func(charClassList.ToArray());
        charClassList.Clear();

        if (effectData_Heal.isEffectOn == true)
        {
            GameObject _effectObj = ObjectPool_Manager.Instance.Get_Func(effectData_Heal.effectObj);
            _effectObj.transform.position = effectData_Heal.effectPos.position;
            _effectObj.transform.rotation = effectData_Heal.effectPos.rotation;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Character")
        {
            Character_Script _targetCharClass = other.gameObject.GetComponent<Character_Script>();

            if (_targetCharClass.groupType == GroupType.Ally)
            {
                charClassList.Add(_targetCharClass);
            }
        }
    }

    public void Deactive_Func()
    {
        isActive = false;

        this.gameObject.SetActive(false);
    }
}
