using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {
    public GameObject prefab;
    private DNAMonster dna;
    private GameObject[] go;
    private Vector3 rootLocation;
    private int score;
    private Vector3 startPos, endPos;
    private int id;

    // Use this for initialization
    void Start () {
        id = 0;
        dna = new DNAMonster(Vector3.zero);
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
        rootLocation = gameObject.transform.position;
        Quaternion worldQuaternion = new Quaternion(0, 0, 0, 0);
        go[id] = new GameObject();
        go[id] = Instantiate(prefab, rootLocation, worldQuaternion);
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
        if (subDna.getChildren() != null){
            int nbChildren = subDna.getChildren().Length;
            for (int i = 0; i < nbChildren; ++i)
            {
                createCube(go[fatherID], subDna.getChild(i));
                //createJoint(subDna.getJoint(i), go[fatherID], go[id-1]);
                createMonster(subDna.getChild(i), id - 1);
            }
        }
    }
    /// <summary>
    /// create the cube according to the ADN (relative placement according to joint parameters
    /// </summary>
    /// <param name="parentBodypart"></param>
    /// <param name="joint"></param>
    public void createCube(GameObject parentBodypart, DNAMonster subDna)
    {
        //Vector3 childWorldLocation = new Vector3(0, 0, 0);
        Quaternion childWorldQuaternion = new Quaternion(0, 0, 0, 0);
        //calcul position/rotation/scale of the bp to instantiate from parent bodyPart and the joint
        Vector3 a = subDna.getParentAnchor();
        Vector3 s = subDna.getBodyPart().getSize();
        Vector3 childWorldLocation = parentBodypart.transform.position + new Vector3(a.x*s.x, a.y*s.y, a.z*s.z);
        //instanciation du monstre
        go[id] = Instantiate(prefab, rootLocation, childWorldQuaternion);
        go[id].GetComponent<Rigidbody>().GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
        id++;
    }
    /// <summary>
    /// create joint according to parameters
    /// </summary>
    public void createJoint(Joint joint, GameObject parent, GameObject child)
    {

    }

    /// <summary>
    /// try to move one monster
    /// </summary>
    public void run()
    {

    }

    //accessors
    public int getScore()
    {
        return score;
    }
    public void setScore(int newScore)
    {
        score = newScore;
    }
    public DNAMonster getDna()
    {
        return dna;
    }
    public void setDna(DNAMonster newDna)
    {
        dna = newDna;
    }
}
