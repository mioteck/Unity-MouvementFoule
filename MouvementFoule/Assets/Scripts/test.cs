using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

    public GameObject parent;
    public string t = "";

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && other.gameObject.GetInstanceID() != parent.GetInstanceID())
        {
            Debug.Log(t);
        }
            
    }
}
