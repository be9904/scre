using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class BlurProgram : MonoBehaviour
{
    // Reference renderer feature asset
    [SerializeField] private FullScreenBlurFeature blurFeature;

    // UI Elements
    private UIDocument programUI;
    private Label title;
    private Label description;
    private SliderInt sliderUI;
    private Button resetButton;
    private Button returnButton;
    
    // Start is called before the first frame update
    void OnEnable()
    {
        // bind UI elements
        programUI = GetComponent<UIDocument>();
        title = programUI.rootVisualElement.Q<Label>("Title");
        description = programUI.rootVisualElement.Q<Label>("Description");
        sliderUI = programUI.rootVisualElement.Q<SliderInt>("BlurStrength");
        resetButton = programUI.rootVisualElement.Q<Button>("Reset");
        returnButton = programUI.rootVisualElement.Q<Button>("Return");

        // initial values
        blurFeature.passSettings.blurStrength = 5;
        sliderUI.value = 5;

        // set text
        title.text = "Full Screen Box Blur";
        description.text = 
            "A full screen box blur effect implemented with URP custom renderer feature. " +
            "The blit material performs vertical blur and horizontal blur respectively.";
        
        // register events
        sliderUI.RegisterCallback<ChangeEvent<int>>(evt =>
        {
            blurFeature.passSettings.blurStrength = evt.newValue;
            blurFeature.Create();
        });
        
        resetButton.RegisterCallback<ClickEvent>(evt =>
        {
            blurFeature.passSettings.blurStrength = 0;
            blurFeature.Create();
            sliderUI.value = 0;
        });
        
        returnButton.RegisterCallback<ClickEvent>(evt =>
        {
            Debug.Log("Go to main menu");
            // return to list
        });
    }

    private void OnDisable()
    {
        blurFeature.passSettings.blurStrength = 5;
        blurFeature.Create();
        sliderUI.value = 5;
    }
}
