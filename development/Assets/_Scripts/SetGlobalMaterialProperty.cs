using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SetGlobalMaterialProperty : MonoBehaviour
{
    [Header("Vector3")]
    public string posPropertyName;
    public Vector3 posValue;

    [Header("Float")]
    public string WaveDistName;
    public float WaveDistance;

    public string WaveOpacityName;
    public float WaveOpacityValue;

    [Header("Color")]
    public string LightColorProperty;
    public Color LightColor;
    

    //MaterialPropertyBlock block;
    //public Renderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        //block = new MaterialPropertyBlock();
        //renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (!renderer) return;

        posValue = transform.position;
        if (posPropertyName != "") Shader.SetGlobalVector(posPropertyName, posValue);

        if (WaveDistName != "") Shader.SetGlobalFloat(WaveDistName, WaveDistance);

        if (WaveOpacityName != "") Shader.SetGlobalFloat(WaveOpacityName, WaveOpacityValue);

        if (LightColorProperty != "") Shader.SetGlobalColor(LightColorProperty, LightColor);

        //if (posPropertyName != "") block.SetVector(posPropertyName, posValue);

        //renderer.SetPropertyBlock(block);
    }
}
