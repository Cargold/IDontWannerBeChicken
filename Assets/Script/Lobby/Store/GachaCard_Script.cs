using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GachaCard_Script : MonoBehaviour
{
    public Gacha_Script gachaClass;

    public Image bgImage;
    public Image iconImage;
    public Text gachaText;

    public void Init_Func(Gacha_Script _gachaClass)
    {
        gachaClass = _gachaClass;
    }

    public void Active_Func(Sprite _bgSprite, Sprite _iconSprite, string _gachaText)
    {
        bgImage.sprite = _bgSprite;
        bgImage.SetNativeSize();

        iconImage.sprite = _iconSprite;
        iconImage.SetNativeSize();

        gachaText.text = _gachaText;

        this.transform.localScale = Vector3.zero;
        this.transform.DOPunchScale(Vector3.one, 0.25f).OnComplete(ResizeClear_Func);
    }

    void ResizeClear_Func()
    {
        gachaClass.CardResizeClear_Func();
    }
}
