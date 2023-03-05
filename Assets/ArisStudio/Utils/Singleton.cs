using System;
using UnityEngine;

namespace ArisStudio.Utils
{
    /// <summary>
    /// Generic singleton base class for global, single-instance MonoBehaviours.
    /// </summary>
    /// <typeparam name="T">The class type to define</typeparam>
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();

                    if (instance == null)
                    {
                        instance = new GameObject(
                            $"{typeof(T).Name} [Singleton]",
                            typeof(T)
                        ).GetComponent<T>();
                    }
                }

                DontDestroyOnLoad(instance.gameObject);
                return instance;
            }
        }

        protected virtual void OnInitialize()
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();

                if (instance == null)
                    instance = this as T;

                DontDestroyOnLoad(instance.gameObject);
            }
            else if (instance != this)
                Destroy(instance.gameObject);
        }

        void Awake()
        {
            OnInitialize();
        }

        // Clear the instance field when destroyed.
        protected virtual void OnDestroy() => instance = null;
    }

    /*
    * Taken from:
    * https://blog.mzikmund.com/2019/01/a-modern-singleton-in-unity/
    */
    // public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    // {
    //     private static readonly Lazy<T> LazyInstance = new Lazy<T>(CreateSingleton);

    //     public static T Instance => LazyInstance.Value;

    //     private static T instance;

    //     private static T CreateSingleton()
    //     {
    //         if (instance == null)
    //         {
    //             // Search for an instance of the singleton in the scene.
    //             instance = FindObjectOfType<T>();

    //             // If no instance was found...
    //             if (instance == null)
    //             {
    //                 // create a new one
    //                 // instance = new GameObject(typeof(T).Name, typeof(T)).GetComponent<T>();
    //                 GameObject obj = new GameObject($"{typeof(T).Name} (singleton)");
    //                 instance = obj.AddComponent<T>();
    //             }
    //         }

    //         DontDestroyOnLoad(instance.gameObject);
    //         return instance;
    //     }

    // protected virtual void Awake()
    // {
    //     if (instance == null)
    //     {
    //         instance = this as T;
    //         DontDestroyOnLoad(this.gameObject);
    //     }
    //     else
    //         Destroy(gameObject);
    // }

    // Clear the instance field when destroyed.
    //     protected virtual void OnDestroy() => instance = null;
    // }
}
