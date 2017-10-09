using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _Instance;
	protected static bool isSingletonLoaded = false;
    private static bool applicationIsQuitting = false;

    private static object _lock = new object();
 
	public static T Instance
	{
		get
		{
            if (isSingletonLoaded == false)
            {
                Debug.Log("[Singleton] : " + typeof(T) + " 접근, 아직 생성되지 않은 싱글턴에 접근하였습니다.");

                SetGenerate_Func();
            }

            return _Instance;
        }
	}

	void OnDestroy ()
	{
		applicationIsQuitting = true;
	}

    public static bool SetGenerate_Func()
    {
        if (applicationIsQuitting)
        {
            Debug.LogWarning("[Singleton]" + typeof(T) + "은(는) 파괴되었습니다.");
        }

        lock (_lock) //멀티스레드 대비
        {
            if (_Instance == null)
            {
                _Instance = (T)FindObjectOfType(typeof(T));

                if (1 < FindObjectsOfType(typeof(T)).Length)
                {
                    Debug.LogError("Bug : " + typeof(T) + " 싱글턴이 이미 존재합니다.");
                }
                else
                {
                    isSingletonLoaded = true;

                    GameObject singleton = new GameObject();
                    _Instance = singleton.AddComponent<T>();
                    singleton.name = "(singleton)" + typeof(T).ToString();

                    DontDestroyOnLoad(singleton);

                    Debug.Log("[Singleton]" + typeof(T) + " 생성, 객체이름은 " + singleton);

                    return true;
                }
            }
        }

        return false;
    }
}