using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class FishProgram : MonoBehaviour
{
    [SerializeField] private Material fishMaterial;
    
    // shader properties
    private static readonly int amplitudeID = Shader.PropertyToID("_Amplitude");
    private static readonly int frequencyID = Shader.PropertyToID("_Frequency");
    private static readonly int speedID = Shader.PropertyToID("_Speed");
    
    // UI Elements
    private UIDocument      programUI;
    private Label           title;
    private Label           description;
    private Slider          amplitude;
    private Slider          frequency;
    private Slider          speed;
    private Button          returnButton;

    [Header("UI Text Fields")]
    [SerializeField] private SText titleText;
    [SerializeField] private SText descriptionText;

    private void OnEnable()
    {
        BindUIElements();
    }

    private void OnDisable()
    {
        programUI.enabled = false;
    }

    void BindUIElements()
    {
        programUI = GetComponent<UIDocument>();
        var root = programUI.rootVisualElement;

        title = root.Q<Label>("Title");
        description = root.Q<Label>("Description");
        amplitude = root.Q<Slider>("amplitude");
        frequency = root.Q<Slider>("frequency");
        speed = root.Q<Slider>("speed");
        returnButton = root.Q<Button>("Return");
        
        // set text
        title.text = titleText.Value;
        description.text = descriptionText.Value;
        
        // initial values of slider
        amplitude.value = fishMaterial.GetFloat(amplitudeID);
        frequency.value = fishMaterial.GetFloat(frequencyID);
        speed.value = fishMaterial.GetFloat(speedID);
        
        // register events
        amplitude.RegisterCallback<ChangeEvent<float>>(evt =>
        {
            fishMaterial.SetFloat(amplitudeID, evt.newValue);
        });
        
        frequency.RegisterCallback<ChangeEvent<float>>(evt =>
        {
            fishMaterial.SetFloat(frequencyID, evt.newValue);
        });
        
        speed.RegisterCallback<ChangeEvent<float>>(evt =>
        {
            fishMaterial.SetFloat(speedID, evt.newValue);
        });
        
        returnButton.RegisterCallback<ClickEvent>(evt =>
        {
            ProgramUtility.ReturnToMain();
        });
    }
}
