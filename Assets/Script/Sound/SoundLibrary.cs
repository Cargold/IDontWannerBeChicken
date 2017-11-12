using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundLibrary : MonoBehaviour
{
    public SoundGroup[] soundGroups;

    [System.Serializable]
    public class SoundGroup
    {
        public string clipName;
        public AudioClip clip;
    }

    //인덱스화
    //이넘화
    //배열화 시키면 더 쓰기 편해질듯

    Dictionary<string, AudioClip> groupDictionary = new Dictionary<string, AudioClip>();

    void Awake()
    {
        foreach (SoundGroup soundGroup in soundGroups)
        {
            groupDictionary.Add(soundGroup.clipName, soundGroup.clip);
        }
    }

    ////해당이름의 클립으로 재생할 사운드가 랜덤일때. 그룹핑된것들중에 가져오고 재생할 사운드가 한개면 거기서 가져옴.
    //public AudioClip GetClipFromNameRandom(string name)
    //{
    //    if (groupDictionary.ContainsKey(name))
    //    {
    //        AudioClip[] sounds = groupDictionary[name];
    //        return sounds[Random.Range(0, sounds.Length)];
    //    }
    //    return null;
    //}

    public AudioClip GetClipFromName(string name)
    {
        if (groupDictionary.ContainsKey(name))
        {
            AudioClip clip = groupDictionary[name];
            return clip;
        }

        Debug.LogError("Cannot Find Sound Name : " + name);
        return null;
    }
}