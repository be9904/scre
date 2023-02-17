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
    [SerializeField] private SInt mainBlurStrength;
    
    [Header("Glitch Feature")]
    [SerializeField] private GlitchFeature glitchFeature;
    [SerializeField] private Shader glitchShader;
    [SerializeField] private SFloat scanLineJitter;
    [SerializeField] private SFloat verticalJump;
    [SerializeField] private SFloat horizontalShake;
    [SerializeField] private SFloat colorDrift;

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
        blurFeature.SetActive(true);
        blurFeature.passSettings.blurStrength = mainBlurStrength;
        blurFeature.Create();
        
        InitGlitch();
        
        BindUIElements();
    }

    private void OnDisable()
    {
        blurFeature.SetActive(false);
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

    void InitGlitch()
    {
        // set feature active
        glitchFeature.SetActive(true);
        
        Debug.Log("Bind glitch features: Main Program");
        // bind feature settings
        glitchFeature.passSettings.useTexture = false;
        glitchFeature.passSettings.shader = glitchShader;
        glitchFeature.passSettings.scanLineJitter = scanLineJitter;
        glitchFeature.passSettings.verticalJump = verticalJump;
        glitchFeature.passSettings.horizontalShake = horizontalShake;
        glitchFeature.passSettings.colorDrift = colorDrift;
        
        // create feature with new settings
        glitchFeature.Create();
    }
}
