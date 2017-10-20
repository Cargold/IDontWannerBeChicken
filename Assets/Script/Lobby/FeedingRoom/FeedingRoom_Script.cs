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
    public Text upgradeCostText;
    public Image expMainImage;
    public Image expProgressImage;

    public Transform selectPointingTrf;
    public Food_Script selectedFoodClass;
    public Food_Script materialFoodClass;
    public bool isUpgradeReady;
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
    #region Food Control Group
    public void PointDown_Func(Food_Script _foodClass)
    {

    }

    public void PointUp_Func(Food_Script _foodClass)
    {
        if (selectedFoodClass == null || selectedFoodClass != _foodClass && materialFoodClass != _foodClass)
        {
            // 처음 먹이주기에 들어온 경우
            // 선택되지도 않고, 재료 음식도 아닌 음식을 선택한 경우

            SelectNewFood_Func(_foodClass);
        }
        else if(selectedFoodClass == _foodClass)
        {

        }
        else if(materialFoodClass == _foodClass)
        {
            // 재료 음식을 선택 취소한 경우

            if(isUpgradeReady == false)
            {
                UpgradeEnd_Func();
            }
            else if(isUpgradeReady == true)
            {
                int _upgradeCost = selectedFoodClass.GetUpgradeCost_Func();
                if (Player_Data.Instance.SetWealth_Func(WealthType.Gold, -_upgradeCost) == true)
                    Upgrade_Func();
                else
                    UpgradeEnd_Func();
            }
        }
        else
        {
            Debug.LogError("Bug : 어느 음식도 선택되지 않았습니다.");
        }
    }
    void SelectNewFood_Func(Food_Script _foodClass)
    {
        PrintFoodInfo_Func(_foodClass);

        if(selectedFoodClass != null)
        {
            inventoryClass.SetRegroupTrf_Func(selectedFoodClass.transform);
        }

        selectedFoodClass = _foodClass;
        SetTopDepthTrf_Func(selectedFoodClass.transform);
        selectedFoodClass.transform.SetAsLastSibling();

        touchOffsetPos = Input.mousePosition - _foodClass.transform.position;
        Vector3 _dragPos = Input.mousePosition - touchOffsetPos;
        selectPointingTrf.position = _dragPos;
        selectPointingTrf.SetAsLastSibling();

        guideSelectObj.SetActive(true);
        guideDragObj.SetActive(false);
    }
    
    public void DragBegin_Func(Food_Script _foodClass)
    {
        if (selectedFoodClass == _foodClass)
        {
            // 선택된 음식을 드래그한 경우

            DragBeginSelectFood_Func();
        }
        else if(materialFoodClass == null)
        {
            // 재료 음식을 드래그한 경우
            // 업그레이드 상황 시작

            UpgradeBegin_Func(_foodClass);
            PrintUpgradeInfo_Func();
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
    
    void SetTopDepthTrf_Func(Transform _trf)
    {
        _trf.parent = upgradeGroupTrf;
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
        else if(materialFoodClass == _foodClass)
        {
            // 재료 음식을 드래그한 경우

            Vector3 _dragPos = Input.mousePosition - touchOffsetPos;

            materialFoodClass.transform.position = _dragPos;
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

    void PrintFoodInfo_Func(Food_Script _foodClass)
    {
        foodText.text = _foodClass.foodName + " Lv." + _foodClass.level;

        switch (_foodClass.effectMain)
        {
            case FoodEffect_Main.AttackPower:
                mainEffectText.text = "Atk +" + _foodClass.GetMainEffectValue_Func() + "%";
                break;
            case FoodEffect_Main.HealthPoint:
                mainEffectText.text = "HP +" + _foodClass.GetMainEffectValue_Func() + "%";
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

        float _expPer = _foodClass.remainExp / _foodClass.GetMaxExp_Func();
        expMainImage.fillAmount = _expPer;
        expProgressImage.fillAmount = 0f;
    }
    public bool CheckInventoryFood_Func(Food_Script _foodClass)
    {
        return inventoryClass.CheckInventoryFood_Func(_foodClass);
    }
    public void RemoveFoodInInventory_Func(Food_Script _foodClass)
    {
        inventoryClass.RemoveFood_Func(_foodClass);
    }

    #region Upgrade Group
    private void PrintUpgradeInfo_Func()
    {
        float _expTotal = materialFoodClass.GetMaterialExp_Func();
        CalcFoodExpData _calcFoodExpData = selectedFoodClass.GetCalcExpData_Func(_expTotal);

        if (_calcFoodExpData.exp_ReachedPer <= selectedFoodClass.GetExpPer_Func())
            expProgressImage.fillAmount = 1f;
        else
            expProgressImage.fillAmount = _calcFoodExpData.exp_ReachedPer;

        if (0 < _calcFoodExpData.level_UpCount)
        {
            foodText.text = foodText.text + "  <color=#3ed525>+" + _calcFoodExpData.level_UpCount + "</color>";
            mainEffectText.text = mainEffectText.text + " <color=#3ed525>+" + _calcFoodExpData.effectValue_UpValue + "%</color>";
        }

        int _upgradeCost = selectedFoodClass.GetUpgradeCost_Func();
        upgradeCostText.text = _upgradeCost.ToString();
        if (Player_Data.Instance.SetWealth_Func(WealthType.Gold, -_upgradeCost, true) == false)
        {
            upgradeCostText.color = Color.red;
        }
        else
        {
            upgradeCostText.color = Game_Manager.Instance.textColor;
        }
    }
    private void UpgradeBegin_Func(Food_Script _foodClass)
    {
        isUpgradeReady = false;

        materialFoodClass = _foodClass;

        guideSelectObj.SetActive(false);
        guideDragObj.SetActive(true);

        touchOffsetPos = Input.mousePosition - materialFoodClass.transform.position;


        upgradeFocusImage.SetNaturalAlphaColor_Func(0.7f);
        upgradeFocusImage.transform.SetAsLastSibling();

        SetTopDepthTrf_Func(selectedFoodClass.transform);
        selectedFoodClass.transform.SetAsLastSibling();

        selectPointingTrf.SetAsLastSibling();

        SetTopDepthTrf_Func(materialFoodClass.transform);
        materialFoodClass.transform.SetAsLastSibling();
    }
    public  void UpgradeFoodApproach_Func(Food_Script _materialFoodClass)
    {
        if(_materialFoodClass == materialFoodClass)
        {
            isUpgradeReady = true;
        }
    }
    public  void UpgradeFoodAway_Func(Food_Script _exitFoodClass)
    {
        if(_exitFoodClass == materialFoodClass)
        {
            isUpgradeReady = false;
        }
    }
    private void UpgradeEnd_Func()
    {
        isUpgradeReady = false;

        inventoryClass.SetRegroupTrf_Func(selectedFoodClass.transform);
        inventoryClass.SetRegroupTrf_Func(materialFoodClass.transform);

        materialFoodClass = null;

        guideSelectObj.SetActive(true);
        guideDragObj.SetActive(false);

        upgradeFocusImage.SetNaturalAlphaColor_Func(0f);

        PrintFoodInfo_Func(selectedFoodClass);
    }
    private void Upgrade_Func()
    {


        isUpgradeReady = false;

        float _materialExp = materialFoodClass.GetMaterialExp_Func();
        materialFoodClass.Destroy_Func();
        materialFoodClass = null;

        selectedFoodClass.GetExp_Func(_materialExp);
        PrintFoodInfo_Func(selectedFoodClass);

        upgradeFocusImage.SetNaturalAlphaColor_Func(0f);
    }
    #endregion
}