using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlamCol_Script : MonoBehaviour
{
    public Slam_Script slamClass;

    public void Init_Func(Slam_Script _slamClass)
    {
        slamClass = _slamClass;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Character")
        {
            Character_Script _charClass = other.GetComponent<Character_Script>();

            if(_charClass.groupType == GroupType.Enemy)
            {
                
            }
        }
    }
}
