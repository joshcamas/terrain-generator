using UnityEngine;
using System.Collections.Generic;

namespace Ardenfall
{
    public class TerrainGenerator
    {
        public static float[,] GenerateHeights(List<TerrainGenerationLayer> layers, Vector2Int size, Vector2 offset, Vector2 scale)
        {

            float[,] heights = new float[size.x,size.y];

            for (int x=0;x< size.x;x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    float xCoord = offset.x + (float)x / (float)size.x * scale.x;
                    float yCoord = offset.y + (float)y / (float)size.y * scale.y;

                    float value = 0;

                    for (int i = 0; i < layers.Count; i++)
                    {
                        value += layers[i].Sample(new Vector2(xCoord, yCoord));
                    }

                    heights[x, y] = value;
                }
            }

            return heights;
        }

        public static void MultiplyHeightsByTexture(float[,] heights, Vector2Int size, float multiplier,Texture2D texture)
        {
            Color[] colors = texture.GetPixels();

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    float realX = ((float)x / (float)size.x) * texture.width;
                    float realY = ((float)y / (float)size.y) * texture.height;

                    float a = colors[Mathf.FloorToInt(realY) * texture.width + Mathf.FloorToInt(realX)].grayscale;
                    float b = colors[Mathf.CeilToInt(realY) * texture.width + Mathf.CeilToInt(realX)].grayscale;

                    float lerp = Mathf.Lerp(a, b, realX - Mathf.FloorToInt(realX));

                    heights[x, y] *= lerp * multiplier;
                }
            }

        }

        public static Texture2D GenerateTexture(float[,] heights, Vector2Int size, Texture2D texture = null)
        {
            if(texture == null)
                texture = new Texture2D(size.x, size.y);

            if (texture.width != size.x || texture.height != size.y)
            {
                Texture2D.DestroyImmediate(texture);
                texture = new Texture2D(size.x, size.y);
            }

            Color[] pixels = new Color[size.x * size.y];

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    float value = heights[x, y];
                    pixels[y * size.x + x] = new Color(value, value, value);
                }
            }
            texture.SetPixels(pixels);
            texture.Apply();

            return texture;

        }
    }
}