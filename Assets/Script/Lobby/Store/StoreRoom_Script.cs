using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StoreRoom_Script : LobbyUI_Parent
{
    public StorePopUp_Script popUpClass;
    [SerializeField]
    private GameObject resultMessageObj;
    public Gacha_Script gachaClass;

    public Transform storeListTrf;

    private GameObject[] storeCardGroupObjArr;
    private Transform[] storeListCardTrfArr;
    private Text[] storeCardTitleTextArr;
    private Image[] storeCardImageArr;
    private Image[] storeCardPriceIconImageArr;
    private Button[] stpreCardPriceBtnArr;
    private Text[] storeCardPriceTextArr;
    private Transform[] storeCardPriceGroupTrfArr;
    public GameObject storeCardObj;
    public string realPrice;

    public Store_Data[] storeDataArr;

    public Animation anim;
    public StoreState storeState;
    public int listID;
    public int cardID;
    public int storeDataID;

    #region Override Group
    protected override void InitUI_Func()
    {
        Init_Func();

        popUpClass.Init_Func(this);

        this.gameObject.SetActive(false);
    }
    protected override void EnterUI_Func()
    {
        this.gameObject.SetActive(true);

        OnTapBtn_Func(0);
    }
    public override void Exit_Func()
    {
        this.gameObject.SetActive(false);
        
    }
    #endregion
    void Init_Func()
    {
        anim.Play();
        InitCard_Func();
        InitStoreUI_Func();
        InitUIData_Func();
        InitGacha_Func();

        resultMessageObj.SetActive(false);
    }
    void InitCard_Func()
    {
        storeCardGroupObjArr = new GameObject[5];
        for (int _listID = 0; _listID < 5; _listID++)
        {
            storeCardGroupObjArr[_listID] = storeListTrf.GetChild(_listID).gameObject;

            Vector3 _cardPos = new Vector3(-411.3f, 0f, 0f);

            for (int _cardID = 0; _cardID < 4; _cardID++)
            {
                GameObject _cardObj = Instantiate(storeCardObj);
                _cardObj.transform.SetParent(storeCardGroupObjArr[_listID].transform);
                _cardObj.transform.localPosition = _cardPos;
                _cardObj.transform.localScale = Vector3.one;
                _cardPos += Vector3.right * 274.2f;

                StoreCard_Script _storeCard = _cardObj.GetComponent<StoreCard_Script>();
                _storeCard.Init_Func(this, _cardID);
            }
        }
    }
    void InitStoreUI_Func()
    {
        storeListCardTrfArr = new Transform[20];
        storeCardTitleTextArr = new Text[20];
        storeCardImageArr = new Image[20];
        stpreCardPriceBtnArr = new Button[20];
        storeCardPriceGroupTrfArr = new RectTransform[20];
        storeCardPriceIconImageArr = new Image[20];
        storeCardPriceTextArr = new Text[20];

        for (int _listID = 0; _listID < 5; _listID++)
        {
            for (int _cardID = 0; _cardID < 4; _cardID++)
            {
                int _storeDataID = (_listID * 4) + _cardID;

                storeListCardTrfArr[_storeDataID] =
                    storeCardGroupObjArr[_listID].transform.GetChild(_cardID);

                storeCardTitleTextArr[_storeDataID] =
                    storeListCardTrfArr[_storeDataID].GetChild(2).GetComponent<Text>();

                storeCardImageArr[_storeDataID] =
                    storeListCardTrfArr[_storeDataID].GetChild(0).GetComponent<Image>();

                stpreCardPriceBtnArr[_storeDataID] = storeCardImageArr[_storeDataID].GetComponent<Button>();

                if (storeDataArr[_storeDataID].costType == WealthType.Real)
                {
                    storeListCardTrfArr[_storeDataID].GetChild(3).gameObject.SetActive(false);
                    storeListCardTrfArr[_storeDataID].GetChild(4).gameObject.SetActive(true);

                    storeCardPriceGroupTrfArr[_storeDataID] = storeListCardTrfArr[_storeDataID].GetChild(4);

                    storeCardPriceTextArr[_storeDataID] =
                            storeCardPriceGroupTrfArr[_storeDataID].GetChild(0)
                            .GetComponent<Text>();
                }
                else
                {
                    storeListCardTrfArr[_storeDataID].GetChild(4).gameObject.SetActive(false);
                    storeListCardTrfArr[_storeDataID].GetChild(3).gameObject.SetActive(true);

                    storeCardPriceGroupTrfArr[_storeDataID] = storeListCardTrfArr[_storeDataID].GetChild(3);

                    storeCardPriceIconImageArr[_storeDataID]
                        = storeCardPriceGroupTrfArr[_storeDataID].GetChild(1).GetComponent<Image>();
                    
                    storeCardPriceTextArr[_storeDataID]
                        = storeCardPriceGroupTrfArr[_storeDataID].GetChild(0).GetComponent<Text>();
                }

                
            }
        }
    }
    void InitUIData_Func()
    {
        for (int _listID = 0; _listID < 5; _listID++)
        {
            for (int _cardID = 0; _cardID < 4; _cardID++)
            {
                int _storeDataID = (_listID * 4) + _cardID;

                storeCardTitleTextArr[_storeDataID].text
                    = storeDataArr[_storeDataID].goodsTitle;
                if(0 < storeDataArr[_storeDataID].goodsAmount)
                {
                    storeCardTitleTextArr[_storeDataID].text
                        += " " + string.Format("{0:N0}", storeDataArr[_storeDataID].goodsAmount);
                }
                
                storeCardImageArr[_storeDataID].sprite = storeDataArr[_storeDataID].storeCardSprite;
                storeCardImageArr[_storeDataID].SetNativeSize();
                
                if (storeDataArr[_storeDataID].costType == WealthType.Real)
                    storeCardPriceTextArr[_storeDataID].text = realPrice + storeDataArr[_storeDataID].costValue;
                else if(storeDataArr[_storeDataID].costType == WealthType.Gold)
                {
                    storeCardPriceTextArr[_storeDataID].text = storeDataArr[_storeDataID].costValue.ToString();

                    storeCardPriceIconImageArr[_storeDataID].sprite =
                        DataBase_Manager.Instance.wealthSpriteArr[0];

                    storeCardPriceIconImageArr[_storeDataID].SetNativeSize();

                    storeCardPriceIconImageArr[_storeDataID].transform.localScale = Vector3.one * 0.5f;
                }
                else if (storeDataArr[_storeDataID].costType == WealthType.Mineral)
                {
                    storeCardPriceTextArr[_storeDataID].text = storeDataArr[_storeDataID].costValue.ToString();

                    storeCardPriceIconImageArr[_storeDataID].sprite =
                        DataBase_Manager.Instance.wealthSpriteArr[1];

                    storeCardPriceIconImageArr[_storeDataID].SetNativeSize();

                    storeCardPriceIconImageArr[_storeDataID].transform.localScale = Vector3.one * 0.5f;
                }
            }
        }
    }
    void InitGacha_Func()
    {
        gachaClass.Init_Func(this);
    }

    public void OnTapBtn_Func(int _listID)
    {
        storeState = (StoreState)_listID;
        listID = _listID;

        for (int i = 0; i < 5; i++)
        {
            if (_listID != i)
                storeCardGroupObjArr[i].SetActive(false);
            else
                storeCardGroupObjArr[i].SetActive(true);
        }
    }
    public void OnListButton_Func(int _cardID)
    {
        cardID = _cardID;
        storeDataID = (listID * 4) + _cardID;
        
        popUpClass.Active_Func(storeDataArr[storeDataID]);
    }

    public void BuyStoreGoods_Func(int _storeDataID)
    {
        Store_Data _storeData = storeDataArr[_storeDataID];

        switch (_storeData.storeGoodsType)
        {
            case StoreGoodsType.Wealth:
                Player_Data.Instance.AddWealth_Func((WealthType)_storeData.goodsID, _storeData.goodsAmount);
                OnResultMessage_Func();
                break;
            case StoreGoodsType.FoodBox:
                FoodGrade _foodGrade = FoodGrade.Common;
                if (_storeData.goodsID == 1)
                    _foodGrade = FoodGrade.Rare;

                for (int i = 0; i < _storeData.goodsAmount; i++)
                {
                    int _foodID = GetFoodRandID_Func(_foodGrade);
                    Player_Data.Instance.AddFood_Func(_foodID);
                }
                break;
            case StoreGoodsType.Trophy:
                for (int i = 0; i < _storeData.goodsAmount; i++)
                {
                    Player_Data.Instance.AddTrophy_Func(TrophyType.Random, true);
                }
                break;
            case StoreGoodsType.Drink:
                int _drinkID = _storeData.goodsID;
                int _drinkNum = _storeData.goodsAmount;
                Player_Data.Instance.AddDrink_Func(_drinkID, _drinkNum);
                break;
            case StoreGoodsType.Package:
                break;
        }
    }
    void OnResultMessage_Func()
    {
        resultMessageObj.SetActive(true);
    }

    public int GetFoodRandID_Func(FoodGrade _checkFoodGrade = FoodGrade.Common)
    {
        int _perBonos = 0;
        if (_checkFoodGrade == FoodGrade.Rare)
            _perBonos = 70;
        else if (_checkFoodGrade == FoodGrade.Legend)
            _perBonos = 95;

        FoodGrade _foodGrade = FoodGrade.None;
        int _randValue = Random.Range(_perBonos, 100);
        if (0 <= _randValue && _randValue < 70)
        {
            _foodGrade = FoodGrade.Common;
        }
        else if (70 <= _randValue && _randValue < 95)
        {
            _foodGrade = FoodGrade.Rare;
        }
        else
        {
            _foodGrade = FoodGrade.Legend;
        }

        int _foodNum = DataBase_Manager.Instance.foodDataArr.Length;
        int _randFoodID = Random.Range(0, _foodNum);
        while (true)
        {
            if (_foodGrade == DataBase_Manager.Instance.foodDataArr[_randFoodID].foodGrade)
            {
                break;
            }

            _randFoodID++;
            if (_foodNum <= _randFoodID)
                _randFoodID = 0;
        }

        return _randFoodID;
    }
}