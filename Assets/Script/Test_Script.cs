using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Test_Script : MonoBehaviour
{
    public bool isTest;

    void Update()
    {
        if (isTest == false) return;
        isTest = false;
        
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Test : " + other.transform.position);
    }
}
