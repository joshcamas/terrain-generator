using UnityEngine;
using System.Collections.Generic;

namespace Ardenfall
{
    [System.Serializable]
    public class TerrainGenerationLayer
    {
        public List<Octave> octaves;
        public float power = 1;
        public float multiplier = 1;
        public float addition = 0;

        [System.Serializable]
        public class Octave
        {
            public Vector2 frequency = Vector2.one;
            public Vector2 offset = Vector2.zero;
            public float multiplier = 1;
            public float min = 0;
            public float max = 1;
            public virtual float Sample(Vector2 position)
            {
                float val = multiplier * Mathf.PerlinNoise(position.x * frequency.x, position.y * frequency.y + offset.y);
                return Mathf.Clamp(val,min,max);
            }
        }

        public virtual float Sample(Vector2 position)
        {
            float value = 0;

            for(int i = 0; i < octaves.Count; i++)
            {
                value += octaves[i].Sample(position);
            }

            float v = multiplier * Mathf.Pow(value, power) + addition;

            return v;
        }
    }
}