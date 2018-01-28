using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStart_Script : MonoBehaviour
{
    public void BattleStart_Func()
    {
        Battle_Manager.Instance.HeroEnterStage_Func();
    }

    public void PlaySfx_Func()
    {
        SoundSystem_Manager.Instance.PlaySFX_Func(SoundType.SFX_btn_start1);
    }

    public void AniStart_Func()
    {
        Lobby_Manager.Instance.mainLobbyClass.SetSortingOrder_Func();
    }
}
