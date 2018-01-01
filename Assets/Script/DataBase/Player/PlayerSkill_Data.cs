using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PlayerSkill_Data
{
    public int skillID;
    public bool isUnlock;
    public int skillLevel;
    public Skill_Parent skillParentClass;

    public void Init_Func(int _skillID)
    {
        skillID = _skillID;
        isUnlock = false;
        skillLevel = 1;

        GameObject _skillObj = GameObject.Instantiate(DataBase_Manager.Instance.skillDataObjArr[_skillID]);
        skillParentClass = _skillObj.GetComponent<Skill_Parent>();
    }

    public void UnlockSkill_Func()
    {
        if (isUnlock == false)
        {
            isUnlock = true;
            skillLevel = 1;
        }
        else
        {
            Debug.LogError("Bug : 이미 해금된 스킬을 획득");
        }
    }

    public void LevelUpSkill_Func(int _fixedValue = -1)
    {
        if(_fixedValue == -1)
        {
            if(skillLevel < 20)
            {
                skillLevel++;
            }
            else
            {
                Debug.LogError("Bug : 스킬 레벨이 최대치에 도달했음에도 레벨업이 발생함");
            }
        }
        else
        {
            skillLevel = _fixedValue;

            if (20 < _fixedValue)
            {
                Debug.LogError("Bug : 스킬 레벨이 최대치에 도달했음에도 레벨업이 발생함");
            }
        }

        string _saveType = SaveSystem_Manager.Instance.SetRename_Func(SaveType.Skill_zzzSkillIDzzz_Level, skillID);
        SaveSystem_Manager.Instance.SaveData_Func(_saveType, skillLevel);
    }
}
