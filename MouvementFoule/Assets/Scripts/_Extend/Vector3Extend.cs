using UnityEngine;
using System.Collections;

public static class Vector3Extend {

	public static Vector3 Rand(this Vector3 v, float nx, float mx, float ny, float my, float nz, float mz)
    {
        float x = Random.Range(nx, mx);
        float y = Random.Range(ny, my);
        float z = Random.Range(nz, mz);

        return new Vector3(x, y, z);
    }
}
