using System;
using UnityEngine;

public class FirstScene : MonoBehaviour
{
    [SerializeField] private GameObject Doll1;

    private void Start()
    {
        Doll1.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        Doll1.SetActive(true);
    }
}
