using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetVelocity : MonoBehaviour {

    [SerializeField]
    Rigidbody physic;

    [SerializeField]
    Vector3 angularVelocity;

    [SerializeField]
    float speed;

    [SerializeField]
    bool useSinus;

    [SerializeField]
    float frequecy;

    [SerializeField]
    float offset;

    public float frameVelocity;

	// Use this for initialization
	void Start () {
        frameVelocity = 0f;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate()
    {
        if (useSinus)
        {
            frameVelocity = Mathf.Sin(offset + Time.time * frequecy) * speed;
        }
        else
        {
            frameVelocity = Mathf.Cos(offset + Time.time * frequecy) * speed;
        }

        physic.angularVelocity = new Vector3(frameVelocity, angularVelocity.y, angularVelocity.z);
    }


}
