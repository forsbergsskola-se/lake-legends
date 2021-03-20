using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EditorExtensions
{
    public static class TextureCreator
    {
        public static Texture2D GetTextureFromGreyScaleValue(Vector2Int dimensions, float greyAmount)
        {
            return GetTextureOfColor(dimensions, new Color(greyAmount, greyAmount, greyAmount, 1));
        }

        public static Texture2D EncodeAndCreateAsset(Texture2D texture2D, string path)
        {
            var bytes = texture2D.EncodeToPNG();
            File.WriteAllBytes($"{Application.dataPath}/{path}", bytes);
            return AssetDatabase.LoadAssetAtPath<Texture2D>(path);
        }

        public static Texture BlendColors(Texture2D texture2D, Color color)
        {
            var texture = new Texture2D(texture2D.width, texture2D.height);
            Graphics.CopyTexture(texture2D, texture);
            for (var i = 0; i < texture.width; i++)
            {
                for (var j = 0; j < texture.height; j++)
                {
                    var orgColor = texture.GetPixel(i, j);
                    if (orgColor.a != 0)
                    {
                        if (orgColor.r < 1)
                        {
                            var newColor = new Color(color.r, color.g, color.b, Mathf.Lerp(orgColor.r, color.a, 0.5f));
                            texture.SetPixel(i, j, newColor);
                        }
                        else
                        {
                            texture.SetPixel(i, j, color);
                        }
                    }
                }   
            }
            texture.Apply();
            return texture;
        }
        
        public static Texture ReplaceNonTransparentPixels(Texture2D texture2D, Color color)
        {
            var texture = new Texture2D(texture2D.width, texture2D.height);
            Graphics.CopyTexture(texture2D, texture);
            for (var i = 0; i < texture.width; i++)
            {
                for (var j = 0; j < texture.height; j++)
                {
                    if (texture.GetPixel(i, j).a != 0)
                    {
                        texture.SetPixel(i, j, color);
                    }
                }   
            }
            texture.Apply();
            return texture;
        }

        public static Texture2D GetTextureOfColor(Vector2Int dimensions, Color color)
        {
            var texture2d = new Texture2D(dimensions.x, dimensions.y, TextureFormat.RGBA32, false);
            texture2d.SetPixels(GetColors(color, dimensions.x * dimensions.y).ToArray());
            texture2d.Apply();
            return texture2d;
        }

        private static IEnumerable<Color> GetColors(Color color, int size)
        {
            for (var i = 0; i < size; i++)
            {
                yield return color;
            }
        }
    }
}