using UnityEngine;
using System.Collections;

public enum SType {GROUND, ROCK, FOOD};

public class map //: MonoBehaviour
{ 
    private static int width = 32, height = 32;
    private static Renderer rend;
    private SType[,] myMap;

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
    public map(GameObject plane)
    {
        myMap = new SType[width, height];
        rend = plane.GetComponent<Renderer>();
        for (int i = 0; i < width; ++i)
        {
            for (int j = 0; j < height; ++j)
            {
                myMap[i, j] = SType.GROUND;
            }
        }
        myMap[0, 0] = SType.FOOD;

        myMap[1, 0] = SType.FOOD;
        myMap[1, 25] = SType.FOOD;
        myMap[1, 26] = SType.FOOD;
        myMap[1, 27] = SType.FOOD;
        myMap[1, 28] = SType.FOOD;

        myMap[2, 0] = SType.FOOD;
        myMap[2, 30] = SType.FOOD;

        myMap[3, 0] = SType.FOOD;
        myMap[3, 1] = SType.FOOD;
        myMap[3, 2] = SType.FOOD;
        myMap[3, 3] = SType.FOOD;
        myMap[3, 4] = SType.FOOD;
        myMap[3, 5] = SType.FOOD;
        myMap[3, 24] = SType.FOOD;
        myMap[3, 30] = SType.FOOD;

        myMap[4, 5] = SType.FOOD;
        myMap[4, 24] = SType.FOOD;
        myMap[4, 30] = SType.FOOD;

        myMap[5, 5] = SType.FOOD;
        myMap[5, 30] = SType.FOOD;

        myMap[6, 5] = SType.FOOD;

        myMap[7, 24] = SType.FOOD;
        myMap[7, 28] = SType.FOOD;
        myMap[7, 29] = SType.FOOD;

        myMap[8, 5] = SType.FOOD;
        myMap[8, 24] = SType.FOOD;
        myMap[8, 27] = SType.FOOD;

        myMap[9, 5] = SType.FOOD;
        myMap[9, 24] = SType.FOOD;
        myMap[9, 27] = SType.FOOD;

        myMap[10, 5] = SType.FOOD;
        myMap[10, 24] = SType.FOOD;
        myMap[10, 27] = SType.FOOD;

        myMap[11, 5] = SType.FOOD;
        myMap[11, 24] = SType.FOOD;
        myMap[11, 27] = SType.FOOD;

        myMap[12, 5] = SType.FOOD;
        myMap[12, 6] = SType.FOOD;
        myMap[12, 7] = SType.FOOD;
        myMap[12, 8] = SType.FOOD;
        myMap[12, 9] = SType.FOOD;
        myMap[12, 10] = SType.FOOD;
        myMap[12, 12] = SType.FOOD;
        myMap[12, 13] = SType.FOOD;
        myMap[12, 14] = SType.FOOD;
        myMap[12, 15] = SType.FOOD;
        myMap[12, 18] = SType.FOOD;
        myMap[12, 19] = SType.FOOD;
        myMap[12, 20] = SType.FOOD;
        myMap[12, 21] = SType.FOOD;
        myMap[12, 22] = SType.FOOD;
        myMap[12, 23] = SType.FOOD;
        myMap[12, 27] = SType.FOOD;

        myMap[13, 27] = SType.FOOD;

        myMap[14, 27] = SType.FOOD;

        myMap[16, 26] = SType.FOOD;
        myMap[16, 25] = SType.FOOD;
        myMap[16, 24] = SType.FOOD;
        myMap[16, 21] = SType.FOOD;
        myMap[16, 19] = SType.FOOD;
        myMap[16, 18] = SType.FOOD;
        myMap[16, 17] = SType.FOOD;

        myMap[17, 16] = SType.FOOD;

        myMap[20, 15] = SType.FOOD;
        myMap[20, 14] = SType.FOOD;
        myMap[20, 11] = SType.FOOD;
        myMap[20, 10] = SType.FOOD;
        myMap[20, 9] = SType.FOOD;
        myMap[20, 8] = SType.FOOD;

        myMap[21, 5] = SType.FOOD;

        myMap[22, 5] = SType.FOOD;

        myMap[23, 15] = SType.FOOD;
        myMap[23, 23] = SType.FOOD;

        myMap[24, 3] = SType.FOOD;
        myMap[24, 4] = SType.FOOD;
        myMap[24, 18] = SType.FOOD;

        myMap[25, 2] = SType.FOOD;

        myMap[26, 2] = SType.FOOD;
        myMap[26, 14] = SType.FOOD;
        myMap[26, 22] = SType.FOOD;

        myMap[27, 2] = SType.FOOD;
        myMap[27, 14] = SType.FOOD;
        myMap[27, 19] = SType.FOOD;

        myMap[28, 14] = SType.FOOD;

        myMap[29, 3] = SType.FOOD;
        myMap[29, 4] = SType.FOOD;
        myMap[29, 6] = SType.FOOD;
        myMap[29, 9] = SType.FOOD;
        myMap[29, 12] = SType.FOOD;
    }
    public map(map mapCpy)
    {
        myMap = (SType[,])mapCpy.getMap().Clone();
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
                        texture.SetPixel(width - 1 - x, y, Color.gray);
                        break;
                    case SType.ROCK:
                        texture.SetPixel(width - 1 - x, y, Color.black);
                        break;
                    case SType.FOOD:
                        texture.SetPixel(width - 1 - x, y, Color.cyan);
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


