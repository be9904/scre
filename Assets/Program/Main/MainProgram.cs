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

        about.text = mainAbout.Value;
        members.text = mainMembers.Value;
    }
}
