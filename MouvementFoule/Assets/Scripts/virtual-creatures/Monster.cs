using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {
    public GameObject prefab;
    public DNAMonster dna;
    public GameObject[] go;
    public int score;
    private int id;
    public Vector3 startPos, endPos;

	// Use this for initialization
	void Start () {
        dna = new DNAMonster();
        id = 0;
        go = new GameObject[dna.getSize()];
        initMonster();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// initialise the first cube of the monster and launch the creating process
    /// </summary>
    public void initMonster()
    {
        //need to instanciate the first bodypart 
        Vector3 worldLocation = new Vector3(0, 0, 0);
        Quaternion worldQuaternion = new Quaternion(0, 0, 0, 0);
        go[id] = Instantiate(prefab, worldLocation, worldQuaternion);
        go[id].GetComponent<Rigidbody>().GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
        id++;
        createMonster(dna,0);
    }
    /// <summary>
    /// generate all the part of the monster recursively
    /// </summary>
    /// <param name="subDna"></param>
    /// <param name="fatherID"></param>
    public void createMonster(DNAMonster subDna, int fatherID)
    {
        int nbChildren = subDna.getChildren().Length;
        for(int i = 0; i < nbChildren; ++i)
        {
            createCube(go[fatherID], subDna.getJoint(i));
            createJoint(subDna.getJoint(i), go[fatherID], go[id]);
            createMonster(subDna.getChild(i), id-1);
        }
    }
    /// <summary>
    /// create the cube according to the ADN (relative placement according to joint parameters
    /// </summary>
    /// <param name="parentBodypart"></param>
    /// <param name="joint"></param>
    public void createCube(GameObject parentBodypart, Joint joint)
    {
        BodyPart childBP = joint.GetChild();
        Vector3 childWorldLocation = new Vector3(0, 0, 0);
        Quaternion childWorldQuaternion = new Quaternion(0, 0, 0, 0);
        //calcul position/rotation/scale of the bp to instantiate from parent bodyPart and the joint

        go[id] = Instantiate(prefab, childWorldLocation, childWorldQuaternion);
        go[id].GetComponent<Rigidbody>().GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
        id++;
    }
    /// <summary>
    /// create joint according to parameters
    /// </summary>
    public void createJoint(Joint joint, GameObject parent, GameObject child)
    {

    }
}
