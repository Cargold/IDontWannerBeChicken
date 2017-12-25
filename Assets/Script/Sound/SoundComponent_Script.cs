using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundComponent_Script : MonoBehaviour
{
    public SoundType soundType;
    public bool isSfx = true;

    public void OnButton_Func()
    {
        if(isSfx == true)
            SoundSystem_Manager.Instance.PlaySFX_Func(soundType);
        else
            SoundSystem_Manager.Instance.PlayBGM_Func(soundType);
    }
}
