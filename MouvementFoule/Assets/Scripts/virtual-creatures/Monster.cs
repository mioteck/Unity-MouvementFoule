using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {
    public static float LifeDuration = 10;
    //cube Prefab
    public GameObject prefab;
    //dna of one monster (use by genetic algorith)
    private DNAMonster dna;
    //list of cube gameobject generate from dna
    private GameObject[] go;
    //others
    private int id;
    private int score;
    private float count;
    private float startTime;
    private Vector3 startPos, endPos;
    

    // Use this for initialization
    void Start () {
        id = 0;
        count = 0;
        startTime = Time.time;
        startPos = gameObject.transform.position;
        dna = new DNAMonster(Vector3.zero, 0);
        go = new GameObject[dna.getSize()];
        initMonster();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if(Time.time - startTime > LifeDuration)
        {
            //destroy
            destroyMonster();
        }
        else
        {
            run(count);
            count += 0.1f;
            if (count > 1.0f)
            {
                count = 0.0f;
            }
        }
	}

    /// <summary>
    /// initialise the first cube of the monster and launch the creating process
    /// </summary>
    public void initMonster()
    {
        //need to instanciate the first bodypart 
        go[id] = Instantiate(prefab, startPos, new Quaternion(0, 0, 0, 0));
        go[id].transform.localScale = dna.getBodyPart().getSize();
        if (dna.getChildren() != null)
        {
            int nbChildren = dna.getChildren().Length;
            for (int i = 0; i < nbChildren; ++i)
            {
                createMonster(dna.getChild(i), 0);
            }
        }
    }
    /// <summary>
    /// generate all the part of the monster recursively
    /// </summary>
    /// <param name="subDna"></param>
    /// <param name="fatherID"></param>
    public void createMonster(DNAMonster subDna, int fatherID)
    {
        id++;
        createCube(go[fatherID], subDna);
        createJoint(go[fatherID], go[id], subDna);
        int tempID = id;
        if (subDna.getChildren() != null){
            int nbChildren = subDna.getChildren().Length;
            for (int i = 0; i < nbChildren; ++i)
            {
                createMonster(subDna.getChild(i), tempID);
            }
        }
    }
    /// <summary>
    /// create the cube according to the ADN (relative placement according to joint parameters
    /// </summary>
    /// <param name="parentBodypart"></param>
    /// <param name="joint"></param>
    public void createCube(GameObject parent, DNAMonster subDna)
    {
        //calcul position/rotation/scale of the bp to instantiate from parent bodyPart and the joint
        //Vector3 a = subDna.invertAnchor(subDna.getParentAnchor());
        Vector3 a = subDna.getParentAnchor();
        Vector3 cs = subDna.getBodyPart().getSize();
        Vector3 ps = parent.transform.localScale;
        Vector3 childWorldLocation = parent.transform.position + new Vector3(a.x* ((cs.x + ps.x) / 2), a.y* ((cs.y + ps.y) / 2), a.z* ((cs.z + ps.z) / 2));
        //instanciation du monstre
        go[id] = Instantiate(prefab, Vector3.zero, new Quaternion(0, 0, 0, 0));
        go[id].transform.position = childWorldLocation;
        go[id].transform.localScale = cs;
    }
    /// <summary>
    /// create joint according to parameters
    /// </summary>
    public void createJoint(GameObject parent, GameObject child, DNAMonster childDna)
    {
        if (childDna.getParentAnchor() != Vector3.zero)
        {
            //init
            SoftJointLimit softJointLimit = new SoftJointLimit();
            SoftJointLimitSpring softJointLimitSpring = new SoftJointLimitSpring();
            CharacterJoint joint = parent.AddComponent<CharacterJoint>();
            //general setup
            joint.autoConfigureConnectedAnchor = true;
            joint.enableCollision = false;
            joint.connectedBody = child.GetComponent<Rigidbody>();
            Vector3 a = childDna.getParentAnchor();
            Vector3 ps = parent.transform.localScale;
            Vector3 anchor = new Vector3(a.x * (ps.x / 2), a.y * (ps.y / 2), a.z * (ps.z / 2));
            joint.anchor = anchor;
            //configure lowTwistLimit
            softJointLimit.limit = -1;
            softJointLimit.bounciness = 0;
            softJointLimit.contactDistance = 0;
            joint.lowTwistLimit = softJointLimit;
            //configure highTwistLimit
            softJointLimit.limit = 1;
            softJointLimit.bounciness = 0;
            softJointLimit.contactDistance = 0;
            joint.highTwistLimit = softJointLimit;
            //configure SwingLimitSpring
            softJointLimitSpring.spring = 40;
            softJointLimitSpring.damper = 40;
            joint.swingLimitSpring = softJointLimitSpring;
            //configure Swing 1 limit
            softJointLimit.limit = 20;
            softJointLimit.bounciness = 1;
            softJointLimit.contactDistance = 0;
            joint.swing1Limit = softJointLimit;
            //configure Swing 2 limit
            softJointLimit.limit = 20;
            softJointLimit.bounciness = 1;
            softJointLimit.contactDistance = 0;
            joint.swing2Limit = softJointLimit;
        }
    }

    /// <summary>
    /// try to move one monster
    /// </summary>
    public void run(float count)
    {
        foreach(GameObject g in go)
        {
            g.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(5, 20), Random.Range(-10, 20), Random.Range(-20, 20)));
        }
    }

    /// <summary>
    /// utility methode for destroying monster
    /// </summary>
    public void destroyMonster()
    {
        foreach (GameObject g in go)
        {
            Destroy(g);
        }
        Destroy(this);
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
