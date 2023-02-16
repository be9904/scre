using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(UIDocument))]
public class MainProgram : MonoBehaviour
{
    [SerializeField] private FullScreenBlurFeature blurFeature;
    [SerializeField] private GlitchFeature glitchFeature;
    [SerializeField] private SInt mainBlurStrength;

    // UI Elements
    private UIDocument mainUI;
    private Label about;
    private Label members;
    private List<Button> shaders;
    private List<Button> textures;
    private List<Button> srp;

    [SerializeField] private SText mainAbout;
    [SerializeField] private SText mainMembers;
    
    void OnEnable()
    {
        blurFeature.SetActive(true);
        glitchFeature.SetActive(true);
        blurFeature.passSettings.blurStrength = mainBlurStrength;
        
        BindUIElements();
    }

    private void OnDisable()
    {
        blurFeature.SetActive(false);
        glitchFeature.SetActive(false);
    }

    void BindUIElements()
    {
        mainUI = GetComponent<UIDocument>();
        about = mainUI.rootVisualElement.Q<Label>("about-field");
        members = mainUI.rootVisualElement.Q<Label>("members-field");
        
        // mainUI.rootVisualElement.Q<Button>("shaders-01-button").RegisterCallback<ClickEvent>(evt =>
        // {
        //     SmoothLoading.SetNextSceneIndex(2);
        // });
        // mainUI.rootVisualElement.Q<Button>("shaders-02-button").RegisterCallback<ClickEvent>(evt =>
        // {
        //     SmoothLoading.SetNextSceneIndex(3);
        // });
        // mainUI.rootVisualElement.Q<Button>("shaders-03-button").RegisterCallback<ClickEvent>(evt =>
        // {
        //     SmoothLoading.SetNextSceneIndex(4);
        //     
        // });
        
        mainUI.rootVisualElement.Q<Button>("textures-01-button").RegisterCallback<ClickEvent>(evt =>
        {  
            SmoothLoading.SetNextSceneIndex(2);
            SceneManager.LoadScene(1);
        });
        mainUI.rootVisualElement.Q<Button>("textures-02-button").RegisterCallback<ClickEvent>(evt =>
        {  
            SmoothLoading.SetNextSceneIndex(3);
            SceneManager.LoadScene(1);
        });
        mainUI.rootVisualElement.Q<Button>("srp-01-button").RegisterCallback<ClickEvent>(evt =>
        {  
            SmoothLoading.SetNextSceneIndex(4);
            SceneManager.LoadScene(1);
        });

        about.text = mainAbout.Value;
        members.text = mainMembers.Value;
    }
}