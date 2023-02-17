using System;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

[RequireComponent(typeof(UIDocument))]
public class BlackHoleCamera : MonoBehaviour
{
    [Header("Settings")] [SerializeField] private Material blackHoleMaterial;
    [SerializeField] private BlackHole target;

    [SerializeField] private new Camera camera;
    [SerializeField] private Vector3 initialRotation;
    [SerializeField] private float initialDistance;

    [Header("Parameters")] public Vector3 offset;
    public float panSensitivity;
    public bool sqrScroll;
    public float scrollSensitivity;

    private int positionId;
    private int distanceId;

    private bool rightClicked;
    private float rotation;
    private float nod;
    private float offsetDistance;

    private Camera          mainCam;

    private UIDocument      programUI;
    private Label           title;
    private Label           description;
    private Slider          radiusSlider;
    private Slider          discRadiusSlider;
    private Slider          rotationSensitivitySlider;
    private Slider          scrollSensitivitySlider;
    private Button          returnButton;

    [Header("UI Elements")]
    [SerializeField] private SText titleText;
    [SerializeField] private SText descriptionText;

#if UNITY_EDITOR
    private void Reset()
    {
        camera = GetComponent<Camera>();
    }
#endif

    private void OnEnable()
    {
        BindUIElements();
    }

    private void OnDisable()
    {
        programUI.enabled = false;
    }

    private void Start()
    {
        positionId = Shader.PropertyToID("_Position");
        distanceId = Shader.PropertyToID("_Distance");

        nod = initialRotation.x;
        rotation = initialRotation.y;
        offsetDistance = sqrScroll ? Mathf.Sqrt(initialDistance) : initialDistance;

        mainCam = Camera.main;
    }

    private void FixedUpdate()
    {
        if (!ProgramUtility.CheckBounds(mainCam)) return;
        
        var pivot = offset + target.transform.position;
        var toCamera = Quaternion.Euler(nod, rotation, 0) * Vector3.back;

        transform.position = toCamera * (sqrScroll ? offsetDistance * offsetDistance : offsetDistance) + pivot;
        transform.forward = -toCamera;
    }

    private void Update()
    {
        if (!ProgramUtility.CheckBounds(mainCam)) return;
        
        var prevRightClicked = rightClicked;
        if (Input.GetMouseButton(1))
        {
            rightClicked = true;
            rotation += Input.GetAxis("Mouse X") * panSensitivity;
            nod = Math.Clamp(nod - Input.GetAxis("Mouse Y") * panSensitivity, -89, 89);
        }
        else rightClicked = false;

        if (!prevRightClicked && rightClicked)
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
        else if (prevRightClicked && !rightClicked)
        {
            Cursor.lockState = CursorLockMode.None;
        }

        var minDistance = target.radius * 1.2f;
        offsetDistance = Math.Max(offsetDistance - Input.mouseScrollDelta.y * scrollSensitivity,
            sqrScroll ? Mathf.Sqrt(minDistance) : minDistance);
    }

    private void LateUpdate()
    {
        if (!ProgramUtility.CheckBounds(mainCam)) return;
        
        var targetPosition = target.transform.position;
        var thisToTarget = targetPosition - transform.position;

        blackHoleMaterial.SetVector(positionId, camera.WorldToViewportPoint(targetPosition));
        blackHoleMaterial.SetFloat(distanceId,
            (Vector3.Dot(thisToTarget, transform.forward) > 0 ? 1 : -1) * thisToTarget.magnitude);
    }

    void BindUIElements()
    {
        programUI = GetComponent<UIDocument>();
        var root = programUI.rootVisualElement;
        
        title = root.Q<Label>("Title");
        description = root.Q<Label>("Description");
        radiusSlider = root.Q<Slider>("radius");
        discRadiusSlider = root.Q<Slider>("disc-radius");
        rotationSensitivitySlider = root.Q<Slider>("rotate");
        scrollSensitivitySlider = root.Q<Slider>("scroll");
        returnButton = programUI.rootVisualElement.Q<Button>("Return");
        
        // set text
        title.text = titleText.Value;
        description.text = descriptionText.Value;
        
        // register events
        radiusSlider.RegisterCallback<ChangeEvent<float>>(evt =>
        {
            target.radius = evt.newValue;
        });
        
        discRadiusSlider.RegisterCallback<ChangeEvent<float>>(evt =>
        {
            target.discRadius = evt.newValue;
        });
        
        rotationSensitivitySlider.RegisterCallback<ChangeEvent<float>>(evt =>
        {
            panSensitivity = evt.newValue;
        });
        
        scrollSensitivitySlider.RegisterCallback<ChangeEvent<float>>(evt =>
        {
            scrollSensitivity = evt.newValue;
        });

        returnButton.RegisterCallback<ClickEvent>(evt =>
        {
            ProgramUtility.ReturnToMain();
        });
    }
}