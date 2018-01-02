using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassiveBombardCol_Script : MonoBehaviour
{
    public MassiveBombard_Script massiveBombardClass;
    public SphereCollider sphereCol;
    public List<Character_Script> charClassList;
    public float rotateSpeed;
    public bool isActive;
    public CharEffectData effectData_Bomb;

    public SoundType[] sfxArr_Bomb;

    public void Init_Func(MassiveBombard_Script _massiveBombardClass)
    {
        massiveBombardClass = _massiveBombardClass;
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
        Transform _rend = this.transform.GetChild(0);

        while (isActive == true)
        {
            _rend.Rotate(Vector3.forward * rotateSpeed);
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

        massiveBombardClass.SetTarget_Func(this.transform.position, charClassList.ToArray());
        charClassList.Clear();

        effectData_Bomb.ActiveEffect_Func();

        SoundSystem_Manager.Instance.PlaySFX_Func(sfxArr_Bomb);

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
