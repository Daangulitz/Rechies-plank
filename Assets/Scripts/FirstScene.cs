using System;
using UnityEngine;

public class FirstScene : MonoBehaviour
{
    [SerializeField] private GameObject Doll1;
    public GameObject SFX;

    private void Start()
    {
        SFX.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        Doll1.SetActive(true);
        SFX.SetActive(true);
    }
}
