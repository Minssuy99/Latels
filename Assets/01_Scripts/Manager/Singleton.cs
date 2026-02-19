using UnityEngine;

public class SingletonBase : MonoBehaviour
{
    [Header("Singleton Setting")]
    [SerializeField] protected bool DontDestroy = true;
}

public class Singleton<T> : SingletonBase where T : MonoBehaviour
{
    protected static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<T>();

                if (_instance == null)
                {
                    _instance = new GameObject(typeof(T).Name).AddComponent<T>();
                }
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;

            if (DontDestroy)
            {
                if (transform.parent != null)
                {
                    DontDestroyOnLoad(transform.root.gameObject);
                }
                else
                {
                    DontDestroyOnLoad(gameObject);
                }
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
    protected virtual void OnDestroy()
    {
        if (_instance == this as T)
            _instance = null;
    }
}