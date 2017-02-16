using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeparationHelper : MonoBehaviour
{

    public GameObject parent;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && other.gameObject.GetInstanceID() != parent.GetInstanceID())
        {
            Vector3 direction = parent.transform.position - other.gameObject.transform.position;
        }
    }
}
