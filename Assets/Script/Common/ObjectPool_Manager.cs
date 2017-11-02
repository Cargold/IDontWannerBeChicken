using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 오브젝트 풀 매니저 싱글톤
/// </summary>
public class ObjectPool_Manager : MonoBehaviour
{
    public static ObjectPool_Manager Instance;

    public int defaultAmount = 10;
    public List<GameObject> poolList;
    private GameObject sampleFolderObj;
    public int[] poolAmount;
    
    Dictionary<string, ObjectPool> objectPoolList = new Dictionary<string, ObjectPool>();

    public IEnumerator Init_Cor()
    {
        Instance = this;

        yield return InitFolder_Cor();
        yield return InitUnit_Cor();
        yield return InitFood_Cor();
        yield return InitMonster_Cor();
        yield return InitEffect_Cor();
        yield return InitSkill_Cor();

        yield return InitObjectPool_Cor();
	}
    IEnumerator InitFolder_Cor()
    {
        sampleFolderObj = new GameObject();
        sampleFolderObj.transform.parent = this.transform;
        sampleFolderObj.transform.localPosition = Vector3.zero;
        sampleFolderObj.name = "SampleFolder";
        sampleFolderObj.SetActive(false);

        poolList = new List<GameObject>();

        yield break;
    }
    IEnumerator InitUnit_Cor()
    {
        int _poolListCount = poolList.Count;
        int _unitDataNum = DataBase_Manager.Instance.unitDataArr.Length;
        for (int i = 0; i < _unitDataNum; i++, _poolListCount++)
        {
            GameObject _unitObj = Instantiate(DataBase_Manager.Instance.unitDataObjArr[i]);
            poolList.Add(_unitObj);
            poolList[_poolListCount].transform.SetParent(sampleFolderObj.transform);

            Unit_Data _unitData = DataBase_Manager.Instance.unitDataArr[i];

            Unit_Script _unitClass = poolList[_poolListCount].GetComponent<Unit_Script>();
            _unitClass.SetData_Func(_unitData);

            poolList[_poolListCount].name = _unitClass.charName;

            DataBase_Manager.Instance.unitClassDic.Add(i, _unitClass);
        }

        yield break;
    }
    IEnumerator InitFood_Cor()
    {
        int _poolListCount = poolList.Count;
        int _foodDataNum = DataBase_Manager.Instance.foodDataArr.Length;
        for (int i = 0; i < _foodDataNum; i++, _poolListCount++)
        {
            poolList.Add(Instantiate(DataBase_Manager.Instance.foodDataObjArr[i]));
            poolList[_poolListCount].transform.SetParent(sampleFolderObj.transform);
            Food_Script _foodClass = poolList[_poolListCount].GetComponent<Food_Script>();
            Food_Data _foodData = DataBase_Manager.Instance.foodDataArr[i];
            _foodClass.SetData_Func(_foodData);

            poolList[_poolListCount].name = _foodClass.foodName;
        }

        yield break;
    }
    IEnumerator InitMonster_Cor()
    {
        int _poolListCount = poolList.Count;
        int _monsterDataNum = DataBase_Manager.Instance.monsterDataArr.Length;
        for (int i = 0; i < _monsterDataNum; i++, _poolListCount++)
        {
            GameObject _monsterObj = Instantiate(DataBase_Manager.Instance.monsterDataObjArr[i]);
            poolList.Add(_monsterObj);
            poolList[_poolListCount].transform.SetParent(sampleFolderObj.transform);

            Unit_Data _charData = DataBase_Manager.Instance.monsterDataArr[i];

            Unit_Script _unitClass = poolList[_poolListCount].GetComponent<Unit_Script>();
            _unitClass.SetData_Func(_charData);

            poolList[_poolListCount].name = _unitClass.charName;

            DataBase_Manager.Instance.monsterClassDic.Add(i, _unitClass);
        }

        yield break;
    }
    IEnumerator InitEffect_Cor()
    {
        yield break;
    }
    IEnumerator InitSkill_Cor()
    {
        yield break;
    }
    IEnumerator InitObjectPool_Cor()
    {
        for (int i = 0; i < poolList.Count; i++)
        {
            ObjectPool objectPool = new ObjectPool();
            objectPool.source = poolList[i];
            objectPoolList[poolList[i].name] = objectPool;

            // 하이라키에 추가한다 
            GameObject folder = new GameObject();
            folder.name = poolList[i].name;
            folder.transform.parent = this.transform;
            objectPool.folder = folder;

            int amount = defaultAmount;
            if (poolAmount.Length > i && poolAmount[i] != 0)
                amount = poolAmount[i];

            for (int j = 0; j < amount; j++)
            {
                GameObject inst = Instantiate(objectPool.source);
                inst.name = poolList[i].name;
                inst.SetActive(false);
                inst.transform.SetParent(folder.transform);
                objectPool.unusedList.Add(inst);

                // 한번에 풀을 생성할때의 부하를 줄이기 위해서 코루틴을 사용한다
                // 꺼져, 느려
                //yield return null;
            }

            objectPool.maxAmount = amount;
            yield return null;
        }
    }

    public GameObject Get_Func(string name)
    {
        if(!objectPoolList.ContainsKey(name))
        {
            Debug.Log("[ObjectPoolManager] Can't Find ObjectPool! - " + name);
            return null;
        }

        ObjectPool pool = objectPoolList[name];
        if(pool.unusedList.Count > 0)
        {
            GameObject obj = pool.unusedList[0];            
            pool.unusedList.RemoveAt(0);
            obj.SetActive(true);
            return obj;
        }
        else // 사용 가능한 오브젝트가 없을때
        {
            GameObject obj = Instantiate(pool.source);            
            obj.transform.SetParent(pool.folder.transform);
            obj.name = pool.source.name;
            return obj;
        }        
    }
    public void Free_Func(GameObject obj)
    {
        string keyName = obj.name;
        if (!objectPoolList.ContainsKey(keyName))
        {
            Debug.LogError("[ObjectPoolManager] Can't Find Free ObjectPool! - " + name);
            return;
        }
        else
        {
            ObjectPool pool = objectPoolList[keyName];
            obj.SetActive(false);
            pool.unusedList.Add(obj);
            obj.transform.SetParent(pool.folder.transform);
        }
    }
}

public class ObjectPool
{
    public GameObject source;
    public int maxAmount;
    public GameObject folder;

    public List<GameObject> unusedList = new List<GameObject>();
}