using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandPresence : MonoBehaviour
{
    public InputActionReference triggerActionReference;
    public InputActionReference gripActionReference;
    public Animator handAnimator;

    void Start()
    {
        triggerActionReference.action.Enable();
        gripActionReference.action.Enable();
    }

    void UpdateHandAnimation()
    {
        float triggerValue = triggerActionReference.action.ReadValue<float>();
        handAnimator.SetFloat("Trigger", triggerValue);

        float gripValue = gripActionReference.action.ReadValue<float>();
        handAnimator.SetFloat("Grip", gripValue);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHandAnimation();
    }

}