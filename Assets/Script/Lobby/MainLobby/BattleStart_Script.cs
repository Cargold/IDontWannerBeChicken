using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStart_Script : MonoBehaviour
{
    public void BattleStart_Func()
    {
        Battle_Manager.Instance.HeroEnterStage_Func();
    }
}
