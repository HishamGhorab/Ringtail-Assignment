using UnityEngine;
using UnityEngine.UIElements;

namespace ColorPicker
{
    public static class ColorationExtenstion
    {
        //Updates the ColorPlane Texture pixel by pixel based on the generated gradient from the given color
        public static void UpdateColorPlane(this Texture2D texture, Color newColor)
        {
            Color.RGBToHSV(newColor, out var h, out _, out _);
            newColor = Color.HSVToRGB(h, 1, 1);

            var width = texture.width;
            var height = texture.height;

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var color = GenerateGradientFromColor(newColor, x, y, width, height);
                    texture.SetPixel(x, y, color);
                }
            }

            texture.Apply(true);
        }

        //Reads pixel color of ColorPlane texture at given mouse position
        public static Color ReadPixelColorAtMousePosition(this VisualElement colorPlane, Vector2 localMousePosition)
        {
            var textureRect = colorPlane.worldBound;
            var pixelUv = new Vector2(
                localMousePosition.x / textureRect.width,
                1 - (localMousePosition.y / textureRect.height));

            return colorPlane.style.backgroundImage.value.texture.GetPixelBilinear(pixelUv.x, pixelUv.y);
        }

        private static Color GenerateGradientFromColor(Color selectedColor, int x, int y, int width, int height)
        {
            var v = (float)x / (width - 1);
            var u = (float)y / (height - 1);

            var color = Color.Lerp(Color.white, selectedColor, v);
            color = Color.Lerp(Color.black, color, u);
            color.a = 1f;
            return color;
        }
    }
}
