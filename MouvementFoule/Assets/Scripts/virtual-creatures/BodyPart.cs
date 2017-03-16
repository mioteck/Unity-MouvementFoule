using UnityEngine;

public enum BodyType { LEG, BODY, CUBE, DEFAULT };

public class BodyPart{
    public static float MAX = 6.0f;
    public static float MIN = 0.2f;
    private Vector3 size;
    private BodyType type;

    public BodyPart(Vector3 newSize, BodyType newType = BodyType.DEFAULT)
    {
        size = newSize;
        type = newType;
    }
    public BodyPart(BodyPart bodyPart)
    {
        type = bodyPart.type;
        size = bodyPart.size;
    }
    /*public BodyPart(BodyType type)
    {
        switch (type)
        {
            case BodyType.LEG:
                size = new Vector3(Random.Range(MIN, MAX), Random.Range(MIN, MAX), Random.Range(MIN, MAX));
                break;
            case BodyType.BODY: 
                size = new Vector3(Random.Range(MAX / 2, MAX), Random.Range(MAX / 2, MAX), Random.Range(MAX / 2, MAX));
                break;
            case BodyType.CUBE:
                float rand = Random.Range(MAX / 2, MAX);
                size = new Vector3(rand, rand, rand);
                break;
            default:
                type = BodyType.DEFAULT;
                size = new Vector3(Random.Range(MIN, MAX), Random.Range(MIN, MAX), Random.Range(MIN, MAX));
                break;
        }
    }*/
    public BodyPart(BodyType type, Vector3 axe)
    {
        this.type = type;
        switch (type)
        {
            case BodyType.LEG:
                size = new Vector3(Random.Range(MIN + axe.x * MAX / 2, MAX/2 + axe.x * MAX / 2), Random.Range(MIN + axe.y * MAX / 2, MAX/2 + axe.y * MAX / 2), Random.Range(MIN + axe.z * MAX / 2, MAX/2 + axe.z * MAX / 2));
                break;
            case BodyType.BODY:
                size = new Vector3(Random.Range(MIN + MAX / 2 - axe.x * MAX / 2, MAX - axe.x * MAX / 2), Random.Range(MIN + MAX / 2 - axe.y * MAX / 2, MAX - axe.y * MAX / 2), Random.Range(MIN + MAX / 2 - axe.z * MAX / 2, MAX - axe.z * MAX / 2));
                break;
            case BodyType.CUBE:
                float rand = Random.Range(MAX / 2, MAX);
                size = new Vector3(rand, rand, rand);
                break;
            default:
                type = BodyType.DEFAULT;
                size = new Vector3(Random.Range(MIN, MAX), Random.Range(MIN, MAX), Random.Range(MIN, MAX));
                break;
        }
    }


    public Vector3 getSize()
    {
        return size;
    }
    public void setSize(Vector3 newSize)
    {
        size = newSize;
    }
    public BodyType getType()
    {
        return type;
    }
    public void setType(BodyType newType)
    {
        type = newType;
    }

}
