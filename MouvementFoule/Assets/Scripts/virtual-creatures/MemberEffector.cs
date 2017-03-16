using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemberEffector : MonoBehaviour {

    [SerializeField]
    Rigidbody physic;

    public Vector3 angularForce;

    public bool constraintRotationX;
    public bool constraintRotationY;
    public bool constraintRotationZ;

    void Start()
    {
        if (constraintRotationX)
            physic.constraints |= RigidbodyConstraints.FreezeRotationX;
        if (constraintRotationY)
            physic.constraints |= RigidbodyConstraints.FreezeRotationY;
        if (constraintRotationZ)
            physic.constraints |= RigidbodyConstraints.FreezeRotationZ;
    }

    void FixedUpdate()
    {
        physic.angularVelocity = angularForce;
    }

}
