using UnityEngine;
using UnityEngine.UIElements;

namespace ColorPicker
{
    public interface IColorPicker
    {
        //Updates the ColorPlane UI element based on the sampled color.
        void UpdateColorPlaneTexture(Color color);

        //Sample 3d object color at the mouse position
        Color SampleObjectColorAtMousePosition(Vector2 mousePosition);

        //Enables the Cursor UI on Color Plane and updates the color TextFields (RGB, Hex, Alpha) according to the mouse position on ColorPlane Texture
        void OnColorPlaneMouseDown(MouseDownEvent evt);

        //Updates the color TextFields (RGB, Hex, Alpha) according to the mouse position on ColorPlane Texture
        void OnColorPlaneMouseMove(MouseMoveEvent evt);

        //Disables the Cursor UI on Color Plane and updates the color TextFields (RGB, Hex, Alpha) according to the mouse position on ColorPlane Texture
        void OnColorPlaneMouseUp(MouseUpEvent evt);
    }
}
