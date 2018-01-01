using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePlate_Script : MonoBehaviour
{
    public FeedingRoom_Script feedingRoomClass;

    public enum GuideType
    {
        None = -1,
        Init,
        Drag,
        Upgrade,
        StoneRemove,
    }

    public GameObject initObj;
    public Text initText;

    public GameObject dragObj;
    public Text dragText;
    
    public GameObject upgradeObj;
    public Text upgradeCostText;

    public GameObject removeObj;
    public Text removeText;
    public Text removeCostText;

    public Food_Script stoneClass;

    private GuideType guideState;
    private int removeCost;

    public void Init_Func(FeedingRoom_Script _feedingRoomClass)
    {
        feedingRoomClass = _feedingRoomClass;

        initText.text = TranslationSystem_Manager.Instance.FeedingRoom_InitText;
        dragText.text = TranslationSystem_Manager.Instance.FoodDragGuide;
        removeText.text = TranslationSystem_Manager.Instance.FeedingRoom_RemoveText;

        guideState = GuideType.None;
        SetInitState_Func();
    }
    public void SetInitState_Func()
    {
        DisableState_Func();

        guideState = GuideType.Init;

        initObj.SetActive(true);
    }
    public void SetDragState_Func()
    {
        DisableState_Func();

        guideState = GuideType.Drag;

        dragObj.SetActive(true);
    }
    public void SetUpgradeState_Func(int _cost, Color _textColor)
    {
        DisableState_Func();

        guideState = GuideType.Upgrade;

        upgradeObj.SetActive(true);

        upgradeCostText.text = _cost.ToString();
        upgradeCostText.color = _textColor;
    }
    public void SetStoneRemove_Func(int _cost, Food_Script _stoneClass)
    {
        DisableState_Func();

        guideState = GuideType.StoneRemove;

        removeObj.SetActive(true);

        removeCostText.text = _cost.ToString();

        removeCost = _cost;

        stoneClass = _stoneClass;
    }
    void DisableState_Func()
    {
        switch (guideState)
        {
            case GuideType.None:
                initObj.SetActive(false);
                dragObj.SetActive(false);
                upgradeObj.SetActive(false);
                removeObj.SetActive(false);
                break;
            case GuideType.Init:
                initObj.SetActive(false);
                break;
            case GuideType.Drag:
                dragObj.SetActive(false);
                break;
            case GuideType.Upgrade:
                upgradeObj.SetActive(false);
                break;
            case GuideType.StoneRemove:
                removeObj.SetActive(false);
                break;
        }
    }
    public void OnButton_Remove_Func()
    {
        if(guideState == GuideType.StoneRemove)
        {
            bool _isPayable = Player_Data.Instance.PayWealth_Func(WealthType.Mineral, removeCost);

            if(_isPayable == true)
            {
                feedingRoomClass.RemoveStone_Func(stoneClass);
            }
            else
            {

            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Food")
        {
            Food_Script _materialFoodClass = collision.transform.parent.GetComponent<Food_Script>();
            feedingRoomClass.UpgradeFoodApproach_Func(_materialFoodClass);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Food")
        {
            Food_Script _exitFoodClass = collision.transform.parent.GetComponent<Food_Script>();
            feedingRoomClass.UpgradeFoodAway_Func(_exitFoodClass);
        }
    }
}
