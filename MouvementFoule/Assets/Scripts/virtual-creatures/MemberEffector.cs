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
        Refresh();
    }

    void FixedUpdate()
    {
        physic.angularVelocity = angularForce;
    }

    public void Refresh()
    {
        if (constraintRotationX)
            physic.constraints |= RigidbodyConstraints.FreezeRotationX;
        if (constraintRotationY)
            physic.constraints |= RigidbodyConstraints.FreezeRotationY;
        if (constraintRotationZ)
            physic.constraints |= RigidbodyConstraints.FreezeRotationZ;
    }

}
