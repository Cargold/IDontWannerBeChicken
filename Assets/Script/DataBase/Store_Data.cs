using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Store_Data
{
    public int storeDataID;
    public Sprite storeCardSprite;

    public WealthType costType;
    public int costValue;

    public StoreGoodsType storeGoodsType;
    public string goodsTitle;
    public string[] goodsDescArr;
    public int goodsID;
    public int goodsAmount;
}
