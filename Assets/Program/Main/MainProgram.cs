using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class MainProgram : MonoBehaviour
{
    [SerializeField] private FullScreenBlurFeature blurFeature;
    [SerializeField] private GlitchFeature glitchFeature;
    [SerializeField] private SInt mainBlurStrength;
    
    private UIDocument mainUI;
    private VisualElement glitchBackground;
    
    void OnEnable()
    {
        blurFeature.SetActive(true);
        glitchFeature.SetActive(true);
        blurFeature.passSettings.blurStrength = mainBlurStrength;
        
        mainUI = GetComponent<UIDocument>();
        glitchBackground = mainUI.rootVisualElement.Q<VisualElement>("glitch-background");
    }

    private void OnDisable()
    {
        blurFeature.SetActive(false);
        glitchFeature.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
