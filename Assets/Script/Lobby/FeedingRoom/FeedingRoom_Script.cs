using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeedingRoom_Script : LobbyUI_Parent
{
    public Inventory_Script inventoryClass;

    public Text foodText;
    public Text mainEffectText;
    public Text subEffectText;
    public GameObject guideSelectObj;
    public GameObject guideDragObj;
    public Image expMainImage;
    public Image expProgressImage;

    public Transform selectPointingTrf;
    public Food_Script selectedFoodClass;
    public Food_Script upgradeFoodClass;
    private Vector3 touchOffsetPos;
    public Image upgradeFocusImage;
    public Transform upgradeGroupTrf;

    public enum FeedingRoomState
    {
        None = -1,
        SelectFood,
        ChooseUpgrade,
    }
    public FeedingRoomState feedingRoomState;

    #region Override Group
    protected override void InitUI_Func()
    {
        inventoryClass.Init_Func(this);

        this.gameObject.SetActive(false);
    }

    protected override void EnterUI_Func()
    {
        this.gameObject.SetActive(true);

        inventoryClass.Active_Func();

        PointUp_Func(inventoryClass.GetFoodRand_Func());
    }
    
    public override void Exit_Func()
    {
        inventoryClass.Deactive_Func();

        this.gameObject.SetActive(false);
    }
    #endregion

    void PrintFoodInfo_Func(Food_Script _foodClass)
    {
        foodText.text = _foodClass.foodName;

        switch (_foodClass.effectMain)
        {
            case FoodEffect_Main.AttackPower:
                mainEffectText.text = "Atk +" + _foodClass.mainEffectValue + "%";
                break;
            case FoodEffect_Main.HealthPoint:
                mainEffectText.text = "HP +" + _foodClass.mainEffectValue + "%";
                break;
            case FoodEffect_Main.Gizzard:
                mainEffectText.text = "Nothing On You, Baby ~";
                break;
        }

        switch (_foodClass.effectSub)
        {
            case FoodEffect_Sub.Critical:
                subEffectText.text = "Crit +" + _foodClass.subEffectValue + "%";
                break;
            case FoodEffect_Sub.SpawnInterval:
                subEffectText.text = "Spawn Timer -" + _foodClass.subEffectValue + "%";
                break;
            case FoodEffect_Sub.DecreaseHP:
                subEffectText.text = "HP -" + _foodClass.subEffectValue + "%";
                break;
            case FoodEffect_Sub.DefenceValue:
                subEffectText.text = "Defence +" + _foodClass.subEffectValue + "%";
                break;
            case FoodEffect_Sub.DecreaseAttack:
                subEffectText.text = "Atk -" + _foodClass.subEffectValue + "%";
                break;
            case FoodEffect_Sub.Bounus_Fish:
                subEffectText.text = "Fish Food +" + _foodClass.subEffectValue + "%";
                break;
            case FoodEffect_Sub.Bonus_Apple:
                subEffectText.text = "Fish Food +" + _foodClass.subEffectValue + "%";
                break;
        }

        guideSelectObj.SetActive(true);
        guideSelectObj.SetActive(false);

        float _expPer = _foodClass.maxExp / _foodClass.recentExp;
        expMainImage.fillAmount = _expPer;
        expProgressImage.fillAmount = 0f;
    }

    #region Food Control Group
    public void PointDown_Func(Food_Script _foodClass)
    {

    }

    public void PointUp_Func(Food_Script _foodClass)
    {
        if (selectedFoodClass == null || selectedFoodClass != _foodClass && upgradeFoodClass != _foodClass)
        {
            // 처음 먹이주기에 들어온 경우
            // 선택되지도 않고, 재료 음식도 아닌 음식을 선택한 경우

            SelectNewFood_Func(_foodClass);
        }
        else if(selectedFoodClass == _foodClass)
        {

        }
        else if(upgradeFoodClass == _foodClass)
        {
            // 재료 음식을 선택 취소한 경우

            CancelUgrade_Func();
        }
        else
        {
            Debug.LogError("Bug : 어느 음식도 선택되지 않았습니다.");
        }
    }
    void SelectNewFood_Func(Food_Script _foodClass)
    {
        PrintFoodInfo_Func(_foodClass);

        selectedFoodClass = _foodClass;
        selectedFoodClass.transform.SetAsLastSibling();

        touchOffsetPos = Input.mousePosition - _foodClass.transform.position;
        Vector3 _dragPos = Input.mousePosition - touchOffsetPos;
        selectPointingTrf.position = _dragPos;
        selectPointingTrf.SetAsLastSibling();

        guideSelectObj.SetActive(true);
        guideDragObj.SetActive(false);
    }
    void CancelUgrade_Func()
    {
        inventoryClass.SetRegroupTrf_Func(selectedFoodClass.transform);
        inventoryClass.SetRegroupTrf_Func(upgradeFoodClass.transform);
        inventoryClass.SetRegroupTrf_Func(selectPointingTrf);

        upgradeFoodClass = null;

        guideSelectObj.SetActive(true);
        guideDragObj.SetActive(false);

        upgradeFocusImage.SetNaturalAlphaColor_Func(0f);
    }

    public void DragBegin_Func(Food_Script _foodClass)
    {
        if (selectedFoodClass == _foodClass)
        {
            // 선택된 음식을 드래그한 경우

            DragBeginSelectFood_Func();
        }
        else if(upgradeFoodClass == null)
        {
            // 재료 음식을 드래그한 경우
            // 업그레이드 상황 시작

            UpgradeBegin_Func(_foodClass);
        }
        else
        {
            Debug.LogError("Bug : 드래그를 시도하는 음식은 선택된 것도 아니며, 재료도 선택된 상태입니다.");
        }
    }
    void DragBeginSelectFood_Func()
    {
        touchOffsetPos = Input.mousePosition - selectedFoodClass.transform.position;
        Vector3 _dragPos = Input.mousePosition - touchOffsetPos;
        selectPointingTrf.position = _dragPos;
        selectPointingTrf.SetAsLastSibling();
    }
    void UpgradeBegin_Func(Food_Script _foodClass)
    {
        upgradeFoodClass = _foodClass;

        guideSelectObj.SetActive(false);
        guideDragObj.SetActive(true);

        touchOffsetPos = Input.mousePosition - upgradeFoodClass.transform.position;
        

        upgradeFocusImage.SetNaturalAlphaColor_Func(0.7f);
        upgradeFocusImage.transform.SetAsLastSibling();

        selectedFoodClass.transform.parent = upgradeGroupTrf;
        selectedFoodClass.transform.SetAsLastSibling();

        selectPointingTrf.parent = upgradeGroupTrf;
        selectPointingTrf.SetAsLastSibling();

        upgradeFoodClass.transform.parent = upgradeGroupTrf;
        upgradeFoodClass.transform.SetAsLastSibling();
    }

    public void Dragging_Func(Food_Script _foodClass)
    {
        if (selectedFoodClass == _foodClass)
        {
            // 선택된 음식을 드래그한 경우

            Vector3 _dragPos = Input.mousePosition - touchOffsetPos;

            selectedFoodClass.transform.position = _dragPos;
            selectPointingTrf.position = _dragPos;
        }
        else if(upgradeFoodClass == _foodClass)
        {
            // 재료 음식을 드래그한 경우

            Vector3 _dragPos = Input.mousePosition - touchOffsetPos;

            upgradeFoodClass.transform.position = _dragPos;
        }
        else
        {
            Debug.LogError("Bug : 드래그 중인 음식은 선택되지도, 재료도 아닙니다.");
        }
    }

    public void DragEnd_Func(Food_Script _foodClass)
    {
        
    }
    #endregion
}