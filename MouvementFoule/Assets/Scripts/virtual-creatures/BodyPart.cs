using UnityEngine;


public class BodyPart{
    public static float MAX_X = 4.0f;
    public static float MAX_Y = 4.0f;
    public static float MAX_Z = 4.0f;
    private Vector3 size;

    public BodyPart(float x, float y, float z)
    {
        size = new Vector3(x, y, z);
    }
    public BodyPart()
    {
        size = new Vector3(Random.Range(0.3f, MAX_X), Random.Range(0.3f, MAX_Y), Random.Range(0.3f, MAX_Z));
    }
    public BodyPart(BodyPart bodyPart)
    {
        size.x = bodyPart.size.x;
        size.y = bodyPart.size.y;
        size.z = bodyPart.size.z;
    }
    public Vector3 getSize()
    {
        return size;
    }
    public void setSize(Vector3 newSize)
    {
        size = newSize;
    }
}
