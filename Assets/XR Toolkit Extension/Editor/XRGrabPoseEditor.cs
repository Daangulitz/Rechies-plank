using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[CustomEditor(typeof(XRGrabPose))]
public class XRGrabPoseEditor : Editor
{
    #region Serialized Properties
    SerializedProperty PrimaryAttachPoint;
    SerializedProperty rightHandPose;
    SerializedProperty leftHandPose;

    SerializedProperty MultipleHands;
    SerializedProperty SecondaryAttachPoint;

    SerializedProperty rightHandPoseSecondGrab;
    SerializedProperty leftHandPoseSecondGrab;

    private bool PrimaryHandsFoldout = true;
    private bool SecondaryHandsFoldout = false;

    private bool PrimaryLeftHandFoldout = false;
    private bool PrimaryRightHandFoldout = false;

    private bool SecondaryLeftHandFoldout = false;
    private bool SecondaryRightHandFoldout = false;

    private XRGrabInteractable grabInteractable;
    #endregion

    private void OnEnable()
    {
        rightHandPose = serializedObject.FindProperty("rightHandPose");
        leftHandPose = serializedObject.FindProperty("leftHandPose");

        MultipleHands = serializedObject.FindProperty("MultipleHands");

        rightHandPoseSecondGrab = serializedObject.FindProperty("rightHandPoseSecondGrab");
        leftHandPoseSecondGrab = serializedObject.FindProperty("leftHandPoseSecondGrab");

        PrimaryAttachPoint = serializedObject.FindProperty("PrimaryAttachPoint");
        SecondaryAttachPoint = serializedObject.FindProperty("SecondaryAttachPoint");

        grabInteractable = ((XRGrabPose)target).GetComponent<XRGrabInteractable>();
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // drawer
        #region Primary Hands
        PrimaryHandsFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(PrimaryHandsFoldout, "Primary Hands");
        if (PrimaryHandsFoldout)
        {
            EditorGUILayout.PropertyField(PrimaryAttachPoint);

            #region Primary Attach Point Creation
            if (PrimaryAttachPoint.objectReferenceValue == null)
            {
                EditorGUILayout.HelpBox("Primary Attach Point is not assigned", MessageType.Warning);
                // create a button to create a new attach point
                if (GUILayout.Button("Create Primary Attach Point"))
                {
                    // create from prefab
                    GameObject primaryAttachPointPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/XR Toolkit Extension/Editor/EditorPrefabs/AttachPoint.prefab", typeof(GameObject));
                    GameObject primaryAttachPointGameObject = (GameObject)PrefabUtility.InstantiatePrefab(primaryAttachPointPrefab);
                    primaryAttachPointGameObject.transform.parent = ((XRGrabPose)target).transform;
                    primaryAttachPointGameObject.transform.localPosition = Vector3.zero;
                    primaryAttachPointGameObject.transform.localRotation = Quaternion.identity;
                    //set scale to 5
                    primaryAttachPointGameObject.transform.localScale = new Vector3(3, 3, 3);
                    primaryAttachPointGameObject.name = "PrimaryAttachPoint";
                    PrimaryAttachPoint.objectReferenceValue = primaryAttachPointGameObject.transform;
                    serializedObject.ApplyModifiedProperties();

                    grabInteractable.attachTransform = primaryAttachPointGameObject.transform;

                    // set editor focus to the new object
                    Selection.activeGameObject = primaryAttachPointGameObject;

                }
            }
            #endregion
            #region Primary Attach Point Validation
            else if (PrimaryAttachPoint.objectReferenceValue != null)
            {
                EditorGUILayout.HelpBox("Primary Attach Point is assigned", MessageType.Info);
                // Ensure the GameObject has been assigned
                Transform primaryAttachPointTransform = (Transform)PrimaryAttachPoint.objectReferenceValue;
                GameObject primaryAttachPointGameObject = primaryAttachPointTransform.gameObject;
                Transform rightGrabPoseTransform = primaryAttachPointGameObject.transform.Find("RightGrabPose");
                Transform leftGrabPoseTransform = primaryAttachPointGameObject.transform.Find("LeftGrabPose");

                // Check if the RightGrabPose and LeftGrabPose GameObjects are found
                if (rightGrabPoseTransform != null && leftGrabPoseTransform != null)
                {
                    rightHandPose.objectReferenceValue = rightGrabPoseTransform.gameObject;
                    leftHandPose.objectReferenceValue = leftGrabPoseTransform.gameObject;

                    EditorGUILayout.PropertyField(rightHandPose);
                    EditorGUILayout.PropertyField(leftHandPose);


                }
                else
                {
                    // Optionally, log an error or warning if the objects are not found
                    if (rightGrabPoseTransform == null)
                    {
                        Debug.LogWarning("RightGrabPose GameObject not found.");
                    }
                    if (leftGrabPoseTransform == null)
                    {
                        Debug.LogWarning("LeftGrabPose GameObject not found.");
                    }
                }
            }
            #endregion
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        #endregion

        #region Secondary Hands
        SecondaryHandsFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(SecondaryHandsFoldout, "Secondary Hands");
        if (SecondaryHandsFoldout)
        {
            EditorGUILayout.PropertyField(MultipleHands);
            if (MultipleHands.boolValue)
            {
                grabInteractable.selectMode = InteractableSelectMode.Multiple;
                if (SecondaryHandsFoldout && MultipleHands.boolValue)
                {
                    EditorGUILayout.PropertyField(SecondaryAttachPoint);

                    if (SecondaryAttachPoint.objectReferenceValue == null)
                    {
                        EditorGUILayout.HelpBox("Primary Attach Point is not assigned", MessageType.Warning);
                        // create a button to create a new attach point
                        if (GUILayout.Button("Create Primary Attach Point"))
                        {
                            // create from prefab
                            GameObject SecondaryAttachPointPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/XR Toolkit Extension/Editor/EditorPrefabs/AttachPoint.prefab", typeof(GameObject));
                            GameObject SecondaryAttachPointGameObject = (GameObject)PrefabUtility.InstantiatePrefab(SecondaryAttachPointPrefab);
                            SecondaryAttachPointGameObject.transform.parent = ((XRGrabPose)target).transform;
                            SecondaryAttachPointGameObject.transform.localPosition = Vector3.zero;
                            SecondaryAttachPointGameObject.transform.localRotation = Quaternion.identity;
                            //set scale to 5
                            SecondaryAttachPointGameObject.transform.localScale = new Vector3(3, 3, 3);
                            SecondaryAttachPointGameObject.name = "SecondaryAttachPoint";
                            SecondaryAttachPoint.objectReferenceValue = SecondaryAttachPointGameObject.gameObject;
                            serializedObject.ApplyModifiedProperties();

                            grabInteractable.secondaryAttachTransform = SecondaryAttachPointGameObject.transform;

                            // set editor focus to the new object
                            Selection.activeGameObject = SecondaryAttachPointGameObject;

                        }
                    }
                    else if (SecondaryAttachPoint.objectReferenceValue != null)
                    {
                        EditorGUILayout.HelpBox("Secondary Attach Point is assigned", MessageType.Info);
                        // Ensure the GameObject has been assigned
                        Transform secondaryAttachPointTransform = (Transform)SecondaryAttachPoint.objectReferenceValue;
                        GameObject secondaryAttachPointGameObject = secondaryAttachPointTransform.gameObject;
                        Transform rightGrabPoseTransformSecond = secondaryAttachPointGameObject.transform.Find("RightGrabPose");
                        Transform leftGrabPoseTransformSecond = secondaryAttachPointGameObject.transform.Find("LeftGrabPose");

                        // Check if the RightGrabPoseSecond and LeftGrabPoseSecond GameObjects are found
                        if (rightGrabPoseTransformSecond != null && leftGrabPoseTransformSecond != null)
                        {
                            rightHandPoseSecondGrab.objectReferenceValue = rightGrabPoseTransformSecond.gameObject;
                            leftHandPoseSecondGrab.objectReferenceValue = leftGrabPoseTransformSecond.gameObject;

                            EditorGUILayout.PropertyField(rightHandPoseSecondGrab);
                            EditorGUILayout.PropertyField(leftHandPoseSecondGrab);
                        }
                        else
                        {
                            // Optionally, log an error or warning if the objects are not found
                            if (rightGrabPoseTransformSecond == null)
                            {
                                Debug.LogWarning("RightGrabPose GameObject not found.");
                            }
                            if (leftGrabPoseTransformSecond == null)
                            {
                                Debug.LogWarning("LeftGrabPose GameObject not found.");
                            }
                        }
                    }
                } 
            }
            else
            {
                grabInteractable.selectMode = InteractableSelectMode.Single;
            }
        }

        EditorGUILayout.EndFoldoutHeaderGroup();
        #endregion




        serializedObject.ApplyModifiedProperties();
    }




    void OnSceneGUI()
    {
        XRGrabPose xrGrabPose = (XRGrabPose)target;
        Transform primaryAttachPoint = xrGrabPose.PrimaryAttachPoint;
        Transform secondaryAttachPoint = xrGrabPose.SecondaryAttachPoint;

        Rigidbody rb = xrGrabPose.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = xrGrabPose.gameObject.AddComponent<Rigidbody>();
        }

        if (primaryAttachPoint != null)
        {
            Handles.color = Color.green;
            Handles.SphereHandleCap(0, primaryAttachPoint.position, primaryAttachPoint.rotation, 0.1f, EventType.Repaint);
        }

        if (secondaryAttachPoint != null)
        {
            // draw a sphere

            Handles.color = Color.red;
            Handles.SphereHandleCap(0, secondaryAttachPoint.position, secondaryAttachPoint.rotation, 0.1f, EventType.Repaint);
            
        }

        // draw a sphere at the center of mass
        Handles.color = Color.blue;
        Handles.SphereHandleCap(0, rb.worldCenterOfMass, rb.rotation, 0.1f, EventType.Repaint);


    }



}
