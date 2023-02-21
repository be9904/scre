using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class RTProgram : MonoBehaviour
{
    // Reference renderer feature asset
    [SerializeField] private FullScreenRTFeature RTFeature;
    
    // Renderer feature options to tweak at runtime
    [Header("Render Feature Runtime Options")]
    [SerializeField] private SInt kernelID;

    // UI Elements
    private UIDocument      programUI;
    private Label           title;
    private Label           description;
    private DropdownField   dropdown;
    private List<string>    dropdownOptions 
        = new List<string>
        {
            "UV",
            "Simple Noise"
        };
    private Button          returnButton;
    
    [Header("UI Text Fields")]
    [SerializeField] private SText titleText;
    [SerializeField] private SText descriptionText;

    void OnEnable()
    {
        // cache main camera
        ProgramUtility.AdjustView(Camera.main);
        
        // set renderer feature active
        RTFeature.SetActive(true);
        
        // bind UI
        BindUIElements();
    }
    
    private void OnDisable()
    {
        // disable renderer feature
        RTFeature.SetActive(false);
    }

    void BindUIElements()
    {
        // bind UI elements
        programUI = GetComponent<UIDocument>();
        title = programUI.rootVisualElement.Q<Label>("Title");
        description = programUI.rootVisualElement.Q<Label>("Description");
        dropdown = programUI.rootVisualElement.Q<DropdownField>("KernelID");
        returnButton = programUI.rootVisualElement.Q<Button>("Return");

        // initial values
        dropdown.choices = dropdownOptions;
        dropdown.value = dropdownOptions[kernelID.Value];
        dropdown.index = kernelID.Value;

        // set text
        title.text = titleText.Value;
        description.text = descriptionText.Value;
        
        // register events
        dropdown.RegisterCallback<ChangeEvent<string>>(evt =>
        {
            int updateIndex = dropdownOptions.FindIndex(str => str == evt.newValue);
            kernelID.Variable.SetValue(updateIndex);
            dropdown.value = dropdownOptions[updateIndex];
            dropdown.index = updateIndex;
            RTFeature.Create();
        });

        returnButton.RegisterCallback<ClickEvent>(evt =>
        {
            RTFeature.SetActive(false);
            
            ProgramUtility.ReturnToMain();
        });
    }
}
