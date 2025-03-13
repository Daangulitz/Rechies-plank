using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(handData))]
public class HandPoserHelper : Editor
{
    #region Serialized Properties
    SerializedProperty Handedness;
    SerializedProperty Root;
    SerializedProperty Animator;
    SerializedProperty FingerBones;

    // gizmo color, red, alpha 100/255
    Color GizmoColor = new Color(1, 0, 0, 100 / 255f);
    #endregion

    private void OnEnable()
    {
        Handedness = serializedObject.FindProperty("handedness");
        Root = serializedObject.FindProperty("root");
        Animator = serializedObject.FindProperty("animator");
        FingerBones = serializedObject.FindProperty("fingerBones");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(Handedness);
        EditorGUILayout.PropertyField(Root);
        EditorGUILayout.PropertyField(Animator);
        EditorGUILayout.PropertyField(FingerBones);
        // foldout gizmo settings
        GizmoColor = EditorGUILayout.ColorField("Gizmo Color", GizmoColor);

        serializedObject.ApplyModifiedProperties();
    }

    // when the object is selected is should draw the bones, and place a small shpere at the end of each bone, when the sphere is clicked it should show a rotation gizmo

    private int selectedBoneIndex = -1; // Index of the selected bone for rotation

    private void OnSceneGUI()
    {
      

        handData handData = (handData)target;
        Transform[] fingerBones = handData.fingerBones;
        if (fingerBones == null)
            return;

        Handles.color = GizmoColor; // Set the color for the gizmos

        for (int i = 0; i < fingerBones.Length; i++)
        {
            if (fingerBones[i] == null)
                continue;

            if (Handles.Button(fingerBones[i].position, Quaternion.identity, 0.01f, 0.01f, Handles.SphereHandleCap))
            {
                if (selectedBoneIndex == i) // Check if the clicked sphere is already selected
                {
                    selectedBoneIndex = -1; // Deselect the sphere
                    Tools.current = Tool.Move; // Show the normal gizmo again by resetting to the Move tool or any other default tool
                }
                else
                {
                    selectedBoneIndex = i; // Store the selected bone index
                    Tools.current = Tool.None; // Hide the normal gizmo
                }
            }
        }

        if (selectedBoneIndex >= 0 && selectedBoneIndex < fingerBones.Length && fingerBones[selectedBoneIndex] != null)
        {
            EditorGUI.BeginChangeCheck();
            Quaternion newRotation = Handles.RotationHandle(fingerBones[selectedBoneIndex].rotation, fingerBones[selectedBoneIndex].position);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(fingerBones[selectedBoneIndex], "Rotate Bone");
                fingerBones[selectedBoneIndex].rotation = newRotation;
            }
        }
    }
}
