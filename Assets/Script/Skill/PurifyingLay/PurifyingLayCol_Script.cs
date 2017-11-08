using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurifyingLayCol_Script : MonoBehaviour
{
    public PurifyingLay_Script purifyingLayClass;
    public SphereCollider sphereCol;
    public List<Character_Script> charClassList;
    private float layInterval;
    public bool isActive;
    public CharEffectData effectData_Active;

    public void Init_Func(PurifyingLay_Script _purifyingLayClass)
    {
        purifyingLayClass = _purifyingLayClass;
        sphereCol.enabled = false;
        charClassList = new List<Character_Script>();
    }
    public void Active_Func(float _layInterval)
    {
        isActive = true;

        layInterval = _layInterval;

        this.gameObject.SetActive(true);

        StartCoroutine(OnBomb_Cor());
    }
    IEnumerator OnBomb_Cor()
    {
        while(isActive == true)
        {
            sphereCol.enabled = true;

            yield return new WaitForFixedUpdate();

            sphereCol.enabled = false;

            purifyingLayClass.SetTarget_Func(this.transform.position, charClassList.ToArray());
            charClassList.Clear();

            yield return new WaitForSeconds(layInterval);
        }
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
