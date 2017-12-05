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
            goldText.text = string.Format("{0:N0}", _value);
        }
        else if(_wealthType == WealthType.Mineral)
        {
            mineralText.text = string.Format("{0:N0}", _value);
        }
    }

    public void Active_Func()
    {
        this.gameObject.SetActive(true);
    }
    public void Deactive_Func()
    {
        this.gameObject.SetActive(false);
    }
    public void OnLobby_Func()
    {
        this.transform.SetParent(Lobby_Manager.Instance.menuGroupTrf);
    }
    public void OnBattle_Func()
    {
        this.transform.SetParent(Lobby_Manager.Instance.mainLobbyClass.thisRTrf);
    }
}
