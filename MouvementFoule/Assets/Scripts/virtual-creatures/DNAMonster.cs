using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Phenotype { LINE, SPIDER, UNIQUE}

[System.Serializable]
public class DNAMonster{
    public static Vector3[] LOOK_UP_TABLE_XZ = { Vector3.right, Vector3.forward, Vector3.left, Vector3.back };
    private BodyPart bodyPart;
    private DNAMonster[] children;
    private Vector3[] anchor;
    private Vector3 parentAnchor;
    private MoveAction_2 action;
    private int score = 0;

    public DNAMonster(MoveAction_2 moveAction, BodyPart bp, DNAMonsterSer[] children, Vector3[] anchor, Vector3 parentAnchor)
    {
        action = new MoveAction_2(moveAction);
        if (bp == null)
            bodyPart = null;
        else
            bodyPart = new BodyPart(bp);
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

                MoveAction_2 action = new MoveAction_2(false); // TO DO : Constructeur avec MoveActionSer_2

                action.movementType = d.action.movementType;
                action.angularVelocityFactor = d.action.angularVelocityFactor;
                action.freezeNotOnGround = d.action.freezeNotOnGround;
                action.frequencyX = d.action.frequencyX;
                action.frequencyY = d.action.frequencyY;
                action.frequencyZ = d.action.frequencyZ;
                action.waveSpeedX = d.action.waveSpeedX;
                action.waveSpeedY = d.action.waveSpeedY;
                action.waveSpeedZ = d.action.waveSpeedZ;

                this.children[i] = new DNAMonster(action, new BodyPart(d.bodyPartSize), d.children, d.anchors, d.parentAnchor);


            }
        }
        else
        {
            this.children = null;
        }
    } 

    public DNAMonster(DNAMonster dna)
    {
        bodyPart = dna.bodyPart;
        parentAnchor = dna.parentAnchor;
        action = new MoveAction_2(dna.action);
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
    public DNAMonster(Phenotype phenotype, Vector3 newParentAnchor, int length = 2)
    {
        switch (phenotype)
        {
            case Phenotype.LINE:
                bodyPart = new BodyPart(BodyType.LEG, newParentAnchor);
                action = new MoveAction_2(true);
                parentAnchor = newParentAnchor;
                length--;
                if (length > 0)
                {
                    children = new DNAMonster[1];
                    anchor = new Vector3[1];
                    anchor[0] = Vector3.right;
                    children[0] = new DNAMonster(Phenotype.LINE, anchor[0], length);
                }
                break;
            case Phenotype.UNIQUE:
                bodyPart = new BodyPart(BodyType.DEFAULT, newParentAnchor);
                action = new MoveAction_2(true);
                parentAnchor = newParentAnchor;
                break;
            case Phenotype.SPIDER:
                bodyPart = new BodyPart(BodyType.CUBE, newParentAnchor);
                action = new MoveAction_2(false);
                parentAnchor = newParentAnchor;
                //create leg
                DNAMonster leg = new DNAMonster(Phenotype.LINE, Vector3.right, 2);
                //create 4 'legs' by rotating leg
                children = new DNAMonster[4];
                anchor = new Vector3[4];
                for (int i = 0; i < 4; i++)
                {
                    children[i] = new DNAMonster(leg.getRotateSubDna(LOOK_UP_TABLE_XZ[i]));
                    anchor[i] = LOOK_UP_TABLE_XZ[i];
                }
                break;
            default:
                Debug.Log("ERROR in DNAMonster.constructor.phenotype");
                break;
        }

        if (children != null && children.Length > 0)
        {
            bool[] childAt = { false, false, false, false, false, false };
            for (int i = 0; i < children.Length; i++)
            {
                if (anchor[i] == Vector3.left)
                    childAt[0] = true;
                else if (anchor[i] == Vector3.right)
                    childAt[1] = true;
                else if (anchor[i] == Vector3.forward)
                    childAt[2] = true;
                else if (anchor[i] == Vector3.back)
                    childAt[3] = true;
                else if (anchor[i] == Vector3.up)
                    childAt[4] = true;
                else if (anchor[i] == Vector3.down)
                    childAt[5] = true;
            }

            if (childAt[0] && childAt[1])
                action.waveSpeedZ = 0.0f;
            if (childAt[2] && childAt[3])
                action.waveSpeedY = 0.0f;
            if (childAt[4] && childAt[5])
                action.waveSpeedX = 0.0f;
        }
    }

    //anchor functions
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
        for (int i = 0; i < cLength; i++)
            tempChildren[i] = children[i];
        for (int i = 0; i < aLength; i++)
            tempAnchor[i] = anchor[i];
        tempAnchor[aLength] = getFreeAnchorSlot();
        tempChildren[cLength] = new DNAMonster(Phenotype.UNIQUE, getFreeAnchorSlot());
        children = new DNAMonster[cLength + 1];
        anchor = new Vector3[aLength + 1];
        for (int i = 0; i < cLength + 1; i++)
            children[i] = tempChildren[i];
        for (int i = 0; i < aLength + 1; i++)
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

    public void deleteChild(DNAMonster child)
    {
        Vector3[] newAnchors = new Vector3[getAnchor().Length - 1];
        DNAMonster[] children = new DNAMonster[getChildren().Length - 1];

        int count = 0;
        for (int i = 0; i < getChildren().Length; i++)
        {
            if (getChildren()[i] != child)
            {
                children[count] = getChildren()[i];
                newAnchors[count] = getAnchor()[i];
                count++;
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

    //accessors
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

    public void setSubDna(DNAMonster[] newChildren, Vector3[] newAnchor, MoveAction_2 newAction)
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
    public MoveAction_2 getAction()
    {
        return action;
    }
    public void setAction(MoveAction_2 newAction)
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
    /// set a subDna : WARNING : use it with carrefully the new subDna must be accordable
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="newSubDna"></param>
    public void setSubDna(int pos, DNAMonster newSubDna)
    {
        for (int i = 0; i < children.Length; i++)
        {
            if (pos == 1)
            {
                children[i] = new DNAMonster(newSubDna);
                anchor[i] = newSubDna.getParentAnchor();
                break;
            }
            pos -= children[i].getSize();
            if (pos <= 0)
            {
                pos += children[i].getSize() - 1;
                children[i].setSubDna(pos, newSubDna);
            }
        }
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
            else if ((newParentAnchor.x < 0 || newParentAnchor.y < 0 || newParentAnchor.z < 0) && (parentAnchor.x > 0 || parentAnchor.y > 0 || parentAnchor.z > 0))
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

}
