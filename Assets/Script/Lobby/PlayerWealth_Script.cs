using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWealth_Script : MonoBehaviour
{
    public Text goldText;
    public Text mineralText;
    
    public void PrintWealth_Func(WealthType _wealthType, int _value)
    {
        if(_wealthType == WealthType.Gold)
        {
            goldText.text = _value.ToString();
        }
        else if(_wealthType == WealthType.Mineral)
        {
            mineralText.text = _value.ToString();
        }
    }
}
