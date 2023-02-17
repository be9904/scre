using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class MipmapProgram : MonoBehaviour
{
    // Reference renderer feature asset
    [SerializeField] private MipFeature mipFeature;

    // Renderer feature options to tweak at runtime
    [Header("Render Feature Runtime Options")]
    [SerializeField] private SInt mipLevel;
    [SerializeField] private SInt imageIndex;
    [SerializeField] private List<STexture2D> images;

    // UI Elements
    private UIDocument      programUI;
    private Label           title;
    private Label           description;
    private SliderInt       slider;
    private DropdownField   dropdown;
    private List<string>    dropdownOptions;
    private Button          resetButton;
    private Button          returnButton;
    
    [Header("UI Text Fields")]
    [SerializeField] private SText titleText;
    [SerializeField] private SText descriptionText;

    void OnEnable()
    {
        // cache main camera
        ProgramUtility.AdjustView(Camera.main);
        
        // set renderer feature active
        mipFeature.SetActive(true);

        // bind UI
        BindUIElements();

        // initial run to match image index
        mipFeature.passSettings.inputTexture = images[imageIndex];
        mipFeature.Create();
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
        dropdown = programUI.rootVisualElement.Q<DropdownField>("ImageOption");
        resetButton = programUI.rootVisualElement.Q<Button>("Reset");
        returnButton = programUI.rootVisualElement.Q<Button>("Return");

        // initial values
        slider.value = mipLevel.Value;
        dropdownOptions = new List<string>();
        foreach (STexture2D tex in images)
        {
            dropdownOptions.Add(tex.Variable.texture.name);
        }
        dropdown.choices = dropdownOptions;
        dropdown.value = dropdownOptions[imageIndex];
        dropdown.index = imageIndex;

        // set text
        title.text = titleText.Value;
        description.text = descriptionText.Value;
        
        // register events
        slider.RegisterCallback<ChangeEvent<int>>(evt =>
        {
            mipLevel.Variable.SetValue(evt.newValue);
            mipFeature.Create();
        });
        
        dropdown.RegisterCallback<ChangeEvent<string>>(evt =>
        {
            // get changed index
            int updateIndex = dropdownOptions.FindIndex(str => str == evt.newValue);
            
            // update image index
            imageIndex.Variable.SetValue(updateIndex);
            
            // update dropdown option
            dropdown.value = dropdownOptions[updateIndex];
            dropdown.index = updateIndex;
            
            // update and execute render pass
            mipFeature.passSettings.inputTexture = images[updateIndex];
            mipFeature.Create();
        });
        
        resetButton.RegisterCallback<ClickEvent>(evt =>
        {
            ResetSettings();
        });

        returnButton.RegisterCallback<ClickEvent>(evt =>
        {
            mipFeature.SetActive(false);
            
            ProgramUtility.ReturnToMain();
        });
    }
    
    private void ResetSettings()
    {
        // reset variables
        mipLevel.Variable.SetValue(0);
        imageIndex.Variable.SetValue(0);
        
        // execute pass
        mipFeature.Create();
        
        // reset UI elements
        slider.value = 0;
        dropdown.value = dropdownOptions[imageIndex];
        dropdown.index = imageIndex;
    }
}
