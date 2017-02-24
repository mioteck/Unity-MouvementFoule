using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Dir { EAST, SOUTH, WEST, NORTH};
public enum Op { MOVE, RIGHT, LEFT, IF, P2, P3};
public struct Path
{
    public int x, y;
    public Dir dir;

    public Path(int x, int y, Dir dir)
    {
        this.x = x;
        this.y = y;
        this.dir = dir;
    }
}



public class Ant{
    public static int width = 32, height = 32;
    public static int MAX_ENERGY = 800;

    private int x, y, energy,score;
    private Dir dir;
    private map antMap;
    private DNA dna;
    public List<Path> path;

    //constructor
    public Ant(map myMap)
    {
        antMap = new map(myMap);
        dir = Dir.EAST;
        energy = MAX_ENERGY;
        score = 0;
        x = 0;
        y = 0;
        dna = new DNA();
        path = new List<Path>(MAX_ENERGY);
    }
    public Ant(map myMap, Ant initAnt)
    {
        antMap = new map(myMap);
        dir = Dir.EAST;
        energy = MAX_ENERGY;
        score = 0;
        x = 0;
        y = 0;
        dna = new DNA(initAnt.getDna());
        path = new List<Path>(MAX_ENERGY);
    }

    //accessor
    public int getEnergy()
    {
        return energy;
    }
    public DNA getDna()
    {
        return dna;
    }
    public void setDna(DNA newDna)
    {
        dna = new DNA(newDna);
    }
    public void setSubDna(DNA newNode, int count)
    {
        dna.setNode(newNode, ref count);
    }
    public DNA getSubDna(int count)
    {
        DNA nodeI = new DNA();
        dna.getNode(nodeI, ref count);
        return nodeI;
    }
    public int getX() {
        return x;
    }
    public int getY()
    {
        return y;
    }
    public int getScore()
    {
        return score;
    }
    public void setScore(int s)
    {
        score = s;
    }
    public void setEnergy(int e)
    {
        energy = e;
    }
    public map getMap()
    {
        return antMap;
    }
    public void setMap(map newMap)
    {
        antMap = new map(newMap);
    }

    //utility function
    public bool isOperator(Op op)
    {
        if (op == Op.MOVE || op == Op.RIGHT || op == Op.LEFT)
            return true;
        return false;
    }
    public void execute(DNA subDna)
    {
        if (antMap.getMap()[x, y] == SType.FOOD)
        {
            score += 1;
            antMap.setValue(x, y, SType.GROUND);
        }
        if (energy > 0 && score <= 89)
        {
            switch (subDna.getData())
            {
                case Op.MOVE:
                    energy--;
                    move();
                    path.Add(new Path(x, y, dir));
                    break;
                case Op.RIGHT:
                    energy--;
                    right();
                    path.Add(new Path(x, y, dir));
                    break;
                case Op.LEFT:
                    energy--;
                    left();
                    path.Add(new Path(x, y, dir));
                    break;
                case Op.IF:
                    ifFoodAhead(subDna.getChild(0), subDna.getChild(1));
                    break;
                case Op.P2:
                    prog2(subDna.getChild(0), subDna.getChild(1));
                    break;
                case Op.P3:
                    prog3(subDna.getChild(0), subDna.getChild(1), subDna.getChild(2));
                    break;
                default:
                    break;
            }
        }
    }
    public void run()
    {
        while (energy>0 && score <= 89)
            execute(dna);
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
        if (antMap.getMap()[x, y] == SType.FOOD)
        {
            score += 1;
            antMap.setValue(x, y, SType.GROUND);
        }
    }
    //non leaf opérators 
    public void ifFoodAhead(DNA childTrue, DNA childFalse)
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
        if (antMap.getMap()[tempX,tempY] == SType.FOOD)
        {
            execute(childTrue);
        }
        else
        {
            execute(childFalse);
        }
    }
    public void prog2(DNA child1, DNA child2)
    {
        execute(child1);
        execute(child2);
    }
    public void prog3(DNA child1, DNA child2, DNA child3)
    {
        execute(child1);
        execute(child2);
        execute(child3);
    }
}
