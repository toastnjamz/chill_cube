using UnityEngine;

public class RotateSkybox : MonoBehaviour
{
    public float RotateSpeed = 0.5f;
    
    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * RotateSpeed);
    }
}
