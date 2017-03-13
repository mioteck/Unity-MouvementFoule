using System;
using UnityEngine;
using Random = UnityEngine.Random;

/** 
 * On connecte les cubes par leurs faces,
 * Chaque cube à donc au maximum six autres cubes attaché
 * On identifie les face comme suis : 
 *      la face +x correspond à la face atteignable par un déplacement positif en X lorsque le cube à une rotation nul
 *      les autres face sont identifier par -x, +y, -y, +z, -z
 * Par simplicité la face +x est toujours relié au parent (s'il existe) les autres faces sont connectées aux enfants s'ils existent
 * */


public class Joint : ICloneable
{
    public BodyPart parent;
    public BodyPart child;
    public Vector3 parentAnchor;
    public Vector3 childAnchor = Vector3.right;
    


    public Joint(BodyPart parent, BodyPart child)
    {
        this.parent = parent;
        this.child = child;
        parentAnchor = RandomAnchor();
    }

    public Joint(BodyPart parent, BodyPart child, Vector3 parentAnchor)
    {
        this.parent = parent;
        this.child = child;
        this.parentAnchor = parentAnchor;
    }

    public Joint(Joint j)
    {
        parent = new BodyPart(j.parent);
        child = new BodyPart(j.child);
        parentAnchor = j.parentAnchor;
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

    private Vector3 RandomAnchor()
    {
        int val = Random.Range(0, 5);
        switch (val)
        {
            case 0:
                return Vector3.left;
            case 1:
                return Vector3.up;
            case 2:
                return Vector3.down;
            case 3:
                return Vector3.forward;
            case 4:
                return Vector3.back;
            default:
                return Vector3.right;
        }
    }

    public object Clone()
    {
        return new Joint(parent, child, parentAnchor);
    }
}
