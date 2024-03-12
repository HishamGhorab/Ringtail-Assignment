using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class R_ShowAndHideColorPicker : MonoBehaviour
{
    [SerializeField] private UIDocument colorPickerUIDocument;
    
    public bool IsColorPickerEnabled { get { return isColorPickerEnabled; } }
    
    private VisualElement rootVisualElement;
    private VisualElement showPickerButtonVE;
    private VisualElement hidePickerButtonVE;

    private VisualElement colorPickerContainerVE;
    private VisualElement colorPickerCursorVE;

    private bool isColorPickerEnabled;

    private void Awake()
    {
        rootVisualElement = GetComponent<UIDocument>().rootVisualElement;

        if (colorPickerUIDocument != null && colorPickerUIDocument.rootVisualElement != null)
        {
            colorPickerContainerVE = colorPickerUIDocument.rootVisualElement.Q<VisualElement>("MainContainer");
            colorPickerCursorVE = colorPickerUIDocument.rootVisualElement.Q<VisualElement>("PickerCursor");
        }
        else
        {
            Debug.LogError("colorPickerUIDocument or its rootVisualElement is null. Make sure it's properly assigned.");
        }
        
        showPickerButtonVE = rootVisualElement.Q<Button>("ShowColorPickerButton");
        hidePickerButtonVE = rootVisualElement.Q<Button>("HideColorPickerButton");

        showPickerButtonVE?.RegisterCallback<ClickEvent>(OnClickShowColorPicker);
        hidePickerButtonVE?.RegisterCallback<ClickEvent>(OnClickHideColorPicker);
    }

    private void Start()
    {
        isColorPickerEnabled = true;
    }

    private void OnClickShowColorPicker(ClickEvent evt)
    {
        if (!isColorPickerEnabled)
        {
            isColorPickerEnabled = true;
            SetColorPickerVisibility(DisplayStyle.Flex);
        }
    }

    private void OnClickHideColorPicker(ClickEvent evt)
    {
        if (isColorPickerEnabled)
        {
            isColorPickerEnabled = false;
            SetColorPickerVisibility(DisplayStyle.None);
        }
    }

    private void SetColorPickerVisibility(DisplayStyle style)
    {
        colorPickerContainerVE.style.display = style;
        colorPickerCursorVE.style.display = style;
    }
}