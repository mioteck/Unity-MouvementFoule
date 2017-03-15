using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerializeTest : MonoBehaviour {

    public GameObject prefab;

	void Start () {

        DNAMonster dna = new DNAMonster(Vector3.zero, 0);

        GameObject monster = Instantiate(prefab, Vector3.zero, new Quaternion(0, 0, 0, 0));
        Monster monsterComponent = monster.GetComponent<Monster>();
        monsterComponent.initMonster(dna, LayerMask.NameToLayer("physx1"));

        string dnaStr1 = DNASerializer.serialize(dna);
        DNAMonster dnaMonster = DNASerializer.deserializeMonster(dnaStr1);
        string dnaStr2 = DNASerializer.serialize(dnaMonster);

        if (dnaMonster.getBodyPart() == null)
        {
            Debug.Log("dna monster body part = null");
        }

        Debug.Log("DNA Written == DNA Loaded ===> " + dnaStr1.Equals(dnaStr2));

        DNASerializer.writeDNAToFile(dna, "dna.txt");
        DNAMonster dnaMonster2 = DNASerializer.loadDNAFromFile("dna.txt");

        GameObject monster2 = Instantiate(prefab, Vector3.zero + 4 * Vector3.right, new Quaternion(0, 0, 0, 0));
        Monster monsterComp2 = monster2.GetComponent<Monster>();

        if (dnaMonster2.getBodyPart() == null)
        {
            Debug.Log("dna mosnter2 body part = null");
        }


        monsterComp2.initMonster(dnaMonster2, LayerMask.NameToLayer("physx1") + 1);
    }
}
