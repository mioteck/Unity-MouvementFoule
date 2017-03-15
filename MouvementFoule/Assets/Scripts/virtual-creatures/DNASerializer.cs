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
    public MoveActionSer action;
}

[Serializable]
public struct MoveActionSer
{
    public List<ActionType> action;
    public List<int> power;
    public List<Vector3> axe;
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

        serDna.action = new MoveActionSer();
        serDna.action.action = new List<ActionType>();
        foreach (ActionType actionType in dnaMonster.getAction().action)
        {
            serDna.action.action.Add(actionType);
        }

        serDna.action.axe = new List<Vector3>();
        foreach (Vector3 v in dnaMonster.getAction().axe)
        {
            serDna.action.axe.Add(v);
        }

        serDna.action.power = new List<int>();
        foreach (int power in dnaMonster.getAction().power)
        {
            serDna.action.power.Add(power);
        }

        return serDna;
    }
    
    private static DNAMonster translateDNAMonsterSer(DNAMonsterSer dna)
    {
        MoveAction action = new MoveAction(dna.action.action, dna.action.power, dna.action.axe);
        return new DNAMonster(action, new BodyPart(dna.bodyPartSize), dna.children, dna.anchors, dna.parentAnchor);
    }
}
