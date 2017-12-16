using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslationSystem_Manager : MonoBehaviour
{
    public static TranslationSystem_Manager Instance;
    
    public LanguageType languageType;
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
    public string RandomTrophy
    {
        get
        {
            return randomTrophyArr[languageTypeID];
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

    [SerializeField] private string[] trophyRoomNumDescArr;
    [SerializeField] private string[] titleArr;
    [SerializeField] private string[] battleUnitArr;
    [SerializeField] private string[] feedArr;
    [SerializeField] private string[] backArr;
    [SerializeField] private string[] upgradeArr;
    [SerializeField] private string[] foodDragGuideArr;
    [SerializeField] private string[] healthArr;
    [SerializeField] private string[] damageArr;
    [SerializeField] private string[] criticalArr;
    [SerializeField] private string[] defenceArr;
    [SerializeField] private string[] spawnIntervalArr;
    [SerializeField] private string[] expArr;
    [SerializeField] private string[] preparingRaidArr;
    [SerializeField] private string[] chickenRestaurantArr;
    [SerializeField] private string[] chickenRestaurantDestroyedArr;
    [SerializeField] private string[] raidArr;
    [SerializeField] private string[] youAreFriedArr;
    [SerializeField] private string[] okArr;
    [SerializeField] private string[] retryArr;
    [SerializeField] private string[] pauseArr;
    [SerializeField] private string[] warArr;
    [SerializeField] private string[] friedArr;
    [SerializeField] private string[] devsArr;
    [SerializeField] private string[] bgmArr;
    [SerializeField] private string[] sfxArr;
    [SerializeField] private string[] mineralArr;
    [SerializeField] private string[] goldArr;
    [SerializeField] private string[] foodBoxArr;
    [SerializeField] private string[] luxuryFoodBoxArr;
    [SerializeField] private string[] trophyFishingArr;
    [SerializeField] private string[] foodArr;
    [SerializeField] private string[] luxuryFoodArr;
    [SerializeField] private string[] randomTrophyArr;
    [SerializeField] private string[] packageNameArr_0;
    [SerializeField] private string[] packageNameArr_1;
    [SerializeField] private string[] packageNameArr_2;
    [SerializeField] private string[] packageNameArr_3;
    [SerializeField] private string[] packageDescArr_0;
    [SerializeField] private string[] packageDescArr_1;
    [SerializeField] private string[] packageDescArr_2;
    [SerializeField] private string[] packageDescArr_3;
    [SerializeField] private string[] reviewArr;
    [SerializeField] private string[] reviewYesArr;
    [SerializeField] private string[] reviewNoArr;
    [SerializeField] private string[] tutorial_StartArr_0;
    [SerializeField] private string[] tutorial_StartArr_1;
    [SerializeField] private string[] tutorial_StartArr_2;
    [SerializeField] private string[] tutorial_BattleArr_0;
    [SerializeField] private string[] tutorial_BattleArr_1;
    [SerializeField] private string[] tutorial_BattleArr_2;
    [SerializeField] private string[] tutorial_BattleArr_3;
    [SerializeField] private string[] tutorial_PartySettingArr_0;
    [SerializeField] private string[] tutorial_PartySettingArr_1;
    [SerializeField] private string[] tutorial_PartySettingArr_2;
    [SerializeField] private string[] tutorial_PartySettingArr_3;
    [SerializeField] private string[] tutorial_FeedingArr_0;
    [SerializeField] private string[] tutorial_FeedingArr_1;
    [SerializeField] private string[] tutorial_FeedingArr_2;
    [SerializeField] private string[] tutorial_FeedingArr_3;
    [SerializeField] private string[] tutorial_SpecialStageArr_0;
    [SerializeField] private string[] tutorial_SpecialStageArr_1;
    [SerializeField] private string[] tutorial_MuseumArr_0;
    [SerializeField] private string[] tutorial_MuseumArr_1;
    [SerializeField] private string[] loadingArr;
    [SerializeField] private string[] pressToStartArr;

    public IEnumerator Init_Cor()
    {
        Instance = this;

        yield break;
    }
    public void SetLanguageType_Func(LanguageType _languageType)
    {
        languageType = _languageType;
        languageTypeID = (int)_languageType;
    }
}
