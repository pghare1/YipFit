using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraRotation : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject secondaryCamera;
    public Canvas previewPanelCanvas;

    public void ChangeCameraRotation(bool makeCameraAngleChange)
    {
        if (makeCameraAngleChange)
        {
            mainCamera.SetActive(false);
            secondaryCamera.SetActive(true);
            previewPanelCanvas.worldCamera = secondaryCamera.GetComponent<Camera>();
        }
        else
        {
            secondaryCamera.SetActive(false);
            mainCamera.SetActive(true);
            previewPanelCanvas.worldCamera = mainCamera.GetComponent<Camera>();
        }




    }
}
