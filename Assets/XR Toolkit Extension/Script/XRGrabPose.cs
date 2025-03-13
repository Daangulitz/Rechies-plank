using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[RequireComponent(typeof(XRGrabInteractable))]
public class XRGrabPose : MonoBehaviour
{

    public handData rightHandPose;
    public handData leftHandPose;

    public bool MultipleHands = false;

    public handData rightHandPoseSecondGrab;
    public handData leftHandPoseSecondGrab;

    public Transform PrimaryAttachPoint;
    public Transform SecondaryAttachPoint;




    // Start is called before the first frame update
    void Start()
    {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();

        PrimaryAttachPoint = transform.Find("PrimaryAttachPoint");
        SecondaryAttachPoint = transform.Find("SecondaryAttachPoint");
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(setupPose1);
            grabInteractable.selectExited.AddListener(unsetPose1);
        }

        if (rightHandPose != null)
        {
            rightHandPose.gameObject.SetActive(false);
        }

        if (leftHandPose != null)
        {
            leftHandPose.gameObject.SetActive(false);
        }

        if (rightHandPoseSecondGrab != null)
        {
            rightHandPoseSecondGrab.gameObject.SetActive(false);
        }

        if (leftHandPoseSecondGrab != null)
        {
            leftHandPoseSecondGrab.gameObject.SetActive(false);
        }
    }

    // Assuming you have a class-level list of grab interactors
    List<IXRSelectInteractor> grabInteractors = new List<IXRSelectInteractor>();



    public void setupPose1(SelectEnterEventArgs args)
    {
        if (args.interactorObject is NearFarInteractor)
        {
            grabInteractors.Add((XRBaseInteractor)args.interactorObject);

            // Check if it's the first or second grab interactor
            if (grabInteractors.Count == 1)
            {
                Debug.Log("First grab interactor");
            }
            else if (grabInteractors.Count == 2)
            {
                Debug.Log("Second grab interactor");
            }

            GameObject parent = args.interactorObject.transform.parent.gameObject;
            handData handData1 = parent.GetComponent<handData>();

            // Check the handedness of the handData
            if (grabInteractors.Count == 1)
            {
                if (handData1.handedness == handData.HandType.Right)
                {
                    // Use the right hand model
                    rightHandPose.gameObject.SetActive(true);
                }
                else if (handData1.handedness == handData.HandType.Left)
                {
                    // Use the left hand model
                    leftHandPose.gameObject.SetActive(true);
                }
            }
            else if (grabInteractors.Count == 2)
            {
                if (handData1.handedness == handData.HandType.Right)
                {
                    // Use the right hand model
                    rightHandPoseSecondGrab.gameObject.SetActive(true);
                }
                else if (handData1.handedness == handData.HandType.Left)
                {
                    // Use the left hand model
                    leftHandPoseSecondGrab.gameObject.SetActive(true);
                }
            }

            parent.GetComponentsInChildren<SkinnedMeshRenderer>()[0].enabled = false;
        }
    }

    public void unsetPose1(SelectExitEventArgs args)
    {
        if (args.interactorObject is NearFarInteractor)
        {
            // Remove the grab interactor from the list
            grabInteractors.Remove((XRBaseInteractor)args.interactorObject);

            GameObject parent = args.interactorObject.transform.parent.gameObject;
            handData handData1 = parent.GetComponent<handData>();

            // In the original method
            if (grabInteractors.Count == 0)
            {
                HideHandModel(rightHandPose.gameObject, leftHandPose.gameObject, handData1.handedness);
                Debug.Log("First grab interactor removed");
            }
            else if (grabInteractors.Count == 1)
            {
                HideHandModel(rightHandPoseSecondGrab.gameObject, leftHandPoseSecondGrab.gameObject, handData.HandType.Right);
                HideHandModel(rightHandPoseSecondGrab.gameObject, leftHandPoseSecondGrab.gameObject, handData.HandType.Left);
                Debug.Log("Second grab interactor removed");
            }


            parent.GetComponentsInChildren<SkinnedMeshRenderer>()[0].enabled = true;
        }
    }

    private void HideHandModel(GameObject rightHandModel, GameObject leftHandModel, handData.HandType handedness)
    {
        if (handedness == handData.HandType.Right)
        {
            // Hide the right hand model
            rightHandModel.SetActive(false);
        }
        else if (handedness == handData.HandType.Left)
        {
            // Hide the left hand model
            leftHandModel.SetActive(false);
        }
    }
}