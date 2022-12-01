using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    public static T instance;
    private static bool IsBeingDestroyed = false;

    public static T Instance
    {
        get
        {
            if (IsBeingDestroyed || !Application.isPlaying)
                return null;

            if (instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));
            }
            return instance;
        }
    }

    public virtual void Init() { }

    protected void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this as T;
        DontDestroyOnLoad(this);
        Init();
        IsBeingDestroyed = false;
    }

    public virtual void OnDestroy()
    {
        IsBeingDestroyed = true;

        if (!gameObject.scene.isLoaded) return;

        Debug.LogWarning(this.gameObject.name + " got destroyed.");
    }

}
