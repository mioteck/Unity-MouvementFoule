using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

[Serializable]
public struct DNAMonsterSer
{
    public BodyPart bodyPart;
    public Vector3 parentAnchor;
    public int score;
    public Vector3[] anchors;
    public DNAMonsterSer[] children;
}

[Serializable]
public struct BodyPartSer
{
    public Vector3 size;
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
        DNAMonsterSer receivedDNA = JsonUtility.FromJson<DNAMonsterSer>(dnaStr);

        return translateDNAMonsterSer(receivedDNA);
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
        serDna.bodyPart = dnaMonster.getBodyPart();
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

        return serDna;
    }
    
    private static DNAMonster translateDNAMonsterSer(DNAMonsterSer dna)
    {
        return new DNAMonster(dna.bodyPart, dna.children, dna.anchors, dna.parentAnchor);
    }
}
