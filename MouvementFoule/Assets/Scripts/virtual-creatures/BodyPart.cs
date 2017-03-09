using UnityEngine;


public class BodyPart{
    public static int MAX_X = 10;
    public static int MAX_Y = 10;
    public static int MAX_Z = 10;
    public Vector3 size;

    public BodyPart(float x, float y, float z)
    {
        size = new Vector3(x, y, z);
    }
    public BodyPart()
    {
        size = new Vector3(Random.Range(0, MAX_X), Random.Range(0, MAX_Y), Random.Range(0, MAX_Z));
    }
    public BodyPart(BodyPart bodyPart)
    {
        size.x = bodyPart.size.x;
        size.y = bodyPart.size.y;
        size.z = bodyPart.size.z;
    }
}
