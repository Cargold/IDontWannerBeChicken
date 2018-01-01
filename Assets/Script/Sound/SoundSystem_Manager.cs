using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSystem_Manager : MonoBehaviour
{
    public static SoundSystem_Manager Instance;

    [SerializeField] private int maxSfxNum;
    [SerializeField] private AudioSource[] sfxSourceArr;
    [SerializeField] private int currentSfxID;

    [SerializeField] private int maxBgmNum;
    [SerializeField] private AudioSource[] bgmSourceArr;
    [SerializeField] private int currentBgmID;

    [SerializeField] private AudioClip[] audioClipArr;

    public IEnumerator Init_Cor()
    {
        InitManager_Func();
        InitAudio_Func();

        yield break;
    }

    void InitManager_Func()
    {
        Instance = this;
    }
    void InitAudio_Func()
    {
        bgmSourceArr = new AudioSource[maxBgmNum];
        for (int i = 0; i < maxBgmNum; i++)
        {
            GameObject _bgmAudioObj = new GameObject("BgmAudio_" + i);
            _bgmAudioObj.transform.SetParent(this.transform);
            bgmSourceArr[i] = _bgmAudioObj.AddComponent<AudioSource>();
            bgmSourceArr[i].loop = true;
            bgmSourceArr[i].playOnAwake = false;
        }
        currentBgmID = 0;

        sfxSourceArr = new AudioSource[maxSfxNum];
        for (int i = 0; i < maxSfxNum; i++)
        {
            GameObject _sfxAudioObj = new GameObject("SfxAudio_" + i);
            _sfxAudioObj.transform.SetParent(this.transform);
            sfxSourceArr[i] = _sfxAudioObj.AddComponent<AudioSource>();
            sfxSourceArr[i].loop = false;
            sfxSourceArr[i].playOnAwake = false;
        }
        currentSfxID = 0;
    }

    public void PlayBGM_Func(SoundType _soundType)
    {
        PlayBGM_Func((int)_soundType);
    }
    public void PlayBGM_Func(int _soundTypeID)
    {
        bgmSourceArr[0].Stop();
        bgmSourceArr[0].clip = audioClipArr[_soundTypeID];

        if (Player_Data.Instance.isBgmOn == true)
            bgmSourceArr[0].Play();
    }
    public void PlayBGM_Func()
    {
        bgmSourceArr[0].Play();
    }
    public void StopBGM_Func()
    {
        bgmSourceArr[0].Stop();
    }

    public void PlaySFX_Func(SoundType _soundType)
    {
        sfxSourceArr[currentSfxID].clip = audioClipArr[(int)_soundType];

        if(Player_Data.Instance.isSfxOn == true)
            sfxSourceArr[currentSfxID].Play();

        CheckPlaySfxID_Func();
    }

    void CheckPlayBgmID_Func()
    {
        if (maxBgmNum <= currentBgmID + 1)
            currentBgmID = 0;
        else
            currentBgmID++;
    }
    void CheckPlaySfxID_Func()
    {
        if (maxSfxNum <= currentSfxID + 1)
            currentSfxID = 0;
        else
            currentSfxID++;
    }
}
