using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause_Script : MonoBehaviour
{
    public Animation anim;
    public GameObject creditObj;


    public void Init_Func()
    {

    }
    public void Active_Func()
    {
        this.gameObject.SetActive(true);
        creditObj.SetActive(false);
    }
    public void Resume_Func()
    {
        Battle_Manager.Instance.Resume_Func();

        this.gameObject.SetActive(false);
    }
    public void Retreat_Func()
    {
        Battle_Manager.Instance.GameOver_Func(true);
    }
    public void SetBGM_Func(bool _isON)
    {

    }
    public void SetSFX_Func(bool _isON)
    {

    }
}
