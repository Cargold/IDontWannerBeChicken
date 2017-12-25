using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslationSystem_Manager : MonoBehaviour
{
    public static TranslationSystem_Manager Instance;
    
    public LanguageType languageType;
    [System.NonSerialized]
    public int languageTypeID;

    public string trophyRoomNumDesc
    {
        get
        {
            return trophyRoomNumDescArr[languageTypeID];
        }
    }
    public string title
    {
        get
        {
            return titleArr[languageTypeID];
        }
    }
    public string BattleUnit
    {
        get
        {
            return battleUnitArr[languageTypeID];
        }
    }
    public string Feed
    {
        get
        {
            return feedArr[languageTypeID];
        }
    }
    public string Back
    {
        get
        {
            return backArr[languageTypeID];
        }
    }
    public string Upgrade
    {
        get
        {
            return upgradeArr[languageTypeID];
        }
    }
    public string FoodDragGuide
    {
        get
        {
            return foodDragGuideArr[languageTypeID];
        }
    }
    public string Health
    {
        get
        {
            return healthArr[languageTypeID];
        }
    }
    public string Damage
    {
        get
        {
            return damageArr[languageTypeID];
        }
    }
    public string Critical
    {
        get
        {
            return criticalArr[languageTypeID];
        }
    }
    public string Defence
    {
        get
        {
            return defenceArr[languageTypeID];
        }
    }
    public string SpawnInterval
    {
        get
        {
            return spawnIntervalArr[languageTypeID];
        }
    }
    public string Exp
    {
        get
        {
            return expArr[languageTypeID];
        }
    }
    public string PreparingRaid
    {
        get
        {
            return preparingRaidArr[languageTypeID];
        }
    }
    public string ChickenRestaurant
    {
        get
        {
            return chickenRestaurantArr[languageTypeID];
        }
    }
    public string ChickenRestaurantDestroyed
    {
        get
        {
            return chickenRestaurantDestroyedArr[languageTypeID];
        }
    }
    public string Raid
    {
        get
        {
            return raidArr[languageTypeID];
        }
    }
    public string YouAreFried
    {
        get
        {
            return youAreFriedArr[languageTypeID];
        }
    }
    public string Ok
    {
        get
        {
            return okArr[languageTypeID];
        }
    }
    public string Retry
    {
        get
        {
            return retryArr[languageTypeID];
        }
    }
    public string Pause
    {
        get
        {
            return pauseArr[languageTypeID];
        }
    }
    public string War
    {
        get
        {
            return warArr[languageTypeID];
        }
    }
    public string Fried
    {
        get
        {
            return friedArr[languageTypeID];
        }
    }
    public string Devs
    {
        get
        {
            return devsArr[languageTypeID];
        }
    }
    public string Bgm
    {
        get
        {
            return bgmArr[languageTypeID];
        }
    }
    public string Sfx
    {
        get
        {
            return sfxArr[languageTypeID];
        }
    }
    public string Mineral
    {
        get
        {
            return mineralArr[languageTypeID];
        }
    }
    public string Gold
    {
        get
        {
            return goldArr[languageTypeID];
        }
    }
    public string FoodBox
    {
        get
        {
            return foodBoxArr[languageTypeID];
        }
    }
    public string LuxuryFoodBox
    {
        get
        {
            return luxuryFoodBoxArr[languageTypeID];
        }
    }
    public string TrophyFishing
    {
        get
        {
            return trophyFishingArr[languageTypeID];
        }
    }
    public string Food
    {
        get
        {
            return foodArr[languageTypeID];
        }
    }
    public string LuxuryFood
    {
        get
        {
            return luxuryFoodArr[languageTypeID];
        }
    }
    public string TrophyFishDesc
    {
        get
        {
            return trophyFishDescArr[languageTypeID];
        }
    }
    public string PackageNameArr_0
    {
        get
        {
            return packageNameArr_0[languageTypeID];
        }
    }
    public string PackageNameArr_1
    {
        get
        {
            return packageNameArr_1[languageTypeID];
        }
    }
    public string PackageNameArr_2
    {
        get
        {
            return packageNameArr_2[languageTypeID];
        }
    }
    public string PackageNameArr_3
    {
        get
        {
            return packageNameArr_3[languageTypeID];
        }
    }
    public string PackageDescArr_0
    {
        get
        {
            return packageDescArr_0[languageTypeID];
        }
    }
    public string PackageDescArr_1
    {
        get
        {
            return packageDescArr_1[languageTypeID];
        }
    }
    public string PackageDescArr_2
    {
        get
        {
            return packageDescArr_2[languageTypeID];
        }
    }
    public string PackageDescArr_3
    {
        get
        {
            return packageDescArr_3[languageTypeID];
        }
    }
    public string ReviewArr
    {
        get
        {
            return reviewArr[languageTypeID];
        }
    }
    public string ReviewYesArr
    {
        get
        {
            return reviewYesArr[languageTypeID];
        }
    }
    public string ReviewNoArr
    {
        get
        {
            return reviewNoArr[languageTypeID];
        }
    }
    public string Tutorial_StartArr_0
    {
        get
        {
            return tutorial_StartArr_0[languageTypeID];
        }
    }
    public string Tutorial_StartArr_1
    {
        get
        {
            return tutorial_StartArr_1[languageTypeID];
        }
    }
    public string Tutorial_StartArr_2
    {
        get
        {
            return tutorial_StartArr_2[languageTypeID];
        }
    }
    public string Tutorial_BattleArr_0
    {
        get
        {
            return tutorial_BattleArr_0[languageTypeID];
        }
    }
    public string Tutorial_BattleArr_1
    {
        get
        {
            return tutorial_BattleArr_1[languageTypeID];
        }
    }
    public string Tutorial_BattleArr_2
    {
        get
        {
            return tutorial_BattleArr_2[languageTypeID];
        }
    }
    public string Tutorial_BattleArr_3
    {
        get
        {
            return tutorial_BattleArr_3[languageTypeID];
        }
    }
    public string Tutorial_PartySettingArr_0
    {
        get
        {
            return tutorial_PartySettingArr_0[languageTypeID];
        }
    }
    public string Tutorial_PartySettingArr_1
    {
        get
        {
            return tutorial_PartySettingArr_1[languageTypeID];
        }
    }
    public string Tutorial_PartySettingArr_2
    {
        get
        {
            return tutorial_PartySettingArr_2[languageTypeID];
        }
    }
    public string Tutorial_PartySettingArr_3
    {
        get
        {
            return tutorial_PartySettingArr_3[languageTypeID];
        }
    }
    public string Tutorial_FeedingArr_0
    {
        get
        {
            return tutorial_FeedingArr_0[languageTypeID];
        }
    }
    public string Tutorial_FeedingArr_1
    {
        get
        {
            return tutorial_FeedingArr_1[languageTypeID];
        }
    }
    public string Tutorial_FeedingArr_2
    {
        get
        {
            return tutorial_FeedingArr_2[languageTypeID];
        }
    }
    public string Tutorial_FeedingArr_3
    {
        get
        {
            return tutorial_FeedingArr_3[languageTypeID];
        }
    }
    public string Tutorial_SpecialStageArr_0
    {
        get
        {
            return tutorial_SpecialStageArr_0[languageTypeID];
        }
    }
    public string Tutorial_SpecialStageArr_1
    {
        get
        {
            return tutorial_SpecialStageArr_1[languageTypeID];
        }
    }
    public string Tutorial_MuseumArr_0
    {
        get
        {
            return tutorial_MuseumArr_0[languageTypeID];
        }
    }
    public string Tutorial_MuseumArr_1
    {
        get
        {
            return tutorial_MuseumArr_1[languageTypeID];
        }
    }
    public string LoadingArr
    {
        get
        {
            return loadingArr[languageTypeID];
        }
    }
    public string PressToStartArr
    {
        get
        {
            return pressToStartArr[languageTypeID];
        }
    }

    public string ChickenRestaurantArr2
    {
        get
        {
            return chickenRestaurantArr2[languageTypeID];
        }
    }

    public string MaterialFoodDesc
    {
        get
        {
            return materialFoodDesc[languageTypeID];
        }
    }
    public string TabBtnName0
    {
        get
        {
            return tabBtnName0[languageTypeID];
        }
    }
    public string TabBtnName1
    {
        get
        {
            return tabBtnName1[languageTypeID];
        }
    }
    public string TabBtnName2
    {
        get
        {
            return tabBtnName2[languageTypeID];
        }
    }
    public string TabBtnName3
    {
        get
        {
            return tabBtnName3[languageTypeID];
        }
    }
    public string TabBtnName4
    {
        get
        {
            return tabBtnName4[languageTypeID];
        }
    }

    public string MineralDesc_0
    {
        get
        {
            return mineralDesc_0[languageTypeID];
        }
    }

    public string MineralDesc_1
    {
        get
        {
            return mineralDesc_1[languageTypeID];
        }
    }

    public string MineralDesc_2
    {
        get
        {
            return mineralDesc_2[languageTypeID];
        }
    }

    public string MineralDesc_3
    {
        get
        {
            return mineralDesc_3[languageTypeID];
        }
    }

    public string GoldDesc_0
    {
        get
        {
            return goldDesc_0[languageTypeID];
        }
    }

    public string GoldDesc_1
    {
        get
        {
            return goldDesc_1[languageTypeID];
        }
    }

    public string GoldDesc_2
    {
        get
        {
            return goldDesc_2[languageTypeID];
        }
    }

    public string GoldDesc_3
    {
        get
        {
            return goldDesc_3[languageTypeID];
        }
    }

    public string TrophyName
    {
        get
        {
            return trophyName[languageTypeID];
        }
    }

    public string GachaBox
    {
        get
        {
            return gachaBox[languageTypeID];
        }
    }

    [TextArea] [SerializeField] private string[] trophyRoomNumDescArr;
    [TextArea] [SerializeField] private string[] titleArr;
    [TextArea] [SerializeField] private string[] battleUnitArr;
    [TextArea] [SerializeField] private string[] feedArr;
    [TextArea] [SerializeField] private string[] backArr;
    [TextArea] [SerializeField] private string[] upgradeArr;
    [TextArea] [SerializeField] private string[] foodDragGuideArr;
    [TextArea] [SerializeField] private string[] healthArr;
    [TextArea] [SerializeField] private string[] damageArr;
    [TextArea] [SerializeField] private string[] criticalArr;
    [TextArea] [SerializeField] private string[] defenceArr;
    [TextArea] [SerializeField] private string[] spawnIntervalArr;
    [TextArea] [SerializeField] private string[] expArr;
    [TextArea] [SerializeField] private string[] preparingRaidArr;
    [TextArea] [SerializeField] private string[] chickenRestaurantArr;
    [TextArea] [SerializeField] private string[] chickenRestaurantArr2;
    [TextArea] [SerializeField] private string[] chickenRestaurantDestroyedArr;
    [TextArea] [SerializeField] private string[] raidArr;
    [TextArea] [SerializeField] private string[] youAreFriedArr;
    [TextArea] [SerializeField] private string[] okArr;
    [TextArea] [SerializeField] private string[] retryArr;
    [TextArea] [SerializeField] private string[] pauseArr;
    [TextArea] [SerializeField] private string[] warArr;
    [TextArea] [SerializeField] private string[] friedArr;
    [TextArea] [SerializeField] private string[] devsArr;
    [TextArea] [SerializeField] private string[] bgmArr;
    [TextArea] [SerializeField] private string[] sfxArr;
    [TextArea] [SerializeField] private string[] mineralArr;
    [TextArea] [SerializeField] private string[] mineralDesc_0;
    [TextArea] [SerializeField] private string[] mineralDesc_1;
    [TextArea] [SerializeField] private string[] mineralDesc_2;
    [TextArea] [SerializeField] private string[] mineralDesc_3;
    [TextArea] [SerializeField] private string[] goldArr;
    [TextArea] [SerializeField] private string[] goldDesc_0;
    [TextArea] [SerializeField] private string[] goldDesc_1;
    [TextArea] [SerializeField] private string[] goldDesc_2;
    [TextArea] [SerializeField] private string[] goldDesc_3;
    [TextArea] [SerializeField] private string[] foodBoxArr;
    [TextArea] [SerializeField] private string[] luxuryFoodBoxArr;
    [TextArea] [SerializeField] private string[] trophyFishingArr;
    [TextArea] [SerializeField] private string[] foodArr;
    [TextArea] [SerializeField] private string[] luxuryFoodArr;
    [TextArea] [SerializeField] private string[] trophyFishDescArr;
    [TextArea] [SerializeField] private string[] packageNameArr_0;
    [TextArea] [SerializeField] private string[] packageNameArr_1;
    [TextArea] [SerializeField] private string[] packageNameArr_2;
    [TextArea] [SerializeField] private string[] packageNameArr_3;
    [TextArea] [SerializeField] private string[] packageDescArr_0;
    [TextArea] [SerializeField] private string[] packageDescArr_1;
    [TextArea] [SerializeField] private string[] packageDescArr_2;
    [TextArea] [SerializeField] private string[] packageDescArr_3;
    [TextArea] [SerializeField] private string[] reviewArr;
    [TextArea] [SerializeField] private string[] reviewYesArr;
    [TextArea] [SerializeField] private string[] reviewNoArr;
    [TextArea] [SerializeField] private string[] tutorial_StartArr_0;
    [TextArea] [SerializeField] private string[] tutorial_StartArr_1;
    [TextArea] [SerializeField] private string[] tutorial_StartArr_2;
    [TextArea] [SerializeField] private string[] tutorial_BattleArr_0;
    [TextArea] [SerializeField] private string[] tutorial_BattleArr_1;
    [TextArea] [SerializeField] private string[] tutorial_BattleArr_2;
    [TextArea] [SerializeField] private string[] tutorial_BattleArr_3;
    [TextArea] [SerializeField] private string[] tutorial_PartySettingArr_0;
    [TextArea] [SerializeField] private string[] tutorial_PartySettingArr_1;
    [TextArea] [SerializeField] private string[] tutorial_PartySettingArr_2;
    [TextArea] [SerializeField] private string[] tutorial_PartySettingArr_3;
    [TextArea] [SerializeField] private string[] tutorial_FeedingArr_0;
    [TextArea] [SerializeField] private string[] tutorial_FeedingArr_1;
    [TextArea] [SerializeField] private string[] tutorial_FeedingArr_2;
    [TextArea] [SerializeField] private string[] tutorial_FeedingArr_3;
    [TextArea] [SerializeField] private string[] tutorial_SpecialStageArr_0;
    [TextArea] [SerializeField] private string[] tutorial_SpecialStageArr_1;
    [TextArea] [SerializeField] private string[] tutorial_MuseumArr_0;
    [TextArea] [SerializeField] private string[] tutorial_MuseumArr_1;
    [TextArea] [SerializeField] private string[] loadingArr;
    [TextArea] [SerializeField] private string[] pressToStartArr;
    [TextArea] [SerializeField] private string[] materialFoodDesc;
    [TextArea] [SerializeField] private string[] tabBtnName0;
    [TextArea] [SerializeField] private string[] tabBtnName1;
    [TextArea] [SerializeField] private string[] tabBtnName2;
    [TextArea] [SerializeField] private string[] tabBtnName3;
    [TextArea] [SerializeField] private string[] tabBtnName4;
    [TextArea] [SerializeField] private string[] trophyName;
    [TextArea] [SerializeField] private string[] gachaBox;

    public void Init_Func()
    {
        Instance = this;

        SetLanguageType_Func();
    }
    public void SetLanguageType_Func()
    {
        // 기기 언어 불러오기 ㄱㄱ

        LanguageType _languageType;
        //languageType = _languageType;

        languageTypeID = (int)languageType;
    }
}
