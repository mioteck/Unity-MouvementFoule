using UnityEngine;
using System.Collections;

public static class TransformExtend
{

    public static void RandomPosition(this Transform t, float nx, float mx, float ny, float my, float nz, float mz)
    {
        Vector3 v = new Vector3();
        t.transform.position = v.Rand(nx, mx, ny, my, nz, mz);
    }

    public static void RandomPosition(this Transform t, float plage)
    {
        t.RandomPosition(-plage, plage, -plage, plage, -plage, plage);
    }
}
