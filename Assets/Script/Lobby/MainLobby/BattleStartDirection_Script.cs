using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStartDirection_Script : MonoBehaviour
{
    public Transform cageGridGroupTrf;
    public Cage_Script[] cageClassArr;

    public Animation entranceAni;
    public Animation partyAni;

    public IEnumerator Init_Cor()
    {
        int _cageNum = cageGridGroupTrf.childCount;
        cageClassArr = new Cage_Script[_cageNum];
        for (int i = 0; i < cageClassArr.Length; i++)
        {
            cageClassArr[i] = cageGridGroupTrf.GetChild(i).GetComponent<Cage_Script>();
            cageClassArr[i].Init_Func(this);

            yield return null;
        }
    }

    public void EnterUI_Func(GameState _directionState)
    {
        DirectionCage_Func(_directionState);
        DirectionEntrance_Func(_directionState);
        DirectionParty_Func(_directionState);
    }
    void DirectionCage_Func(GameState _directionState)
    {
        if (_directionState == GameState.Lobby)
        {
            for (int i = 0; i < cageClassArr.Length; i++)
            {
                cageClassArr[i].EnterLobby_Func();
            }
        }
        else if (_directionState == GameState.Battle)
        {
            for (int i = 0; i < cageClassArr.Length; i++)
            {
                cageClassArr[i].EnterBattle_Func();
            }
        }
    }
    void DirectionEntrance_Func(GameState _directionState)
    {
        if (_directionState == GameState.Lobby)
        {
            Debug.Log("Test, Lobby");

            entranceAni["EntranceFallanim"].time = 0f;
            entranceAni["EntranceFallanim"].speed = -1f;
            entranceAni.Play();
        }
        else if (_directionState == GameState.Battle)
        {
            entranceAni["EntranceFallanim"].time = 0f;
            entranceAni["EntranceFallanim"].speed = 1f;
            entranceAni.Play();
        }
    }
    void DirectionParty_Func(GameState _directionState)
    {
        if (_directionState == GameState.Lobby)
        {
            Player_Data.Instance.playerHeroData.transform.position = new Vector3(2.45f, 0f, 0f);

            partyAni["Party"].time = 0f;
            partyAni["Party"].speed = -1f;
            partyAni.Play();
        }
        else if (_directionState == GameState.Battle)
        {
            partyAni["Party"].time = 0f;
            partyAni["Party"].speed = 1f;
            partyAni.Play();
        }
    }
}
