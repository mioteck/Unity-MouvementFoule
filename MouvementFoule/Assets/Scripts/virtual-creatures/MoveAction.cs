using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionType { NULL, NEGATE, SIN, COS, ABS, POW2, SQRT}

public class MoveAction{
    public static int[] CHANCE_ACTION = { 20, 10, 25, 25, 6, 7, 7 };
    public static int TOTAL_NB_ACTION = 7;
    public static int MAX_ACTION = 4;
    public static int MAX_POWER = 30;

    public List<ActionType> action = new List<ActionType>();
    public List<int> power = new List<int>();
    public List<Vector3> axe = new List<Vector3>();

    /// <summary>
    /// default constructor, create a randomize MoveAction
    /// </summary>
    public MoveAction()
    {
        int nbAction = Random.Range(0, MAX_ACTION);
        if(nbAction == 0)
        {
            action.Add(ActionType.NULL);
            power.Add(0);
            axe.Add(Vector3.zero);
        }
        else
        {
            for (int i = 0; i < nbAction; i++)
            {
                action.Add(getRandomAction());
                power.Add(Random.Range(0, MAX_POWER));
                axe.Add(getRandomAxe());
            }
        }
    }
    public MoveAction(ActionType a)
    {
        int nbAction = Random.Range(0, MAX_ACTION-1);
        action.Add(a);
        power.Add(Random.Range(0, MAX_POWER));
        axe.Add(getRandomAxe());
    }
    /// <summary>
    /// copy constructor
    /// </summary>
    /// <param name="copy"></param>
    public MoveAction(MoveAction copy)
    {
        foreach(ActionType a in copy.action)
        {
            action.Add(a);
        }
        foreach (int p in copy.power)
        {
            power.Add(p);
        }
        foreach (Vector3 a in copy.axe)
        {
            axe.Add(a);
        }
    }
    /// <summary>
    ///  natural constructor
    /// </summary>
    /// <param name="action"></param>
    /// <param name="power"></param>
    /// <param name="axe"></param>
    public MoveAction(List<ActionType> action, List<int> power, List<Vector3> axe)
    {
        this.action = action;
        this.power = power;
        this.axe = axe;
    }

    /// <summary>
    /// return the torque to apply on time t
    /// </summary>
    /// <param name="t"></param>
    public Vector3 getComputeTorque(float t)
    {
        int id = 0;
        Vector3 result = Vector3.zero;
        foreach(ActionType a in action)
        {
            result += getTorque(id, t);
            id++;
        }
        return result;
    }
    /// <summary>
    /// return the f
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    private Vector3 getTorque(int id, float time)
    {
        switch (action[id])
        {
            case ActionType.NULL:
                return getNull(power[id], axe[id], time);
            case ActionType.NEGATE:
                return getNegate(power[id], axe[id], time);
            case ActionType.SIN:
                return getSin(power[id], axe[id], time);
            case ActionType.COS:
                return getCos(power[id], axe[id], time);
            case ActionType.ABS:
                return getAbs(power[id], axe[id], time);
            case ActionType.POW2:
                return getPow2(power[id], axe[id], time);
            case ActionType.SQRT:
                return getSqrt(power[id], axe[id], time);
            default:
                Debug.Log("ERROR in MoveAction.applyTorque()");
                return Vector3.zero;
        }
    }

    private Vector3 getNull(int power, Vector3 axe, float time)
    {
        return Vector3.zero;
    }
    private Vector3 getNegate(int power, Vector3 axe, float time)
    {
        return -power*axe;
    }
    private Vector3 getSin(int power, Vector3 axe, float time)
    {
        return power * (Mathf.Sin(time)) * axe;
    }
    private Vector3 getCos(int power, Vector3 axe, float time)
    {
        return power * (Mathf.Cos(time)) * axe;
    }
    private Vector3 getAbs(int power, Vector3 axe, float time)
    {
        return new Vector3(Mathf.Abs(power * axe.x), Mathf.Abs(power * axe.y), Mathf.Abs(power * axe.z));
    }
    private Vector3 getPow2(int power, Vector3 axe, float time)
    {
        return new Vector3(Mathf.Pow(power * axe.x, 2), Mathf.Pow(power * axe.y, 2), Mathf.Pow(power * axe.z, 2));
    }
    private Vector3 getSqrt(int power, Vector3 axe, float time)
    {
        return new Vector3(Mathf.Sqrt(power * axe.x), Mathf.Sqrt(power * axe.y), Mathf.Sqrt(power * axe.z));
    }

    /// <summary>
    /// select statistically an action from the liste of probability give by CHANCE_ACTION
    /// </summary>
    /// <returns></returns>
    private ActionType getRandomAction()
    {
        int rand = Random.Range(1, 101);
        int count = 0;
        while(rand > 0)
        {
            rand -= CHANCE_ACTION[count];
            count++;
        }
        count--;
        return (ActionType)count;
    }
    /// <summary>
    /// get a random axe chose from Vector3.right, left, up, down, forward, back
    /// </summary>
    /// <returns></returns>
    private Vector3 getRandomAxe()
    {
        int rand = Random.Range(0, 6);
        switch (rand)
        {
            case 0:
                return Vector3.right;
            case 1:
                return Vector3.left;
            case 2:
                return Vector3.up;
            case 3:
                return Vector3.down;
            case 4:
                return Vector3.forward;
            case 5:
                return Vector3.back;
            default:
                Debug.Log("ERROR in MoveAction.getRandomAxe()");
                return Vector3.zero;
        }
    }
}
