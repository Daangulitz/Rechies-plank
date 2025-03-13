using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
public class LayerManager
{
    public static void AddRequiredLayers()
    {
        AddLayer("Player");
        AddLayer("LeftHand");
        AddLayer("RightHand");
        AddLayer("Interactable");

        // update the physics matrix so player, left hand and right hand can not collide with each other
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("LeftHand"), true);
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Player"), true);
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("RightHand"), true);
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("LeftHand"), LayerMask.NameToLayer("RightHand"), true);
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("RightHand"), LayerMask.NameToLayer("RightHand"), true);
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("LeftHand"), LayerMask.NameToLayer("LeftHand"), true);
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Interactable"), LayerMask.NameToLayer("Interactable"), true);
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Interactable"), LayerMask.NameToLayer("Player"), true);


    }

    private static void AddLayer(string layerName)
    {
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty layersProp = tagManager.FindProperty("layers");

        bool layerExists = false;

        for (int i = 0; i < layersProp.arraySize; i++)
        {
            SerializedProperty layerProp = layersProp.GetArrayElementAtIndex(i);
            if (layerProp.stringValue == layerName)
            {
                layerExists = true;
                break;
            }
        }

        if (!layerExists)
        {
            for (int i = 8; i < layersProp.arraySize; i++)
            {
                SerializedProperty layerProp = layersProp.GetArrayElementAtIndex(i);
                if (string.IsNullOrEmpty(layerProp.stringValue))
                {
                    layerProp.stringValue = layerName;
                    tagManager.ApplyModifiedProperties();
                    Debug.Log($"Layer added: {layerName}");
                    return;
                }
            }

            Debug.LogError("Failed to add layer. Maximum number of layers reached.");
        }
    }
}

public class XRSetupHelper : EditorWindow   
{
    string basePath = "Assets/XR Toolkit Extension/Editor/EditorPrefabs";
    private enum XRType {
        NormalHands,
        PhysicsHands
    }

    private XRType xrType = XRType.PhysicsHands;

    [MenuItem("XR/XR Setup Helper")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(XRSetupHelper));
    }

    private void OnGUI()
    {
        GUILayout.Label("XR Setup Helper", EditorStyles.boldLabel);
        // description
        EditorGUILayout.HelpBox("Choose the type of XR Rig you would like to create.", MessageType.Info);
        xrType = (XRType)EditorGUILayout.EnumPopup("XR Type", xrType);

        switch (xrType)
        {
            case XRType.NormalHands:
                NormalHands();
                break;
            case XRType.PhysicsHands:
                PhysicsHands();
                break;
            default:
                xrType = XRType.PhysicsHands;
                break;
        }
    }

    private void NormalHands()
    {
        ToBeDoneMessage();
    }

    private void PhysicsHands()
    {
        bool isLayerSet = false;
        bool isPhysicsRigCreated = false;
        bool isEventManagerCreated = false;
        bool isScreenFaderManagerCreated = false;

        if (GUILayout.Button("Set Layers"))
        {
            setLayers();
            Debug.Log("Layers Set");
            isLayerSet = true;
        }
        
        if (GUILayout.Button("Create Physics Rig"))
        {
            CreatePhysicsRig();
            Debug.Log("Physics Rig Created");
            isPhysicsRigCreated = true;
        }

        if (GUILayout.Button("Create Event Manager"))
        {
            setEventManager();
            Debug.Log("Event Manager Created");
            isEventManagerCreated = true;
        }

        if (GUILayout.Button("Create Screen Fader Manager"))
        {
            setScreenFaderManager();
            Debug.Log("Screen Fader Manager Created");
            isScreenFaderManagerCreated = true;
        }

        if (isLayerSet && isPhysicsRigCreated && isEventManagerCreated && isScreenFaderManagerCreated)
        {
            EditorGUILayout.HelpBox("XR Setup Completed", MessageType.Info);
        }
    }

    private void ToBeDoneMessage()
    {
        EditorGUILayout.HelpBox("To Be Done", MessageType.Warning);
    }
    public static Transform FindDeepChild(Transform aParent, string aName)
    {
        foreach (Transform child in aParent)
        {
            if (child.name == aName)
                return child;
            Transform result = FindDeepChild(child, aName);
            if (result != null)
                return result;
        }
        return null;
    }

    private void CreatePhysicsRig(){
        // Add the required layers

        
        // instantiate the XROrigin, lefthandpresancer, righthandpresancer, eventsystem Prefabs
        GameObject XROrigin = (GameObject)PrefabUtility.InstantiatePrefab((GameObject)AssetDatabase.LoadAssetAtPath("Assets/XR Toolkit Extension/Editor/EditorPrefabs/XR Origin (XR Rig).prefab", typeof(GameObject)));
        GameObject LeftHandPresence = (GameObject)PrefabUtility.InstantiatePrefab((GameObject)AssetDatabase.LoadAssetAtPath("Assets/XR Toolkit Extension/Editor/EditorPrefabs/Left Hand Presence Variant.prefab", typeof(GameObject)));
        GameObject RightHandPresence = (GameObject)PrefabUtility.InstantiatePrefab((GameObject)AssetDatabase.LoadAssetAtPath("Assets/XR Toolkit Extension/Editor/EditorPrefabs/Right Hand Presence Variant.prefab", typeof(GameObject)));
        
        // set the player layer
        XROrigin.layer = LayerMask.NameToLayer("Player");
        // set the left hand layer
        LeftHandPresence.layer = LayerMask.NameToLayer("LeftHand");
        // get all children and set their layer
        GameObject ColliderLeftHand = LeftHandPresence.transform.Find("Custom Left Hand Model with Collider").gameObject;
        ColliderLeftHand.layer = LayerMask.NameToLayer("LeftHand");

        int leftHandLayer = LayerMask.NameToLayer("LeftHand");
        SetLayerRecursively(ColliderLeftHand, leftHandLayer);



        // set the right hand layer
        RightHandPresence.layer = LayerMask.NameToLayer("RightHand");
        // get all children and set their layer
        GameObject ColliderRightHand = RightHandPresence.transform.Find("Custom Right Hand Model with Collider").gameObject;
        ColliderRightHand.layer = LayerMask.NameToLayer("RightHand");

        // loop through all children and set their layer
        int rightHandLayer = LayerMask.NameToLayer("RightHand");
        SetLayerRecursively(ColliderRightHand, rightHandLayer);

        // Usage
        Transform LeftController = FindDeepChild(XROrigin.transform, "Left Controller");
        Transform RightController = FindDeepChild(XROrigin.transform, "Right Controller");

        // Check if LeftHandPresence and RightHandPresence are not null
        if (LeftHandPresence != null && RightHandPresence != null)
        {
            // Attempt to get the handPhysics component from LeftHandPresence
            var leftHandPhysics = LeftHandPresence.GetComponent<handPhysics>();
            if (leftHandPhysics != null && LeftController != null)
            {
                leftHandPhysics.target = LeftController;
            }
            else
            {
                Debug.LogError("LeftHandPresence handPhysics component or LeftController is null.");
            }

            // Repeat the process for RightHandPresence
            var rightHandPhysics = RightHandPresence.GetComponent<handPhysics>();
            if (rightHandPhysics != null && RightController != null)
            {
                rightHandPhysics.target = RightController;
            }
            else
            {
                Debug.LogError("RightHandPresence handPhysics component or RightController is null.");
            }
        }
        else
        {
            Debug.LogError("LeftHandPresence or RightHandPresence is null.");
        }

    }

    private void setLayers(){
        LayerManager.AddRequiredLayers();

    }

    private void setEventManager(){
        GameObject EventSystem = (GameObject)PrefabUtility.InstantiatePrefab((GameObject)AssetDatabase.LoadAssetAtPath("Assets/XR Toolkit Extension/Editor/EditorPrefabs/EventSystem.prefab", typeof(GameObject)));

    }

    private void setScreenFaderManager(){
        GameObject ScreenFaderManager = (GameObject)PrefabUtility.InstantiatePrefab((GameObject)AssetDatabase.LoadAssetAtPath("Assets/XR Toolkit Extension/Editor/EditorPrefabs/Screen Fader Manager.prefab", typeof(GameObject)));

        ScreenFaderManager.GetComponent<ScreenFaderManager>().screenFader = GameObject.Find("Screen Fader").GetComponent<ScreenFader>();
    }


    void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }


}
