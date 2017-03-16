﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DNAMonster{
    public static int [] NB_CHILDREN_CHANCE = { 10, 90, 0, 0, 0, 0 };//% de chance d'avoir 0, 1, 2, 3, 4, 5 enfants
    public static int MAX_DEPTH = 4;
    public static int MAX_CHILDREN = 1; //<=5 un cube n'a que 6 face!!!!! il faut garder un slot pour le parent
    private BodyPart bodyPart;
    private DNAMonster[] children;
    private Vector3[] anchor;
    private Vector3 parentAnchor;
    private MoveAction action;
    private int score = 0;

    public DNAMonster(BodyPart bp, DNAMonsterSer[] children, Vector3[] anchor, Vector3 parentAnchor)
    {
        if (bp == null)
        {
            bodyPart = null;
        }
        else
        {
            Vector3 size = bp.getSize();
            bodyPart = new BodyPart(size.x, size.y, size.z);
        }
        this.parentAnchor = parentAnchor;

        if (anchor != null)
        {
            this.anchor = new Vector3[anchor.Length];
            for (int i = 0; i < anchor.Length; i++)
            {
                this.anchor[i] = anchor[i];
            }
        }
        else
        {
            this.anchor = null;
        }

        if (children != null)
        {
            this.children = new DNAMonster[children.Length];
            for (int i = 0; i < children.Length; i++)
            {
                DNAMonsterSer d = children[i];
                this.children[i] = new DNAMonster(d.bodyPart, d.children, d.anchors, d.parentAnchor);
            }
        }
        else
        {
            this.children = null;
        }
    } 
    public DNAMonster(Vector3 parentAnchor, int depth = 0)
    {
        bodyPart = new BodyPart();
        this.parentAnchor = parentAnchor;
        action = new MoveAction();
        ++depth;
        if(depth <= MAX_DEPTH)
        {
            int rand = Random.Range(1, 101);
            int nbChildren = 0;
            if (depth > 1 && rand <= NB_CHILDREN_CHANCE[0])
                nbChildren = 0;
            else if (rand <= NB_CHILDREN_CHANCE[1] + NB_CHILDREN_CHANCE[0])
                nbChildren = 1;
            else if (rand <= NB_CHILDREN_CHANCE[2] + NB_CHILDREN_CHANCE[1] + NB_CHILDREN_CHANCE[0])
                nbChildren = 2;
            else if (rand <= NB_CHILDREN_CHANCE[3] + NB_CHILDREN_CHANCE[2] + NB_CHILDREN_CHANCE[1] + NB_CHILDREN_CHANCE[0])
                nbChildren = 3;
            else if (rand <= NB_CHILDREN_CHANCE[4] + NB_CHILDREN_CHANCE[3] + NB_CHILDREN_CHANCE[2] + NB_CHILDREN_CHANCE[1] + NB_CHILDREN_CHANCE[0])
                nbChildren = 4;
            else if (rand <= NB_CHILDREN_CHANCE[5] + NB_CHILDREN_CHANCE[4] + NB_CHILDREN_CHANCE[3] + NB_CHILDREN_CHANCE[2] + NB_CHILDREN_CHANCE[1] + NB_CHILDREN_CHANCE[0])
                nbChildren = 5;
            else
                Debug.Log("ERROR in DNAMonster constructor");
            children = new DNAMonster[nbChildren];
            anchor = new Vector3[nbChildren];
            createAnchor(nbChildren);
            for (int i = 0; i < nbChildren; ++i)
            {
                children[i] = new DNAMonster(anchor[i], depth);
            }
        }

    }
    public DNAMonster(DNAMonster dna)
    {
        bodyPart = dna.bodyPart;
        parentAnchor = dna.parentAnchor;
        action = new MoveAction(dna.action);
        if (dna.children != null)
        {
            children = new DNAMonster[dna.children.Length];
            anchor = new Vector3[dna.anchor.Length];
            for (int i = 0; i < dna.children.Length; ++i)
            {
                children[i] = new DNAMonster(dna.children[i]);
                anchor[i] = dna.anchor[i];
            }
        }
    }

    public void createAnchor(int size)
    {
        bool test = false;
        for (int i = 0; i < size; i++)
        {
            test = true;
            int val = Random.Range(0, 6);
            anchor[i] = associateIntToAnchor(val);
            if(invertAnchor(parentAnchor) == anchor[i])
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
            if (!test)
            {
                i--;
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
    public void addOneBodypart()
    {
        int cLength = 0;
        int aLength = 0;
        if (children != null)
        {
            cLength = children.Length;
            aLength = anchor.Length;
        }
        DNAMonster[] tempChildren = new DNAMonster[cLength + 1];
        Vector3[] tempAnchor = new Vector3[aLength + 1];
        for(int i = 0; i < cLength; i++)
            tempChildren[i] = children[i];
        for(int i = 0; i < aLength; i++)
            tempAnchor[i] = anchor[i];
        tempAnchor[aLength] = getFreeAnchorSlot();
        tempChildren[cLength] = new DNAMonster(tempAnchor[aLength], MAX_DEPTH);
        children = new DNAMonster[cLength + 1];
        anchor = new Vector3[aLength + 1];
        for (int i = 0; i < cLength+1; i++)
            children[i] = tempChildren[i];
        for (int i = 0; i < aLength+1; i++)
            anchor[i] = tempAnchor[i];
    }
    public void deleteOneBodypart()
    {
        int cLength = 0;
        int aLength = 0;
        if (children != null)
        {
            cLength = children.Length - 1;
            aLength = anchor.Length - 1;
            if (cLength <= 0)
            {
                children = null;
                anchor = null;
            }
            else
            {
                DNAMonster[] tempChildren = new DNAMonster[cLength];
                Vector3[] tempAnchor = new Vector3[aLength];
                for (int i = 0; i < cLength; i++)
                    tempChildren[i] = children[i];
                for (int i = 0; i < aLength; i++)
                    tempAnchor[i] = anchor[i];
                children = new DNAMonster[cLength];
                anchor = new Vector3[aLength];
                for (int i = 0; i < cLength; i++)
                    children[i] = tempChildren[i];
                for (int i = 0; i < aLength; i++)
                    anchor[i] = tempAnchor[i];
            }
        }
    }
    public Vector3 getFreeAnchorSlot()
    {
        int rand = Random.Range(0, 6);
        if(parentAnchor != Vector3.zero)
        {
            while (rand == associateAnchorToInt(parentAnchor))
            {
                rand = Random.Range(0, 6);
            }
        }
        if(children == null)
        {
            return associateIntToAnchor(rand);
        }
        bool test = false;
        while(test == false)
        {
            test = true;
            for (int i = 0; i < anchor.Length; i++)
                if(rand == associateAnchorToInt(anchor[i]))
                    test = false;
            if (test == false)
            {
                rand++;
                if (rand >= 6)
                    rand = 0;
            }
        }
        return associateIntToAnchor(rand);
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
    public void setBodypart(BodyPart newBodypart)
    {
        bodyPart = newBodypart;
    }
    public DNAMonster[] getChildren()
    {
        return children;
    }
    public void setSubDna(DNAMonster[] newChildren, Vector3[] newAnchor, MoveAction newAction)
    {
        children = newChildren;
        anchor = newAnchor;
        action = newAction;
    }

    public Vector3[] getAnchor()
    {
        return anchor;
    }
    public Vector3 getParentAnchor()
    {
        return parentAnchor;
    }
    public void setParentAnchor(Vector3 newParentAnchor)
    {
        parentAnchor = newParentAnchor;
    } 

    public int getScore()
    {
        return score;
    }
    public void setScore(int newScore)
    {
        score = newScore;
    }
    public MoveAction getAction()
    {
        return action;
    }
    public void setAction(MoveAction newAction)
    {
        action = newAction;
    }
    /// <summary>
    /// return the correct node in position "pos" : pos range = (1, dna.getSize())
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public DNAMonster getSubDna(int pos)
    {
        for (int i = 0; i < children.Length; i++)
        {
            if (pos == 1)
            {
                return children[i];
            }
            pos -= children[i].getSize();
            if (pos <= 0)
            {
                pos += children[i].getSize() - 1;
                return children[i].getSubDna(pos);
            }
        }
        Debug.Log("ERROR in DNAMonster.getSubDna()");
        return null;
    }
    /// <summary>
    /// return the dna monster rotate in a way that his parentAnchor will now be the anchor get in parameter
    /// </summary>
    /// <param name="newParentAnchor"></param>
    /// <returns></returns>
    public DNAMonster getRotateSubDna(Vector3 newParentAnchor)
    {
        DNAMonster res = new DNAMonster(this);
        if (parentAnchor == newParentAnchor)
        {
            return this;
        }
        Vector3 axis = Vector3.zero;
        //si on garde le meme axe (inverser) on doit effectuer 2 fois la rotation
        if (new Vector3((int)Mathf.Abs(parentAnchor.x), (int)Mathf.Abs(parentAnchor.y), (int)Mathf.Abs(parentAnchor.z)) == new Vector3((int)Mathf.Abs(newParentAnchor.x), (int)Mathf.Abs(newParentAnchor.y), (int)Mathf.Abs(newParentAnchor.z)))
        {
            axis = new Vector3((int)Mathf.Abs(parentAnchor.z), (int)Mathf.Abs(parentAnchor.x), (int)Mathf.Abs(parentAnchor.y));
            res.changeAnchorFromAxis(axis);
        }
        else
        {
            int x = 1 - (int)Mathf.Abs(parentAnchor.x) - (int)Mathf.Abs(newParentAnchor.x);
            int y = 1 - (int)Mathf.Abs(parentAnchor.y) - (int)Mathf.Abs(newParentAnchor.y);
            int z = 1 - (int)Mathf.Abs(parentAnchor.z) - (int)Mathf.Abs(newParentAnchor.z);
            axis = new Vector3(x, y, z);
            if ((newParentAnchor.x > 0 || newParentAnchor.y > 0 || newParentAnchor.z > 0) && (parentAnchor.x < 0 || parentAnchor.y < 0 || parentAnchor.z < 0))
                axis = new Vector3(-x, -y, -z);
        }
        res.changeAnchorFromAxis(axis);
        return res;
    }
    /// <summary>
    /// return the newAnchor corresponding to the oldAnchor get in parameter turn by 90 degree arround the axis 
    /// </summary>
    /// <param name="oldAnchor"></param>
    /// <param name="axis"></param>
    /// <returns></returns>
    private Vector3 getNextAnchorFromAxis(Vector3 oldAnchor,Vector3 axis)
    {
        /*
        Vector3[][] lookUpTable = new Vector3[3][];
        lookUpTable[0] = new Vector3[4] { Vector3.up, Vector3.forward, Vector3.down, Vector3.back };
        lookUpTable[1] = new Vector3[4] { Vector3.right, Vector3.forward, Vector3.left, Vector3.back };
        lookUpTable[2] = new Vector3[4] { Vector3.right, Vector3.up, Vector3.left, Vector3.down };
        */
        if(axis == Vector3.zero)
        {
            Debug.Log("ERROR in DNAMonster.getNextAnchorFromAxis");
            return Vector3.zero;
        }
        if(axis == Vector3.right || axis == Vector3.left)
        {
            if (oldAnchor == Vector3.up) return axis.x * Vector3.forward;
            if (oldAnchor == Vector3.forward) return axis.x * Vector3.down;
            if (oldAnchor == Vector3.down) return axis.x * Vector3.back;
            if (oldAnchor == Vector3.back) return axis.x * Vector3.up;
        }
        if (axis == Vector3.up || axis == Vector3.down)
        {
            if (oldAnchor == Vector3.right) return axis.y * Vector3.forward;
            if (oldAnchor == Vector3.forward) return axis.y * Vector3.left;
            if (oldAnchor == Vector3.left) return axis.y * Vector3.back;
            if (oldAnchor == Vector3.back) return axis.y * Vector3.right;
        }
        if (axis == Vector3.forward || axis == Vector3.back)
        {
            if (oldAnchor == Vector3.right) return axis.z * Vector3.up;
            if (oldAnchor == Vector3.up) return axis.z * Vector3.left;
            if (oldAnchor == Vector3.left) return axis.z * Vector3.down;
            if (oldAnchor == Vector3.down) return axis.z * Vector3.right;
        }
        return oldAnchor;
    }
    /// <summary>
    /// change all anchor of a subDna Tree
    /// </summary>
    /// <param name="axis"></param>
    private void changeAnchorFromAxis(Vector3 axis)
    {
        parentAnchor = getNextAnchorFromAxis(parentAnchor, axis);
        if(children != null)
        {
            for (int i = 0; i < children.Length; i++)
            {
                anchor[i] = getNextAnchorFromAxis(anchor[i], axis);
                children[i].changeAnchorFromAxis(axis);
            }
        }

    }







    /*
    /// <summary>
    /// use the same algorithme of getSubDna but set the node to the new value get in parameter
    /// </summary>
    /// <param name="subDna"></param>
    /// <param name="pos"></param>
    public bool addOneBodypart(int pos)
    {
        for (int i = 0; i < children.Length; i++)
        {
            if (pos == 1)
            {
                children[i].addOneBodypart();
                return true;
            }
            pos -= children[i].getSize();
            if (pos <= 0)
            {
                pos += children[i].getSize() - 1;
                return children[i].addOneBodypart(pos);
            }
        }
        Debug.Log("ERROR in DNAMonster.setSubDna()");
        return false;
    }
    */
}
