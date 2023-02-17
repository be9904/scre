using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class BlurProgram : MonoBehaviour
{
    // Reference renderer feature asset
    [SerializeField] private FullScreenBlurFeature blurFeature;
    
    // Renderer feature options to tweak at runtime
    [Header("Render Feature Runtime Options")]
    [SerializeField] private SInt blurStrength;

    // UI Elements
    private UIDocument      programUI;
    private Label           title;
    private Label           description;
    private SliderInt       slider;
    private Button          resetButton;
    private Button          returnButton;

    [Header("UI Text Fields")]
    [SerializeField] private SText titleText;
    [SerializeField] private SText descriptionText;
    
    void OnEnable()
    {
        Debug.Log(blurStrength.Variable.Value);
        // set renderer feature active
        blurFeature.SetActive(true);
        blurFeature.passSettings.blurStrength = blurStrength;
        blurFeature.Create();
        
        // bind UI elements
        BindUIElements();
    }
    
    private void OnDisable()
    {
        // disable renderer feature
        blurFeature.SetActive(false);
    }

    void BindUIElements()
    {
        // bind UI elements
        programUI = GetComponent<UIDocument>();
        title = programUI.rootVisualElement.Q<Label>("Title");
        description = programUI.rootVisualElement.Q<Label>("Description");
        slider = programUI.rootVisualElement.Q<SliderInt>("BlurStrength");
        resetButton = programUI.rootVisualElement.Q<Button>("Reset");
        returnButton = programUI.rootVisualElement.Q<Button>("Return");

        // initial values
        slider.value = blurStrength.Value;
        Debug.Log("Slider: " + slider.value);

        // set text
        title.text = titleText.Value;
        description.text = descriptionText.Value;
        
        // register events
        slider.RegisterCallback<ChangeEvent<int>>(evt =>
        {
            blurStrength.Variable.SetValue(evt.newValue);
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
            
            ProgramUtility.ReturnToMain();
        });
    }

    private void ResetSettings()
    {
        blurStrength.Variable.SetValue(0);
        blurFeature.Create();
        slider.value = 0;
    }
}
