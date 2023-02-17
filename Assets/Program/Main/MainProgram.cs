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
    [SerializeField] private FullScreenBlurFeature blurFeature;
    [SerializeField] private SInt mainBlurStrength;
    
    [SerializeField] private GlitchFeature glitchFeature;
    [SerializeField] private Shader glitchShader;

    // UI Elements
    private UIDocument mainUI;
    private Label about;
    private Label members;
    private List<Button> shaders;
    private List<Button> textures;
    private List<Button> srp;


    [SerializeField] private SText mainAbout;
    [SerializeField] private SText mainMembers;
    public List<Button> buttons;
    
    void OnEnable()
    {
        blurFeature.SetActive(true);
        glitchFeature.SetActive(true);
        blurFeature.passSettings.blurStrength = mainBlurStrength;
        glitchFeature.passSettings.useTexture = false;
        glitchFeature.passSettings.shader = glitchShader;
        blurFeature.Create();
        
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

        buttons = root.Query<Button>().Where(elem => elem.tooltip == "scene").ToList();

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
}
