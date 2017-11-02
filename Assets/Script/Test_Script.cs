using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Test_Script : MonoBehaviour
{
    public Nature_Script natureClass;

    private void Awake()
    {
        natureClass = this.GetComponent<Nature_Script>();
    }

    void Update()
    {
        
    }
}
