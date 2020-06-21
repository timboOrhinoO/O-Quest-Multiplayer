using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SetMaterialProperty : MonoBehaviour
{

    MaterialPropertyBlock block;
    public Renderer myRenderer;

    [Header("Vector4")]
    public string V4Name;
    public Vector4 V4Value;
    public string V4Name2;
    public Vector4 V4Value2;
    public string V4Name3;
    public Vector4 V4Value3;

    [Header("Float")]
    public string FloatName;
    public float FloatValue;
    public string FloatName2;
    public float FloatValue2;
    public string FloatName3;
    public float FloatValue3;
    public string FloatName4;
    public float FloatValue4;

    // Update is called once per frame
    void Update()
    {

        if (block == null) block = new MaterialPropertyBlock();
        if (!myRenderer) myRenderer = GetComponent<Renderer>();

        if (FloatName != "") block.SetFloat(FloatName, FloatValue);
        if (FloatName != "") block.SetFloat(FloatName2, FloatValue2);
        if (FloatName != "") block.SetFloat(FloatName3, FloatValue3);
        if (FloatName != "") block.SetFloat(FloatName4, FloatValue4);

        if (V4Name != "") block.SetVector(V4Name, V4Value);
        if (V4Name != "") block.SetVector(V4Name2, V4Value2);
        if (V4Name != "") block.SetVector(V4Name3, V4Value3);

        if (myRenderer) myRenderer.SetPropertyBlock(block);
    }
}
