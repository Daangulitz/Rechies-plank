using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handData : MonoBehaviour
{
    public enum HandType
    {
        Left,
        Right
    }

    [SerializeField] public HandType handedness;
    [SerializeField] public Transform root;
    [SerializeField] public Animator animator;
    [SerializeField] public Transform[] fingerBones;
}
