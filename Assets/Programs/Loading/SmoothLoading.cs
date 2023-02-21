using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class SmoothLoading : MonoBehaviour
{
    private AsyncOperation loadOperation;
    private static int sceneToLoad;

    // UI Elements
    private UIDocument loadingUI;
    private RadialProgress radialProgress;

    // current progress
    private float currentProgress = 0f;

    // load scene settings
    [SerializeField] [Range(1, 5)] private float loadTime = 1f;
    [SerializeField] [Range(0, 2)] private float loadDelay = 1f;

    // call this function to change the scene index to load
    public static void SetNextSceneIndex(int sceneIndex) => sceneToLoad = sceneIndex;
    
    void OnEnable()
    {
        // bind UI
        loadingUI = GetComponent<UIDocument>();
        radialProgress = loadingUI.rootVisualElement.Q<RadialProgress>("radial-progress");
        radialProgress.progress = 0;
    }

    void Start()
    {
        // set initial progress
        currentProgress = 0;
        
        // start loading next scene
        loadOperation = SceneManager.LoadSceneAsync(sceneToLoad);
        loadOperation.allowSceneActivation = false;
    }

    // Update is called once per frame
    void Update()
    {
        // update radial progress bar
        radialProgress.progress = currentProgress * 100f / loadTime;

        // load next scene when target time is reached
        if (currentProgress > loadTime + loadDelay)
        {
            loadOperation.allowSceneActivation = true;
            loadingUI.enabled = false;
        }
        
        // update current progress
        currentProgress += Time.deltaTime;
    }
}
