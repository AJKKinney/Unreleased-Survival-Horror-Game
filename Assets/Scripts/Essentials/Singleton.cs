using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AustenKinney.Essentials
{
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        [SerializeField] internal bool sceneOnly = false;

        private static T instance;
        private static bool isBeingDestroyed = false;

        public static T Instance
        {
            get
            {
                if (isBeingDestroyed || !Application.isPlaying)
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

            if (sceneOnly == false)
            {
                if (transform.parent == null)
                {
                    DontDestroyOnLoad(this);
                }
                else
                {
                    DontDestroyOnLoad(transform.parent);
                }
            }

            Init();
            isBeingDestroyed = false;
        }

        public virtual void OnDestroy()
        {
            //isBeingDestroyed = true;

            if (!gameObject.scene.isLoaded) return;

            Debug.LogWarning(this.gameObject.name + " was destroyed.");
        }

    }
}
