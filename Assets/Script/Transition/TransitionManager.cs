using System.Collections;
using System.Collections.Generic;
//using Save;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Transition
{
    public class TransitionManager : Singleton<TransitionManager>//, ISaveable
    {
        //[SceneName]
        public string startSceneName = string.Empty;
        private CanvasGroup fadeCanvasGroup;
        private bool isFade;

        // public string GUID => GetComponent<DataGUID>().guid;

        // protected override void Awake()
        // {
        //     base.Awake();
        //     Screen.SetResolution(1920, 1080, FullScreenMode.Windowed, 0);
        //     SceneManager.LoadScene("UI", LoadSceneMode.Additive);
        // }

        private void OnEnable()
        {
            EventHandler.TransitionEvent += OnTransitionEvent;
            // EventHandler.StartNewGameEvent += OnStartNewGameEvent;
            // EventHandler.EndGameEvent += OnEndGameEvent;
        }

        private void OnDisable()
        {
            EventHandler.TransitionEvent -= OnTransitionEvent;
            // EventHandler.StartNewGameEvent -= OnStartNewGameEvent;
            // EventHandler.EndGameEvent -= OnEndGameEvent;
        }

        // private void OnEndGameEvent()
        // {
        //     StartCoroutine(UnloadScene());
        // }

        // private void OnStartNewGameEvent(int obj)
        // {
        //     StartCoroutine(LoadSaveDataScene(startSceneName));
        // }


        // private void Start()
        // {
        //     // ISaveable saveable = this;
        //     // saveable.RegisterSaveable();

        //     fadeCanvasGroup = FindObjectOfType<CanvasGroup>();

        //     StartCoroutine(LoadSceneSetActive(startSceneName));
        // }

     private IEnumerator Start()
        {
            // ISaveable saveable = this;
            // saveable.RegisterSaveable();

            fadeCanvasGroup = FindObjectOfType<CanvasGroup>();
            yield return LoadSceneSetActive(startSceneName);
            EventHandler.CallAfterSceneLoadedEvent();
            //StartCoroutine(LoadSceneSetActive(startSceneName));
        }

        private void OnTransitionEvent(string sceneToGo, Vector3 positionToGo)
        {
            if (!isFade)
                StartCoroutine(Transition(sceneToGo, positionToGo));
        }


        // 场景切换,"sceneName"目标场景,"targetPosition"目标位置
        private IEnumerator Transition(string sceneName, Vector3 targetPosition)
        {
             EventHandler.CallBeforeSceneUnloadEvent();
             yield return Fade(1);

            yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

            yield return LoadSceneSetActive(sceneName);
             //移动人物坐标
             EventHandler.CallMoveToPosition(targetPosition);
             EventHandler.CallAfterSceneLoadedEvent();
             yield return Fade(0);
        }

        // 加载场景并设置为激活,"sceneName"场景名
        private IEnumerator LoadSceneSetActive(string sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);

            SceneManager.SetActiveScene(newScene);
        }


        // 淡入淡出场景,targetAlpha中，1是黑，0是透明
        private IEnumerator Fade(float targetAlpha)
        {
            isFade = true;

            fadeCanvasGroup.blocksRaycasts = true;

            float speed = Mathf.Abs(fadeCanvasGroup.alpha - targetAlpha) / Settings.fadeDuration;

            while (!Mathf.Approximately(fadeCanvasGroup.alpha, targetAlpha))
            {
                fadeCanvasGroup.alpha = Mathf.MoveTowards(fadeCanvasGroup.alpha, targetAlpha, speed * Time.deltaTime);
                yield return null;
            }

            fadeCanvasGroup.blocksRaycasts = false;

            isFade = false;
        }


    //     private IEnumerator LoadSaveDataScene(string sceneName)
    //     {
    //         yield return Fade(1f);

    //         if (SceneManager.GetActiveScene().name != "PersistentScene")    //在游戏过程中 加载另外游戏进度
    //         {
    //             EventHandler.CallBeforeSceneUnloadEvent();
    //             yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    //         }

    //         yield return LoadSceneSetActive(sceneName);
    //         EventHandler.CallAfterSceneLoadedEvent();
    //         yield return Fade(0);
    //     }


    //     private IEnumerator UnloadScene()
    //     {
    //         EventHandler.CallBeforeSceneUnloadEvent();
    //         yield return Fade(1f);
    //         yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    //         yield return Fade(0);
    //     }



    //     public GameSaveData GenerateSaveData()
    //     {
    //         GameSaveData saveData = new GameSaveData();
    //         saveData.dataSceneName = SceneManager.GetActiveScene().name;

    //         return saveData;
    //     }

    //     public void RestoreData(GameSaveData saveData)
    //     {
    //         //加载游戏进度场景
    //         StartCoroutine(LoadSaveDataScene(saveData.dataSceneName));
    //     }
     }
}