using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause_Script : MonoBehaviour
{
    public Animation anim;
    public GameObject creditObj;

    public Text[] titleTextArr;
    public Text resumeText;
    public Text surrenderText;
    public Text devText;
    public Text bgmText;
    public Text sfxText;

    public GameObject bgmObj;
    public GameObject sfxObj;

    public void Init_Func()
    {
        RectTransform _thisRTrf = this.gameObject.GetComponent<RectTransform>();
        _thisRTrf.localPosition = Vector3.zero;
        _thisRTrf.anchoredPosition = Vector2.zero;

        titleTextArr[0].text = TranslationSystem_Manager.Instance.Pause;
        titleTextArr[1].text = TranslationSystem_Manager.Instance.Pause;
        resumeText.text = TranslationSystem_Manager.Instance.War;
        surrenderText.text = TranslationSystem_Manager.Instance.Fried;
        devText.text = TranslationSystem_Manager.Instance.Devs;
        bgmText.text = TranslationSystem_Manager.Instance.Bgm;
        sfxText.text = TranslationSystem_Manager.Instance.Sfx;

        if (Player_Data.Instance.isBgmOn == true)
            bgmObj.SetActive(true);
        else
            bgmObj.SetActive(false);

        if (Player_Data.Instance.isSfxOn == true)
            sfxObj.SetActive(true);
        else
            sfxObj.SetActive(false);

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
        bgmObj.SetActive(_isON);
        Player_Data.Instance.isBgmOn = _isON;
        SaveSystem_Manager.Instance.SaveData_Func(SaveType.Player_BGM, _isON);

        if(_isON == true)
        {
            SoundSystem_Manager.Instance.PlayBGM_Func();
        }
        else
        {
            SoundSystem_Manager.Instance.StopBGM_Func();
        }
    }
    public void SetSFX_Func(bool _isON)
    {
        sfxObj.SetActive(_isON);
        Player_Data.Instance.isSfxOn = _isON;
        SaveSystem_Manager.Instance.SaveData_Func(SaveType.Player_SFX, _isON);
    }
}
