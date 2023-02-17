using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class GlitchProgram : MonoBehaviour
{
    // Reference renderer feature asset
    [SerializeField] private GlitchFeature glitchFeature;
    [SerializeField] private Shader glitchShader;
    [SerializeField] private STexture2D inputTexture;
    
    // pass params
    private float verticalJumpTime;
    
    // Renderer feature options to tweak at runtime
    [Header("Render Feature Runtime Options")]
    [SerializeField] private SFloat scanLineJitter;
    [SerializeField] private SFloat verticalJump;
    [SerializeField] private SFloat horizontalShake;
    [SerializeField] private SFloat colorDrift;
    
    // shader properties
    private static readonly int scanLineJitterID = Shader.PropertyToID("_ScanLineJitter");
    private static readonly int verticalJumpID = Shader.PropertyToID("_VerticalJump");
    private static readonly int horizontalShakeID = Shader.PropertyToID("_HorizontalShake");
    private static readonly int colorDriftID = Shader.PropertyToID("_ColorDrift");
    
    // UI Elements
    private UIDocument      programUI;
    private Label           title;
    private Label           description;
    private Slider          scanLineJitterSlider;
    private Slider          verticalJumpSlider;
    private Slider          horizontalShakeSlider;
    private Slider          colorDriftSlider;
    private Button          returnButton;
    
    [Header("UI Text Fields")]
    [SerializeField] private SText titleText;
    [SerializeField] private SText descriptionText;

    private void OnEnable()
    {
        Debug.Log(scanLineJitter.Value);
        Debug.Log(verticalJump.Value);
        Debug.Log(horizontalShake.Value);
        Debug.Log(colorDrift.Value);
        ProgramUtility.AdjustView(Camera.main);

        // set feature as active
        glitchFeature.SetActive(true);
        
        // bind feature settings
        glitchFeature.passSettings.scanLineJitter = scanLineJitter;
        glitchFeature.passSettings.verticalJump = verticalJump;
        glitchFeature.passSettings.horizontalShake = horizontalShake;
        glitchFeature.passSettings.colorDrift = colorDrift;
        glitchFeature.passSettings.useTexture = true;
        glitchFeature.passSettings.inputTexture = inputTexture;
        glitchFeature.passSettings.shader = glitchShader;

        // create feature with new settings
        glitchFeature.Create();
        
        // bind UI Elements
        BindUIElements();
    }

    private void OnDisable()
    {
        glitchFeature.passSettings.useTexture = false;
        glitchFeature.passSettings.inputTexture = null;
        glitchFeature.SetActive(false);
    }

    void BindUIElements()
    {
        programUI = GetComponent<UIDocument>();
        var root = programUI.rootVisualElement;

        title = root.Q<Label>("Title");
        description = root.Q<Label>("Description");
        scanLineJitterSlider = root.Q<Slider>("scanline-jitter");
        verticalJumpSlider = root.Q<Slider>("vertical-jump");
        horizontalShakeSlider = root.Q<Slider>("horizontal-shake");
        colorDriftSlider = root.Q<Slider>("color-drift");
        returnButton = root.Q<Button>("Return");
        
        // set text
        title.text = titleText.Value;
        description.text = descriptionText.Value;
        
        // initial values
        scanLineJitterSlider.value = scanLineJitter;
        verticalJumpSlider.value = verticalJump;
        horizontalShakeSlider.value = horizontalShake;
        colorDriftSlider.value = colorDrift;

        // register events
        scanLineJitterSlider.RegisterCallback<ChangeEvent<float>>(evt =>
        {
            scanLineJitter.Variable.SetValue(evt.newValue);
            glitchFeature.Create();
        });

        verticalJumpSlider.RegisterCallback<ChangeEvent<float>>(evt =>
        {
            verticalJump.Variable.SetValue(evt.newValue);
            glitchFeature.Create();
        });

        horizontalShakeSlider.RegisterCallback<ChangeEvent<float>>(evt =>
        {
            horizontalShake.Variable.SetValue(evt.newValue);
            glitchFeature.Create();
        });

        colorDriftSlider.RegisterCallback<ChangeEvent<float>>(evt =>
        {
            colorDrift.Variable.SetValue(evt.newValue);
            glitchFeature.Create();
        });

        returnButton.RegisterCallback<ClickEvent>(evt =>
        {
            ProgramUtility.ReturnToMain();
        });
        
        // set text
        title.text = titleText.Value;
        description.text = descriptionText.Value;
    }
}
