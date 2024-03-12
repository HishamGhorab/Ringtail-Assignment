using System;
using System.Collections;
using System.Collections.Generic;
using ColorPicker;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class R_ColorPicker : MonoBehaviour, ColorPicker.IColorPicker
{
    [Header("Camera")]
    [SerializeField] Camera renderCamera;
    
    [Header("Texture")]
    [SerializeField] RenderTexture renderTexture;
    [SerializeField] private Texture2D planeTexture;

    [Header("Components")] 
    [SerializeField] private R_ShowAndHideColorPicker showAndHideColorPicker;

    private Color sampledColor;
    
    private VisualElement rootVE;
    private VisualElement valueSubContainer;
    private VisualElement colorPlaneVE;
    
    private VisualElement pickerCursorVE;
    private VisualElement cursorColorSliderVE;
    private VisualElement cursorAlphaSliderVE;

    private VisualElement hexValueContainerVE;
    private VisualElement RColorContainerVE;
    private VisualElement GColorContainerVE;
    private VisualElement bColorContainerVE;
    private VisualElement aColorContainerVE;
    
    private Label hexValueLabel;
    private Label rColorValueLabel;
    private Label gColorValueLabel;
    private Label bColorValueLabel;
    private Label aColorValueLabel;

    private void Awake()
    {
        rootVE = GetComponent<UIDocument>().rootVisualElement;
        InitUI();
    }
    
    private void Update()
    {
        //Early return if the ColorPicker is Disabled
        if (!showAndHideColorPicker.IsColorPickerEnabled)
            return;
        
        if (Input.GetMouseButtonDown(0))
        {
            //Gets color based on mouse input and updates values
            sampledColor = SampleObjectColorAtMousePosition(Input.mousePosition);
            OnColorChanged(sampledColor);
            UpdateColorPlaneTexture(sampledColor);
        }
        
        MovePickerCursorWithinColorPlane();
    }
    
    //Updates the ColorPlane UI element based on the sampled color.
    public void UpdateColorPlaneTexture(Color color)
    {
        AssignTextureToBackground(colorPlaneVE, planeTexture);
        
        Texture2D texture = colorPlaneVE.style.backgroundImage.value.texture;
        texture.UpdateColorPlane(color);
    }

    //Sample 3d object color at the mouse position
    public Color SampleObjectColorAtMousePosition(Vector2 mousePosition)
    {
        //Convert mouse position to a point in world space
        Ray ray = renderCamera.ScreenPointToRay(mousePosition);
            
        //Read pixels directly based on mouse position
        Texture2D tex = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);
        tex.filterMode = FilterMode.Point;
            
        RenderTexture.active = renderTexture;
        tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        tex.Apply();
 
        //Get pixel color at the mouse position
        Color pixelColor = tex.GetPixel((int)(mousePosition.x / Screen.width * renderTexture.width), 
            (int)(mousePosition.y / Screen.height * renderTexture.height));

        return pixelColor;
    }

    //Enables the Cursor UI on Color Plane and updates the color TextFields (RGB, Hex, Alpha) according to the mouse position on ColorPlane Texture
    public void OnColorPlaneMouseDown(MouseDownEvent evt)
    {
        AssignTextureToBackground(colorPlaneVE, planeTexture);
        
        sampledColor = ColorationExtenstion.ReadPixelColorAtMousePosition(colorPlaneVE, evt.mousePosition);
        OnColorChanged(sampledColor);
    }

    //Updates the color TextFields (RGB, Hex, Alpha) according to the mouse position on ColorPlane Texture
    public void OnColorPlaneMouseMove(MouseMoveEvent evt)
    {
        AssignTextureToBackground(colorPlaneVE, planeTexture);
        
        sampledColor = ColorationExtenstion.ReadPixelColorAtMousePosition(colorPlaneVE, evt.mousePosition);
        OnColorChanged(sampledColor);
    }

    //Disables the Cursor UI on Color Plane and updates the color TextFields (RGB, Hex, Alpha) according to the mouse position on ColorPlane Texture
    public void OnColorPlaneMouseUp(MouseUpEvent evt)
    {
        AssignTextureToBackground(colorPlaneVE, planeTexture);

        sampledColor = ColorationExtenstion.ReadPixelColorAtMousePosition(colorPlaneVE, evt.mousePosition);
        OnColorChanged(sampledColor);
    }

    private void InitUI()
    {
        //Initializes user interface related things
        
        colorPlaneVE = rootVE.Q<VisualElement>("ColorPlane");
        
        valueSubContainer = rootVE.Q<VisualElement>("ValueSubContainer");
        
        pickerCursorVE = rootVE.Q<VisualElement>("PickerCursor");
        cursorColorSliderVE = rootVE.Q<VisualElement>("CursorColorSlider");
        cursorAlphaSliderVE = rootVE.Q<VisualElement>("CursorAlphaSlider");
        
        //color values
        hexValueContainerVE = valueSubContainer.Q<VisualElement>("HexContainer");
        hexValueLabel = hexValueContainerVE.Q<Label>("Value");
        
        RColorContainerVE = valueSubContainer.Q<VisualElement>("RColorContainer");
        rColorValueLabel = RColorContainerVE.Q<Label>("Value");
        
        GColorContainerVE = valueSubContainer.Q<VisualElement>("GColorContainer");
        gColorValueLabel = GColorContainerVE.Q<Label>("Value");
        
        bColorContainerVE = valueSubContainer.Q<VisualElement>("BColorContainer");
        bColorValueLabel = bColorContainerVE.Q<Label>("Value");
        
        aColorContainerVE = valueSubContainer.Q<VisualElement>("AColorContainer");
        aColorValueLabel = aColorContainerVE.Q<Label>("Value");
        
        colorPlaneVE?.RegisterCallback<MouseDownEvent>(OnColorPlaneMouseDown);
        colorPlaneVE?.RegisterCallback<MouseMoveEvent>(OnColorPlaneMouseMove);
        colorPlaneVE?.RegisterCallback<MouseUpEvent>(OnColorPlaneMouseUp);
    }
    
    private void MovePickerCursorWithinColorPlane()
    {
        float minX = 643;
        float minY = 434;
        float maxX = 773;
        float maxY = 563;

        float width = pickerCursorVE.layout.width / 2;
        float height = pickerCursorVE.layout.height / 2;
        
        // Get the mouse position in screen coordinates
        Vector2 mousePosition = Input.mousePosition;
        if (mousePosition.x - width < minX || Screen.height - mousePosition.y - height < minY || mousePosition.x - width> maxX || Screen.height - mousePosition.y - height > maxY)
            return;
        
        pickerCursorVE.style.left = Mathf.Clamp(mousePosition.x - width, 643, 773);
        pickerCursorVE.style.top = Mathf.Clamp(Screen.height - mousePosition.y - height, 434, 563); 
        
    }
    
    private void AssignTextureToBackground(VisualElement VE, Texture2D tex)
    {
        var style = new GUIStyle();
        
        //Assign the texture to the background image property of the style and apply the style to the UI element
        style.normal.background = tex;
        VE.style.backgroundImage = style.normal.background;
    }

    private void OnColorChanged(Color sampledColor)
    {
        UpdateHexRGBATextFields(sampledColor);
        UpdateHueSlider(sampledColor);
        UpdateAlphaSlider(sampledColor);
    }
    
    private void UpdateHexRGBATextFields(Color sampledColor)
    {
        hexValueLabel.text = UnityEngine.ColorUtility.ToHtmlStringRGB(sampledColor);
        
        rColorValueLabel.text = R_Utility.ConvertColorComponentTo255(sampledColor.r).ToString();
        gColorValueLabel.text = R_Utility.ConvertColorComponentTo255(sampledColor.g).ToString();
        bColorValueLabel.text = R_Utility.ConvertColorComponentTo255(sampledColor.b).ToString();
        aColorValueLabel.text = R_Utility.ConvertColorComponentTo255(sampledColor.a).ToString();
    }

    private void UpdateHueSlider(Color sampledColor)
    {
        float hue;
        Color.RGBToHSV(sampledColor,out hue,out _, out _);
        
        float remappedValue = R_Utility.RemapValue(hue, 0, 1, 90, 0);
        cursorColorSliderVE.style.top = Length.Percent(remappedValue);
    }
    
    private void UpdateAlphaSlider(Color sampledColor)
    {
        float alpha = R_Utility.ConvertColorComponentTo255(sampledColor.a);
        float remappedValue = R_Utility.RemapValue(alpha, 0, 255, 90, 0);
        cursorAlphaSliderVE.style.top = Length.Percent(remappedValue);
    }
}
