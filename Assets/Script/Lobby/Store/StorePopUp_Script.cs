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

        contentText.text = _storeData.goodsDesc;

        goodsText.text = _storeData.goodsTitle + " " + string.Format("{0:N0}", _storeData.goodsAmount);

        goodsImage.sprite = _storeData.storeCardSprite;
        goodsImage.SetNativeSize();

        this.gameObject.SetActive(true);
    }
    public void OnPayButton_Func()
    {
        bool _isPayable
            = Player_Data.Instance.PayWealth_Func(storeData.costType, storeData.costValue, true);
        
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
    void ResetCard_Func()
    {
        btnRTrf.localScale = Vector3.one;
    }

    public void Deactive_Func()
    {
        this.gameObject.SetActive(false);
    }
}
