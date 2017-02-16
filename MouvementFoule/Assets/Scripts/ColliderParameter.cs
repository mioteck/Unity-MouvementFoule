using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderParameter : MonoBehaviour {
    [Space]
    public float SeparationRadius = 3f;
    [Space]
    public float CohesionRadius = 20f;  

	// Use this for initialization
	void Start () {
        SphereCollider[] allSphereCollider = FindObjectsOfType<SphereCollider>();
        foreach(SphereCollider SC in allSphereCollider)
        {
            if(SC.tag == "Separation")
            {
                SC.radius = SeparationRadius;
            }
            if(SC.tag == "Cohesion")
            {
                SC.radius = CohesionRadius;
            }
        }
	}
}
