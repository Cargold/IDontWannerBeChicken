using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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

    public Transform cardGroupTrf;
    public GameObject[] rewardObjArr;
    public Image[] resultRewardBGImageArr;
    public Image[] resultRewardImageArr;
    public Text[] resultRewardTextArr;
    public Sprite[] resultRewardBGSpriteArr;

    private bool isActive;
    private bool isVictory;
    private BattleType battleType;

    public void Init_Func(Battle_Manager _battleManaer)
    {
        battleManager = _battleManaer;

        RectTransform _thisRTrf = this.gameObject.GetComponent<RectTransform>();
        _thisRTrf.localPosition = Vector3.zero;
        _thisRTrf.anchoredPosition = Vector2.zero;

        rewardObjArr = new GameObject[4];
        resultRewardBGImageArr = new Image[4];
        resultRewardTextArr = new Text[4];
        resultRewardImageArr = new Image[4];
        for (int i = 0; i < 4; i++)
        {
            rewardObjArr[i] = cardGroupTrf.GetChild(i).gameObject;

            resultRewardBGImageArr[i] = rewardObjArr[i].GetComponent<Image>();

            resultRewardTextArr[i] = rewardObjArr[i].transform.Find("Title").GetComponent<Text>();

            resultRewardImageArr[i] = rewardObjArr[i].transform.Find("Mask").GetChild(0).GetComponent<Image>();

            rewardObjArr[i].SetActive(false);
        }

        this.gameObject.SetActive(false);
    }
    public void Active_Func(BattleType _battleType, bool _isVictory, Reward_Data[] _rewardDataArr)
    {
        isActive = true;

        this.gameObject.SetActive(true);

        for (int i = 0; i < 4; i++)
        {
            rewardObjArr[i].SetActive(false);
            rewardObjArr[i].transform.localScale = Vector3.one;
        }

        StartCoroutine(RewardEffect_Cor(_battleType, _isVictory, _rewardDataArr));
    }
    IEnumerator RewardEffect_Cor(BattleType _battleType, bool _isVictory, Reward_Data[] _rewardDataArr)
    {
        isActive = true;
        battleType = _battleType;
        isVictory = _isVictory;

        if (_isVictory == false)
        {
            titleText.text = TranslationSystem_Manager.Instance.YouAreFried;
            btnText_Left.text = TranslationSystem_Manager.Instance.Ok;
            btnText_Right.text = TranslationSystem_Manager.Instance.Retry;
            btnAD.SetActive(false);
        }
        else if (_isVictory == true)
        {
            if (_battleType == BattleType.Normal)
            {
                titleText.text = TranslationSystem_Manager.Instance.ChickenRestaurantDestroyed;
                btnText_Left.text = TranslationSystem_Manager.Instance.Ok;
                btnText_Right.text = TranslationSystem_Manager.Instance.Raid;
                
                btnAD.SetActive(true);
            }
            else if (_battleType == BattleType.Special)
            {
                titleText.text = TranslationSystem_Manager.Instance.ChickenRestaurantDestroyed;
                btnText_Left.text = TranslationSystem_Manager.Instance.Raid;
                btnText_Right.text = TranslationSystem_Manager.Instance.Ok;
                btnAD.SetActive(false);
            }
        }

        for (int i = 0; i < _rewardDataArr.Length; i++)
        {
            int _rewardID = _rewardDataArr[i].rewardID;
            int _rewardAmount = _rewardDataArr[i].rewardAmount;
            resultRewardImageArr[i].rectTransform.localScale = Vector3.one;

            resultRewardBGImageArr[i].sprite = resultRewardBGSpriteArr[2];
            resultRewardBGImageArr[i].SetNativeSize();

            switch (_rewardDataArr[i].rewardType)
            {
                case RewardType.Wealth:
                    SetRewardWealth_Func(i, _rewardID, _rewardAmount);
                    break;

                case RewardType.Food:
                    SetRewardFood_Func(i, _rewardID, _rewardAmount);
                    break;

                case RewardType.FoodBoxLevel:
                    SetRewardFoodBox_Func(i, _rewardID, _rewardAmount);
                    break;

                case RewardType.Unit:
                    SetRewardUnit_Func(i, _rewardID, _rewardAmount);
                    break;

                //case RewardType.PopulationPoint:
                //    SetRewardPopulationPoint_Func(i, _rewardID, _rewardAmount);
                //    break;

                case RewardType.Skill:
                    SetRewardSkill_Func(i, _rewardID, _rewardAmount);
                    break;

                case RewardType.Trophy:
                    SetRewardTrophy_Func(i, _rewardID, _rewardAmount);
                    break;
            }

            rewardObjArr[i].SetActive(true);
            rewardObjArr[i].transform.DOPunchScale(Vector3.one, 0.5f);

            yield return new WaitForSeconds(0.5f);
        }
    }
    void SetRewardWealth_Func(int _rewardObjID, int _rewardID, int _rewardAmount)
    {
        resultRewardImageArr[_rewardObjID].sprite = DataBase_Manager.Instance.wealthSpriteArr[_rewardID];
        resultRewardImageArr[_rewardObjID].SetNativeSize();
        resultRewardImageArr[_rewardObjID].rectTransform.localScale = Vector3.one * 2f;

        if (_rewardID == 0)
        {
            resultRewardTextArr[_rewardObjID].text = TranslationSystem_Manager.Instance.Gold + " " + _rewardAmount;

            resultRewardBGImageArr[_rewardObjID].sprite = resultRewardBGSpriteArr[0];
            resultRewardBGImageArr[_rewardObjID].SetNativeSize();
        }
        else if (_rewardID == 1)
        {
            resultRewardTextArr[_rewardObjID].text = TranslationSystem_Manager.Instance.Mineral + " " +_rewardAmount;
        }
    }
    void SetRewardFood_Func(int _rewardObjID, int _rewardID, int _rewardAmount)
    {
        Food_Data _foodData = DataBase_Manager.Instance.foodDataArr[_rewardID];

        resultRewardImageArr[_rewardObjID].sprite = _foodData.foodSprite;
        resultRewardImageArr[_rewardObjID].SetNativeSize();

        resultRewardTextArr[_rewardObjID].text = _foodData.nameArr[TranslationSystem_Manager.Instance.languageTypeID];

        resultRewardBGImageArr[_rewardObjID].sprite = resultRewardBGSpriteArr[1];
        resultRewardBGImageArr[_rewardObjID].SetNativeSize();
    }
    void SetRewardFoodBox_Func(int _rewardObjID, int _rewardID, int _rewardAmount)
    {
        resultRewardImageArr[_rewardObjID].sprite = DataBase_Manager.Instance.gachaBoxSprite;
        resultRewardImageArr[_rewardObjID].SetNativeSize();

        resultRewardTextArr[_rewardObjID].text = TranslationSystem_Manager.Instance.GachaBox;

        resultRewardBGImageArr[_rewardObjID].sprite = resultRewardBGSpriteArr[2];
        resultRewardBGImageArr[_rewardObjID].SetNativeSize();
    }
    void SetRewardUnit_Func(int _rewardObjID, int _rewardID, int _rewardAmount)
    {
        Unit_Data _unitData = DataBase_Manager.Instance.unitDataArr[_rewardID];

        resultRewardImageArr[_rewardObjID].sprite = _unitData.unitSprite;
        resultRewardImageArr[_rewardObjID].SetNativeSize();

        resultRewardImageArr[_rewardObjID].rectTransform.localPosition = _unitData.cardPortraitPos;

        resultRewardImageArr[_rewardObjID].rectTransform.localScale = Vector3.one * _unitData.cardImageSize;

        resultRewardTextArr[_rewardObjID].text = _unitData.charNameArr[TranslationSystem_Manager.Instance.languageTypeID];
    }
    //void SetRewardPopulationPoint_Func(int _rewardObjID, int _rewardID, int _rewardAmount)
    //{
    //    resultRewardImageArr[_rewardObjID].sprite = DataBase_Manager.Instance.populationPointSprite;
    //    resultRewardImageArr[_rewardObjID].SetNativeSize();
    //    resultRewardImageArr[_rewardObjID].rectTransform.localScale = Vector3.one * 2f;

    //    resultRewardTextArr[_rewardObjID].text = "닭 한 마리 추가요!";
    //}
    void SetRewardSkill_Func(int _rewardObjID, int _rewardID, int _rewardAmount)
    {
        Skill_Data _skillData = DataBase_Manager.Instance.skillDataArr[_rewardID];

        resultRewardImageArr[_rewardObjID].sprite = _skillData.skillSprite;
        resultRewardImageArr[_rewardObjID].SetNativeSize();

        resultRewardTextArr[_rewardObjID].text = _skillData.skillNameArr[TranslationSystem_Manager.Instance.languageTypeID];
    }
    void SetRewardTrophy_Func(int _rewardObjID, int _rewardID, int _rewardAmount)
    {
        Trophy_Data _trophyData = DataBase_Manager.Instance.trophyDataArr[_rewardID];

        resultRewardImageArr[_rewardObjID].sprite = _trophyData.trophySprite;
        resultRewardImageArr[_rewardObjID].SetNativeSize();                                                                                 

        resultRewardTextArr[_rewardObjID].text = _trophyData.nameArr[TranslationSystem_Manager.Instance.languageTypeID];
    }

    public void OnButton_Func(bool _isLeft)
    {
        // Call : Btn Event

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
                    // Next Stage

                    battleManager.NextStage_Func();
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
    public void WatchAD_Func()
    {
        int _adValue = battleManager.GetGoldByWatchedAD_Func();
        resultRewardTextArr[0].text = "골드 " + _adValue;
        rewardObjArr[0].transform.DOPunchScale(Vector3.one, 0.5f);

        btnAD.gameObject.SetActive(false);
    }

    public void Deactive_Func()
    {
        isActive = false;
        this.gameObject.SetActive(false);
    }
}
