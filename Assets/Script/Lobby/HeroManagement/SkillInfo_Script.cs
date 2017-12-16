using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillInfo_Script : MonoBehaviour
{
    public HeroManagement_Script heroManagementClass;

    public Text nameText;
    public Text descText;

    public void Init_Func(HeroManagement_Script _heroManagementClass)
    {
        heroManagementClass = _heroManagementClass;
    }

    public void PrintSelectCardInfo_Func(SkillCard_Script _skillCardClass)
    {
        PlayerSkill_Data _playerSkillData = Player_Data.Instance.skillDataArr[_skillCardClass.cardID];
        int _skillLevel = _playerSkillData.skillLevel;
        Skill_Parent _skillClass = _playerSkillData.skillParentClass;

        nameText.text = _skillClass.skillNameArr[TranslationSystem_Manager.Instance.languageTypeID] + " Lv." + _skillLevel;

        int _varNum = _skillClass.skillVarArr.Length;
        List<float> _varUpgradeValueArr = new List<float>();
        for (int i = 0; i < 10; i++)
        {
            if(i < _varNum)
            {
                _varUpgradeValueArr.Add(
                    _skillClass.skillVarArr[i].initValue + (_skillLevel * _skillClass.skillVarArr[i].upgradeValue)
                    );
            }
            else
            {
                _varUpgradeValueArr.Add(0f);
            }
        }

        string _descByFormatting = "";
        _descByFormatting = string.Format(_skillClass.skillDescArr[TranslationSystem_Manager.Instance.languageTypeID], _varUpgradeValueArr[0], _varUpgradeValueArr[1], _varUpgradeValueArr[2], _varUpgradeValueArr[3], _varUpgradeValueArr[4]);

        descText.text = _descByFormatting;
    }
}
