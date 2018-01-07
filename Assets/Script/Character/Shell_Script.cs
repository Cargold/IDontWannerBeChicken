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
    public CharEffectData effectData_Deactive;

    public SoundType[] sfxArr_Fire;
    public SoundType[] sfxArr_Bomb;

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

        SoundSystem_Manager.Instance.PlaySFX_Func(sfxArr_Fire);
    }

    public void OnAttack_Func()
    {
        if(this.gameObject.activeInHierarchy == true)
            StartCoroutine(OnAttack_Cor());
    }
    IEnumerator OnAttack_Cor()
    {
        sphereCol.enabled = true;

        effectData_ArriveTarget.ActiveEffect_Func();

        yield return new WaitForFixedUpdate();

        sphereCol.enabled = false;

        SoundSystem_Manager.Instance.PlaySFX_Func(sfxArr_Bomb);

        Deactive_Func();
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
                    effectData_ContactTarget.ActiveEffect_Func();

                    // 투사체와 충돌된 상황이므로 거리 계산할 필요 없음
                    charClass.OnAttackPlural_Func(contactCharClassList[i], false);

                    sphereCol.enabled = false;

                    SoundSystem_Manager.Instance.PlaySFX_Func(sfxArr_Bomb);

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

            //if(_targetCharClass == null || charClass == null)
            //{
            //    Debug.Log("Test, This : " + this.gameObject.name);
            //    Debug.Log("Test, Char : " + charClass);
            //    Debug.Log("Test, _targetCharClass : " + _targetCharClass);
            //}

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
        if (this.gameObject.activeInHierarchy == false) return;

        effectData_Deactive.ActiveEffect_Func();

        sphereCol.enabled = false;
        StopAllCoroutines();

        contactCharClassList.Clear();

        charClass.spawnShellClass = null;

        ObjectPool_Manager.Instance.Free_Func(this.gameObject);
    }
}