using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GachaCard_Script : MonoBehaviour
{
    public Gacha_Script gachaClass;

    public bool isActive;
    public Image bgImage;
    public Image iconImage;
    public Text gachaText;

    public void Init_Func(Gacha_Script _gachaClass)
    {
        gachaClass = _gachaClass;

        isActive = false;

        Deactive_Func();
    }

    public void SetData_Func(Sprite _bgSprite, Sprite _iconSprite, string _gachaText)
    {
        bgImage.sprite = _bgSprite;
        bgImage.SetNativeSize();

        iconImage.sprite = _iconSprite;
        iconImage.SetNativeSize();

        gachaText.text = _gachaText;

        this.transform.localScale = Vector3.zero;
        this.gameObject.SetActive(true);
    }

    public void Active_Func()
    {
        isActive = true;
        this.transform.DOScale(Vector3.one, 0.25f).OnComplete(ResizeClear_Func);

        SoundSystem_Manager.Instance.PlaySFX_Func(SoundType.SFX_btn_card);
    }

    void ResizeClear_Func()
    {
        gachaClass.CardResizeClear_Func();
    }

    public void Deactive_Func()
    {
        isActive = false;

        this.transform.localScale = Vector3.zero;

        this.gameObject.SetActive(false);
    }
}
