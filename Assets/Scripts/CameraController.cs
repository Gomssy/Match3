using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float targetAspect = 10f / 20f;
    void Start()
    {
        AdjustCamera();
    }
    void AdjustCamera()
    {
        Camera camera = GetComponent<Camera>();

        float scaleHeight = (float)Screen.height / Screen.width;
        Debug.Log("scaleHeight =" + scaleHeight);
        if(scaleHeight >= 1.6f)
        {
            float camHeight = 7f * scaleHeight;
            camera.orthographicSize = camHeight;
        }
        else
        {
            camera.orthographicSize = 10f;
        }
    }
}
    