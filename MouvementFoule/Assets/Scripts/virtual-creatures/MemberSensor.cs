using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemberSensor : MonoBehaviour {

    public bool isGrounded;

    [SerializeField]
    Rigidbody physic;

    public Rigidbody GetPhysic()
    {
        return physic;
    }

    void OnColliderEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnColliderExit(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
