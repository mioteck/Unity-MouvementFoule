using UnityEngine;
using System.Collections;

public enum SType {GROUND, ROCK, FOOD};

public class map //: MonoBehaviour
{ 
    private static int width = 32, height = 32;
    private static SType[,] myMap;
    private static Renderer rend;

    public map(int seed, GameObject plane)
    {
        myMap = new SType[width, height];
        Random.InitState(seed);
        rend = plane.GetComponent<Renderer>();
        for (int i = 0; i<width; ++i)
        {
            for(int j = 0; j<height; ++j)
            {
                int r = Random.Range(0, 40);
                if(r<37)
                    myMap[i, j] = SType.GROUND;
                else if(r<=40)
                    myMap[i, j] = SType.FOOD;
                else
                    myMap[i, j] = SType.ROCK;
            }
        }
    }
    public int getWidth()
    {
        return width;
    }
    public int getHeight()
    {
        return height;
    }
    public SType[,] getMap()
    {
        return myMap;
    }
    public void setValue(int x, int y, SType value)
    {
        myMap[x, y] = value;
    }
    public void applyRender()
    {
        // duplicate the original texture and assign to the material
        Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false, false);
        texture.filterMode = FilterMode.Point;
        //apply texture according to map
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                switch (myMap[x, y])
                {
                    case SType.GROUND:
                        texture.SetPixel(x, y, Color.gray);
                        break;
                    case SType.ROCK:
                        texture.SetPixel(x, y, Color.black);
                        break;
                    case SType.FOOD:
                        texture.SetPixel(x, y, Color.cyan);
                        break;
                    default:
                        break;
                }
            }
        }
        // actually apply all SetPixels, don't recalculate mip levels
        texture.Apply(false);
        rend.material.mainTexture = texture;
    }
}


