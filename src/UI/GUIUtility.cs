using System.IO;
using UnityEngine;

namespace BagOfTricks.UI
{
    internal static class GUIUtility
    {
        internal static Texture2D LoadTexture(string path)
        {
            byte[] imageData = File.ReadAllBytes(path);
            var texture = new Texture2D(2, 2);
            texture.LoadImage(imageData);
            return texture;
        }

        internal static Texture2D CreateColoredTexture(Texture2D originalTexture, Color color)
        {
            var coloredTexture = new Texture2D(originalTexture.width, originalTexture.height);
            for (int x = 0; x < originalTexture.width; x++)
            {
                for (int y = 0; y < originalTexture.height; y++)
                {
                    Color originalColor = originalTexture.GetPixel(x, y);
                    // Apply the new color but preserve the original alpha value
                    coloredTexture.SetPixel(x, y, new Color(color.r, color.g, color.b, originalColor.a));
                }
            }
            coloredTexture.Apply();
            return coloredTexture;
        }

        internal static Texture2D CreateTexture(int width, int height, Color color)
        {
            var texture = new Texture2D(width, height);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    texture.SetPixel(x, y, color);
                }
            }
            texture.Apply();
            return texture;
        }
    }
}
