using System;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class BlurProgram : MonoBehaviour
{
    // Reference renderer feature asset
    [SerializeField] private FullScreenBlurFeature blurFeature;
    
    // Renderer feature options to tweak at runtime
    [SerializeField] private SInt blurStrength;

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
        // set renderer feature active
        blurFeature.SetActive(true);
        
        // bind UI elements
        programUI = GetComponent<UIDocument>();
        title = programUI.rootVisualElement.Q<Label>("Title");
        description = programUI.rootVisualElement.Q<Label>("Description");
        sliderUI = programUI.rootVisualElement.Q<SliderInt>("BlurStrength");
        resetButton = programUI.rootVisualElement.Q<Button>("Reset");
        returnButton = programUI.rootVisualElement.Q<Button>("Return");

        // initial values
        sliderUI.value = blurStrength.Variable.Value;

        // set text
        title.text = "Full Screen Box Blur";
        description.text = 
            "A full screen box blur effect implemented with URP custom renderer feature. " +
            "The blit material performs vertical blur and horizontal blur respectively.";
        
        // register events
        sliderUI.RegisterCallback<ChangeEvent<int>>(evt =>
        {
            blurStrength.Variable.SetValue(evt.newValue);
            Debug.Log("Slider: " + blurStrength.Variable.Value);
            blurFeature.Create();
        });
        
        resetButton.RegisterCallback<ClickEvent>(evt =>
        {
            ResetSettings();
        });
        
        returnButton.RegisterCallback<ClickEvent>(evt =>
        {
            // disable renderer feature
            blurFeature.SetActive(false);
            
            Debug.Log("Go to main menu");
            // return to list
        });
    }

    private void OnDisable()
    {
        // disable renderer feature
        blurFeature.SetActive(false);
    }

    private void ResetSettings()
    {
        blurStrength.Variable.SetValue(0);
        blurFeature.Create();
        sliderUI.value = 0;
    }
}
