using UnityEngine;

namespace ArisStudio.Utility
{
    /// <summary>
    /// Generic singleton base class for global, single-instance MonoBehaviours.
    /// </summary>
    /// <typeparam name="T">The class type to define</typeparam>
    public abstract class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        // Storage for the singleton instance.
        protected static T instance;

        // Property that either creates or returns the existing singleton instance.
        public static T Instance
        {
            get
            {
                // Check if the instance has already been set.
                if (instance != null)
                    return instance;

                // Search for an instance of the singleton in the scene.
                instance = FindObjectOfType<T>();
                if (instance != null)
                    return instance;

                // If no instance was found, create a new game object and add the singleton component to it.
                instance = new GameObject(typeof(T).Name, typeof(T)).GetComponent<T>();
                return instance;
            }
        }

        // Property that determines whether the singleton should be destroyed when the scene is unloaded.
        protected bool Persistent { get; set; }

        // Initialize the singleton instance.
        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
                MakePersistent();
            }
            else
            {
                MakePersistent();
                return;
                // Destroy(instance.gameObject);
            }
        }

        void MakePersistent()
        {
            if (Persistent)
            {
                // Suppress warning 'consider using SetParent method instead of parent property'
                // if game object has Rect Transform.
                if (instance.TryGetComponent<RectTransform>(out RectTransform rectTransform))
                    rectTransform.SetParent(null, false);
                // suppress warning message when singleton is a child of a game object
                else if (instance.TryGetComponent<Transform>(out Transform instanceTransform))
                    instanceTransform.parent = null;

                DontDestroyOnLoad(instance.gameObject);
            }
        }

        // Clear the instance field when destroyed.
        protected virtual void OnDestroy() => instance = null;
    }
}
