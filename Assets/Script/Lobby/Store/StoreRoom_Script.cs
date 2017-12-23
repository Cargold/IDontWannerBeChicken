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

    public Text[] tabBtnTextArr;

    #region Override Group
    protected override void InitUI_Func()
    {
        Init_Func();

        popUpClass.Init_Func(this);

        this.gameObject.SetActive(false);
    }
    protected override void EnterUI_Func(int _referenceID = -1)
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
        InitPackage_Func();
        InitUIData_Func();
        InitGacha_Func();

        //tabBtnTextArr[0].text = TranslationSystem_Manager.Instance.TabBtnName0;
        //tabBtnTextArr[1].text = TranslationSystem_Manager.Instance.TabBtnName1;
        //tabBtnTextArr[2].text = TranslationSystem_Manager.Instance.TabBtnName2;
        //tabBtnTextArr[3].text = TranslationSystem_Manager.Instance.TabBtnName3;
        //tabBtnTextArr[4].text = TranslationSystem_Manager.Instance.TabBtnName4;

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
    void InitPackage_Func()
    {
        for (int i = 0; i < 4; i++)
        {
            if(Player_Data.Instance.isPackageAlreadyBuyArr[i] == true)
            {
                Destroy(storeListCardTrfArr[i + 16].gameObject);
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

                #region Translate
                if(_listID == 0)
                {
                    storeDataArr[_storeDataID].goodsTitle
                    = TranslationSystem_Manager.Instance.Mineral;
                    
                    if (_cardID == 0)
                    {
                        storeDataArr[_storeDataID].goodsDescArr[0]
                            = TranslationSystem_Manager.Instance.MineralDesc_0;
                    }
                    else if (_cardID == 1)
                    {
                        storeDataArr[_storeDataID].goodsDescArr[0]
                            = TranslationSystem_Manager.Instance.MineralDesc_1;
                    }
                    else if (_cardID == 2)
                    {
                        storeDataArr[_storeDataID].goodsDescArr[0]
                            = TranslationSystem_Manager.Instance.MineralDesc_2;
                    }
                    else if (_cardID == 3)
                    {
                        storeDataArr[_storeDataID].goodsDescArr[0]
                            = TranslationSystem_Manager.Instance.MineralDesc_3;
                    }
                }
                else if (_listID == 1)
                {
                    storeDataArr[_storeDataID].goodsTitle
                    = TranslationSystem_Manager.Instance.Gold;

                    if (_cardID == 0)
                    {
                        storeDataArr[_storeDataID].goodsDescArr[0]
                            = TranslationSystem_Manager.Instance.GoldDesc_0;
                    }
                    else if (_cardID == 1)
                    {
                        storeDataArr[_storeDataID].goodsDescArr[0]
                            = TranslationSystem_Manager.Instance.GoldDesc_1;
                    }
                    else if (_cardID == 2)
                    {
                        storeDataArr[_storeDataID].goodsDescArr[0]
                            = TranslationSystem_Manager.Instance.GoldDesc_2;
                    }
                    else if (_cardID == 3)
                    {
                        storeDataArr[_storeDataID].goodsDescArr[0]
                            = TranslationSystem_Manager.Instance.GoldDesc_3;
                    }
                }
                else if (_listID == 2)
                {
                    if(_cardID == 0)
                    {
                        storeDataArr[_storeDataID].goodsTitle
                            = TranslationSystem_Manager.Instance.FoodBox;

                        storeDataArr[_storeDataID].goodsDescArr[0]
                            = TranslationSystem_Manager.Instance.Food;
                    }
                    else if (_cardID == 1)
                    {
                        storeDataArr[_storeDataID].goodsTitle
                            = TranslationSystem_Manager.Instance.LuxuryFoodBox;

                        storeDataArr[_storeDataID].goodsDescArr[0]
                            = TranslationSystem_Manager.Instance.LuxuryFood;
                    }
                    else if (_cardID == 2)
                    {
                        storeDataArr[_storeDataID].goodsTitle
                            = string.Format(TranslationSystem_Manager.Instance.TrophyFishing, storeDataArr[_storeDataID].goodsAmount);

                        storeDataArr[_storeDataID].goodsDescArr[0]
                            = string.Format(TranslationSystem_Manager.Instance.TrophyFishDesc, storeDataArr[_storeDataID].goodsAmount);
                    }
                    else if (_cardID == 3)
                    {
                        storeDataArr[_storeDataID].goodsTitle
                            = string.Format(TranslationSystem_Manager.Instance.TrophyFishing, storeDataArr[_storeDataID].goodsAmount);

                        storeDataArr[_storeDataID].goodsDescArr[0]
                            = string.Format(TranslationSystem_Manager.Instance.TrophyFishDesc, storeDataArr[_storeDataID].goodsAmount);
                    }
                }
                else if (_listID == 3)
                {
                    storeDataArr[_storeDataID].goodsTitle
                            = DataBase_Manager.Instance.drinkDataArr[_cardID].nameArr[TranslationSystem_Manager.Instance.languageTypeID];

                    storeDataArr[_storeDataID].goodsDescArr[0]
                            = DataBase_Manager.Instance.drinkDataArr[_cardID].descArr[TranslationSystem_Manager.Instance.languageTypeID];
                }
                else if (_listID == 4)
                {
                    if (_cardID == 0)
                    {
                        storeDataArr[_storeDataID].goodsTitle
                            = TranslationSystem_Manager.Instance.PackageNameArr_0;

                        storeDataArr[_storeDataID].goodsDescArr[0]
                            = TranslationSystem_Manager.Instance.PackageDescArr_0;
                    }
                    else if (_cardID == 1)
                    {
                        storeDataArr[_storeDataID].goodsTitle
                            = TranslationSystem_Manager.Instance.PackageNameArr_1;

                        storeDataArr[_storeDataID].goodsDescArr[0]
                            = TranslationSystem_Manager.Instance.PackageDescArr_1;
                    }
                    else if (_cardID == 2)
                    {
                        storeDataArr[_storeDataID].goodsTitle
                            = TranslationSystem_Manager.Instance.PackageNameArr_2;

                        storeDataArr[_storeDataID].goodsDescArr[0]
                            = TranslationSystem_Manager.Instance.PackageDescArr_2;
                    }
                    else if (_cardID == 3)
                    {
                        storeDataArr[_storeDataID].goodsTitle
                            = TranslationSystem_Manager.Instance.PackageNameArr_3;

                        storeDataArr[_storeDataID].goodsDescArr[0]
                            = TranslationSystem_Manager.Instance.PackageDescArr_3;
                    }
                }
                #endregion

                storeCardTitleTextArr[_storeDataID].text
                    = storeDataArr[_storeDataID].goodsTitle;

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

        if(16<=_storeDataID)
        {
            Destroy(storeListCardTrfArr[_storeDataID].gameObject);
        }

        gachaClass.BuyStoreGoods_Func(_storeData);
    }
    public void OnResultMessage_Func()
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
    public int GetBuyDrinkNum_Func(int _drinkID)
    {
        _drinkID += 12;
        
        return storeDataArr[_drinkID].goodsAmount;
    }
}