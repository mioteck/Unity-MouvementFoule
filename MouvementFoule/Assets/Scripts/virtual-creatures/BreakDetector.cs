using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Detect broken joints and set a broken flag on the corresponding monster
public class BreakDetector : MonoBehaviour {

    public Monster owner;

    void OnJointBreak(float breakForce)
    {
        owner.isBroken = true;
    }
}
