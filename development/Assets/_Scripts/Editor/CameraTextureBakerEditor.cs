using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CameraTextureBaker))]
public class CameraTextureBakerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var cameraTexturesBaker = (CameraTextureBaker) target;
        DrawDefaultInspector();
        
        GUILayout.Space(10);

        if (GUILayout.Button("Bake depth map", GUILayout.Height(25)))
        {
            cameraTexturesBaker.BakeMapDepth();
        }
        
        if (GUILayout.Button("Bake normals map", GUILayout.Height(25)))
        {
            cameraTexturesBaker.BakeMapNormals();
        }
    }
}
