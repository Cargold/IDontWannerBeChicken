using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeCol_Script : MonoBehaviour
{
    public Grenade_Script grenadeClass;
    public SphereCollider sphereCol;
    public List<Character_Script> charClassList;
    public float rotateSpeed;
    public bool isActive;
    public CharEffectData effectData_Bomb;

    public void Init_Func(Grenade_Script _grenadeClass)
    {
        grenadeClass = _grenadeClass;
        sphereCol.enabled = false;
        charClassList = new List<Character_Script>();
    }

    public void Active_Func()
    {
        isActive = true;

        this.gameObject.SetActive(true);
        StartCoroutine(Rotate_Cor());
    }
    IEnumerator Rotate_Cor()
    {
        while(isActive == true)
        {
            this.transform.Rotate(Vector3.forward * rotateSpeed);
            yield return new WaitForFixedUpdate();
        }
    }

    public void Bomb_Func()
    {
        StartCoroutine(OnBomb_Cor());
    }
    IEnumerator OnBomb_Cor()
    {
        sphereCol.enabled = true;

        yield return new WaitForFixedUpdate();

        sphereCol.enabled = false;

        grenadeClass.SetTarget_Func(charClassList.ToArray());
        charClassList.Clear();

        effectData_Bomb.ActiveEffect_Func();

        Deactive_Func();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Character")
        {
            Character_Script _targetCharClass = other.gameObject.GetComponent<Character_Script>();

            if (_targetCharClass.groupType == GroupType.Enemy)
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
