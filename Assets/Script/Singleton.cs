using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance = null;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                if (FindObjectOfType<T>() != null)
                    _instance = FindObjectOfType<T>();
                else
                    new GameObject().AddComponent<T>().name = "Singleton_" + typeof(T).ToString();
            }
            return _instance;
        }
        protected set => _instance = value;
    }

    protected virtual void Awake()
    {
        if (_instance != null && _instance.gameObject.GetInstanceID() != gameObject.GetInstanceID())
        {
            Debug.LogError("Singleton already exist " + _instance.name);
            Destroy(gameObject);
        }
        else
            _instance = this.GetComponent<T>();
    }
}
