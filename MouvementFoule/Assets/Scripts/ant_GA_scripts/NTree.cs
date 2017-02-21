using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public delegate void TreeVisitor<T>(T nodeData);

public class NTree<T>
{
    private T data;
    private LinkedList<NTree<T>> children;
    private int length; 

    public NTree()
    {
        length = 1;
        children = new LinkedList<NTree<T>>();
        calcLength();
    }
    public NTree(T data)
    {
        this.data = data;
        length = 1;
        children = new LinkedList<NTree<T>>();
        calcLength();
    }

    public void AddChild(T data)
    {
        NTree<T> temp = new NTree<T>(data);
        children.AddFirst(temp);
        length += temp.length;
    }
    public LinkedList<NTree<T>> GetChildren()
    {
        return children;
    }
    public NTree<T> GetChild(int i)
    {
        foreach (NTree<T> n in children)
            if (--i == 0)
                return n;
        return null;
    }
    public NTree<T> Clone()
    {
        NTree<T> temp = new NTree<T>();
        temp.data = data;
        temp.length = length;
        temp.children = new LinkedList<NTree<T>>(children);
        return temp;
    }
    public int getLength()
    {
        return length;
    }
    public void calcLength()
    {
        int i = 0;
        size(ref i);
        length = i;
    }
    public void GetNodeI(ref NTree<T> nodeSearch, int count, ref int i)
    {
        ++i;
        if (i == count)
        {
            nodeSearch.data = data;
            nodeSearch.children = new LinkedList<NTree<T>>(children);
            nodeSearch.length = length;
        }
        foreach (NTree<T> kid in children)
        {
            if (i < count)
            {
                kid.GetNodeI(ref nodeSearch, count, ref i);
            }
        }
            
    }
    public void SetNodeI(ref NTree<T> newNode, int count, ref int i)
    {
        ++i;
        if (i == count)
        {
            data = newNode.data;
            children = new LinkedList<NTree<T>>(newNode.GetChildren());
            length = newNode.length;
        }
        foreach (NTree<T> kid in children)
        {
            if (i < count)
            {
                kid.SetNodeI(ref newNode, count, ref i);
            }
        }
    }
    public void size(ref int i)
    {
        ++i;
        foreach (NTree<T> child in children)
        {
            child.size(ref i);
        }
    }
    public T getData()
    {
        return data;
    }
    public void setData(T newData)
    {
        data = newData;
    }
    public void Traverse(NTree<T> node, TreeVisitor<T> visitor)
    {
        visitor(node.data);
        foreach (NTree<T> kid in node.children)
            Traverse(kid, visitor);
    }
    public void toString(ref string str)
    {
        str += data;
        int i = 0;
        if (children.Count > 0)
        {
            str += " [ ";
            foreach (NTree<T> child in children)
            {
                ++i;
                child.toString(ref str);
                if (i<children.Count)
                    str += " _ ";
            }
            str += " ]";
        }
    }

}
