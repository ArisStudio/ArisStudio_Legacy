using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace ArisStudio.Utils
{
    /// <summary>
    /// Scene Loader to be attached to a Game Object.
    /// </summary>
    [AddComponentMenu("Aris Studio/Utility/Scene Loader")]
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField]
        SerializableScene m_SerializableScene;

        [SerializeField]
        LoadSceneMode m_LoadSceneMode;

        [SerializeField]
        bool m_PreloadOnStart;

        [SerializeField]
        bool m_UseSceneManager;

        [Header("Events")]
        [SerializeField]
        UnityEvent m_OnSceneLoaded;

        // [SerializeField]
        // UnityEvent _onSceneActivated;

        [SerializeField]
        UnityEvent m_OnSceneLoading;

        [SerializeField]
        UnityEvent m_OnSceneUnloaded;

        [SerializeField]
        AsyncOperation _asyncLoadOperation = null;
        AsyncOperation _asyncUnloadOperation = null;
        bool _isListeningForLoadCompletedEvent = false;
        bool _isListeningForUnloadCompletedEvent = false;

        WaitForEndOfFrame _waitforEndOfFrame = new WaitForEndOfFrame();

        public float currentLoadingProgress { get; private set; }

        IEnumerator Start()
        {
            yield return _waitforEndOfFrame;

            if (m_PreloadOnStart)
                PreLoadAsync();
        }

        void OnEnable()
        {
            SceneManager.sceneUnloaded += OnSceneUnloaded;
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.activeSceneChanged += OnActiveStateChaned;
        }

        void OnDisable()
        {
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.activeSceneChanged -= OnActiveStateChaned;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            //Debug.Log($"Scene loaded: {scene.name}, {(LoadSceneMode)mode}");

            if (scene == null)
                return;

            if (scene.name == m_SerializableScene.SceneName)
                m_OnSceneLoaded?.Invoke();
        }

        void OnSceneUnloaded(Scene scene)
        {
            if (scene == null)
                return;

            //Debug.Log($"Scene unloaded: {scene.name}");

            if (scene.name == m_SerializableScene.SceneName)
                m_OnSceneUnloaded?.Invoke();
        }

        void OnActiveStateChaned(Scene oldScene, Scene newScene)
        {
            //Debug.Log($"Scene changed from: {oldScene.name} to {newScene.name}");
        }

        void Load(LoadSceneMode loadSceneMode)
        {
            if (m_SerializableScene != null && m_SerializableScene.IsValid())
                LoadScene(m_SerializableScene.SceneName, loadSceneMode);
        }

        Task<AsyncOperation> LoadAsync(
            LoadSceneMode loadSceneMode,
            bool allowSceneActivation = true
        )
        {
            if (m_SerializableScene != null && m_SerializableScene.IsValid())
            {
                return LoadSceneAsync(
                    m_SerializableScene.SceneName,
                    loadSceneMode,
                    allowSceneActivation
                );
            }

            return null;
        }

        Scene GetSceneIfActive()
        {
            Scene scene = new Scene();

            if (m_SerializableScene != null)
                scene = SceneManager.GetSceneByName(m_SerializableScene.SceneName);

            return scene;
        }

        public void SetActive()
        {
            Scene scene = GetSceneIfActive();

            if (!scene.IsValid())
                throw new Exception($"Scene is invalid!");

            SceneManager.SetActiveScene(scene);
        }

        public void Load()
        {
            Load(m_LoadSceneMode);
        }

        public async void LoadAsync()
        {
            if (m_UseSceneManager)
                await SceneLoaderManager.Instance.LoadAsync(m_SerializableScene.SceneName);
            else
            {
                Debug.Log($"Async Load Operation is null = {_asyncLoadOperation == null}");

                if (_asyncLoadOperation == null)
                    await LoadAsync(m_LoadSceneMode);
                else
                {
                    if (!_asyncLoadOperation.isDone)
                        m_OnSceneLoading?.Invoke();

                    while (!_asyncLoadOperation.isDone)
                        currentLoadingProgress = _asyncLoadOperation.progress;

                    if (_asyncLoadOperation.progress >= 0.9f)
                    {
                        _asyncLoadOperation.allowSceneActivation = true;
                        DisableOnAsyncLoadCompletedListener();
                    }
                }
            }
        }

        public async void PreLoadAsync()
        {
            if (_asyncLoadOperation == null)
                await LoadAsync(m_LoadSceneMode, false);
        }

        public void RestartCurrentScene()
        {
            LoadScene(SceneManager.GetActiveScene().name);
        }

        public void UnloadCurrentScene()
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
        }

        public void LoadScene(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
        {
            SceneManager.LoadScene(sceneName, loadSceneMode);
        }

        public Task<AsyncOperation> LoadSceneAsync(
            string sceneName,
            LoadSceneMode loadSceneMode = LoadSceneMode.Single,
            bool allowSceneActivation = true
        )
        {
            DisableOnAsyncLoadCompletedListener();
            // Begin to load the Scene you have specified.
            _asyncLoadOperation = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
            if (!_asyncLoadOperation.isDone)
                m_OnSceneLoading?.Invoke();
            while (!_asyncLoadOperation.isDone)
                currentLoadingProgress = _asyncLoadOperation.progress;
            // Decide whether to let the scene activate until it loaded.
            _asyncLoadOperation.allowSceneActivation = allowSceneActivation;
            _asyncLoadOperation.completed += OnLoadCompleted;
            _isListeningForLoadCompletedEvent = true;
            // return _asyncLoadOperation;
            return Task.FromResult(_asyncLoadOperation);
        }

        void OnLoadCompleted(AsyncOperation asyncOperation)
        {
            _asyncLoadOperation = null;
            m_OnSceneLoaded?.Invoke();
        }

        void DisableOnAsyncLoadCompletedListener()
        {
            if (_isListeningForLoadCompletedEvent)
            {
                if (_asyncLoadOperation != null)
                {
                    _asyncLoadOperation.completed -= OnLoadCompleted;
                    _asyncLoadOperation = null;
                }

                _isListeningForLoadCompletedEvent = false;
            }
        }

        public void Unload()
        {
            SceneManager.UnloadSceneAsync(m_SerializableScene.SceneName);
        }

        public async void UnloadAsync()
        {
            if (_asyncUnloadOperation == null)
                await UnloadSceneAsync(m_SerializableScene.SceneName);
        }

        public Task<AsyncOperation> UnloadSceneAsync(string sceneName)
        {
            DisableOnAsyncUnloadCompletedListener();
            _asyncUnloadOperation = SceneManager.UnloadSceneAsync(
                sceneName,
                UnloadSceneOptions.None
            );
            _asyncUnloadOperation.completed += OnAsyncUnloadCompleted;
            _isListeningForUnloadCompletedEvent = true;
            // return _asyncUnloadOperation;
            return Task.FromResult(_asyncUnloadOperation);
        }

        void OnAsyncUnloadCompleted(AsyncOperation asyncOperation)
        {
            _asyncUnloadOperation = null;
            m_OnSceneUnloaded?.Invoke();
        }

        void DisableOnAsyncUnloadCompletedListener()
        {
            if (_isListeningForUnloadCompletedEvent)
            {
                if (_asyncUnloadOperation != null)
                {
                    _asyncUnloadOperation.completed -= OnAsyncUnloadCompleted;
                    _asyncUnloadOperation = null;
                }

                _isListeningForUnloadCompletedEvent = false;
            }
        }

        void OnDestroy()
        {
            DisableOnAsyncLoadCompletedListener();
            DisableOnAsyncUnloadCompletedListener();
        }

        //public IEnumerator _TestLoadSceneAsync(LoadSceneMode mode = LoadSceneMode.Single)
        //{
        //    yield return null;

        //    AsyncOperation asyncOp = SceneManager.LoadSceneAsync(SceneName, mode);

        //    yield return new WaitForSeconds(5f);

        //    yield return asyncOp;
        //}
    }
}
