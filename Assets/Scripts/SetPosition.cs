using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.XR.CoreUtils;

public class setPosition : MonoBehaviour
{
    public InputActionProperty recenterButton;
    
    public Transform target;

    private void Start()
    {
        recenter();
    }

    void Update()
    {
        if (recenterButton.action.WasPressedThisFrame())
        {
            recenter();
        }
    }

    public void recenter()
    {
        XROrigin xrorigin = GetComponent<XROrigin>();
        xrorigin.MoveCameraToWorldLocation(target.position);
        xrorigin.MatchOriginUpOriginForward(target.up,target.forward);
    }
}