using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GradientGenerate : MonoBehaviour
{

    public Material rocksMaterial;
    public bool realtimeGeneration;
    public Gradient lutGradient;
    public Vector2Int lutTextureSize;
    public Texture2D lutTexture;

    private void FixedUpdate()
    {
        if (realtimeGeneration)
            GenerateLutTexture();
        
    }

    private void GenerateLutTexture() { 

        lutTexture = new Texture2D(lutTextureSize.x, lutTextureSize.y){wrapMode = TextureWrapMode.Clamp};

        for (var x = 0; x < lutTextureSize.x; x++)
        {
            var color = lutGradient.Evaluate(x / (float)lutTextureSize.x);
            for (var y = 0; y < lutTextureSize.y; y++)
            {
                lutTexture.SetPixel(x, y, color);
            }
        }

        lutTexture.Apply();
        rocksMaterial.SetTexture("GradientRamp", lutTexture);

    }
}
