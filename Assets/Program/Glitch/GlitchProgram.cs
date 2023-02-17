using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class GlitchProgram : MonoBehaviour
{
    // Reference renderer feature asset
    [SerializeField] private FullScreenRTFeature RTFeature;
    
    // pass params
    private float verticalJumpTime;
    
    // feature settings
    private float scanLineJitter;
    private float verticalJump;
    private float horizontalShake;
    private float colorDrift;
    
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
        ProgramUtility.AdjustView(Camera.main);
        
        RTFeature.SetActive(true);
        
        BindUIElements();
    }

    private void OnDisable()
    {
        RTFeature.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGlitchProperties();
    }
    
    void UpdateGlitchProperties()
    {
        verticalJumpTime += Time.deltaTime * verticalJump * 11.3f;

        var sl_thresh = Mathf.Clamp01(1.0f - scanLineJitter * 1.2f);
        var sl_disp = 0.002f + Mathf.Pow(scanLineJitter, 3) * 0.05f;
        Shader.SetGlobalVector(scanLineJitterID, new Vector2(sl_disp, sl_thresh));

        var vj = new Vector2(verticalJump, verticalJumpTime);
        Shader.SetGlobalVector(verticalJumpID, vj);
        Shader.SetGlobalFloat(horizontalShakeID, horizontalShake * 0.2f);

        var cd = new Vector2(colorDrift * 0.04f, Time.time * 606.11f);
        Shader.SetGlobalVector(colorDriftID, cd);
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
        
        // register events
        scanLineJitterSlider.RegisterCallback<ChangeEvent<float>>(evt =>
        {
            scanLineJitter = evt.newValue;
        });

        verticalJumpSlider.RegisterCallback<ChangeEvent<float>>(evt =>
        {
            verticalJump = evt.newValue;
        });

        horizontalShakeSlider.RegisterCallback<ChangeEvent<float>>(evt =>
        {
            horizontalShake = evt.newValue;
        });

        colorDriftSlider.RegisterCallback<ChangeEvent<float>>(evt =>
        {
            colorDrift = evt.newValue;
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
