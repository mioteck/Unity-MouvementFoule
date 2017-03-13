using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DNAMonster{
    public static int MAX_BODYPART = 10;
    public static int MAX_CHILDREN = 4; //<5 un cube n'a que 6 face!!!!!
    private BodyPart bodyPart;
    private DNAMonster[] children;
    private Joint[] joints;
    public Vector3[] anchor;
    public Vector3 parentAnchor;
    private int score = 0;

    public DNAMonster()
    {
        new DNAMonster(Vector3.zero);
    }
    public DNAMonster(Vector3 parentAnchor, int count = 0)
    {
        bodyPart = new BodyPart();
        int rand = 0;
        if(count < MAX_BODYPART)
        {
            rand = Random.Range(0, MAX_CHILDREN);
            children = new DNAMonster[rand];
            //joints = new Joint[rand];
            createAnchor(rand);
            for (int i = 0; i < rand; ++i)
            {
                ++count;
                children[i] = new DNAMonster(invertAnchor(anchor[i]), count);
                //joints[i] = new Joint(bodyPart, children[i].bodyPart);
            }
        }

    }
    public DNAMonster(DNAMonster dna)
    {
        bodyPart = dna.bodyPart;
        parentAnchor = dna.parentAnchor;
        if (dna.children != null)
        {
            children = new DNAMonster[dna.children.Length];
            anchor = new Vector3[dna.anchor.Length];
            //joints = new Joint[dna.joints.Length];
            for (int i = 0; i < dna.children.Length; ++i)
            {
                children[i] = new DNAMonster(dna.children[i]);
                anchor[i] = dna.anchor[i];
                //joints[i] = new Joint(dna.joints[i]);
            }
        }
    }


    public void createAnchor(int size)
    {
        anchor = new Vector3[size];
        bool test = false;
        int val = 0;
        for (int i = 0; i < size; i++)
        {
            while (!test)
            {
                test = true;
                val = Random.Range(0, 6);
                anchor[i] = associateIntToAnchor(val);
                if(parentAnchor == anchor[i])
                {
                    test = false;
                }
                for (int j = 0; j < i; j++)
                {
                    if (anchor[j] == anchor[i])
                    {
                        test = false;
                    }
                }
            }
        }
    }
    public Vector3 associateIntToAnchor(int val)
    {
        switch (val)
        {
            case 0:
                return Vector3.right;
            case 1:
                return Vector3.left;
            case 2:
                return Vector3.up;
            case 3:
                return Vector3.down;
            case 4:
                return Vector3.forward;
            case 5:
                return Vector3.back;
            default:
                Debug.Log("ERROR in DNAMonster::associateIntToAnchor");
                return Vector3.right;
        }
    }
    public int associateAnchorToInt(Vector3 val)
    {
        if(val == Vector3.right)
            return 0;
        if(val == Vector3.left)
            return 1;
        if(val == Vector3.up)
            return 2;
        if (val == Vector3.down)
            return 3;
        if (val == Vector3.forward)
            return 4;
        if (val == Vector3.back)
            return 5;
        Debug.Log("ERROR in DNAMonster::associateAnchorToInt");
        return 0;
    }
    public Vector3 invertAnchor(Vector3 a)
    {
        return new Vector3(-a.x, -a.y, -a.z);
    }

    public int getSize()
    {
        int res = 1;
        if (children != null)
        {
            for (int i = 0; i < children.Length; ++i)
            {
                res += children[i].getSize();
            }
        }
        return res;
    }
    public BodyPart getBodyPart()
    {
        return bodyPart;
    }
    public DNAMonster[] getChildren()
    {
        return children;
    }
    public DNAMonster getChild(int i)
    {
        return children[i];
    }
    public Joint[] getJoints()
    {
        return joints;
    }
    public Joint getJoint(int i)
    {
        return joints[i];
    }
    public Vector3[] getAnchor()
    {
        return anchor;
    }
    public void setAnchor(Vector3[] newAnchor)
    {
        anchor = newAnchor;
    }
    public Vector3 getParentAnchor()
    {
        return parentAnchor;
    }
    public void setParentAnchor(Vector3 newParentAnchor)
    {
        parentAnchor = newParentAnchor;
    } 

    public DNAMonster getSubDna(int i)
    {
        DNAMonster temp = new DNAMonster(Vector3.zero);
        getNode(temp, ref i);
        return temp;
    }
    public void setSubDna(DNAMonster temp, int i)
    {
        setNode(temp, ref i);
    }
    private void getNode(DNAMonster nodeI, ref int pos)
    {
        --pos;
        if (pos == 0)
        {
            nodeI = new DNAMonster(this);
            /*nodeI.bodyPart = bodyPart;
            nodeI.children = null;
            if (children != null)
            {
                nodeI.children = new DNAMonster[children.Length];
                for (int i = 0; i < children.Length; ++i)
                {
                    nodeI.children[i] = new DNAMonster(children[i]);
                }
            }*/
        }
        else if (pos > 0 && children != null)
        {
            for (int i = 0; i < children.Length; ++i)
            {
                children[i].getNode(nodeI, ref pos);
            }
        }
    }
    private void setNode(DNAMonster newNode, ref int pos)
    {
        --pos;
        if (pos == 0)
        {
            bodyPart = newNode.bodyPart;
            parentAnchor = newNode.parentAnchor;
            children = null;
            if (newNode.children != null)
            {
                children = new DNAMonster[newNode.children.Length];
                anchor = new Vector3[newNode.anchor.Length];
                for (int i = 0; i < newNode.children.Length; ++i)
                {
                    children[i] = new DNAMonster(newNode.children[i]);
                    anchor[i] = newNode.anchor[i];
                    //this.joints[i] = new Joint(newNode.joints[i]);
                }
            }
        }
        else if (pos > 0 && children != null)
        {
            for (int i = 0; i < children.Length; ++i)
            {
                children[i].setNode(newNode, ref pos);
            }
        }
    }
    public int getScore()
    {
        return score;
    }
    public void setScore(int newScore)
    {
        score = newScore;
    }
}
