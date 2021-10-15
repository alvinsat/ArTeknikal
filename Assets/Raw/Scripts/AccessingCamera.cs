using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class AccessingCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        VuforiaARController.Instance.RegisterVuforiaStartedCallback(OnVuforiaStarted);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnVuforiaStarted()
    {
        CameraDevice.Instance.SetFocusMode(
           CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
    }

    void OnApplicationPause(bool pause)
    {
        if (!pause) // resumed
        {
            // Set again autofocus mode when app is resumed
            CameraDevice.Instance.SetFocusMode(
                CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
        }
    }
}
