using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWealth_Script : MonoBehaviour
{
    public Text goldText;
    public Text mineralText;

    public void PrintGold_Func(int _goldValue)
    {
        goldText.text = _goldValue.ToString();
    }

    public void PrintMineral_Func(int _mineralValue)
    {
        mineralText.text = _mineralValue.ToString();
    }

    public void EnterMineralStore_Func()
    {

    }
}
