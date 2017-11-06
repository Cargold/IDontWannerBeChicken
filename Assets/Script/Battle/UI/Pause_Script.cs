using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause_Script : MonoBehaviour
{
    public Animation anim;
    public GameObject creditObj;


    public void Init_Func()
    {
        RectTransform _thisRTrf = this.gameObject.GetComponent<RectTransform>();
        _thisRTrf.localPosition = Vector3.zero;
        _thisRTrf.anchoredPosition = Vector2.zero;

        this.gameObject.SetActive(false);
    }
    public void Active_Func()
    {
        this.gameObject.SetActive(true);
        creditObj.SetActive(false);

        Time.timeScale = 0f;
    }
    public void Resume_Func()
    {
        Time.timeScale = 1f;

        Battle_Manager.Instance.Resume_Func();

        this.gameObject.SetActive(false);
    }
    public void Retreat_Func()
    {
        Time.timeScale = 1f;

        Battle_Manager.Instance.GameOver_Func(true);

        this.gameObject.SetActive(false);
    }
    public void SetBGM_Func(bool _isON)
    {

    }
    public void SetSFX_Func(bool _isON)
    {

    }
}
