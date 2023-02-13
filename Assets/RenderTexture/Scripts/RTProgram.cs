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
    private SliderInt sliderUI;
    private Button returnButton;

    // Start is called before the first frame update
    void OnEnable()
    {
        // set renderer feature active
        RTFeature.SetActive(true);
        
        // bind UI elements
        programUI = GetComponent<UIDocument>();
        title = programUI.rootVisualElement.Q<Label>("Title");
        description = programUI.rootVisualElement.Q<Label>("Description");
        sliderUI = programUI.rootVisualElement.Q<SliderInt>("BlurStrength");
        returnButton = programUI.rootVisualElement.Q<Button>("Return");

        // initial values
        sliderUI.value = kernelID.Variable.Value;

        // set text
        title.text = "Full Screen Box Blur";
        description.text = 
            "A full screen box blur effect implemented with URP custom renderer feature. " +
            "The blit material performs vertical blur and horizontal blur respectively.";
        
        // register events
        sliderUI.RegisterCallback<ChangeEvent<int>>(evt =>
        {
            kernelID.Variable.SetValue(evt.newValue);
            Debug.Log("Slider: " + kernelID.Variable.Value);
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
