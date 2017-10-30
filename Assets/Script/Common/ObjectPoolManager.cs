using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 오브젝트 풀 매니저 싱글톤
/// </summary>
public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance;

    public int defaultAmount = 10;
    public List<GameObject> poolList;
    public int[] poolAmount;

    Dictionary<int, Unit_Script> unitDic = new Dictionary<int, Unit_Script>();
    Dictionary<string, ObjectPool> objectPoolList = new Dictionary<string, ObjectPool>();

    public IEnumerator Init_Cor()
    {
        Instance = this;

        SetPoolingObj_Func();

        yield return InitObjectPool_Cor();
	}
    void SetPoolingObj_Func()
    {
        GameObject _sampleFolderObj = new GameObject();
        _sampleFolderObj.transform.parent = this.transform;
        _sampleFolderObj.transform.localPosition = Vector3.zero;
        _sampleFolderObj.name = "SampleFolder";
        _sampleFolderObj.SetActive(false);

        poolList = new List<GameObject>();

        int _poolListCount = poolList.Count;
        int _charDataNum = DataBase_Manager.Instance.unitDataArr.Length;
        for (int i = 0; i < _charDataNum; i++, _poolListCount++)
        {
            poolList.Add(Instantiate(DataBase_Manager.Instance.unitDataObjArr[i]));
            poolList[_poolListCount].transform.SetParent(_sampleFolderObj.transform);

            Unit_Data _charData = DataBase_Manager.Instance.unitDataArr[i];

            Unit_Script _unitClass = poolList[_poolListCount].GetComponent<Unit_Script>();
            _unitClass.SetData_Func(_charData);

            poolList[_poolListCount].name = _unitClass.charName;

            unitDic.Add(i, _unitClass);
        }
        
        int _foodDataNum = DataBase_Manager.Instance.foodDataArr.Length;
        for (int i = 0; i < _foodDataNum; i++, _poolListCount++)
        {
            poolList.Add(Instantiate(DataBase_Manager.Instance.foodDataObjArr[i]));
            poolList[_poolListCount].transform.SetParent(_sampleFolderObj.transform);
            Food_Script _foodClass = poolList[_poolListCount].GetComponent<Food_Script>();
            Food_Data _foodData = DataBase_Manager.Instance.foodDataArr[i];
            _foodClass.SetData_Func(_foodData);

            poolList[_poolListCount].name = _foodClass.foodName;
        }
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
    public Unit_Script GetUnitClass_Func(int _unitID)
    {
        Unit_Script _unitClass = null;

        if (unitDic.TryGetValue(_unitID, out _unitClass) == false)
        {
            Debug.LogError("Bug : 유닛 ID가 설정치를 벗어났슴다");
        }

        return _unitClass;
    }
}

public class ObjectPool
{
    public GameObject source;
    public int maxAmount;
    public GameObject folder;

    public List<GameObject> unusedList = new List<GameObject>();
}