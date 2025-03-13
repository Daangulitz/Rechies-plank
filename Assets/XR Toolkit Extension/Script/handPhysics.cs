using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handPhysics : MonoBehaviour
{
    [SerializeField] public Transform target;
    private Rigidbody rb;

    private Collider[] handColliders;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        handColliders = GetComponentsInChildren<Collider>();
    }

    public void enableHandColliders()
    {
        foreach (Collider col in handColliders)
        {
            if (col != null)
            {
                col.enabled = true;
            }
        }
    }

    public void disableHandColliders()
    {
        foreach (Collider col in handColliders)
        {
            if (col != null)
            {
                col.enabled = false;
            }
        }
    }

    public void enableHandCollidersDelayed(float delay)
    {
       Invoke("enableHandColliders", delay);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.linearVelocity = (target.position - transform.position) / Time.deltaTime;

        Quaternion rotationDifference = target.rotation * Quaternion.Inverse(transform.rotation);
        rotationDifference.ToAngleAxis(out float angle, out Vector3 axis);

        Vector3 rotationDiffInDegrees = angle * axis;

        rb.angularVelocity = (rotationDiffInDegrees * Mathf.Deg2Rad / Time.deltaTime);
    }
}
