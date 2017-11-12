using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    //재생기능

    //오디오 소스 풀링.
    SoundLibrary soundLibrary;
    //
    AudioSource[] fxSources;
    AudioSource[] musicSources;

    int activeMusicIndex;
    int currentFXIndex;

    private void Awake()
    {
        Instance = this;

        musicSources = new AudioSource[2];
        for (int i = 0; i < musicSources.Length; i++)
        {
            GameObject newMusicSource = new GameObject("Music source_" + (i + 1));
            musicSources[i] = newMusicSource.AddComponent<AudioSource>();
            musicSources[i].loop = true;
            musicSources[i].playOnAwake = false;
            newMusicSource.transform.parent = transform;
        }

        fxSources = new AudioSource[10];
        for( int i = 0; i < fxSources.Length; i++ )
        {
            GameObject newFxSource = new GameObject("FX source_" + (i + 1));
            fxSources[i] = newFxSource.AddComponent<AudioSource>();
            fxSources[i].loop = false;
            fxSources[i].playOnAwake = false;
            newFxSource.transform.parent = transform;
        }

        activeMusicIndex = 0;
        currentFXIndex = 0;

        soundLibrary = GetComponent<SoundLibrary>();
    }


    //외부에서 플레이 뮤직하고 클립을 설정. (씬에 따라 노래 다르게 나오게 하기..)
    //private void OnLevelWasLoaded(int level)
    //{
    //
    //}

    //배경음 루핑은 동적생성한 오디오 소스에서는 설정이 안되어있으니.
    //Invoke로 다시 노래길이만큼 후에 다시 플레이 뮤직 함수를 다시 호출하는구만.

    public void PlayMusic(string _musicName, float _fadeDuration = 1)
    {
        AudioClip clip = soundLibrary.GetClipFromName(_musicName);
        if (clip == null) return;

        activeMusicIndex = 1 - activeMusicIndex;
        musicSources[activeMusicIndex].clip = clip;
        musicSources[activeMusicIndex].Play();

        StartCoroutine(AnimateMusicCrossFadeCo(_fadeDuration));
    }

    //음악 페이드인 페이드아웃.
    IEnumerator AnimateMusicCrossFadeCo(float _fadeDuration)
    {
        float percent = 0;

        while(percent < 1)
        {
            percent += Time.deltaTime * 1 / _fadeDuration;          //퍼센트가 1 짜리인데 1을 듀레이션분의 1로 나눈다..흐음..
            //슬라이더의 볼륨은 0 ~ 1 로나옴.

            musicSources[1 - activeMusicIndex].volume = Mathf.Lerp(0, 0, percent); // 마스터볼륨 * 음악볼륨 을 퍼센트로 보간함. 0 ~ 1 볼륨까지 키움.
            musicSources[activeMusicIndex].volume = Mathf.Lerp(1, 1, percent); // 1로 갈건데 마스터 음악볼륨을 퍼센트로 보간  

            yield return null;
        }
    }

    //일시적으로 1회 음악을 플레이 ( 배경음악을 잠시 멈추고 플레이한다. )
    public void PlayMusicOnce(string _soundName, float _volume = 1)
    {
        musicSources[activeMusicIndex].Stop();

        PlaySound(_soundName, _volume);

        StartCoroutine(MusicRestartCo(fxSources[currentFXIndex].clip.length));
    }

    IEnumerator MusicRestartCo(float _time)
    {
        yield return new WaitForSeconds(_time);

        musicSources[activeMusicIndex].Play();
    }

    //재생시간을 정해서 플레이하는 기능
    public void PlaySoundLimitTime(string _soundName, float _limitTime, float _volume = 1)
    {
        PlaySound(_soundName, _volume);
        int index = currentFXIndex;
        StartCoroutine(StopAfterDelayCo(index, _limitTime));

    }

    IEnumerator StopAfterDelayCo(int index, float _time)
    {
        yield return new WaitForSeconds(_time);
        fxSources[index].Stop();
    }


    //volume 0 ~ 1 
    public void PlaySound(string _soundName, float _volume = 1)
    {
        AudioClip clip = soundLibrary.GetClipFromName(_soundName);
        if (clip == null) return;
                
        currentFXIndex++;
        if (currentFXIndex >= fxSources.Length)
        {
            currentFXIndex = 0;
        }

        fxSources[currentFXIndex].clip = clip;    
        fxSources[currentFXIndex].PlayOneShot(clip, _volume);
    }

    //volume 0 ~ 1 
    public void PlaySoundRepeat(string _soundName, float _repeatTime, float _term,  float _volume = 1)
    {
        StartCoroutine(RepeatSoundCo(_soundName, _repeatTime, _term));
    }

    //repeat time 동안 반복하는데 일정 텀마다 반복.
    IEnumerator RepeatSoundCo(string _soundName, float _RepeatTime, float _term)
    {
        float fDelayTime = 0;

        while(_RepeatTime > 0)
        {
            fDelayTime += Time.deltaTime;
            _RepeatTime -= Time.deltaTime;

            if( fDelayTime >= _term)
            {
                PlaySound(_soundName);
                fDelayTime = 0;
            }            

            yield return null;
        }
    }
}

