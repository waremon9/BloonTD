using UnityEngine;

public abstract class MySingleton<T> : MonoBehaviour where T : MySingleton<T>
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                new GameObject($"{typeof(T).Name} (created)").AddComponent<T>();
            }

            return _instance;
        }
    }

    public abstract bool DoDestroyOnLoad { get; }


    protected virtual void Awake()
    {
        if (_instance == null)
        {
            if (!DoDestroyOnLoad)
            {
                DontDestroyOnLoad(this);
            }
            _instance = (T)this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}