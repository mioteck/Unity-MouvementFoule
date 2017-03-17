using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public struct DNAMonsterSer
{
    public Vector3 bodyPartSize;
    public Vector3 parentAnchor;
    public int score;
    public Vector3[] anchors;
    public DNAMonsterSer[] children;
    public MoveActionSer_2 action;
}

[Serializable]
public struct MoveActionSer
{
    public List<ActionType> action;
    public List<int> power;
    public List<Vector3> axe;
}

[Serializable]
public struct MoveActionSer_2
{
    public float waveSpeedX;
    public float waveSpeedY;
    public float waveSpeedZ;

    public float frequencyX;
    public float frequencyY;
    public float frequencyZ;

    public Vector3 angularVelocityFactor;

    public MoveAction_2.MovementType movementType;

    public bool freezeNotOnGround;
}

// Utility class to serialize/deserialize monster's DNA
public class DNASerializer {

    // DNA -> string
    public static string serialize(DNAMonster dna)
    {
        return JsonUtility.ToJson(translateDNAMonster(dna));
    }

    // string -> DNA
    public static DNAMonster deserializeMonster(string dnaStr)
    {
        return translateDNAMonsterSer(JsonUtility.FromJson<DNAMonsterSer>(dnaStr));
    }

    // DNA -> file
    public static void writeDNAToFile(DNAMonster dna, string filename)
    {
        StreamWriter sr = File.CreateText(filename);
        sr.Write(serialize(dna));
        sr.Close();
    }

    // fileName -> DNA
    public static DNAMonster loadDNAFromFile(string filename)
    {
        if (File.Exists(filename))
        {
            string jsonStr = File.ReadAllText(filename);
            return deserializeMonster(jsonStr);
        }
        else
        {
            Debug.Log("Cannot find dna file " + filename);
            return null;
        }
    }

    private static DNAMonsterSer translateDNAMonster(DNAMonster dnaMonster)
    {
        DNAMonsterSer serDna = new DNAMonsterSer();
        serDna.bodyPartSize = dnaMonster.getBodyPart().getSize();
        serDna.parentAnchor = dnaMonster.getParentAnchor();
        serDna.score = dnaMonster.getScore();
        serDna.anchors = dnaMonster.getAnchor();

        if (dnaMonster.getChildren() != null)
        {
            serDna.children = new DNAMonsterSer[dnaMonster.getChildren().Length];

            for (int i = 0; i < serDna.children.Length; i++)
            {
                serDna.children[i] = translateDNAMonster(dnaMonster.getChildren()[i]);
            }
        }
        else
        {
            serDna.children = null;
        }

        serDna.action = new MoveActionSer_2();
        serDna.action.movementType = dnaMonster.getAction().movementType;
        serDna.action.angularVelocityFactor = dnaMonster.getAction().angularVelocityFactor;
        serDna.action.freezeNotOnGround = dnaMonster.getAction().freezeNotOnGround;

        serDna.action.frequencyX = dnaMonster.getAction().frequencyX;
        serDna.action.frequencyY = dnaMonster.getAction().frequencyY;
        serDna.action.frequencyZ = dnaMonster.getAction().frequencyZ;

        serDna.action.waveSpeedX = dnaMonster.getAction().waveSpeedX;
        serDna.action.waveSpeedY = dnaMonster.getAction().waveSpeedY;
        serDna.action.waveSpeedZ = dnaMonster.getAction().waveSpeedZ;

        return serDna;
    }
    
    private static DNAMonster translateDNAMonsterSer(DNAMonsterSer dna)
    {
        MoveAction_2 action = new MoveAction_2(dna.action);

        action.movementType = dna.action.movementType;
        action.angularVelocityFactor = dna.action.angularVelocityFactor;
        action.freezeNotOnGround = dna.action.freezeNotOnGround;
        action.frequencyX = dna.action.frequencyX;
        action.frequencyY = dna.action.frequencyY;
        action.frequencyZ = dna.action.frequencyZ;
        action.waveSpeedX = dna.action.waveSpeedX;
        action.waveSpeedY = dna.action.waveSpeedY;
        action.waveSpeedZ = dna.action.waveSpeedZ;

        return new DNAMonster(action, new BodyPart(dna.bodyPartSize), dna.children, dna.anchors, dna.parentAnchor);
    }
}
