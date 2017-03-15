using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {
    //time of life
    public static float LifeDuration = 8;
    //cube Prefab
    public GameObject prefab;
    // True when one of the joints of the monster breaks
    public bool isBroken;
    //dna of one monster (use by genetic algorith)
    private DNAMonster dna;
    //list of cube gameobject generate from dna
    private GameObject[] go;
    //true if monster have been generated
    private bool isGenerate;
    // Physic layer in which the monster is simulated
    private int physxLayer;
    //others
    private int id;
    private float timeCount;
    private float startTime;
    private Vector3 startPos;

    // Use this for initialization
    void Awake()
    {
        isGenerate = false;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (isGenerate)
        {
            if (Time.time - startTime > LifeDuration || isBroken)
            {
                disable();
            }
            else
            {
                timeCount += Time.deltaTime;
                if (timeCount > 1.0f)
                    timeCount = -1.0f;
                run(timeCount);
            }
        }
	}

    // Disable the current monster and all its body parts
    void disable()
    {
        for (int i = 0; i < go.Length; i++)
        {
            go[i].SetActive(false);
        }
        gameObject.SetActive(false);
    }

    /// <summary>
    /// initialise the first cube of the monster and launch the creating process
    /// </summary>
    public void initMonster(DNAMonster AGDna, int physxLayer)
    {
        this.physxLayer = physxLayer;
        //init var of monster
        id = 0;
        timeCount = 0;
        startTime = Time.time;
        startPos = gameObject.transform.position;
        dna = new DNAMonster(AGDna);
        go = new GameObject[dna.getSize()];
        //need to instanciate the first bodypart 
        go[id] = Instantiate(prefab, startPos, new Quaternion(0, 0, 0, 0));
        go[id].layer = physxLayer;

        go[id].transform.localScale = dna.getBodyPart().getSize();
        if (dna.getChildren() != null)
        {
            int nbChildren = dna.getChildren().Length;
            for (int i = 0; i < nbChildren; ++i)
            {
                createMonster(dna.getChildren()[i], 0);
            }
        }
        isGenerate = true;
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
                createMonster(subDna.getChildren()[i], tempID);
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
        go[id].layer = physxLayer;
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
            addBreakDetectionTo(parent);
            //general setup
            joint.autoConfigureConnectedAnchor = true;
            joint.enableCollision = false;
            joint.connectedBody = child.GetComponent<Rigidbody>();
            Vector3 a = childDna.getParentAnchor();
            Vector3 ps = parent.transform.localScale;
            Vector3 anchor = new Vector3(a.x * (ps.x / 2), a.y * (ps.y / 2), a.z * (ps.z / 2));
            joint.anchor = anchor;
            joint.axis = new Vector3(a.x+0.2f, a.y+0.2f, a.z+0.2f).normalized;
            //joint.swingAxis = new Vector3(Mathf.Abs(a.x), Mathf.Abs(a.y), Mathf.Abs(a.z));
            //configure lowTwistLimit
            softJointLimit.limit = -180;
            softJointLimit.bounciness = 1;
            softJointLimit.contactDistance = 0;
            joint.lowTwistLimit = softJointLimit;
            //configure highTwistLimit
            softJointLimit.limit = 180;
            softJointLimit.bounciness = 1;
            softJointLimit.contactDistance = 0;
            joint.highTwistLimit = softJointLimit;
            //configure SwingLimitSpring
            softJointLimitSpring.spring = 0;
            softJointLimitSpring.damper = 0;
            joint.swingLimitSpring = softJointLimitSpring;
            //configure Swing 1 limit
            softJointLimit.limit = 0.2f;
            softJointLimit.bounciness = 0;
            softJointLimit.contactDistance = 0;
            joint.swing1Limit = softJointLimit;
            //configure Swing 2 limit
            softJointLimit.limit = 0.2f;
            softJointLimit.bounciness = 0;
            softJointLimit.contactDistance = 0;
            joint.swing2Limit = softJointLimit;
            //configure resistance of the joint
            joint.breakForce = 3000;
            joint.breakForce = 3000;
        }
    }

    void addBreakDetectionTo(GameObject go)
    {
        BreakDetector breakDetector = go.AddComponent<BreakDetector>();
        breakDetector.owner = this;
    }

    /// <summary>
    /// try to move one monster
    /// </summary>
    public void run(float t)
    {
        int i = 1;
        int size = dna.getSize();
        foreach (GameObject g in go)
        {
            if (i < size)
            {
                Vector3 torque = dna.getSubDna(i).getAction().getComputeTorque(t);
                if (torque != Vector3.zero)
                    g.GetComponent<Rigidbody>().AddRelativeTorque(torque);
                i++;
            }
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
    public DNAMonster getDna()
    {
        return dna;
    }
    public void setDna(DNAMonster newDna)
    {
        dna = newDna;
    }

    public Vector3 getPosition()
    {
        return go[0].transform.position;
    }

    public Vector3 getStartPos()
    {
        return startPos;
    }
}
