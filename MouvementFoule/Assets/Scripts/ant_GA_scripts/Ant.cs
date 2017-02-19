using UnityEngine;
using System.Collections;

public enum Dir { EAST, SOUTH, WEST, NORTH};
public enum Op { MOVE, RIGHT, LEFT, IF, P2, P3};


public class Ant{
    public static int MAX_DEPTH = 100;
    public static int MAX_ENERGY = 400;
    private static int width, height;
    private int x, y, energy;
    private Dir dir;
    private map antMap;
    private NTree<Op> dna;
    //constructor
    public Ant(map myMap)
    {
        antMap = myMap;
        width = antMap.getWidth();
        height = antMap.getHeight();
        dir = Dir.EAST;
        x = 0;
        y = 0;
        energy = MAX_ENERGY;
        int rand = Random.Range(0, 5);
        dna = new NTree<Op>((Op)rand);
        if (!isOperator((Op)rand))
        {
            createSubDna(dna, 0);
        }
    }
    //accessor
    public int getEnergy()
    {
        return energy;
    }
    public NTree<Op> getDna()
    {
        return dna;
    }
    public int getX() {
        return x;
    }
    public int getY()
    {
        return y;
    }
    //utility function
    public bool isOperator(Op op)
    {
        if (op == Op.MOVE || op == Op.RIGHT || op == Op.LEFT)
            return true;
        return false;
    }
    void createSubDna(NTree<Op> subDna, int depth)
    {
        if(!isOperator(subDna.getData())) { 
            depth++;
            int rand1, rand2, rand3;
            if (depth < MAX_DEPTH)
            {
                rand1 = Random.Range(0, 5);
                rand2 = Random.Range(0, 5);
                rand3 = Random.Range(0, 5);
            }
            else
            {
                rand1 = Random.Range(0, 2);
                rand2 = Random.Range(0, 2);
                rand3 = Random.Range(0, 2);
            }
            subDna.AddChild((Op)rand1);
            subDna.AddChild((Op)rand2);
            if(subDna.getData() == Op.P3)
            {
                subDna.AddChild((Op)rand3);
            }
            if (!isOperator(subDna.GetChild(1).getData()))
            {
                createSubDna(subDna.GetChild(1), depth);
            }
            if (!isOperator(subDna.GetChild(2).getData()))
            {
                createSubDna(subDna.GetChild(2), depth);
            }
            if (subDna.getData() == Op.P3 && !isOperator(subDna.GetChild(3).getData()))
            {
                createSubDna(subDna.GetChild(3), depth);
            }
        }
    }
    public void execute(NTree<Op> subDna)
    {
        switch (subDna.getData())
        {
            case Op.MOVE:
                energy--;
                move();
                break;
            case Op.RIGHT:
                energy--;
                right();
                break;
            case Op.LEFT:
                energy--;
                left();
                break;
            case Op.IF:    
                ifFoodAhead(subDna.GetChild(1), subDna.GetChild(2));
                break;
            case Op.P2:
                prog2(subDna.GetChild(1), subDna.GetChild(2));
                break;
            case Op.P3:
                prog3(subDna.GetChild(1), subDna.GetChild(2), subDna.GetChild(3));
                break;
            default:
                break;
        }
    }
    //opérators : respectivily move the ant according to the name of the function
    public void right()
    {
        ++dir;
        if ((int)dir > 3)
        {
            dir = Dir.EAST;
        }
    }
    public void left()
    {
        --dir;
        if ((int)dir < 0)
        {
            dir = Dir.NORTH;
        }
    }
    public void move()
    {
        switch (dir)
        {
            case Dir.EAST:
                ++x;
                if (x >= width)
                    x = 0;
                break;
            case Dir.SOUTH:
                ++y;
                if (y >= height)
                    y = 0;
                break;
            case Dir.WEST:
                --x;
                if (x < 0)
                    x = width-1;
                break;
            case Dir.NORTH:
                --y;
                if (y < 0)
                    y = height-1;
                break;
            default:
                break;
        }
    }
    //non leaf opérators 
    public void ifFoodAhead(NTree<Op> childTrue, NTree<Op> childFalse)
    {
        int tempX = x, tempY = y;
        switch (dir)
        {
            case Dir.EAST:
                ++tempX;
                if (tempX >= width)
                    tempX = 0;
                break;
            case Dir.SOUTH:
                ++tempY;
                if (tempY >= height)
                    tempY = 0;
                break;
            case Dir.WEST:
                --tempX;
                if (tempX < 0)
                    tempX = width - 1;
                break;
            case Dir.NORTH:
                --tempY;
                if (tempY < 0)
                    tempY = height - 1;
                break;
            default:
                break;
        }
        if (antMap.getMap()[x,y] == SType.FOOD)
        {
            execute(childTrue);
        }
        else
        {
            execute(childFalse);
        }
    }
    public void prog2(NTree<Op> child1, NTree<Op> child2)
    {
        execute(child1);
        execute(child2);
    }
    public void prog3(NTree<Op> child1, NTree<Op> child2, NTree<Op> child3)
    {
        execute(child1);
        execute(child2);
        execute(child3);
    }
}
