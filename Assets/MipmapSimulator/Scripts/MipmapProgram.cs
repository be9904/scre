using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class MipmapProgram : MonoBehaviour
{
    // Reference renderer feature asset
    [SerializeField] private MipFeature mipFeature;
    [SerializeField] private ComputeShader computeShader;
    
    // Renderer feature options to tweak at runtime
    [SerializeField] private SInt mipLevel;

    // UI Elements
    private UIDocument      programUI;
    private Label           title;
    private Label           description;
    private SliderInt       slider;
    private Button          resetButton;
    private Button          returnButton;
    
    [SerializeField] private SText titleText;
    [SerializeField] private SText descriptionText;

    void OnEnable()
    {
        // cache main camera
        ProgramUtility.AlignView(Camera.main);
        
        // set renderer feature active
        mipFeature.SetActive(true);

        // bind UI
        BindUIElements();
    }
    
    private void OnDisable()
    {
        // disable renderer feature
        mipFeature.SetActive(false);
    }

    void BindUIElements()
    {
        // bind UI elements
        programUI = GetComponent<UIDocument>();
        title = programUI.rootVisualElement.Q<Label>("Title");
        description = programUI.rootVisualElement.Q<Label>("Description");
        slider = programUI.rootVisualElement.Q<SliderInt>("Miplevel");
        resetButton = programUI.rootVisualElement.Q<Button>("Reset");
        returnButton = programUI.rootVisualElement.Q<Button>("Return");

        // initial values
        slider.value = mipLevel.Value;

        // set text
        title.text = titleText.Value;
        description.text = descriptionText.Value;
        
        // register events
        slider.RegisterCallback<ChangeEvent<int>>(evt =>
        {
            mipLevel.Variable.SetValue(evt.newValue);
            mipFeature.Create();
        });
        
        resetButton.RegisterCallback<ClickEvent>(evt =>
        {
            ResetSettings();
        });

        returnButton.RegisterCallback<ClickEvent>(evt =>
        {
            mipFeature.SetActive(false);
            
            Debug.Log("Go to main menu");
            // return to list
        });
    }
    
    private void ResetSettings()
    {
        mipLevel.Variable.SetValue(0);
        mipFeature.Create();
        slider.value = 0;
    }
}
