using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class Program : MonoBehaviour
{
    // UI Elements
    protected UIDocument      programUI;
    protected VisualElement   rootVE;
    protected Label           title;
    protected Label           description;
    protected Button          returnButton;
    
    [Header("UI Text Fields")]
    [SerializeField] private SText titleText;
    [SerializeField] private SText descriptionText;
    
    [Header("Scriptable Renderer Feature")]
    [SerializeField] protected ScriptableRendererFeature rendererFeature;

    protected virtual void Init()
    {
        rendererFeature.SetActive(true);
        BindUI();
    }

    protected virtual void Disable()
    {
        rendererFeature.SetActive(false);
    }

    void BindUI()
    {
        // get main component and root ve
        programUI = GetComponent<UIDocument>();
        rootVE = programUI.rootVisualElement;

        // queue UI elements
        title = rootVE.Q<Label>("Title");
        description = rootVE.Q<Label>("Description");
        returnButton = rootVE.Q<Button>("Return");
        
        // set text
        title.text = titleText.Value;
        description.text = descriptionText.Value;
        
        // register callback event
        returnButton.RegisterCallback<ClickEvent>(evt =>
        {
            ProgramUtility.ReturnToMain();
        });
    }
}
