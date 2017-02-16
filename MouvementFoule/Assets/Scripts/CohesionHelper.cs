using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CohesionHelper : MonoBehaviour
{

    public GameObject parent;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && other.gameObject.GetInstanceID() != parent.GetInstanceID())
        {
        }

    }
}
