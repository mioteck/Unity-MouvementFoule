using UnityEngine;


public class BodyPart{
    public static float MAX_X = 5.0f;
    public static float MAX_Y = 5.0f;
    public static float MAX_Z = 5.0f;
    private Vector3 size;

    public BodyPart(Vector3 size)
    {
        this.size = size;
    }

    public BodyPart(float x, float y, float z)
    {
        size = new Vector3(x, y, z);
    }
    public BodyPart()
    {
        size = new Vector3(Random.Range(0.1f, MAX_X), Random.Range(0.1f, MAX_Y), Random.Range(0.1f, MAX_Z));
        /*int rand = Random.Range(0, 3);
        if (rand == 0)
            size = new Vector3(5.0f, 0.2f, 0.2f);
        else if (rand == 1)
            size = new Vector3(0.2f, 5.0f, 0.2f);
        else
            size = new Vector3(0.2f, 0.2f, 5.0f);*/
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
