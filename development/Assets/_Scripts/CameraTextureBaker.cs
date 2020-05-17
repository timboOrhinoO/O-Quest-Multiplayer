using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class CameraTextureBaker : MonoBehaviour
{
    public RenderTexture depthRenderTexture;
    public RenderTexture normalsRenderTexture;

    public float rendScale, rendOffsetX, rendOffsetZ, rendOffsetY, rendCameraClipFar;

    public bool RuntimeRendering;
    
    private Camera _cam; 
    

    public void BakeMapDepth()
    {
        if (!depthRenderTexture)
        {
            Debug.LogWarning("Set output RenderTexture first");
            return;
        }
        
        SetCameraSettings();
        BakeMap(depthRenderTexture, "Global_Scene_Depth");
    }
    
    public void BakeMapNormals()
    {
        if (!normalsRenderTexture)
        {
            Debug.LogWarning("Set output RenderTexture first");
            return;
        }
        
        SetCameraSettings();
        BakeMap(normalsRenderTexture, "Global_Scene_Normals");
    }

    private void SetCameraSettings()
    {
        if (!_cam)
            _cam = GetComponent<Camera>();
    
        rendScale = _cam.orthographicSize * 2;
        rendOffsetX = _cam.transform.position.x - _cam.orthographicSize;
        rendOffsetZ = _cam.transform.position.z - _cam.orthographicSize;
        rendOffsetY = _cam.transform.position.y - _cam.farClipPlane;
        rendCameraClipFar = _cam.farClipPlane;
        
        Shader.SetGlobalFloat("Global_Cam_RendScale", rendScale);
        Shader.SetGlobalFloat("Global_Cam_RendOffset_X", rendOffsetX);
        Shader.SetGlobalFloat("Global_Cam_RendOffset_Z", rendOffsetZ);
        Shader.SetGlobalFloat("Global_Cam_RendOffset_Y", rendOffsetY);
        Shader.SetGlobalFloat("Global_Cam_RendClip_Far", rendCameraClipFar);
    }

    private void BakeMap(RenderTexture rendTex, string globalTexName)
    {
        _cam = GetComponent<Camera>();
        _cam.enabled = true;
        
        _cam.targetTexture = rendTex;
        Shader.SetGlobalTexture(globalTexName, rendTex);
        
        _cam.Render();
        _cam.enabled = false;
    }
   
}