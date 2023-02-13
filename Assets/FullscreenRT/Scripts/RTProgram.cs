using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class RTProgram : MonoBehaviour
{
    // Reference renderer feature asset
    [SerializeField] private FullScreenRTFeature RTFeature;
    
    // Renderer feature options to tweak at runtime
    [SerializeField] private SInt kernelID;

    // UI Elements
    private UIDocument      programUI;
    private Label           title;
    private Label           description;
    private DropdownField   dropdown;
    private List<string>    dropdownOptions = new List<string>{"Noise", "UV"};
    private Button          returnButton;

    void OnEnable()
    {
        // cache main camera
        ProgramUtility.AlignView(Camera.main);
        
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
        title.text = "Full Screen Render Textures";
        description.text = 
            "This scene was made to understand the blit process in URP.\n\n" +
            "Full screen effects can be implemented by blitting from the camera color target " +
            "to a temporary render texture,\n\nand blitting the temporary texture back to " + 
            "the camera color target." + 
            "\n\nThe first kernel generates basic uv coordinates and the second kernel generates " +
            "a simple noise texture.\n\n";
        
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
            
            Debug.Log("Go to main menu");
            // return to list
        });
    }
}
