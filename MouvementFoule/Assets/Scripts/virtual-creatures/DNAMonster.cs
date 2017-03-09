using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DNAMonster{
    public static int MAX_DEPTH = 6;
    public static int MAX_CHILDREN = 2;
    private BodyPart bodyPart;
    private DNAMonster[] children;
    private Joint[] joints;

    public DNAMonster(int depth = 0)
    {
        bodyPart = new BodyPart();
        int rand = 0;
        if(depth < MAX_DEPTH)
        {
            rand = Random.Range(0, MAX_CHILDREN);
            children = new DNAMonster[rand];
            joints = new Joint[rand];
            for (int i = 0; i < rand; ++i)
            {
                children[i] = new DNAMonster(depth);
                joints[i] = new Joint(bodyPart, children[i]);
            }
            ++depth;
        }

    }
    public DNAMonster(DNAMonster dna)
    {
        bodyPart = dna.bodyPart;
        if (dna.children != null)
        {
            children = new DNAMonster[dna.children.Length];
            for (int i = 0; i < dna.children.Length; ++i)
            {
                children[i] = new DNAMonster(dna.children[i]);
                joints[i] = new Joint(dna.joints[i]);
            }
        }
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
    public void getNode(DNAMonster nodeI, ref int pos)
    {
        --pos;
        if (pos == 0)
        {
            nodeI.bodyPart = bodyPart;
            nodeI.children = null;
            if (children != null)
            {
                nodeI.children = new DNAMonster[children.Length];
                for (int i = 0; i < children.Length; ++i)
                {
                    nodeI.children[i] = new DNAMonster(children[i]);
                }
            }
        }
        else if (pos > 0 && children != null)
        {
            for (int i = 0; i < children.Length; ++i)
            {
                children[i].getNode(nodeI, ref pos);
            }
        }
    }
    public void setNode(DNAMonster newNode, ref int pos)
    {
        --pos;
        if (pos == 0)
        {
            this.bodyPart = newNode.bodyPart;
            this.children = null;
            if (newNode.children != null)
            {
                this.children = new DNAMonster[newNode.children.Length];
                for (int i = 0; i < newNode.children.Length; ++i)
                {
                    this.children[i] = new DNAMonster(newNode.children[i]);
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

}
