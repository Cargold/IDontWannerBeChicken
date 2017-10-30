using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result_Script : MonoBehaviour
{
    public Battle_Manager battleManager;

    public enum ResultType
    {
        None = -1,
        VictoryNormal,
        VictorySpeical,
        Defeat,
    }

    public Text titleText;

    public Text btnText_Left;
    public Text btnText_Right;
    public GameObject btnAD;

    public GameObject[] resultRewardObjArr;
    public Image[] resultRewardImageArr;
    public Text[] resultRewardTextArr;

    private bool isVictory;
    private BattleType battleType;

    public void Init_Func(Battle_Manager _battleManaer)
    {
        battleManager = _battleManaer;

        RectTransform _thisRTrf = this.gameObject.GetComponent<RectTransform>();
        _thisRTrf.localPosition = Vector3.zero;
        _thisRTrf.anchoredPosition = Vector2.zero;

        this.gameObject.SetActive(false);
    }

    public void Active_Func(BattleType _battleType, bool _isVictory, RewardData[] _rewardDataArr)
    {
        this.gameObject.SetActive(true);

        battleType = _battleType;
        isVictory = _isVictory;

        for (int i = 0; i < 3; i++)
        {
            if(i< _rewardDataArr.Length)
            {
                resultRewardObjArr[i].SetActive(true);

                int _rewardID = _rewardDataArr[i].rewardID;
                int _rewardAmount = _rewardDataArr[i].rewardAmount;

                switch (_rewardDataArr[i].rewardType)
                {
                    case RewardType.Wealth:
                        resultRewardImageArr[i].sprite = DataBase_Manager.Instance.wealthSpriteArr[_rewardID];
                        resultRewardImageArr[i].SetNativeSize();

                        if(_rewardID == 0)
                        {
                            resultRewardTextArr[i].text = "골드 " + _rewardAmount;
                        }
                        else if(_rewardID == 1)
                        {
                            resultRewardTextArr[i].text = "미네랄 " +  +_rewardAmount;
                        }
                        break;
                    case RewardType.Food:
                        Food_Data _foodData = DataBase_Manager.Instance.foodDataArr[_rewardID];

                        resultRewardImageArr[i].sprite = _foodData.foodSprite;
                        resultRewardImageArr[i].SetNativeSize();

                        resultRewardTextArr[i].text = _foodData.foodName;
                        break;
                    case RewardType.Unit:
                        resultRewardImageArr[i].sprite = DataBase_Manager.Instance.unitDataArr[_rewardDataArr[i].rewardID].unitSprite;
                        resultRewardImageArr[i].SetNativeSize();
                        break;
                    case RewardType.PopulationPoint:
                        resultRewardImageArr[i].sprite = Game_Manager.Instance.populationSpriteArr[_rewardID];
                        resultRewardImageArr[i].SetNativeSize();
                        break;
                    case RewardType.Skill:
                        resultRewardImageArr[i].sprite = DataBase_Manager.Instance.skillDataArr[_rewardDataArr[i].rewardID].skillSprite;
                        resultRewardImageArr[i].SetNativeSize();
                        break;
                }
            }
            else
                resultRewardObjArr[i].SetActive(false);
        }

        if (_isVictory == false)
        {
            titleText.text = "당신은 튀겨졌습니다...";
            btnText_Left.text = "확인";
            btnText_Right.text = "재시도";
            btnAD.SetActive(false);
        }
        else if(_isVictory == true)
        {
            if(_battleType == BattleType.Normal)
            {
                titleText.text = "치킨집 파괴!";
                btnText_Left.text = "약탈하기";
                btnText_Right.text = "";
                btnAD.SetActive(true);
            }
            else if(_battleType == BattleType.Special)
            {
                titleText.text = "악의 치킨집 파괴!";
                btnText_Left.text = "전진하기";
                btnText_Right.text = "끝내기";
                btnAD.SetActive(false);
            }
        }
    }

    public void OnButton_Func(bool _isLeft)
    {
        if (isVictory == false)
        {
            if (_isLeft == true)
            {
                // Enter Lobby

                battleManager.ExitBattle_Func();
            }
            else if (_isLeft == false)
            {
                // Retry

                battleManager.Retry_Func();
            }
        }
        else if (isVictory == true)
        {
            if (battleType == BattleType.Normal)
            {
                if (_isLeft == true)
                {
                    // Get Reward

                    battleManager.ExitBattle_Func();
                }
                else if (_isLeft == false)
                {
                    // Watch AD

                    battleManager.WatchAD_Func();
                    resultRewardTextArr[0].text = "골드 " + battleManager.rewardDataArr[0].rewardAmount;
                }
            }
            else if (battleType == BattleType.Special)
            {
                if (_isLeft == true)
                {
                    // Next Stage

                    battleManager.NextStage_Func();
                }
                else if (_isLeft == false)
                {
                    // Enter Lobby

                    battleManager.ExitBattle_Func();
                }
            }
        }

        Deactive_Func();
    }

    public void Deactive_Func()
    {
        this.gameObject.SetActive(false);
    }
}
