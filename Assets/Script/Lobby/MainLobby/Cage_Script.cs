using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cage_Script : MonoBehaviour
{
    public BattleStartDirection_Script directionClass;
    public Animation[] animArr;
    public float[] intervalArr;
    [SerializeField]
    private bool isBoost = false;

    public void Init_Func(BattleStartDirection_Script _directionClass)
    {
        directionClass = _directionClass;

        int _animNum = this.transform.childCount - 2;
        animArr = new Animation[_animNum];
        intervalArr = new float[_animNum];
        for (int i = 0; i < _animNum; i++)
        {
            animArr[i] = this.transform.GetChild(i + 1).GetComponent<Animation>();
        }
    }
    
    public void EnterLobby_Func()
    {
        isBoost = false;

        for (int i = 0; i < animArr.Length; i++)
        {
            float _interval = Random.Range(1f, 3f);
            StartCoroutine(StartAni_Cor(i, _interval));
        }
    }
    IEnumerator StartAni_Cor(int _animID, float _interval)
    {
        intervalArr[_animID] = 0f;

        while (true)
        {
            if (isBoost == false)
                intervalArr[_animID] = _interval;
            else if (isBoost == true)
                intervalArr[_animID] = _interval * 0.01f;

            float _calcTime = intervalArr[_animID];
            while (0 <= _calcTime)
            {
                yield return new WaitForFixedUpdate();

                if (isBoost == false)
                    _calcTime -= 0.02f;
                else if (isBoost == true)
                    _calcTime -= 1f;
            }

            if (isBoost == false)
                animArr[_animID]["MainLobbyIdle"].speed = 1f;
            else if (isBoost == true)
                animArr[_animID]["MainLobbyIdle"].speed = 2f;

            animArr[_animID].Play();

            while (animArr[_animID].isPlaying == true)
                yield return null;
        }
    }

    public void EnterBattle_Func()
    {
        isBoost = true;

        StartCoroutine(StopAni_Cor());
    }
    IEnumerator StopAni_Cor()
    {
        yield return new WaitForSeconds(10f);
        StopAllCoroutines();
    }
}
