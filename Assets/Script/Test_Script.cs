using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Script : MonoBehaviour
{
    public bool isTest;
    public Animator aaa;

    void Update()
    {
        if (isTest == false) return;
        isTest = false;


        aaa.SetBool("OnContact", true);
    }
}
