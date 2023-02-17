using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(UIDocument))]
public class MainProgram : MonoBehaviour
{
    [Header("Blur Feature")]
    [SerializeField] private FullScreenBlurFeature blurFeature;
    private int _blurStrength;
    
    [Header("Glitch Feature")]
    [SerializeField] private GlitchFeature glitchFeature;
    [SerializeField] private Shader glitchShader;
    private float _scanLineJitter;
    private float _verticalJump;
    private float _horizontalShake;
    private float _colorDrift;

    // UI Elements
    private UIDocument mainUI;
    private Label about;
    private Label members;
    private List<Button> shaders;
    private List<Button> textures;
    private List<Button> srp;

    [Header("UI Elements")]
    [SerializeField] private SText mainAbout;
    [SerializeField] private SText mainMembers;
    private List<Button> buttons;
    
    void OnEnable()
    {
        InitBlur();
        
        InitGlitch();
        
        BindUIElements();
    }

    private void OnDisable()
    {
        blurFeature.passSettings.blurStrength.Variable.SetValue(_blurStrength);
        blurFeature.SetActive(false);
        
        glitchFeature.passSettings.scanLineJitter.Variable.SetValue(_scanLineJitter);
        glitchFeature.passSettings.verticalJump.Variable.SetValue(_verticalJump);
        glitchFeature.passSettings.horizontalShake.Variable.SetValue(_horizontalShake);
        glitchFeature.passSettings.colorDrift.Variable.SetValue(_colorDrift);
        glitchFeature.SetActive(false);
    }

    void BindUIElements()
    {
        mainUI = GetComponent<UIDocument>();
        var root = mainUI.rootVisualElement;
        about = root.Q<Label>("about-field");
        members = root.Q<Label>("members-field");

        // get all buttons with tooltip 'scene'
        buttons = root.Query<Button>().Where(elem => elem.tooltip == "scene").ToList();

        // register callback for all buttons
        for (int i = 0; i < buttons.Count ; i ++)
        {
            int sceneIndex = i + 2;
            buttons[i].RegisterCallback<ClickEvent>(evt => 
            {
                SmoothLoading.SetNextSceneIndex(sceneIndex);
                SceneManager.LoadScene(1);
            });
        }

        about.text = mainAbout.Value;
        members.text = mainMembers.Value;
    }

    void InitBlur()
    {
        blurFeature.SetActive(true);
        _blurStrength = blurFeature.passSettings.blurStrength;
        blurFeature.passSettings.blurStrength.Variable.SetValue(5);
        blurFeature.Create();
    }

    void InitGlitch()
    {
        // set feature active
        glitchFeature.SetActive(true);
        
        Debug.Log("Bind glitch features: Main Program");
        // bind feature settings
        glitchFeature.passSettings.useTexture = false;
        glitchFeature.passSettings.shader = glitchShader;

        // save scene values
        _scanLineJitter = glitchFeature.passSettings.scanLineJitter;
        _verticalJump = glitchFeature.passSettings.verticalJump;
        _horizontalShake = glitchFeature.passSettings.horizontalShake;
        _colorDrift = glitchFeature.passSettings.colorDrift;
        
        // set main values
        glitchFeature.passSettings.scanLineJitter.Variable.SetValue(0.2f);
        glitchFeature.passSettings.verticalJump.Variable.SetValue(0.05f);
        glitchFeature.passSettings.horizontalShake.Variable.SetValue(0);
        glitchFeature.passSettings.colorDrift.Variable.SetValue(0.35f);
        
        // create feature with new settings
        glitchFeature.Create();
    }
}
