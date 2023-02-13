using System;
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
    private UIDocument programUI;
    private Label title;
    private Label description;
    private DropdownField dropdown;
    private Button returnButton;

    private List<string> dropdownOptions = new List<string>{"UV", "Noise"};

    // Start is called before the first frame update
    void OnEnable()
    {
        // set renderer feature active
        RTFeature.SetActive(true);
        
        // bind UI elements
        programUI = GetComponent<UIDocument>();
        title = programUI.rootVisualElement.Q<Label>("Title");
        description = programUI.rootVisualElement.Q<Label>("Description");
        dropdown = programUI.rootVisualElement.Q<DropdownField>("KernelID");
        returnButton = programUI.rootVisualElement.Q<Button>("Return");

        // initial values
        dropdown.choices = dropdownOptions;
        dropdown.value = dropdownOptions[kernelID.Variable.Value];
        dropdown.index = kernelID.Variable.Value;

        // set text
        title.text = "Full Screen Render Textures";
        description.text = 
            "Instead of adding post process effects to the texture generated from the " +
            "camera color target, this program outputs the render texture generated by " + 
            "a compute shader kernel to the camera directly.\nThe first kernel generates " +
            "basic uv coordinates and the second kernel generates a simple noise texture.";
        
        // register events
        dropdown.RegisterCallback<ChangeEvent<string>>(evt =>
        {
            // debug here ********
            int updateIndex = dropdownOptions.FindIndex(str => str == evt.newValue);
            kernelID.Variable.SetValue(updateIndex);
            dropdown.value = dropdownOptions[updateIndex];
            dropdown.index = updateIndex;
            Debug.Log("Kernel ID: " + kernelID.Variable.Value);
            RTFeature.Create();
        });

        returnButton.RegisterCallback<ClickEvent>(evt =>
        {
            RTFeature.SetActive(false);
            Debug.Log("Go to main menu");
            // return to list
        });
    }

    private void OnDisable()
    {
        // disable renderer feature
        RTFeature.SetActive(false);
    }
}
