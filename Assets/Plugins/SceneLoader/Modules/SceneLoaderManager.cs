using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace ArisStudio.Utility
{
    /// <summary>
    /// Load/unload scenes easily and in many ways.
    /// Modified version of https://github.com/Home-Alone-Studios/scene-loader
    ///
    /// Manager for Scene Loader.
    /// </summary>
    [AddComponentMenu("Aris Studio/Utility/Scene Loader Manager")]
    public class SceneLoaderManager : MonoBehaviourSingleton<SceneLoaderManager>
    {
        [SerializeField]
        GameObject _preloader;

        [SerializeField]
        float _delay = 0f;

        [SerializeField, Range(0, 100)]
        float _loadingProgress = 0;

        [SerializeField]
        UnityEvent _onLoadStart;

        [SerializeField]
        UnityEvent<float> _onLoadProgress;

        [SerializeField]
        UnityEvent _onLoadComplete;

        float _totalLoadProgress;
        float _normalisedLoadProgress;
        List<AsyncOperation> _scenesToLoad = new List<AsyncOperation>();

        new void Awake()
        {
            base.Persistent = true;
            base.Awake();

            SetPreloaderActive(false);
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
        }

        void OnSceneUnloaded(Scene scene)
        {
            //Debug.Log($"Scene unloaded: {scene.name}");
        }

        void OnActiveStateChaned(Scene oldScene, Scene newScene)
        {
            //Debug.Log($"Scene changed from: {oldScene.name} to {newScene.name}");
        }

        public async Task LoadAsync(string sceneName)
        {
            //SetLoadStatus(true);
            //await LoadSceneAsyncTask(sceneName);
            //SetLoadStatus(false);

            await LoadAsync(new string[] { sceneName });
        }

        async Task LoadSceneAsyncTask(string sceneName)
        {
            SetLoadStatus(true);
            AsyncOperation loading = SceneManager.LoadSceneAsync(sceneName);
            loading.allowSceneActivation = false;

            do
            {
                await Task.Delay(100);
                _totalLoadProgress = loading.progress;
            } while (loading.progress < 0.9f);

            await Task.Delay(TimeSpan.FromSeconds(1));

            loading.allowSceneActivation = true;

            await Task.Delay(TimeSpan.FromSeconds(_delay));

            SetLoadStatus(false);
        }

        public async Task LoadAsync(string[] sceneNames)
        {
            SetLoadStatus(true);

            foreach (string sceneName in sceneNames)
            {
                AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
                operation.allowSceneActivation = false;
                _scenesToLoad.Add(operation);
            }

            do
            {
                _totalLoadProgress = 0;
                foreach (AsyncOperation operation in _scenesToLoad)
                {
                    await Task.Delay(100);
                    _totalLoadProgress += operation.progress;
                }

                _normalisedLoadProgress = _totalLoadProgress / _scenesToLoad.Count;
                _loadingProgress = _normalisedLoadProgress * 100f;

                _onLoadProgress?.Invoke(_normalisedLoadProgress);
            } while (_normalisedLoadProgress < 0.9f);

            await Task.Delay(1000);

            foreach (AsyncOperation operation in _scenesToLoad)
                operation.allowSceneActivation = true;

            await Task.Delay(1000);

            await Task.Delay(TimeSpan.FromSeconds(_delay));

            SetLoadStatus(false);
        }

        bool IsLoadedAllScenes(string[] array)
        {
            int expectedToLoadScenes = array.Length;
            int loadedScenes = _scenesToLoad.FindAll(x => x.isDone).Count;
            Debug.Log($"{loadedScenes} - {expectedToLoadScenes}");
            return loadedScenes == expectedToLoadScenes;
        }

        void SetLoadStatus(bool flag)
        {
            if (flag)
            {
                _loadingProgress = _normalisedLoadProgress = _totalLoadProgress = 0;
                _scenesToLoad.Clear();
            }

            SetPreloaderActive(flag);

            if (flag)
                _onLoadStart?.Invoke();
            else
                _onLoadComplete?.Invoke();
        }

        void SetPreloaderActive(bool flag)
        {
            if (_preloader != null)
                _preloader.SetActive(flag);
        }

        void LateUpdate()
        {
            //_loadingProgress = Mathf.MoveTowards(_loadingProgress, _totalLoadingProgress, 3 * Time.deltaTime);
        }
    }
}
