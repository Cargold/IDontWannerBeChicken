using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StorePopUp_Script : MonoBehaviour
{
    public StoreRoom_Script storeRoomClass;

    public Text titleText;
    public Text contentText;
    public Text btnText;
    public RectTransform btnRTrf;
    public Text goodsText;
    public Image goodsImage;
    
    public Store_Data storeData;

    public void Init_Func(StoreRoom_Script _storeRoomClass)
    {
        storeRoomClass = _storeRoomClass;

        this.gameObject.SetActive(false);
    }

    public void Active_Func(Store_Data _storeData)
    {
        storeData = _storeData;

        titleText.text = "구매?";

        string _desc = "";
        for (int i = 0; i < _storeData.goodsDescArr.Length; i++)
        {
            _desc += _storeData.goodsDescArr[i] + "\n";
        }
        contentText.text = _desc;

        goodsText.text = _storeData.goodsTitle + " " + string.Format("{0:N0}", _storeData.goodsAmount);

        goodsImage.sprite = _storeData.storeCardSprite;
        goodsImage.SetNativeSize();

        this.gameObject.SetActive(true);
    }
    public void OnPayButton_Func()
    {
        bool _isPayable = false;
        bool _isPlayReal = false;

        switch (storeData.storeDataID)
        {
            case 0:
            case 1:
            case 2:
            case 3:
            case 16:
            case 17:
            case 18:
            case 19:
                _isPlayReal = true;
                break;
        }

        if(_isPlayReal == false)
        {
            _isPayable
                = Player_Data.Instance.PayWealth_Func(storeData.costType, storeData.costValue);

            if (_isPayable == true)
            {
                storeRoomClass.BuyStoreGoods_Func(storeData.storeDataID);
                Deactive_Func();
            }
            else if (_isPayable == false)
            {
                btnRTrf.DOPunchScale(Vector3.one * 0.1f, 0.25f).OnComplete(ResetCard_Func);
            }
        }
        else
        {
            GetTheMoney_Script.Instance.BuyProductID_Func(storeData.storeDataID);
        }
    }
    void ResetCard_Func()
    {
        btnRTrf.localScale = Vector3.one;
    }

    public void Deactive_Func()
    {
        this.gameObject.SetActive(false);
    }
}
