using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class BlurProgram : MonoBehaviour
{
    [SerializeField] private FullScreenBlurFeature blurFeature;

    private UIDocument programUI;
    private Label title;
    private Label description;
    private SliderInt sliderUI;
    private Button button;
    
    // Start is called before the first frame update
    void OnEnable()
    {
        // bind UI elements
        programUI = GetComponent<UIDocument>();
        title = programUI.rootVisualElement.Q<Label>("Title");
        description = programUI.rootVisualElement.Q<Label>("Description");
        sliderUI = programUI.rootVisualElement.Q<SliderInt>("BlurStrength");
        button = programUI.rootVisualElement.Q<Button>("Reset");

        // initial values
        blurFeature.passSettings.blurStrength = 0;
        sliderUI.value = 0;

        // set text
        title.text = "Full Screen Box Blur";
        description.text = 
            "A full screen box blur effect implemented with a custom renderer feature. " +
            "The blit material performs vertical blur and horizontal blur respectively.";
        
        // register events
        button.RegisterCallback<ClickEvent>(evt =>
        {
            blurFeature.passSettings.blurStrength = 0;
            blurFeature.Create();
            sliderUI.value = 0;
        });
        
        sliderUI.RegisterCallback<ChangeEvent<int>>(evt =>
        {
            blurFeature.passSettings.blurStrength = evt.newValue;
            blurFeature.Create();
        });
    }
}
