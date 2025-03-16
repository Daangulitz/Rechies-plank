using System;
using UnityEngine;

public class SecondeScene : MonoBehaviour
{
    [SerializeField] private GameObject Doll1;
    [SerializeField] private GameObject Doll2;

    private void Start()
    {
        Doll1.SetActive(false);
        Doll2.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        Doll1.SetActive(false);
        Doll2.SetActive(true);
    }
}
