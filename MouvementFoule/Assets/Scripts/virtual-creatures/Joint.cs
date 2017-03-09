using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Joint : ICloneable
{
    public BodyPart parent;
    public BodyPart child;

    public Vector3 parentAnchor;
    public Vector3 childAnchor;

    public Joint(BodyPart parent, BodyPart child)
    {
        this.parent = parent;
        this.child = child;

        parentAnchor = new Vector3(RandomAnchor(), RandomAnchor(), RandomAnchor());
        childAnchor = new Vector3(RandomAnchor(), RandomAnchor(), RandomAnchor());
    }

    public Joint(BodyPart parent, BodyPart child, Vector3 parentAnchor, Vector3 childAnchor)
    {
        this.parent = parent;
        this.child = child;
        this.parentAnchor = parentAnchor;
        this.childAnchor = childAnchor;
    }

    public Joint(Joint j)
    {
        parent = new BodyPart(j.parent);
        child = new BodyPart(j.child);
        parentAnchor = j.parentAnchor;
        childAnchor = j.childAnchor;
    }

    public BodyPart GetParent()
    {
        return parent;
    }

    public BodyPart GetChild()
    {
        return child;
    }

    public Vector3 GetParentAnchor()
    {
        return parentAnchor;
    }

    public Vector3 GetChildAnchor()
    {
        return childAnchor;
    }

    private float RandomAnchor()
    {
        int val = Random.Range(0, 1);
        if (val == 0)
        {
            return -1;
        }

        return 1;
    }

    public object Clone()
    {
        return new Joint(parent, child, parentAnchor, childAnchor);
    }
}
