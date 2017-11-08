using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodThirstCol_Script : MonoBehaviour
{
    public BloodThirst_Script bloodThirstClass;
    public SphereCollider sphereCol;
    public List<Character_Script> charClassList;
    public bool isActive;
    public CharEffectData effectData_Heal;

    public void Init_Func(BloodThirst_Script _bloodThirstClass)
    {
        bloodThirstClass = _bloodThirstClass;
        sphereCol.enabled = false;
        charClassList = new List<Character_Script>();
    }

    public void Active_Func()
    {
        isActive = true;
        this.gameObject.SetActive(true);
    }

    public void Haste_Func()
    {
        StartCoroutine(Haste_Cor());
    }
    IEnumerator Haste_Cor()
    {
        sphereCol.enabled = true;

        yield return new WaitForFixedUpdate();

        sphereCol.enabled = false;

        bloodThirstClass.SetTarget_Func(charClassList.ToArray());
        charClassList.Clear();

        effectData_Heal.ActiveEffect_Func();
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
