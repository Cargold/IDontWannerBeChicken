using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell_Script : MonoBehaviour
{
    public Character_Script charClass;
    public GroupType groupType;
    public SphereCollider sphereCol;
    public SpriteRenderer thisRend;

    public void Init_Func(Character_Script _charClass, int _sortingOrder)
    {
        charClass = _charClass;

        groupType = _charClass.groupType;

        sphereCol.enabled = false;

        if(thisRend != null)
            thisRend.sortingOrder = _sortingOrder;
    }

    public void OnAttack_Func()
    {
        StartCoroutine(OnAttack_Cor());
    }
    IEnumerator OnAttack_Cor()
    {
        sphereCol.enabled = true;

        yield return new WaitForFixedUpdate();

        sphereCol.enabled = false;

        ObjectPool_Manager.Instance.Free_Func(this.gameObject);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Character")
        {
            Character_Script _targetCharClass = collision.gameObject.GetComponent<Character_Script>();

            if (groupType != _targetCharClass.groupType)
            {
                charClass.OnAttackPlural_Func(_targetCharClass);
            }
        }
    }
}