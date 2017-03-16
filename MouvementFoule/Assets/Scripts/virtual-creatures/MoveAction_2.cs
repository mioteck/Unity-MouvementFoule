using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction_2 {

    public enum MovementType { RIGID, WAVE, LINEAR }

    public float waveSpeedX = 0.0f;
    public float waveSpeedY = 0.0f;
    public float waveSpeedZ = 0.0f;
    
    public float frequencyX = 0.0f;
    public float frequencyY = 0.0f;
    public float frequencyZ = 0.0f;

    public Vector3 angularVelocityFactor = new Vector3(1.0f, 1.0f, 1.0f);
    public MovementType movementType = MovementType.RIGID;

    public bool freezeNotOnGround = false;

    public MoveAction_2(bool random)
    {
        if (random)
        {
            waveSpeedX = Random.Range(-15.0f, 15.0f);
            waveSpeedY = Random.Range(-15.0f, 15.0f);
            waveSpeedZ = Random.Range(-15.0f, 15.0f);

            frequencyX = Random.Range(0.0f, 20.0f);
            frequencyY = Random.Range(0.0f, 20.0f);
            frequencyZ = Random.Range(0.0f, 20.0f);

            angularVelocityFactor = new Vector3(
                Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f)
            );

            int rollthedice = Random.Range(0, 2);
            if (rollthedice == 0)
                movementType = MovementType.RIGID;
            else if (rollthedice == 1)
                movementType = MovementType.WAVE;
            else
                movementType = MovementType.LINEAR;

            freezeNotOnGround = (Random.Range(0.0f, 10.0f) > 5f);
        }
    }

    public MoveAction_2(MoveAction_2 copy)
    {
        waveSpeedX = copy.waveSpeedX;
        waveSpeedY = copy.waveSpeedY;
        waveSpeedZ = copy.waveSpeedZ;

        frequencyX = copy.frequencyX;
        frequencyY = copy.frequencyY;
        frequencyZ = copy.frequencyZ;

        angularVelocityFactor = copy.angularVelocityFactor;
        movementType = copy.movementType;

        freezeNotOnGround = copy.freezeNotOnGround;
    }

    public Vector3 getComputeTorque(float time, MemberSensor sensor)
    {
        if (freezeNotOnGround && ! sensor.isGrounded)
            return new Vector3(0.0f, 0.0f, 0.0f);

        Vector3 newAngularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
        if (movementType == MovementType.WAVE)
        {
            newAngularVelocity = new Vector3(
                Mathf.Sin(time * frequencyX) * waveSpeedX,
                Mathf.Sin(time * frequencyY) * waveSpeedY,
                Mathf.Sin(time * frequencyZ) * waveSpeedZ
            );
        }
        else if (movementType == MovementType.LINEAR)
        {
            newAngularVelocity = new Vector3(
                waveSpeedX,
                waveSpeedY,
                waveSpeedZ
            );
        }

        return new Vector3(
            newAngularVelocity.x * angularVelocityFactor.x,
            newAngularVelocity.x * angularVelocityFactor.y,
            newAngularVelocity.x * angularVelocityFactor.z
        );
    }

}
