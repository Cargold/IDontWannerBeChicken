using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Gacha_Script : MonoBehaviour
{
    public StoreRoom_Script storeRoomClass;

    public bool isActive = false;

    public Transform boxTrf;
    public Text boxText;
    public Image boxImage;
    public GachaCard_Script[] gachaCardArr;
    private int gachaCountMax;
    private int gachaCount;
    public GameObject clearBtnObj;

    public Sprite[] cardBgSpriteArr;

    enum CardBgType
    {
        Wealth = 0,
        Food = 1,
        Special = 2,
    }

    public void Init_Func(StoreRoom_Script _storeRoomClass)
    {
        storeRoomClass = _storeRoomClass;

        for (int i = 0; i < gachaCardArr.Length; i++)
        {
            gachaCardArr[i].Init_Func(this);
        }

        gachaCountMax = 0;
        gachaCount = 0;

        clearBtnObj.SetActive(false);

        this.gameObject.SetActive(false);
    }

    public void BuyStoreGoods_Func(Store_Data _storeData)
    {
        isActive = true;

        gachaCountMax = 0;
        gachaCount = 0;

        switch (_storeData.storeGoodsType)
        {
            case StoreGoodsType.Wealth:
                BuyWealth_Func(_storeData);
                break;
            case StoreGoodsType.FoodBox:
                BuyFoodBox_Func(_storeData);
                break;
            case StoreGoodsType.Trophy:
                BuyTrophy_Func(_storeData);
                break;
            case StoreGoodsType.Drink:
                BuyDrink_Func(_storeData);
                break;
            case StoreGoodsType.Package:
                BuyPackage_Func(_storeData);
                break;
        }
    }
    private void BuyWealth_Func(Store_Data _storeData)
    {
        Player_Data.Instance.AddWealth_Func((WealthType)_storeData.goodsID, _storeData.goodsAmount);
        storeRoomClass.OnResultMessage_Func();
    }
    private void BuyFoodBox_Func(Store_Data _storeData)
    {
        FoodGrade _foodGrade = FoodGrade.Common;
        if (_storeData.goodsID == 1)
            _foodGrade = FoodGrade.Rare;

        for (int i = 0; i < _storeData.goodsAmount; i++)
        {
            // 플레이어 데이터 기록
            int _foodID = storeRoomClass.GetFoodRandID_Func(_foodGrade);
            Player_Data.Instance.AddFood_Func(_foodID);

            // 가챠용 연출
            Food_Data _foodData = DataBase_Manager.Instance.foodDataArr[_foodID];
            Sprite _foodSprite = _foodData.foodSprite;
            string _foodName = _foodData.foodName;
            SetGachaCard_Func(CardBgType.Food, _foodSprite, _foodName);
        }

        // 가챠 출력
        PrintGacha_Func(_storeData.storeCardSprite, _storeData.goodsTitle);
        this.gameObject.SetActive(true);
    }
    private void BuyTrophy_Func(Store_Data _storeData)
    {
        for (int i = 0; i < _storeData.goodsAmount; i++)
        {
            // 플레이어 데이터 기록
            int _trophyID = Player_Data.Instance.GetTrophyRandID_Func();
            Player_Data.Instance.AddTrophy_Func(_trophyID, true);

            // 가챠용 연출
            Trophy_Data _trophyData = DataBase_Manager.Instance.trophyDataArr[_trophyID];
            Sprite _trophySprite = _trophyData.trophySprite;
            string _trophyName = _trophyData.nameArr[TranslationSystem_Manager.Instance.languageTypeID];
            SetGachaCard_Func(CardBgType.Special, _trophySprite, _trophyName);
        }

        // 가챠 출력
        PrintGacha_Func(_storeData.storeCardSprite, _storeData.goodsTitle);
        this.gameObject.SetActive(true);
    }
    private void BuyDrink_Func(Store_Data _storeData)
    {
        // 플레이어 데이터 기록
        int _drinkID = _storeData.goodsID;
        int _drinkNum = _storeData.goodsAmount;
        Player_Data.Instance.AddDrink_Func(_drinkID, _drinkNum);

        // 가챠용 연출
        Drink_Data _drinkData = DataBase_Manager.Instance.drinkDataArr[_drinkID];
        for (int i = 0; i < _drinkNum; i++)
        {
            SetGachaCard_Func(CardBgType.Special, _drinkData.drinkSprite, _drinkData.nameArr[TranslationSystem_Manager.Instance.languageTypeID]);
        }

        // 가챠 출력
        PrintGacha_Func(_storeData.storeCardSprite, _storeData.goodsTitle);
        this.gameObject.SetActive(true);
    }
    private void BuyPackage_Func(Store_Data _storeData)
    {
        Player_Data.Instance.BuyPackage_Func(_storeData.goodsID);

        switch (_storeData.goodsID)
        {
            case 0:
                // 플레이어 데이터 기록
                Player_Data.Instance.AddWealth_Func(WealthType.Mineral, 500);

                // 연출
                storeRoomClass.OnResultMessage_Func();
                break;
            case 1:
                // 미네랄
                Player_Data.Instance.AddWealth_Func(WealthType.Mineral, 1000);

                // 음식
                FoodGrade _foodGrade = FoodGrade.Rare;
                for (int i = 0; i < 15; i++)
                {
                    // 플레이어 데이터 기록
                    
                    int _foodID = storeRoomClass.GetFoodRandID_Func(_foodGrade);
                    Player_Data.Instance.AddFood_Func(_foodID);

                    // 가챠용 세팅
                    Food_Data _foodData = DataBase_Manager.Instance.foodDataArr[_foodID];
                    Sprite _foodSprite = _foodData.foodSprite;
                    string _foodName = _foodData.foodName;
                    SetGachaCard_Func(CardBgType.Food, _foodSprite, _foodName);
                }

                // 가챠 출력
                PrintGacha_Func(_storeData.storeCardSprite, _storeData.goodsTitle);
                this.gameObject.SetActive(true);
                break;
            case 2:
                // 드링크
                for (int i = 0; i < 4; i++)
                {
                    int _buyDrinkNum_Double = storeRoomClass.GetBuyDrinkNum_Func(i) * 2;

                    // 플레이어 데이터 기록
                    Player_Data.Instance.AddDrink_Func(i, _buyDrinkNum_Double);

                    // 가챠용 세팅
                    Drink_Data _drinkData = DataBase_Manager.Instance.drinkDataArr[i];
                    SetGachaCard_Func(CardBgType.Special, _drinkData.drinkSprite, _drinkData.nameArr[TranslationSystem_Manager.Instance.languageTypeID] + " x" + _buyDrinkNum_Double);
                }

                // 소스
                for (int i = 0; i < 5; i++)
                {
                    // 플레이어 데이터 기록
                    Player_Data.Instance.AddSource_Func(2);
                
                    // 가챠용 세팅
                    Food_Data _sourceData = DataBase_Manager.Instance.sourceDataArr[2];
                    SetGachaCard_Func(CardBgType.Food, _sourceData.foodSprite, _sourceData.nameArr[TranslationSystem_Manager.Instance.languageTypeID]);
                }

                // 가챠 출력
                PrintGacha_Func(_storeData.storeCardSprite, _storeData.goodsTitle);
                this.gameObject.SetActive(true);
                break;
            case 3:
                // 소스 음식
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        // 플레이어 데이터 기록
                        Player_Data.Instance.AddSource_Func(j);
                    }

                    // 가챠용 세팅
                    Food_Data _sourceData = DataBase_Manager.Instance.sourceDataArr[i];
                    SetGachaCard_Func(CardBgType.Food, _sourceData.foodSprite, _sourceData.nameArr[TranslationSystem_Manager.Instance.languageTypeID] +" x3");
                }

                // 고급 음식
                _foodGrade = FoodGrade.Rare;
                for (int i = 0; i < 3; i++)
                {
                    // 플레이어 데이터 기록

                    int _foodID = storeRoomClass.GetFoodRandID_Func(_foodGrade);
                    Player_Data.Instance.AddFood_Func(_foodID);

                    // 가챠용 세팅
                    Food_Data _foodData = DataBase_Manager.Instance.foodDataArr[_foodID];
                    Sprite _foodSprite = _foodData.foodSprite;
                    string _foodName = _foodData.foodName;
                    SetGachaCard_Func(CardBgType.Food, _foodSprite, _foodName);
                }

                // 드링크
                for (int i = 0; i < 4; i++)
                {
                    int _buyDrinkNum = storeRoomClass.GetBuyDrinkNum_Func(i);

                    // 플레이어 데이터 기록
                    Player_Data.Instance.AddDrink_Func(i, _buyDrinkNum);

                    // 가챠용 세팅
                    Drink_Data _drinkData = DataBase_Manager.Instance.drinkDataArr[i];
                    SetGachaCard_Func(CardBgType.Special, _drinkData.drinkSprite, _drinkData.nameArr[TranslationSystem_Manager.Instance.languageTypeID] + " x" + _buyDrinkNum);
                }

                // 미네랄
                Player_Data.Instance.AddWealth_Func(WealthType.Mineral, 300);

                // 가챠 출력
                PrintGacha_Func(_storeData.storeCardSprite, _storeData.goodsTitle);
                this.gameObject.SetActive(true);
                break;
        }
    }

    void SetGachaCard_Func(CardBgType _cardBgType, Sprite _iconSprite, string _text)
    {
        Sprite _cardBg = cardBgSpriteArr[(int)_cardBgType];
        gachaCardArr[gachaCountMax++].SetData_Func(_cardBg, _iconSprite, _text);
    }
    void PrintGacha_Func(Sprite _gachaSprite, string _text)
    {
        boxImage.sprite = _gachaSprite;
        boxImage.SetNativeSize();

        boxText.text = _text;

        boxTrf.localScale = Vector3.one;
        boxTrf.DOScale(Vector3.zero, 0.4f).SetDelay(0.4f).OnComplete(PrintGachaDirection_Func);
    }
    void PrintGachaDirection_Func()
    {
        StartCoroutine("PrintGachaDirection_Cor");
    }
    IEnumerator PrintGachaDirection_Cor()
    {
        for (int i = 0; i < gachaCountMax; i++)
        {
            if (isActive == true)
                gachaCardArr[i].Active_Func();
            else
                yield break;

            yield return new WaitForSeconds(0.1f);
        }
    }
    public void CardResizeClear_Func()
    {
        gachaCount++;

        if (gachaCountMax <= gachaCount)
        {
            clearBtnObj.SetActive(true);
        }
    }
    public void Deactive_Func()
    {
        for (int i = 0; i < gachaCardArr.Length; i++)
        {
            gachaCardArr[i].Deactive_Func();
        }

        isActive = false;

        clearBtnObj.SetActive(false);

        gachaCountMax = 0;
        gachaCount = 0;

        this.gameObject.SetActive(false);
    }
}
