using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Ardenfall
{
    [CreateAssetMenu(menuName = "Ardenfall/Editor/Terrain Generation Asset")]
    public class TerrainGenerationAsset : ScriptableObject
    {
        public Vector2 scale = Vector2.one;
        public Vector2 offset;

        public Texture2D multiplyTexture;
        public float multiplyTextureValue;

        public List<TerrainGenerationLayer> layers;
        
        //Editor Only
        [Header("Editor Testing")]
        public TerrainData terrain;

        [Button]
        public void Generate()
        {
            if (terrain != null)
            {
                Vector2Int res = new Vector2Int(terrain.heightmapResolution, terrain.heightmapResolution);

                float[,] heights = TerrainGenerator.GenerateHeights(layers, res, offset, scale);

                if (multiplyTexture != null)
                    TerrainGenerator.MultiplyHeightsByTexture(heights, res, multiplyTextureValue, multiplyTexture);

                terrain.SetHeights(0,0,heights);
            }
        }

    }
}